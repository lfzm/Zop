using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Domain.Entities
{
    public interface IEntityCache
    {
        /// <summary>
        /// 缓存过期时间（秒）
        /// </summary>
        /// <returns></returns>
        TimeSpan Expiration();
    }
}
