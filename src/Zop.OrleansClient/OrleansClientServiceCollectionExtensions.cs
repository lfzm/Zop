using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using Orleans.Configuration;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using Zop.OrleansClient;
using Zop.OrleansClient.AccessToken;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class OrleansClientServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Orleans客户服务
        /// </summary>
        /// <param name="service">IServiceCollection</param>
        /// <param name="storeOptionsAction">服务配置</param>
        /// <returns></returns>
        public static IServiceCollection AddOrleansClient(this IServiceCollection service, IConfiguration configuration, Action<IClientBuilder> clientBuilder = null)
        {
            service.AddOptions().Configure<OrleansClientOptions>(configuration);
            var options = configuration.Get<OrleansClientOptions>();
            service.AddOrleansClient(options, clientBuilder);
            return service;
        }
        /// <summary>
        /// 添加Orleans客户服务
        /// </summary>
        /// <param name="service">IServiceCollection</param>
        /// <param name="action">服务配置函数</param>
        /// <returns></returns>
        public static IServiceCollection AddOrleansClient(this IServiceCollection service, Action<OrleansClientOptions> action, Action<IClientBuilder> clientBuilder = null)
        {
            service.AddOptions().Configure(action);
            var options = new OrleansClientOptions();
            action.Invoke(options);
            service.AddOrleansClient(options, clientBuilder);
            return service;
        }
        /// <summary>
        /// 添加Orleans客户服务
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <returns></returns>
        public static IServiceCollection AddOrleansClient(this IServiceCollection services, OrleansClientOptions options, Action<IClientBuilder> clientBuilder = null)
        {
            IDictionary<string, IClusterClient> clients = new Dictionary<string, IClusterClient>();
            //循环配置客户端集合
            foreach (var client in options.Clients)
            {
                var builder = new ClientBuilder()
                        .ConfigureDefaults()
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(GetAssembly(client.Name)).WithReferences())
                        .UseLocalhostClustering()
                        .Configure<ClusterOptions>(opt =>
                          {
                              opt.ServiceId = client.Name;
                              if (!string.IsNullOrEmpty(client.ClusterId))
                                  opt.ClusterId = client.ClusterId;
                              else
                                  opt.ClusterId = client.Name;
                          })
                        .UseStaticClustering(opt =>
                        {
                            foreach (var address in client.GatewayList)
                            {
                                var uri = (new IPEndPoint(IPAddress.Parse(address.Address), address.Port)).ToGatewayUri();
                                opt.Gateways.Add(uri);
                            }
                        });

                if (clientBuilder != null)
                    clientBuilder.Invoke(builder);

                //建造一个ClusterClient 
                var clusterClient = builder.Build();
                clients.Add(client.Name, clusterClient);
                services.AddSingleton(factory =>
                {
                    Func<string, IClusterClient> accessor = key =>
                    {
                        clients = factory.GetService<IDictionary<string, IClusterClient>>();
                        if (clients.ContainsKey(key))
                            return clients[key];
                        else
                            return null;
                    };
                    return accessor;
                });
            }
            services.AddSingleton(clients);
            services.AddTransient<IOrleansClient, OrleansClient>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            //添加授权服务
            services.TryAddSingleton(typeof(IKeyedServiceCollection<,>), typeof(KeyedServiceCollection<,>));
            services.AddSingletonNamedService<IAccessTokenService, ClientAccessTokenService>((AccessTokenType.ClientCredentials.ToString()));
            services.AddSingletonNamedService<IAccessTokenService, UserAccessTokenService>((AccessTokenType.UserCredentials.ToString()));
            return services;
        }
        /// <summary>
        /// 根据命名空间查找程序集
        /// </summary>
        /// <param name="name">命名空间名称</param>
        /// <returns></returns>
        private static Assembly GetAssembly(string name)
        {
            AssemblyName[] allAssembly = Assembly
                 .GetEntryAssembly()//获取默认程序集
                 .GetReferencedAssemblies();//获取所有引用程序集

            foreach (var item in allAssembly)
            {
                if (item.Name.ToLower() == name.ToLower())
                    return Assembly.Load(item);
            }
            Console.Write(name + " assembly not found");
            throw new Exception(name + " assembly not found");
        }
    }
}
