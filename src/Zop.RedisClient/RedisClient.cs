using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Zop.RedisClient
{
    /// <summary>
    /// Redis客户端
    /// </summary>
    public class RedisClient: IRedisClient
    {
        private readonly ILogger logger;
        private readonly RedisOpions options;

        /// <summary>
        /// 链接字符串
        /// </summary>
        private ConnectionMultiplexer conn;
        /// <summary>
        /// Redis客户端
        /// </summary>
        /// <param name="_logger">日志记录器</param>
        /// <param name="_opions">redis客户端配置</param>
        public RedisClient(ILogger<RedisClient> _logger, IOptions<RedisOpions> _opions)
        {
            this.logger = _logger;
            this.options = _opions?.Value;
        }

        /// <summary>
        /// 连接Redis
        /// </summary>
        /// <returns></returns>
        public ConnectionMultiplexer Connect()
        {
            //如果已经连接就直接返回
            if (conn != null && conn.IsConnected)
                return conn;

            if (this.options.ConfigurationOptions == null)
                conn = ConnectionMultiplexer.Connect(this.options.Configuration);
            else
                conn = ConnectionMultiplexer.Connect(this.options.ConfigurationOptions);

            //注册如下事件
            conn.ConnectionFailed += MuxerConnectionFailed;
            conn.ConnectionRestored += MuxerConnectionRestored;
            conn.ErrorMessage += MuxerErrorMessage;
            conn.ConfigurationChanged += MuxerConfigurationChanged;
            conn.HashSlotMoved += MuxerHashSlotMoved;
            conn.InternalError += MuxerInternalError;

            return conn;
        }

        /// <summary>
        /// 获取缓存库
        /// </summary>
        /// <param name="dbNum"></param>
        /// <returns></returns>
        public IDatabase GetDatabase(int dbNum = 0)
        {
            RedisClientExtensions.KeyPrefix = this.options.KeyPrefix;
            return this.Connect().GetDatabase(dbNum);
        }

        #region Redis客户端事件

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            this.logger.LogInformation("Configuration changed: " + e.EndPoint);
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            this.logger.LogError("ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            this.logger.LogError("ConnectionRestored: " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            this.logger.LogError("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            this.logger.LogInformation("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            this.logger.LogError("InternalError:Message" + e.Exception.Message);
        }
        #endregion Redis客户端事件



    }
}
