using System.ComponentModel.DataAnnotations;

namespace Zop.DTO
{
    /// <summary>
    /// 分页结果请求对象
    /// </summary>
    public class PagedResultRequestDto:RequestDto
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int PagedCount { get; set; } = 10;
        /// <summary>
        /// 获取数量
        /// </summary>
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
    }
}
