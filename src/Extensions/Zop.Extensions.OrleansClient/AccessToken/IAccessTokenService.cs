using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zop.Extensions.OrleansClient
{
    /// <summary>
    /// 访问令牌服务
    /// </summary>
    public interface IAccessTokenService
    {
        /// <summary>
        /// 访问Token
        /// </summary>
        string AccessToken { get; }
        /// <summary>
        /// 过期时间
        /// </summary>
        DateTime ExpirationTime { get; }
        /// <summary>
        /// 令牌是否已经过期
        /// </summary>
        bool IsExpiration { get; }
        /// <summary>
        /// 刷新访问令牌
        /// </summary>
        /// <returns></returns>
        Task<string> RefreshAccessToken();
    }

    public enum AccessTokenType
    {
        /// <summary>
        /// 默认授权
        /// </summary>
        Default,
        /// <summary>
        /// 用户授权凭证
        /// </summary>
        UserCredentials,
        /// <summary>
        /// 客户端授权凭证
        /// </summary>
        ClientCredentials,
        /// <summary>
        /// 不需要授权凭证
        /// </summary>
        NotCredentials,
        /// <summary>
        /// Orlenas 上下文获取配置
        /// </summary>
        OrleansContext
    }
}
