using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 实体变动
    /// </summary>
    public class ChangeEntry
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntryType { get { return this.NewestEntry != null ? this.NewestEntry.GetType() : this.OriginalEntry.GetType(); } }

        /// <summary>
        /// 原来的实体
        /// </summary>
        public object OriginalEntry { get; set; }
        /// <summary>
        /// 新的实体对象
        /// </summary>
        public object NewestEntry { get; set; }
        /// <summary>
        /// 实体变动类型
        /// </summary>
        public ChangeEntryType Type { get; set; }
        /// <summary>
        /// 导航属性
        /// </summary>
        public string Navigation { get; set; }
        /// <summary>
        /// 变动属性集合
        /// </summary>
        public IList<ChangeEntryPropertys> ChangePropertys { get; } = new List<ChangeEntryPropertys>();

    }
}
