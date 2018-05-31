using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using Zop.OrleansClient;
using Zop.OrleansClient.AccessToken;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrleansClientServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Orleans客户服务
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddOrleansClient(this IServiceCollection services, Action<IOrleansClientBuilder> builder)
        {
            builder.Invoke(new OrleansClientBuilder(services));

            services.AddSingleton<IOrleansClient, OrleansClient>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            //添加授权服务
            services.TryAddSingleton(typeof(IKeyedServiceCollection<,>), typeof(KeyedServiceCollection<,>));
            services.AddSingletonNamedService<IAccessTokenService, ClientAccessTokenService>((AccessTokenType.ClientCredentials.ToString()));
            services.AddSingletonNamedService<IAccessTokenService, UserAccessTokenService>((AccessTokenType.UserCredentials.ToString()));
            return services;
        }

       
    }
}
