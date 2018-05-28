using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.OrleansClient.Configuration
{
    public class OrleansClientOptions
    {
        public OrleansClientOptions()
        {
            this.StaticGatewayList = new List<Uri>();
        }
        /// <summary>
        /// 客户端名称
        /// </summary>
        public string ServiceId { get; set; }
        /// <summary>
        /// 集群ID
        /// </summary>
        public string ClusterId { get; set; }
        /// <summary>
        /// 是否为本地服务
        /// </summary>
        public bool IsLocalHost { get; set; } = false;
        /// <summary>
        /// 客户端网关列表
        /// </summary>
        public List<Uri> StaticGatewayList { get; set; }
        /// <summary>
        /// Zeekeeper的连接字符串
        /// </summary>
        public string ZooKeeperConnectionString { get; set; }
    }
}
