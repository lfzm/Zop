using System.ComponentModel.DataAnnotations;

namespace Zop.DTO
{
    /// <summary>
    /// 分页结果请求对象
    /// </summary>
    public class PagedRequestDto:IRequestDto
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Range(1, int.MaxValue)]
        public virtual int PageNo { get; set; } = 10;
        /// <summary>
        /// 每页数量
        /// </summary>
        [Range(0, int.MaxValue)]
        public virtual int PageSize { get; set; }
    }
}
