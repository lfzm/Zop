using System;
using System.Collections.Generic;
using System.Text;
using Zop.Toolkit.IDGenerator;
using Zop.Toolkit.IDGenerator.Snowflake;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// ID生成器服务配置
    /// </summary>
    public static class IDGeneratorBuilderExtensions
    {
        /// <summary>
        /// 添加ID生成器服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddIDGenerator(this IServiceCollection services)
        {
            services.AddSingleton<IWorkerOpation, WorkerOpation>();
            services.AddSingleton<IIDGenerated, IdWorker>();
            return services;
        }
    }
}
