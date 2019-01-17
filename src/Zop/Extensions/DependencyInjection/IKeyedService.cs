using System;

namespace Zop.Extensions.DependencyInjection
{
    public interface IKeyedService<TKey, out TService> : IEquatable<TKey>
    {
        TKey Key { get; }
        TService GetService(IServiceProvider services);
    }
}
