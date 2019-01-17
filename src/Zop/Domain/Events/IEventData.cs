using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Domain.Events
{
    /// <summary>
    /// 事件数据
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// 事件发生的时间
        /// </summary>
        DateTime EventTime { get; }
        /// <summary>
        /// 事件名称
        /// </summary>
        string EventName { get; }
    }
}
