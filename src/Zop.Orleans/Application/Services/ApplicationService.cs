using Orleans;
using Orleans.Runtime;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Zop.Application.Services
{

    /// <summary>
    /// 这个类可以用作应用程序服务的基类
    /// </summary>
    public class ApplicationService : Grain, IApplicationService, IAuthorizeService
    {
        /// <summary>
        /// 授权用户信息
        /// </summary>
        public ClaimsPrincipal User
        {
            get
            {
                return (ClaimsPrincipal)RequestContext.Get(AuthorizeConstant.UserPrincipalKey);
            }
        }
    }

    /// <summary>
    /// 这个类可以用作应用程序服务的基类
    /// </summary>
    public class ApplicationService<TState> : Grain<TState>, IApplicationService<TState>, IAuthorizeService where TState : class, new()
    {
        /// <summary>
        /// 授权用户信息
        /// </summary>
        public ClaimsPrincipal User
        {
            get
            {
                return (ClaimsPrincipal)RequestContext.Get(AuthorizeConstant.UserPrincipalKey);
            }
        }
        ///<inheritdoc/>
        public Task<TState> ReadState()
        {
            return Task.FromResult(base.State);
        }
       
        ///<inheritdoc/>
        public Task ClearState()
        {
            base.ClearStateAsync();
            return Task.CompletedTask;
        }
    }
    public class AuthorizeConstant
    {
        public const string UserPrincipalKey = "default_UserPrincipalKey";
    }
}
