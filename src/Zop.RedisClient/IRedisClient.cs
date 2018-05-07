using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.RedisClient
{
    public interface IRedisClient
    {
        /// <summary>
        /// 连接Redis
        /// </summary>
        /// <returns></returns>
        ConnectionMultiplexer Connect();
        /// <summary>
        /// 获取缓存库
        /// </summary>
        /// <param name="dbNum"></param>
        /// <returns></returns>
        IDatabase GetDatabase(int dbNum = 0);
    }
}
