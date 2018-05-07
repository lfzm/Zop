using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.RedisClient
{
    /// <summary>
    /// Redis配置
    /// </summary>
    public class RedisOpions
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Configuration { get; set; }
        /// <summary>
        /// 系统Key的前缀
        /// </summary>
        public string KeyPrefix { get; set; } = "";
        /// <summary>
        /// StackExchange.Redis配置
        /// </summary>
        public ConfigurationOptions ConfigurationOptions { get; set; } 
    }
}
