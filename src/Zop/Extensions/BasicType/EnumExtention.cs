using System.Collections.Concurrent;
using System.Collections.Generic;
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
        /// <returns></returns>
        public static string GetDescription(this Enum en)
        {
            var type = en.GetType();
            var name = en.ToString();
            var attr = type.GetField(name).GetCustomAttributes(typeof(EnumDescriptAttribute), false);
            if (attr.Length > 0)
                return ((EnumDescriptAttribute)attr[0]).Description;
            else
            {
                attr = type.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attr == null ? name : ((DescriptionAttribute)attr[0]).Description;
            }
        }


        private static ConcurrentDictionary<string, Dictionary<string, string>> enumDirs
           = new ConcurrentDictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// 获取枚举字典列表
        /// </summary>
        /// <param name="en">枚举类型</param>
        /// <param name="isIntValue">返回枚举值是否是int类型</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDescriptionList(this Enum en, bool isIntValue = true)
        {
            var enType = en.GetType();
            string key = string.Concat(enType.FullName, isIntValue);

            Dictionary<string, string> enums = enumDirs.GetOrAdd(key, (k) =>
              {
                  var dirs = new Dictionary<string, string>();
                  var values = Enum.GetValues(enType);
                  foreach (var value in values)
                  {
                      var name = value.ToString();
                      string resultValue = isIntValue ? ((int)value).ToString() : name;

                      string description;
                      var attr = enType.GetField(name).GetCustomAttributes(typeof(EnumDescriptAttribute), false);
                      if (attr.Length > 0)
                          description = ((EnumDescriptAttribute)attr[0]).Description;
                      else
                      {
                          attr = enType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                          description = attr == null ? name : ((DescriptionAttribute)attr[0]).Description;
                      }
                      dirs.Add(resultValue, description);
                  }
                  return dirs;
              });

            if (enums != null)
                return enums.Copy();
            else
                return enums;
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
    public class EnumDescriptAttribute : Attribute
    {
        /// <summary>
        /// 自定义枚举描述构造函数
        /// </summary>
        /// <param name="description"></param>
        public EnumDescriptAttribute(string description)
        {
            this.Description = description;
        }

        /// <summary>
        ///   描述信息
        /// </summary>
        public string Description { get; set; }
    }


}
