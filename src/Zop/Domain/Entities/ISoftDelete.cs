using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 软商城
    /// </summary>
    public interface ISoftDelete
    {
        /// <summary>
        /// 实体是否删除
        /// </summary>
        bool IsDeleted { get; set; }
    }
}
