#region Copyright (C) 2017 AL系列开源项目

/***************************************************************************
*　　	文件功能描述：枚举扩展方法类
*
*　　	创建人： 阿凌
*       创建人Email：513845034@qq.com
*       
*****************************************************************************/
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.ComponentModel;

namespace System
{
    /// <summary>
    /// 枚举扩展方法类
    /// </summary>
    public static class EnumExtention
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="en">枚举对象</param>
        /// <param name="isFlag">是否二进制取和</param>
        /// <param name="operate">多个描述分隔符</param>
        /// <returns></returns>
        public static string GetDesp(this Enum en, bool isFlag, string operate)
        {
            var values = en.GetType().ToEnumDirs();
            int current = (int)Enum.Parse(en.GetType(), en.ToString());

            if (isFlag)
            {
                StringBuilder strResult = new StringBuilder();
                foreach (var value in values)
                {
                    int tempKey = value.Key.ToInt32();
                    if ((tempKey & current) == tempKey)
                    {
                        if (strResult.Length != 0)
                        {
                            strResult.Append(operate);
                        }
                        strResult.Append(value.Value);
                    }
                }
                return strResult.ToString();
            }
            KeyValuePair<string, string> keypair = values.FirstOrDefault(e => e.Key == current.ToString());
            return keypair.Key == null ? "不存在的枚举值" : keypair.Value;
        }
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <returns></returns>
        public static string GetDesp(this Enum en)
        {
            return en.GetDesp(false, ",");
        }

        private static ConcurrentDictionary<string, Dictionary<string, string>> enumDirs
           = new ConcurrentDictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 获取枚举字典列表
        /// </summary>
        /// <param name="enType">枚举类型</param>
        /// <param name="isIntValue">返回枚举值是否是int类型</param>
        /// <returns></returns>
        public static Dictionary<string, string> ToEnumDirs(this Type enType, bool isIntValue = true)
        {
            if (!enType.IsEnum)
                throw new ArgumentException("获取枚举字典，参数必须是枚举类型！");

            string key = string.Concat(enType.FullName, isIntValue);
            Dictionary<string, string> dirs;
            enumDirs.TryGetValue(key, out dirs);

            if (dirs != null)
                return dirs.Copy();

            dirs = new Dictionary<string, string>();
            var values = Enum.GetValues(enType);

            foreach (var value in values)
            {
                var name = Enum.GetName(enType, value);
                string resultValue = isIntValue ? ((int)value).ToString() : value.ToString();

                var attr = enType.GetField(name).GetCustomAttributes(typeof(ALDescriptAttribute), false);
                if (attr.Length == 0)
                {
                    attr = enType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                }
                dirs.Add(resultValue, attr == null ? name : ((ALDescriptAttribute)attr[0]).Description);
            }
            enumDirs.TryAdd(key, dirs);
            return dirs.Copy();
        }
        /// <summary>
        /// 枚举直接转换
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="en"></param>
        /// <returns></returns>
        public static TEnum Parse<TEnum>(this Enum en)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), en.ToString());
        }
    }

    /// <summary>
    /// 自定义枚举描述属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ALDescriptAttribute : Attribute
    {
        /// <summary>
        /// 自定义枚举描述构造函数
        /// </summary>
        /// <param name="description"></param>
        public ALDescriptAttribute(string description)
        {
            this.Description = description;
        }

        /// <summary>
        ///   描述信息
        /// </summary>
        public string Description { get; set; }
    }


}
