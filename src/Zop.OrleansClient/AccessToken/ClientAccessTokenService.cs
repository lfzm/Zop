using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Zop.OrleansClient.Configuration;

namespace Zop.OrleansClient.AccessToken
{
    /// <summary>
    /// 客户端授权 ClientCredentials
    /// </summary>
    public class ClientAccessTokenService : IAccessTokenService
    {
        private DateTime expirationTime = DateTime.MinValue;
        private string accessToken;
        private readonly OrleansAuthOptions options;
        private readonly ILogger logger;
        /// <summary>
        /// 客户端访问令牌
        /// </summary>
        /// <param name="_options"></param>
        /// <param name="_logger"></param>
        public ClientAccessTokenService(IOptions<OrleansAuthOptions> _options, ILogger<ClientAccessTokenService> _logger)
        {
            this.options = _options?.Value;
            this.logger = _logger;
        }

        /// <summary>
        /// 是否已经过期
        /// </summary>
        public bool IsExpiration
        {
            get
            {
                if (expirationTime == DateTime.MinValue)
                    return false;
                else if (expirationTime < DateTime.Now)
                    return false;
                else return true;
            }
        }
        /// <summary>
        /// 访问Token
        /// </summary>
        public string AccessToken
        {
            get
            {
                if (IsExpiration)
                    return accessToken;
                else
                {
                    var task = this.RefreshAccessToken();
                    task.Wait();
                    return task.Result;
                }
            }
        }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpirationTime => expirationTime;
        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <returns></returns>
        public async Task<string> RefreshAccessToken()
        {
            // 从元数据中发现客户端
            DiscoveryClient client = new DiscoveryClient(this.options.Authority);
            client.Policy.RequireHttps = this.options.Authority.ToLower().Contains("https");
            var disco = await client.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            // 请求令牌
            var tokenClient = new TokenClient(disco.TokenEndpoint, this.options.ClientId, this.options.ClientSecret);
            var tokenResponseTask = tokenClient.RequestClientCredentialsAsync(string.Join(" ", this.options.ClientApiScope.ToArray()));

            tokenResponseTask.Wait();
            var tokenResponse = tokenResponseTask.Result;
            if (tokenResponse.IsError)
            {
                logger.LogError("ClientCredentials Get AccessToken Error Message:{Error}", tokenResponse.Error);
                return "";
            }
            logger.LogDebug("  AccessToken Response:{Message}", tokenResponse.Json);
            this.accessToken = tokenResponse.AccessToken;
            //提前一分钟获取
            this.expirationTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn - 60);
            return this.accessToken;
        }

      
    }
}
