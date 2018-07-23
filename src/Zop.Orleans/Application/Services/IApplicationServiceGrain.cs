using System;
using System.Collections.Generic;
using System.Text;
using Zop.Domain.Entities;
using Orleans;

namespace Zop.Application.Services
{
    /// <summary>
    /// 应用服务接口，基类默认实现，用于快捷访问领域
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    public interface IApplicationServiceGrain<TState> : IApplicationService<TState>, IGrainWithIntegerKey, IGrainWithStringKey, IGrainWithGuidKey
        where TState : class, IEntity, new()
    {
    }
}
