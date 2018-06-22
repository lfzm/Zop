using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Application.Events;

namespace Orleans
{
    /// <summary>
    /// 领域事件发布
    /// </summary>
    public static class StreamProviderEventPublishExtensions
    {
        /// <summary>
        /// 事件发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"><see cref="IStreamProvider"/></param>
        /// <param name="streamId">streamId</param>
        /// <param name="eventData">eventData</param>
        /// <returns></returns>
        public static Task PublishAsync<T>(this IStreamProvider stream, Guid streamId, T eventData) where T : IEventData
        {
            return stream.GetStream<T>(streamId, eventData.EventName).OnNextAsync(eventData);
        }

        /// <summary>
        /// 事件发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"><see cref="IStreamProvider"/></param>
        /// <param name="eventData">eventData</param>
        /// <returns></returns>
        public static Task PublishAsync<T>(this IStreamProvider stream, T eventData) where T : IEventData
        {
            return stream.PublishAsync<T>(Guid.NewGuid(), eventData);
        }

    
    }
}
