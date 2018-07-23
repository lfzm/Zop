using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Application.Events
{
    public abstract class EventData : IEventData
    {
        public DateTime EventTime { get; } = DateTime.Now;

        public abstract string EventName { get; }
    }
}
