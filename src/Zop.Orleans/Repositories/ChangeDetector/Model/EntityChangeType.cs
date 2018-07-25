using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 实体变动类型
    /// </summary>
    public enum EntityChangeType
    {
        /// <summary>
        /// 修改
        /// </summary>
        Modify,
        /// <summary>
        /// 移除
        /// </summary>
        Remove,
        /// <summary>
        /// 添加
        /// </summary>
        Addition
    }
}
