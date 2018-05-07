using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.DTO
{
    /// <summary>
    /// 分页结果输出对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PagedResultResponseDto<T> : ListResultResponseDto<T>
    {
        /// <summary>
        /// Creates a new <see cref="PagedResultResponseDto{T}"/> object.
        /// </summary>
        public PagedResultResponseDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="PagedResultResponseDto{T}"/> object.
        /// </summary>
        /// <param name="totalCount">Total count of Items</param>
        /// <param name="items">List of items in current page</param>
        public PagedResultResponseDto(int totalCount, IReadOnlyList<T> items)
            : base(items)
        {
            TotalCount = totalCount;
        }

        /// <summary>
        /// Total count of Items.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
