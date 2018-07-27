using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Extensions.OrleansClient.Configuration
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
        /// 客户端网关列表
        /// </summary>
        public List<Uri> StaticGatewayList { get; set; }
        /// <summary>
        /// Zeekeeper的地址
        /// </summary>
        public string ZooKeeperAddress { get; set; }
        /// <summary>
        /// Consul地址
        /// </summary>
        public string ConsulAddress { get; set; }
    }
}
