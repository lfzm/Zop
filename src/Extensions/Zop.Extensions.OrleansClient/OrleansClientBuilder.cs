using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using Zop.Extensions.OrleansClient.Configuration;

namespace Zop.Extensions.OrleansClient
{
    public class OrleansClientBuilder : IOrleansClientBuilder
    {
        private readonly IServiceCollection services;

        public OrleansClientBuilder(IServiceCollection services)
        {
            this.services = services;
        }

        public IOrleansClientBuilder AddClient(Action<OrleansClientOptions> options, Action<IClientBuilder> builder = null)
        {
            this.services.AddOptions().Configure(options);
            var opt = new OrleansClientOptions();
            options.Invoke(opt);
            return this.AddClient(opt, builder);
        }

        public IOrleansClientBuilder AddClient(IConfiguration configuration, Action<IClientBuilder> builder = null)
        {
            this.services.AddOptions().Configure<OrleansClientOptions>(configuration);
            var option = configuration.Get<OrleansClientOptions>();
            return this.AddClient(option, builder);
        }

        public IOrleansClientBuilder AddClient(OrleansClientOptions options, Action<IClientBuilder> builder = null)
        {
            this.services.AddTransientNamedService(options.ServiceId, (service, key) =>
            {
                if (options == null)
                    throw new ArgumentNullException("OrleansClientOptions Cannot be NULL");
                if (string.IsNullOrEmpty(options.ServiceId))
                    throw new ArgumentNullException("ServiceId Can not be empty");

                var build = new ClientBuilder()
                    .Configure<ClusterOptions>(opt =>
                    {
                        opt.ServiceId = options.ServiceId;
                        if (!string.IsNullOrEmpty(options.ClusterId))
                            opt.ClusterId = options.ClusterId;
                        else
                            opt.ClusterId = options.ServiceId;
                    });
                builder?.Invoke(build);

                //配置静态网关
                if (options.StaticGatewayList.Count > 0)
                {
                    build.UseStaticClustering((StaticGatewayListProviderOptions opt) =>
                    {
                        foreach (var gateway in options.StaticGatewayList)
                        {
                            opt.Gateways.Add(gateway);
                        }
                    });
                }

                return build;
            });
            return this;
        }

   
    }
}
