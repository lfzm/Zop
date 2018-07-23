using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Extensions.RedisClient;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Redis服务配置扩展类
    /// </summary>
    public static class RedisServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Redis服务
        /// </summary>
        /// <param name="service">IServiceCollection</param>
        /// <param name="configuration">服务配置</param>
        /// <returns></returns>
        public static IServiceCollection AddRedis(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddOptions().Configure<RedisOpions>(configuration);
            service.AddRedis();
            return service;
        }
        /// <summary>
        /// 添加Redis服务
        /// </summary>
        /// <param name="service">IServiceCollection</param>
        /// <param name="action">服务配置函数</param>
        /// <returns></returns>
        public static IServiceCollection AddRedis(this IServiceCollection service, Action<RedisOpions> action)
        {
            service.AddOptions().Configure(action);
            service.AddRedis();
            return service;
        }

        /// <summary>
        /// 添加Redis服务
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            services.AddSingleton<IRedisClient,RedisClient>();
            return services;
        }
    }
}
