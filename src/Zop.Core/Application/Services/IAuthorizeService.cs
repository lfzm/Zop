using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Zop.Application.Services
{
    public interface IAuthorizeService
    {

        /// <summary>
        /// 授权用户信息
        /// </summary>
        ClaimsPrincipal User { get; }
    }
}
