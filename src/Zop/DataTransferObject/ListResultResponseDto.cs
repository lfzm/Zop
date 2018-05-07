using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.DTO
{
    /// <summary>
    /// List结果输出数据对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ListResultResponseDto<T> : ResultResponseDto
    {
        /// <summary>
        /// List of items.
        /// </summary>
        public IReadOnlyList<T> Items
        {
            get { return _items ?? (_items = new List<T>()); }
            set { _items = value; }
        }
        private IReadOnlyList<T> _items;

        /// <summary>
        /// Creates a new <see cref="ListResultResponseDto{T}"/> object.
        /// </summary>
        public ListResultResponseDto()
        {

        }

        /// <summary>
        /// Creates a new <see cref="ListResultResponseDto{T}"/> object.
        /// </summary>
        /// <param name="items">List of items</param>
        public ListResultResponseDto(IReadOnlyList<T> items)
        {
            Items = items;
        }
    }
}
