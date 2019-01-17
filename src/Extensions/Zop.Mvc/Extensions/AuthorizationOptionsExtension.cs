using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 授权配置扩展类
    /// </summary>
    public static class AuthorizationOptionsExtension
    {
        /// <summary>
        /// 添加用户ID授权过滤器
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static AuthorizationOptions AddPolicyUserId(this AuthorizationOptions options)
        {
            options.AddPolicy(AuthorizePolicy.USERID, p => p.RequireClaim(JwtClaimTypes.Subject));
            return options;
        }
    }
}
namespace Microsoft.AspNetCore.Authorization
{
    /// <summary>
    /// 授权政策
    /// </summary>
    public static class AuthorizePolicy
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public const string USERID = "USERID";
    }
}