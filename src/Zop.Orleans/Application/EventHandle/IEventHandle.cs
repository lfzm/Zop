using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Application.EventHandle
{
    /// <summary>
    /// 事件处理基类
    /// </summary>
    public interface IEventHandle : IGrainWithIntegerKey
    {
    }
}
