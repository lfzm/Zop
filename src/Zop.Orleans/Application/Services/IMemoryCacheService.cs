using Orleans;
using System.Threading.Tasks;
using Zop.Domain.Entities;

namespace Zop.Application.Services
{
    /// <summary>
    /// 内存缓存服务（Grain）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IMemoryCacheService : IGrainWithGuidKey
    {
        /// <summary>
        /// 获取应用服务的缓存实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="grainKey">GrainKey</param>
        /// <returns></returns>
        Task<TEntity> ReadAsync<TEntity>(object grainKey) where TEntity : class, IEntity, IEntityCache, new();
    }
}
