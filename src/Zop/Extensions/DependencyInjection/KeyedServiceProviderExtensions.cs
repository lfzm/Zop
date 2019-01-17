using System;
using System.Collections.Generic;

namespace Zop.Extensions.DependencyInjection
{
    public static class KeyedServiceProviderExtensions
    {
        /// <summary>
        /// Acquire a service by key.
        /// </summary>
        public static TService GetServiceByKey<TKey, TService>(this IServiceProvider services, TKey key)
            where TService : class
        {
            IKeyedServiceCollection<TKey, TService> collection = (IKeyedServiceCollection<TKey, TService>)services.GetService(typeof(IKeyedServiceCollection<TKey, TService>));
            return collection?.GetService(services, key);
        }

        /// <summary>
        /// Acquire a service by key.  Throws KeyNotFound exception if key is not found.
        /// </summary>
        public static TService GetRequiredServiceByKey<TKey, TService>(this IServiceProvider services, TKey key)
            where TService : class
        {
            return services.GetServiceByKey<TKey, TService>(key) ?? throw new KeyNotFoundException(key?.ToString());
        }

        /// <summary>
        /// Acquire a service by name.
        /// </summary>
        public static TService GetServiceByName<TService>(this IServiceProvider services, string name)
            where TService : class
        {
            return services.GetServiceByKey<string, TService>(name);
        }

        /// <summary>
        /// Acquire a service by name.  Throws KeyNotFound exception if name is not found.
        /// </summary>
        public static TService GetRequiredServiceByName<TService>(this IServiceProvider services, string name)
            where TService : class
        {
            return services.GetRequiredServiceByKey<string, TService>(name);
        }
    }
}
