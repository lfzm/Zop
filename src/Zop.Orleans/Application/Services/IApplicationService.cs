using Orleans.Concurrency;
using System.Threading.Tasks;
using Zop.Domain.Entities;

namespace Zop.Application.Services
{
    /// <summary>
    /// 这个接口必须由所有的应用服务来实现，以便按照约定标识它们。
    /// </summary>
    public interface IApplicationService<TState> : IApplicationService
         where TState : class, IEntity, new()
    {
        /// <summary>
        /// 获取应用服务的状态
        /// </summary>
        /// <returns>返回状态</returns>
        [AlwaysInterleave]
        Task<TState> ReadAsync();

        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        Task AddAsync(TState state);

        /// <summary>
        /// 清除应用服务的状态
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();
    }
}
