using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 变动探测器
    /// </summary>
    public interface IChangeDetector
    {
        /// <summary>
        /// 检查变动
        /// </summary>
        /// <param name="newEntry">新实体</param>
        /// <param name="oldEntry">旧实体</param>
        /// <returns></returns>
        IChangeManager DetectChanges(object newEntry, object oldEntry);
    }
}
