using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Storage;
using System;
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
            //配置差异对比服务
            services.TryAddSingleton<IChangeDetector, ChangeDetector>();
            services.TryAddSingleton<IChangeManager, ChangeManager>();
            services.TryAddSingleton<IChangeManagerFactory, ChangeManagerFactory>();
            services.AddMemoryCache(opt =>
            {
                opt.ExpirationScanFrequency = TimeSpan.FromHours(2);
            });

            //配置Orleans 的存储配置
            services.TryAddSingleton<IGrainStorage>(sp => sp.GetServiceByName<IGrainStorage>(ProviderConstants.DEFAULT_STORAGE_PROVIDER_NAME));
            services.AddSingletonNamedService<IGrainStorage, RepositoryStorage>(storageName);

            //对应实体的仓储配置
            var builder = new RepositoryBuilder(services);
            builer.Invoke(builder);
            return services;
        }

    }
}
