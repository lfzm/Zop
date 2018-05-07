using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 实体属性变动对象
    /// </summary>
    public class ChangeEntryPropertys
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 原来的值
        /// </summary>
        public object OriginalValue { get; set; }
        /// <summary>
        /// 新的值
        /// </summary>
        public object NewestValue { get; set; }
    }
}
