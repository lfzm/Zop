using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zop.OrleansClient.AccessToken
{
    /// <summary>
    /// Orleans 上下文的TokenService
    /// </summary>
    public class OrleansContextTokenService : IAccessTokenService
    {
        private readonly ILogger logger;
        public OrleansContextTokenService(ILogger<UserAccessTokenService> _logger)
        {
            this.logger = _logger;
        }

        public string AccessToken
        {
            get
            {
                //Orleans 内部调用
                var accessToken = RequestContext.Get("Authorization")?.ToString();
                if (!string.IsNullOrEmpty(accessToken))
                    return accessToken.ToString().Replace("Bearer ", "");
                else
                    return "";

            }
        }

        public bool IsExpiration
        {
            get
            {
                if (ExpirationTime == DateTime.MinValue)
                    return false;
                else if (ExpirationTime > DateTime.UtcNow)
                    return false;
                else return true;
            }
        }

        public DateTime ExpirationTime
        {
            get
            {
                return DateTime.MinValue;
            }
        }

        public Task<string> RefreshAccessToken()
        {
            return Task.FromResult(this.AccessToken);
        }
    }
}
