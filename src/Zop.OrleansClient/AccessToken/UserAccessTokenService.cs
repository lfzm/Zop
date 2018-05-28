using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Zop.OrleansClient.Configuration;

namespace Zop.OrleansClient.AccessToken
{
    /// <summary>
    /// 用户AccessToken服务
    /// </summary>
    public class UserAccessTokenService : IAccessTokenService
    {
        private readonly OrleansAuthOptions options;
        private readonly ILogger logger;
        private readonly IHttpContextAccessor httpContextAccessor;
        public UserAccessTokenService(
            IOptions<OrleansAuthOptions> _options,
            ILogger<UserAccessTokenService> _logger,
            IHttpContextAccessor _httpContextAccessor)
        {
            this.options = _options?.Value;
            this.logger = _logger;
            this.httpContextAccessor = _httpContextAccessor;
        }

        public string AccessToken
        {
            get
            {
                //Orleans 内部调用
                var accessToken = RequestContext.Get("Authorization")?.ToString();
                if (!string.IsNullOrEmpty(accessToken))
                {
                    return accessToken.ToString().Replace("Bearer ", "");
                }
                //刷新访问令牌
                if (IsExpiration)
                    return httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
                else
                {

                    return this.RefreshAccessToken().Result;
                }
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
                var take = httpContextAccessor.HttpContext.GetTokenAsync("expires_at");
                take.Wait();
                var expires_at = take.Result;
                if (string.IsNullOrEmpty(expires_at))
                    return DateTime.MinValue;
                else
                    return Convert.ToDateTime(expires_at);

            }
        }

        public async Task<string> RefreshAccessToken()
        {
            string refreshToken = await httpContextAccessor.HttpContext.GetTokenAsync("refresh_token");
            if (string.IsNullOrEmpty(refreshToken))
                throw new Exception("刷新令牌不能为空");

            DiscoveryClient client = new DiscoveryClient(options.Authority);
            client.Policy.RequireHttps = options.Authority.ToLower().Contains("https");
            var disco = await client.GetAsync();
            if (disco.IsError) throw new Exception(disco.Error);

            var tokenClient = new TokenClient(disco.TokenEndpoint, options.ClientId, options.ClientSecret);
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(refreshToken);

            if (!tokenResult.IsError)
            {
                var old_id_token = await httpContextAccessor.HttpContext.GetTokenAsync("id_token");
                var new_access_token = tokenResult.AccessToken;
                var new_refresh_token = tokenResult.RefreshToken;

                //存储Token
                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                var tokens = new List<AuthenticationToken>();
                tokens.Add(new AuthenticationToken { Name = "id_token", Value = old_id_token });
                tokens.Add(new AuthenticationToken { Name = "access_token", Value = new_access_token });
                tokens.Add(new AuthenticationToken { Name = "refresh_token", Value = new_refresh_token });
                tokens.Add(new AuthenticationToken { Name = "expires_at", Value = expiresAt.ToString("o", CultureInfo.InvariantCulture) });

                var info = await httpContextAccessor.HttpContext.AuthenticateAsync("Cookies");
                info.Properties.StoreTokens(tokens);
                await httpContextAccessor.HttpContext.SignInAsync("Cookies", info.Principal, info.Properties);

                return httpContextAccessor.HttpContext.GetTokenAsync("access_token").Result;
            }
            else
                throw new Exception("刷新访问令牌失败：error=" + tokenResult.ErrorDescription);
        }
    }
}
