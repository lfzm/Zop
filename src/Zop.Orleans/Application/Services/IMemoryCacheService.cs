using Orleans;
using System.Threading.Tasks;

namespace Zop.Application.Services
{
    /// <summary>
    /// 内存缓存服务（Grain）
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IMemoryCacheService<TEntity> : IGrainWithIntegerKey,IGrainWithStringKey,IGrainWithGuidKey
          where TEntity : class, new()
    {
        /// <summary>
        /// 获取应用服务的缓存实体 (默认绝对缓存30分钟)
        /// </summary>
        /// <param name="absoluteExpiration">过期时间(单位：秒)</param>
        /// <returns></returns>
        Task<TEntity> ReadAsync(long absoluteExpiration= 1800);
    }
}
