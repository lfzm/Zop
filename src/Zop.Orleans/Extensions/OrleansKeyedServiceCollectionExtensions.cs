using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;


namespace Orleans.Runtime
{
    public static class OrleansKeyedServiceCollectionExtensions
    {
        /// <summary>
        /// Register a scoped keyed service
        /// </summary>
        public static IServiceCollection AddScopedKeyedService<TKey, TService>(this IServiceCollection collection, TKey key, Func<IServiceProvider, TKey, TService> factory)
            where TService : class
        {

            return collection.AddScoped<IKeyedService<TKey, TService>>(sp => new KeyedSingletonService<TKey, TService>(key, sp, factory));
        }

        /// <summary>
        /// Register a scoped keyed service
        /// </summary>
        public static IServiceCollection AddScopedKeyedService<TKey, TService, TInstance>(this IServiceCollection collection, TKey key)
            where TInstance : class, TService
            where TService : class
        {
            collection.TryAddTransient<TInstance>();
            return collection.AddScoped<IKeyedService<TKey, TService>>(sp => new KeyedSingletonService<TKey, TService, TInstance>(key, sp));
        }

        /// <summary>
        /// Register a scoped named service
        /// </summary>
        public static IServiceCollection AddScopedNamedService<TService>(this IServiceCollection collection, string name, Func<IServiceProvider, string, TService> factory)
            where TService : class
        {
            return collection.AddScopedKeyedService<string, TService>(name, factory);
        }

        /// <summary>
        /// Register a scoped named service
        /// </summary>
        public static IServiceCollection AddScopedNamedService<TService, TInstance>(this IServiceCollection collection, string name)
            where TInstance : class, TService
            where TService : class
        {
            return collection.AddScopedKeyedService<string, TService, TInstance>(name);
        }

    }
}
