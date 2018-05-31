using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zop.OrleansClient.Configuration;

namespace Zop.OrleansClient
{
    public class OrleansClient : IOrleansClient
    {
        private readonly Dictionary<string, IClusterClient> clients = new Dictionary<string, IClusterClient>();
        private readonly IServiceProvider ServiceProvider;
        private readonly OrleansAuthOptions Options;
        private readonly ILogger Logger;
        public OrleansClient(IServiceProvider serviceProvider, IOptions<OrleansAuthOptions> options, ILogger<OrleansClient> logger)
        {
            this.Logger = logger;
            this.ServiceProvider = serviceProvider;
            this.Options = options?.Value;

        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetAuthorization(accessType);
            return i;

        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetAuthorization(accessType);
            return i;
        }

        public TGrainInterface GetGrain<TGrainInterface>(string primaryKey, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetAuthorization(accessType);
            return i;

        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            this.SetAuthorization(accessType);
            return i;
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            this.SetAuthorization(accessType);
            return i;

        }


        /// <summary>
        /// 获取Orleans ClusterClient
        /// </summary>
        /// <typeparam name="TGrainInterface"></typeparam>
        private IClusterClient GetClusterClient<TGrainInterface>()
        {
            IClusterClient client = null;
            string name = typeof(TGrainInterface).Namespace;

            int attempt = 0;
            while (true)
            {
                try
                {
                    if (clients.ContainsKey(name))
                        client = clients[name];
                    else
                    {
                        lock (clients)
                        {
                            if (!clients.ContainsKey(name))
                            {
                                client = BuilderClient(name);
                                clients.Add(name, client);
                            }
                        }
                    }


                    if (!client.IsInitialized)
                    {
                        //客户端未初始化，连接服务端
                        client.Connect().Wait();
                        Logger.LogDebug($"Connection {name} Sucess...");
                    }

                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                    attempt++;
                    if (attempt <= this.Options.InitializeAttemptsBeforeFailing)
                    {
                        client = BuilderClient(name);
                        clients[name] = client;

                        Logger.LogDebug($"Attempt {attempt} of " + this.Options.InitializeAttemptsBeforeFailing + " failed to initialize the Orleans client.");
                        Task.Delay(TimeSpan.FromSeconds(4)).Wait();
                        continue;
                    }
                    Logger.LogError($"Connection {name} Faile...");
                    throw new Exception($"Connection {name} Faile...");
                }
                return client;
            }
        }

        private IClusterClient BuilderClient(string name)
        {
            IClientBuilder builder = this.ServiceProvider.GetRequiredServiceByName<IClientBuilder>(name);
            return builder.Build();
        }

        /// <summary>
        /// 设置授权码
        /// </summary>
        /// <param name="accessType"></param>
        private void SetAuthorization(AccessTokenType tokenType)
        {
            if (tokenType == AccessTokenType.Default)
                tokenType = Options.DefaultTokenType;
            var tokenService = ServiceProvider.GetRequiredServiceByName<IAccessTokenService>((tokenType.ToString()));
            if (tokenService != null)
            {
                RequestContext.Set("Authorization", string.Format("Bearer {0}", tokenService.AccessToken));
            }
        }
    }
}
