using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Toolkit.IDGenerator
{
    /// <summary>
    /// ID生成配置服务(分布式的使用)
    /// </summary>
    public interface IWorkerOpation
    {
        /// <summary>
        /// 获取机器码
        /// </summary>
        /// <returns></returns>
        long GetWorkerId();

        /// <summary>
        /// 获取数据中心ID
        /// </summary>
        /// <returns></returns>
        long GetDatacenterId();
    }
}
