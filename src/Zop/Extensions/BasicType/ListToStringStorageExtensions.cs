using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    /// <summary>
    /// List转换为String类型进行存储扩展
    /// </summary>
    public static class ListToStringStorageExtensions
    {
        /// <summary>
        /// 转换List
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="splitStr">拆分间隔字符</param>
        /// <returns></returns>
        public static List<string> GetList(this string source, char splitStr = ';')
        {
            if (source.IsNull())
                return new List<string>();
            return source.TrimEnd(splitStr).Split(splitStr).ToList();
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="value">值</param>
        /// <param name="splitStr">拆分间隔字符</param>
        public static string SetValue(this string source, string value, char splitStr = ';')
        {
            if (value.IsNull())
                return source;
            if (source.GetList().Exists(f => f == value))
                return source;
            source += value + splitStr;
            return source;
        }
        /// <summary>
        /// 设置值 (会清除之前存储的值)
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="values">值</param>
        /// <param name="splitStr">拆分间隔字符</param>
        public static string SetValue(this string source, List<string> values, char splitStr = ';')
        {
            if (!values.Any())
                return source;
            foreach (var value in values)
            {
                source = source.SetValue(value, splitStr);
            }
            return source;
        }
        /// <summary>
        /// 覆盖之前的值
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="values">值</param>
        /// <param name="splitStr">拆分间隔字符</param>
        public static string Cover(this string source, List<string> values, char splitStr = ';')
        {
            source = "";
            source = source.SetValue(values, splitStr);
            return source;
        }
        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="value">值</param>
        /// <param name="splitStr">拆分间隔字符</param>
        public static string RemoveValue(this string source, string value, char splitStr = ';')
        {
            if (value.IsNull())
                return source;
            if (!source.GetList().Exists(f => f == value)) return source;
            source = source.Replace(value + splitStr, "");
            return source;
        }

        /// <summary>
        /// 移除值
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="values">值</param>
        /// <param name="splitStr">拆分间隔字符</param>
        public static string RemoveValue(this string source, List<string> values, char splitStr = ';')
        {
            if (!values.Any())
                return source;
            foreach (var value in values)
            {
                source= source.RemoveValue(value, splitStr);
            }
            return source;
        }
        /// <summary>
        /// 清除所有的值
        /// </summary>
        /// <param name="source">源字符串</param>
        public static string Clear(this string source)
        {
            source = "";
            return source;
        }
    }
}
