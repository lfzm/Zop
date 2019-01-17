using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zop.Extensions.DependencyInjection
{
    public class KeyedServiceCollection<TKey, TService> : IKeyedServiceCollection<TKey, TService>
         where TService : class
    {
        public TService GetService(IServiceProvider services, TKey key)
        {
            return this.GetServices(services).FirstOrDefault(s => s.Equals(key))?.GetService(services);
        }

        public IEnumerable<IKeyedService<TKey, TService>> GetServices(IServiceProvider services)
        {
            return services.GetServices<IKeyedService<TKey, TService>>();
        }
    }

}
