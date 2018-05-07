using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;

namespace Zop.Extensions.DependencyInjection
{
    public class KeyedService<TKey, TService> : IKeyedService<TKey, TService>
        where TService : class
    {
        private readonly Func<IServiceProvider, TKey, TService> factory;

        public TKey Key { get; }

        public KeyedService(TKey key, IServiceProvider services, Func<IServiceProvider, TKey, TService> factory)
        {
            this.Key = key;
            this.factory = factory;
        }

        public TService GetService(IServiceProvider services) => factory(services, Key);

        public bool Equals(TKey other)
        {
            return Equals(Key, other);
        }
    }

    public class KeyedService<TKey, TService, TInstance> : KeyedService<TKey, TService>
        where TInstance : TService
        where TService : class
    {
        public KeyedService(TKey key, IServiceProvider services)
            : base(key, services, (sp, k) => sp.GetService<TInstance>())
        {
        }
    }
}
