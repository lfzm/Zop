using Orleans;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans
{
    /// <summary>
    /// GrainReference 扩展类
    /// </summary>
    public static  class IAddressableExtensions
    {
        /// <summary>
        /// 获取 PrimaryKey
        /// </summary>
        /// <param name="grainReference"></param>
        /// <returns></returns>
        public static object GetPrimaryKeyObject(this IAddressable addressable)
        {
            var key = addressable.GetPrimaryKeyString();
            if (key != null)
                return key;
            if (addressable.IsPrimaryKeyBasedOnLong())
            {
                var key1 = addressable.GetPrimaryKeyLong();
                if (key1 > 0)
                    return key1;
            }
            return addressable.GetPrimaryKey();

        }
    }
}
