using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;

namespace Zop.Application.Services
{
    [StatelessWorker]
    public class MemoryCacheService<TEntity> : Grain, IMemoryCacheService<TEntity> where TEntity : class, new()
    {
        ///<inheritdoc/>
        public async Task<TEntity> ReadAsync(long absoluteExpiration = 1800)
        {
            string key = typeof(TEntity).FullName + this.GetPrimaryKeyObject();
            var cache = this.ServiceProvider.GetRequiredService<IMemoryCache>();

            TEntity cached;
            if (cache.TryGetValue(key, out cached))
                return cached;

            object primaryKey = this.GetPrimaryKeyObject();
            //前往Grain获取数据
            IApplicationService<TEntity> service;
            if (primaryKey.GetType() == typeof(long))
            {
                long _primaryKey = (long)primaryKey;
                service = this.GrainFactory.GetStateGrain<TEntity>(_primaryKey);
            }
            else if (primaryKey.GetType() == typeof(string))
            {
                string _primaryKey = (string)primaryKey;
                service = this.GrainFactory.GetStateGrain<TEntity>(_primaryKey);
            }
            else
                service = this.GrainFactory.GetStateGrain<TEntity>((Guid)primaryKey);

            cached = await service.ReadAsync();
            //存储到内存中
            cache.Set(key, cached, TimeSpan.FromSeconds(absoluteExpiration));

            return cached;
        }
    }
}
