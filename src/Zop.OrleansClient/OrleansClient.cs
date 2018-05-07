using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Runtime;

namespace Zop.OrleansClient
{
    public class OrleansClient : IOrleansClient
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly OrleansClientOptions Options;
        private IClusterClient client;
        private AccessTokenType AccessTokenType;
        public OrleansClient(IServiceProvider serviceProvider, IOptions<OrleansClientOptions> options)
        {
            this.ServiceProvider = serviceProvider;
            this.Options = options?.Value;
            if (this.Options != null)
                AccessTokenType = this.Options.DefaultTokenType;

        }

        public void BindGrainReference(Orleans.Runtime.IAddressable grain)
        {
            client.BindGrainReference(grain);
        }

        public Task<TGrainObserverInterface> CreateObjectReference<TGrainObserverInterface>(IGrainObserver obj) where TGrainObserverInterface : IGrainObserver
        {
            return this.GetClusterClient<TGrainObserverInterface>().CreateObjectReference<TGrainObserverInterface>(obj);
        }

        public Task DeleteObjectReference<TGrainObserverInterface>(IGrainObserver obj) where TGrainObserverInterface : IGrainObserver
        {
            return this.GetClusterClient<TGrainObserverInterface>().DeleteObjectReference<TGrainObserverInterface>(obj);
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetAuthorization();
            return i;
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey
        {

            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetAuthorization();
            return i;
        }

        public TGrainInterface GetGrain<TGrainInterface>(string primaryKey, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
            this.SetAuthorization();
            return i;
        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey
        {
            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            this.SetAuthorization();
            return i;
        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey
        {

            var i = this.GetClusterClient<TGrainInterface>().GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
            this.SetAuthorization();
            return i;
        }


        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, AccessTokenType accessType) where TGrainInterface : IGrainWithGuidKey
        {
            this.AccessTokenType = accessType;
            return this.GetGrain<TGrainInterface>(primaryKey);

        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, AccessTokenType accessType) where TGrainInterface : IGrainWithIntegerKey
        {
            this.AccessTokenType = accessType;
            return this.GetGrain<TGrainInterface>(primaryKey);
        }

        public TGrainInterface GetGrain<TGrainInterface>(string primaryKey, AccessTokenType accessType) where TGrainInterface : IGrainWithStringKey
        {
            this.AccessTokenType = accessType;
            return this.GetGrain<TGrainInterface>(primaryKey);

        }

        public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, AccessTokenType accessType) where TGrainInterface : IGrainWithGuidCompoundKey
        {
            this.AccessTokenType = accessType;
            return this.GetGrain<TGrainInterface>(primaryKey, keyExtension, null);

        }

        public TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, AccessTokenType accessType) where TGrainInterface : IGrainWithIntegerCompoundKey
        {
            this.AccessTokenType = accessType;
            return this.GetGrain<TGrainInterface>(primaryKey, keyExtension, null);

        }


        /// <summary>
        /// 获取Orleans ClusterClient
        /// </summary>
        /// <typeparam name="TGrainInterface"></typeparam>
        private IClusterClient GetClusterClient<TGrainInterface>()
        {
            string name = typeof(TGrainInterface).Namespace;
            this.client = this.GetServiceByName(name);

            int attempt = 0;
            while (true)
            {
                try
                {
                    if (this.client.IsInitialized)
                        return this.client;
                    //客户端未初始化，连接服务端
                    this.client.Connect().Wait();

                }
                catch (Exception ex)
                {
                    if (ex.InnerException!=null &&  ex.InnerException is Orleans.Runtime.SiloUnavailableException)
                    {
                        attempt++;
                        Console.WriteLine($"Attempt {attempt} of " + this.Options.InitializeAttemptsBeforeFailing + " failed to initialize the Orleans client.");
                        if (attempt <= this.Options.InitializeAttemptsBeforeFailing)
                        {
                            Task.Delay(TimeSpan.FromSeconds(4)).Wait();
                            continue;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 设置授权码
        /// </summary>
        /// <param name="accessType"></param>
        private void SetAuthorization()
        {
            if (this.AccessTokenType == AccessTokenType.NotCredentials)
                return;
            var tokenService = ServiceProvider.GetRequiredServiceByName<IAccessTokenService>((this.AccessTokenType.ToString()));
            if (tokenService != null)
            {
              RequestContext.Set("Authorization", string.Format("Bearer {0}", tokenService.AccessToken));
            }

        }
        private IClusterClient GetServiceByName(string name)
        {
            var client = ServiceProvider.GetRequiredService<IDictionary<string, IClusterClient>>();
            if (client.ContainsKey(name))
                return client[name];
            else
                return null;
        }
    }
}
