using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Extensions.DependencyInjection
{
    public interface IKeyedService<TKey, out TService> : IEquatable<TKey>
    {
        TKey Key { get; }
        TService GetService(IServiceProvider services);
    }
}
