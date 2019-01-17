using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace Zop.Extensions.DependencyInjection
{
    public static class KeyedServiceCollectionExtensions
    {
        /// <summary>
        /// Register a transient keyed service
        /// </summary>
        public static IServiceCollection AddTransientKeyedService<TKey, TService>(this IServiceCollection collection, TKey key, Func<IServiceProvider, TKey, TService> factory)
             where TService : class
        {
            collection.AddKeyedServiceCollection();
            return collection.AddSingleton<IKeyedService<TKey, TService>>(sp => new KeyedService<TKey, TService>(key, sp, factory));
        }

        /// <summary>
        /// Register a transient keyed service
        /// </summary>
        public static IServiceCollection AddTransientKeyedService<TKey, TService, TInstance>(this IServiceCollection collection, TKey key)
            where TInstance : class, TService
            where TService : class
        {
            collection.AddKeyedServiceCollection();
            collection.TryAddTransient<TInstance>();
            return collection.AddSingleton<IKeyedService<TKey, TService>>(sp => new KeyedService<TKey, TService, TInstance>(key, sp));
        }

        /// <summary>
        /// Register a singleton keyed service
        /// </summary>
        public static IServiceCollection AddSingletonKeyedService<TKey, TService>(this IServiceCollection collection, TKey key, Func<IServiceProvider, TKey, TService> factory)
            where TService : class
        {
            collection.AddKeyedServiceCollection();
            return collection.AddSingleton<IKeyedService<TKey, TService>>(sp => new KeyedSingletonService<TKey, TService>(key, sp, factory));
        }

        /// <summary>
        /// Register a singleton keyed service
        /// </summary>
        public static IServiceCollection AddSingletonKeyedService<TKey, TService, TInstance>(this IServiceCollection collection, TKey key)
            where TInstance : class, TService
            where TService : class
        {
            collection.AddKeyedServiceCollection();
            collection.TryAddTransient<TInstance>();
            return collection.AddSingleton<IKeyedService<TKey, TService>>(sp => new KeyedSingletonService<TKey, TService, TInstance>(key, sp));
        }

        /// <summary>
        /// Register a scoped keyed service
        /// </summary>
        public static IServiceCollection AddScopedKeyedService<TKey, TService>(this IServiceCollection collection, TKey key, Func<IServiceProvider, TKey, TService> factory)
            where TService : class
        {
            collection.AddKeyedServiceCollection();
            return collection.AddScoped<IKeyedService<TKey, TService>>(sp => new KeyedSingletonService<TKey, TService>(key, sp, factory));
        }

        /// <summary>
        /// Register a scoped keyed service
        /// </summary>
        public static IServiceCollection AddScopedKeyedService<TKey, TService, TInstance>(this IServiceCollection collection, TKey key)
            where TInstance : class, TService
            where TService : class
        {
            collection.AddKeyedServiceCollection();
            collection.TryAddTransient<TInstance>();
            return collection.AddScoped<IKeyedService<TKey, TService>>(sp => new KeyedSingletonService<TKey, TService, TInstance>(key, sp));
        }


        /// <summary>
        /// Register a transient named service
        /// </summary>
        public static IServiceCollection AddTransientNamedService<TService>(this IServiceCollection collection, string name, Func<IServiceProvider, string, TService> factory)
            where TService : class
        {
            return collection.AddTransientKeyedService<string, TService>(name, factory);
        }

        /// <summary>
        /// Register a transient named service
        /// </summary>
        public static IServiceCollection AddTransientNamedService<TService, TInstance>(this IServiceCollection collection, string name)
            where TInstance : class, TService
            where TService : class
        {
            return collection.AddTransientKeyedService<string, TService, TInstance>(name);
        }

        /// <summary>
        /// Register a singleton named service
        /// </summary>
        public static IServiceCollection AddSingletonNamedService<TService>(this IServiceCollection collection, string name, Func<IServiceProvider, string, TService> factory)
            where TService : class
        {
            return collection.AddSingletonKeyedService<string, TService>(name, factory);
        }

        /// <summary>
        /// Register a singleton named service
        /// </summary>
        public static IServiceCollection AddSingletonNamedService<TService, TInstance>(this IServiceCollection collection, string name)
            where TInstance : class, TService
            where TService : class
        {
            return collection.AddSingletonKeyedService<string, TService, TInstance>(name);
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

        public static IServiceCollection AddKeyedServiceCollection(this IServiceCollection collection)
        {
            if (collection.Where(f => f.ServiceType == typeof(IKeyedServiceCollection<,>)).Count() == 0)
            {
                collection.TryAddSingleton(typeof(IKeyedServiceCollection<,>), typeof(KeyedServiceCollection<,>));
            }
            return collection;


        }
    }
}
