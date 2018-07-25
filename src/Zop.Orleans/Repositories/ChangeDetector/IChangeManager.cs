using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 变动管理器
    /// </summary>
    public interface IChangeManager
    {
        /// <summary>
        /// 根据实体类型和唯一标示获取实体变动信息对象
        /// </summary>
        /// <returns></returns>
        EntityChange GetChange();

        /// <summary>
        /// 根据变动类型获取实体对象
        /// </summary>
        /// <param name="changeType">变更类型</param>
        /// <returns></returns>
        IList<EntityDifference> GetDifferences(EntityChangeType changeType);

        /// <summary>
        /// 获取实体的差异
        /// </summary>
        /// <param name="entityHashCode">当前实体的哈希值</param>
        /// <returns></returns>
        EntityDifference GetDifference(int entityHashCode);

    }
}
