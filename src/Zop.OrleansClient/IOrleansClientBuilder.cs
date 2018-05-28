using Microsoft.Extensions.Configuration;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.OrleansClient.Configuration;

namespace Zop.OrleansClient
{
    /// <summary>
    /// OrleansClient 建筑器
    /// </summary>
    public interface IOrleansClientBuilder
    {
        /// <summary>
        /// 添加Orleans客户端
        /// </summary>
        /// <param name="options">配置</param>
        /// <param name="builder"></param>
        /// <returns></returns>
        IOrleansClientBuilder AddClient(Action<OrleansClientOptions> options, Action<IClientBuilder> builder = null);
        /// <summary>
        /// 添加Orleans客户端
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <param name="builder"></param>
        /// <returns></returns>
        IOrleansClientBuilder AddClient(IConfiguration configuration, Action<IClientBuilder> builder = null);
        /// <summary>
        /// 添加客户端授权
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IOrleansClientBuilder AddAuthentication(Action<OrleansAuthOptions> options);
        /// <summary>
        /// 添加客户端授权
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <returns></returns>
        IOrleansClientBuilder AddAuthentication(IConfiguration configuration);

    }
}
