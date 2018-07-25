using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 状态管理器工厂接口
    /// </summary>
    public interface IChangeManagerFactory
    {
        /// <summary>
        /// 创建状态管理器
        /// </summary>
        /// <param name="comparisonResult">比较结果</param>
        /// <returns></returns>
        IChangeManager Create(EntityChange entityChange, ComparisonResult comparisonResult);
    }
}
