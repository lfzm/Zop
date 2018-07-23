using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Extensions.OrleansClient.Configuration
{
    public class OrleansAuthOptions
    {
        public OrleansAuthOptions()
        {
            this.ClientApiScope = new List<string>();
            this.ClientIdentityScope = new List<string>();
        }

        /// <summary>
        /// 初始化重试失败次数
        /// </summary>
        public int InitializeAttemptsBeforeFailing { get; set; } = 10;

        #region 授权认证信息
        /// <summary>
        /// 默认授权方式
        /// </summary>
        public AccessTokenType DefaultTokenType { get; set; } = AccessTokenType.NotCredentials;
        /// <summary>
        /// 认证服务ApiUrl
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// 认证服务分配客户端Id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// 客户端秘钥
        /// </summary>
        public string ClientSecret { get; set; }
        /// <summary>
        /// 认证方案
        /// </summary>
        public string AuthenticationScheme { get; set; } = "oidc";
        /// <summary>
        /// 访问的API范围
        /// </summary>
        public List<string> ClientApiScope { get; set; }
        /// <summary>
        /// 访问的Identity资源范围
        /// </summary>
        public List<string> ClientIdentityScope { get; set; }
        #endregion

    }


}
