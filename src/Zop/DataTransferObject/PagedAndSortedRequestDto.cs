using System;

namespace Zop.DTO
{
    /// <summary>
    /// 分页排序结果请求对象
    /// </summary>
    [Serializable]
    public class PagedAndSortedRequestDto : PagedRequestDto
    {
        /// <summary>
        /// Sorting information.
        /// Should include sorting field and optionally a direction (ASC or DESC)
        /// Can contain more than one field separated by comma (,).
        /// </summary>
        /// <example>
        /// Examples:
        /// "Name"
        /// "Name DESC"
        /// "Name ASC, Age DESC"
        /// </example>
        public virtual string Sorting { get; set; }
    }
}
