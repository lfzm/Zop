using Orleans;
using System.Threading.Tasks;
using Zop.Domain.Entities;

namespace Zop.Application.Services
{
    /// <summary>
    /// 内存缓存服务（Grain）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IMemoryCacheService : IGrainWithIntegerKey, IGrainWithStringKey, IGrainWithGuidKey

    {
        /// <summary>
        /// 获取应用服务的缓存实体 (默认绝对缓存5分钟)
        /// </summary>
        /// <param name="absoluteExpiration">过期时间(单位：秒)</param>
        /// <returns></returns>
        Task<TEntity> ReadAsync<TEntity>(long absoluteExpiration = 300) where TEntity : class, IEntity, new();
    }
}
