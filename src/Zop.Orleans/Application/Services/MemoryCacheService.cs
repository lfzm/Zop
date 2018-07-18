using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;
using Zop.Domain.Entities;

namespace Zop.Application.Services
{
    [StatelessWorker]
    public class MemoryCacheService : Grain, IMemoryCacheService
    {
        ///<inheritdoc/>
        public async Task<TEntity> ReadAsync<TEntity>(object grinaKey) where TEntity : class, IEntity,IEntityCache, new()
        {
            string key = typeof(TEntity).FullName + this.GetPrimaryKeyObject();
            var cache = this.ServiceProvider.GetRequiredService<IMemoryCache>();

            TEntity cached;
            if (cache.TryGetValue(key, out cached))
                return cached;

            //前往Grain获取数据
            IApplicationService<TEntity> service;
            if (grinaKey.GetType() == typeof(long))
            {
                long _primaryKey = (long)grinaKey;
                service = this.GrainFactory.GetStateGrain<TEntity>(_primaryKey);
            }
            else if (grinaKey.GetType() == typeof(string))
            {
                string _primaryKey = (string)grinaKey;
                service = this.GrainFactory.GetStateGrain<TEntity>(_primaryKey);
            }
            else
                service = this.GrainFactory.GetStateGrain<TEntity>((Guid)grinaKey);

            TEntity newCached = await service.ReadAsync();
            if (newCached == null)
                return null;
            lock (cache)
            {
                //存储到内存中
                if (!cache.TryGetValue(key, out cached))
                {
                    cache.Set(key, newCached, newCached.Expiration());
                    cached = newCached;
                }
            }
            return cached;
        }
    }
}
