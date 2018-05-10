using IdentityModel;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// ClaimsPrincipal 扩展类
    /// </summary>
    public static class ClaimsPrincipalExtension
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserId(this ClaimsPrincipal principal)
        {
            var sub = principal.FindFirst(JwtClaimTypes.Subject)?.Value;
            if (string.IsNullOrEmpty(sub))
                sub = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(sub))
                return null;
            return sub;
        }
        /// <summary>
        /// 客户端Id
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string ClientId(this ClaimsPrincipal principal)
        {
            var clientId = principal.FindFirst(JwtClaimTypes.ClientId)?.Value;
            if (clientId.IsNull())
                return null;
            return clientId;
        }
        
        /// <summary>
        /// 昵称
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string NickName(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(JwtClaimTypes.NickName)?.Value;
        }
    }
}
