using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zop.Toolkit.IDGenerator
{
    /// <summary>
    /// ID生成器
    /// </summary>
    public interface IIDGenerated
    {
        /// <summary>
        /// 生成ID
        /// </summary>
        /// <returns></returns>
        Task<long> NextIdAsync();
        /// <summary>
        /// 生成ID
        /// </summary>
        /// <returns></returns>
        long NextId();
    }
}
