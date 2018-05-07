using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;
using System;
using System.Linq.Expressions;
using System.Reflection;
using Zop.Domain.Entities;
using Zop.Repositories;
using Zop.Repositories.ChangeDetector;

namespace Orleans
{
    public static class RepositorySiloBuilderExtensions
    {

        /// <summary>
        /// 添加支付应用服务
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="builer"><see cref="RepositoryBuilder"/> 配置</param>
        /// <param name="storageName">Storage的名称</param>
        /// <returns></returns>
        public static IServiceCollection AddRepositoryStorage(this IServiceCollection services, Action<RepositoryBuilder> builer, string storageName = RepositoryStorage.DefaultName)
        {
            services.TryAddTransient<IChangeDetector, ChangeDetector>();
            services.TryAddTransient<IChangeManager, ChangeManager>();
            services.TryAddTransient<IChangeManagerFactory, ChangeManagerFactory>();

            services.TryAddSingleton<IGrainStorage>(sp => sp.GetServiceByName<IGrainStorage>(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME));
            services.AddScopedNamedService<IGrainStorage, RepositoryStorage>(storageName);

            var builder = new RepositoryBuilder(services);
            builer.Invoke(builder);
            return services;
        }

    }
}
