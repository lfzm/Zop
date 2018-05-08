using System;
using System.Collections.Generic;
using Orleans;
using System.Text;
using System.Threading.Tasks;
using Orleans.Concurrency;

namespace Zop.Application.Services
{
    /// <summary>
    /// 这个接口必须由所有的应用服务来实现，以便按照约定标识它们。
    /// </summary>
    public interface IApplicationService<TState> : IApplicationService, IGrainWithIntegerKey, IGrainWithStringKey, IGrainWithGuidKey
         where TState : class, new()
    {
        /// <summary>
        /// 获取应用服务的状态
        /// </summary>
        /// <returns>返回状态</returns>
        [AlwaysInterleave]
        Task<TState> ReadState();

        /// <summary>
        /// 清除应用服务的状态
        /// </summary>
        /// <returns></returns>
        Task ClearState();

    }
}
