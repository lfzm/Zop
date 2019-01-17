using System;
using System.Collections.Generic;

namespace Zop.Extensions.DependencyInjection
{

    /// <summary>
    /// Collection of services that can be disambiguated by key
    /// </summary>
    public interface IKeyedServiceCollection<TKey, out TService>
        where TService : class
    {
        IEnumerable<IKeyedService<TKey, TService>> GetServices(IServiceProvider services);
        TService GetService(IServiceProvider services, TKey key);
    }
}
