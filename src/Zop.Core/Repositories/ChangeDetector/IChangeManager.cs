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
        /// <param name="type">实体类型</param>
        /// <param name="Id">唯一标示</param>
        /// <returns></returns>
        ChangeEntry GetChanger(Type type, object Id);

        /// <summary>
        /// 根据变动类型获取实体对象
        /// </summary>
        /// <param name="changeType">变更类型</param>
        /// <returns></returns>
        IList<ChangeEntry> GetChangers(ChangeEntryType changeType);
        /// <summary>
        /// 把实体变动信息添加至管理器中
        /// </summary>
        /// <param name="change">实体变动信息</param>
        void AddChanger(ChangeEntry change);
        /// <summary>
        /// 清除所有的变更信息
        /// </summary>
        void ClearChanger();
    }
}
