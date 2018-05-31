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
        public async Task<TEntity> ReadAsync<TEntity>(long absoluteExpiration = 300) where TEntity : class, IEntity, new()
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

            TEntity newCached = await service.ReadAsync();

            lock (cache)
            {
                //存储到内存中
                if (!cache.TryGetValue(key, out cached))
                {
                    cache.Set(key, newCached, TimeSpan.FromSeconds(absoluteExpiration));
                    cached = newCached;
                }
            }
            return cached;
        }
    }
}
