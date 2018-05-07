#region Copyright (C) 2017 AL系列开源项目

/*       
　　	文件功能描述：String类型扩展类

　　	创建人：阿凌
        创建人Email：513845034@qq.com

*/
#endregion
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// String类型扩展类
    /// </summary>
    public static class StringExtention
    {
        /// <summary>
        /// 获取字符长度
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetStringLength(this string s)
        {
            return string.IsNullOrEmpty(s) ? 0 : Encoding.Default.GetBytes(s).Length;
        }
        /// <summary>
        /// 字符串拆分
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="splitStr">拆分间隔字符</param>
        /// <returns></returns>
        public static string[] SplitString(this string sourceStr, string splitStr)
        {
            string[] strArray;
            if ((string.IsNullOrEmpty(sourceStr) ? 0 : (!string.IsNullOrEmpty(splitStr) ? 1 : 0)) == 0)
                strArray = new string[0];
            else if (sourceStr.IndexOf(splitStr) == -1)
                strArray = new string[1]
                {
          sourceStr
                };
            else if (splitStr.Length == 1)
                strArray = sourceStr.Split(splitStr[0]);
            else
                strArray = Regex.Split(sourceStr, Regex.Escape(splitStr), RegexOptions.IgnoreCase);
            return strArray;
        }
        /// <summary>
        /// 字符串拆分
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        public static string[] SplitString(this string sourceStr)
        {
            return StringExtention.SplitString(sourceStr, ",");
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string SubString(this string sourceStr, int startIndex, int length)
        {
            return string.IsNullOrEmpty(sourceStr) ? "" : (sourceStr.Length < startIndex + length ? sourceStr.Substring(startIndex) : sourceStr.Substring(startIndex, length));
        }
        /// <summary> 
        /// 截取字符串 默认0开始
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="length">截取长度</param>
        public static string SubString(this string sourceStr, int length)
        {
            return StringExtention.SubString(sourceStr, 0, length);
        }
        /// <summary> 
        /// 从尾部截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="length">截取长度</param>
        public static string SubStringEnd(this string sourceStr, int length)
        {
            if (sourceStr.Length <= length)
                return sourceStr;
            return StringExtention.SubString(sourceStr, sourceStr.Length - length, length);
        }
        /// <summary>
        /// 删除头部字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">删除的字符</param>
        /// <returns></returns>
        public static string TrimStart(this string sourceStr, string trimStr)
        {
            return StringExtention.TrimStart(sourceStr, trimStr, true);
        }
        /// <summary>
        /// 删除头部字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">删除的字符</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string TrimStart(this string sourceStr, string trimStr, bool ignoreCase)
        {
            return !string.IsNullOrEmpty(sourceStr) ? ((string.IsNullOrEmpty(trimStr) ? 0 : (sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture) ? 1 : 0)) != 0 ? sourceStr.Remove(0, trimStr.Length) : sourceStr) : string.Empty;
        }
        /// <summary>
        /// 删除尾部字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">删除的字符</param>
        /// <returns></returns>
        public static string TrimEnd(this string sourceStr, string trimStr)
        {
            return StringExtention.TrimEnd(sourceStr, trimStr, true);
        }
        /// <summary>
        /// 删除尾部字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">删除的字符</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string TrimEnd(this string sourceStr, string trimStr, bool ignoreCase)
        {
            return !string.IsNullOrEmpty(sourceStr) ? ((string.IsNullOrEmpty(trimStr) ? 0 : (sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture) ? 1 : 0)) != 0 ? sourceStr.Substring(0, sourceStr.Length - trimStr.Length) : sourceStr) : string.Empty;
        }
        /// <summary>
        /// 删除字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">删除的字符</param>
        /// <returns></returns>
        public static string Trim(this string sourceStr, string trimStr)
        {
            return StringExtention.Trim(sourceStr, trimStr, true);
        }
        /// <summary>
        /// 删除字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="trimStr">删除的字符</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static string Trim(this string sourceStr, string trimStr, bool ignoreCase)
        {
            string str;
            if (string.IsNullOrEmpty(sourceStr))
                str = string.Empty;
            else if (string.IsNullOrEmpty(trimStr))
            {
                str = sourceStr;
            }
            else
            {
                if (sourceStr.StartsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                    sourceStr = sourceStr.Remove(0, trimStr.Length);
                if (sourceStr.EndsWith(trimStr, ignoreCase, CultureInfo.CurrentCulture))
                    sourceStr = sourceStr.Substring(0, sourceStr.Length - trimStr.Length);
                str = sourceStr;
            }
            return str;
        }
        /// <summary>
        /// 字符串替换
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="newStr">新字符串</param>
        /// <param name="startIndex">开始位置</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string Replace(this string sourceStr, string newStr, int startIndex, int length)
        {
            string oldStr = sourceStr.SubString(startIndex,length);
            return sourceStr.Replace(oldStr, newStr);
        }

        /// <summary>
        /// 字符串转化成 UInt32
        /// </summary>
        /// <param name="obj">要转化的值</param>
        /// <param name="defaultValue">如果转化失败，返回的默认值</param>
        /// <returns></returns>
        public static UInt32 ToUInt32(this string obj, UInt32 defaultValue = 0)
        {
            try
            {
                UInt32 returnValue = 0;
                if (UInt32.TryParse(obj, out returnValue))
                {
                    return returnValue;
                }
            }
            catch (Exception)
            {

            }
            return defaultValue;
        }
        /// <summary>
        /// 字符串转化成 Int32
        /// </summary>
        /// <param name="obj">要转化的值</param>
        /// <param name="defaultValue">如果转化失败，返回的默认值</param>
        /// <returns></returns>
        public static int ToInt32(this string obj, int defaultValue = 0)
        {
            try
            {
                int returnValue = 0;
                if (int.TryParse(obj, out returnValue))
                {
                    return returnValue;
                }
            }
            catch (Exception)
            {

            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转化成 long
        /// </summary>
        /// <param name="obj">要转化的值</param>
        /// <param name="defaultValue">如果转化失败，返回的默认值</param>
        /// <returns></returns>
        public static long ToInt64(this string obj, int defaultValue = 0)
        {
            try
            {
                long returnValue = 0;
                if (long.TryParse(obj, out returnValue))
                {
                    return returnValue;
                }
            }
            catch (Exception)
            {

            }
            return defaultValue;
        }
        /// <summary>
        /// 字符串转化成 Decimal
        /// </summary>
        /// <param name="obj">要转化的值</param>
        /// <param name="defaultValue">如果转化失败，返回的默认值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string obj, decimal defaultValue = 0)
        {
            try
            {
                decimal returnValue = 0;
                if (decimal.TryParse(obj, out returnValue))
                {
                    return returnValue;
                }
            }
            catch (Exception)
            {

            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转化成 Double
        /// </summary>
        /// <param name="obj">要转化的值</param>
        /// <param name="defaultValue">如果转化失败，返回的默认值</param>
        /// <returns></returns>
        public static double ToDouble(this string obj, double defaultValue = 0)
        {
            try
            {
                double returnValue = 0;
                if (double.TryParse(obj, out returnValue))
                {
                    return returnValue;
                }
            }
            catch (Exception)
            {

            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转化成 float
        /// </summary>
        /// <param name="obj">要转化的值</param>
        /// <param name="defaultValue">如果转化失败，返回的默认值</param>
        /// <returns></returns>
        public static float ToFloat(this string obj, float defaultValue = 0)
        {
            try
            {
                float returnValue = 0;
                if (float.TryParse(obj, out returnValue))
                {
                    return returnValue;
                }
            }
            catch (Exception)
            {

            }
            return defaultValue;
        }

        /// <summary>
        /// 字符串转化成 DateTime
        /// </summary>
        /// <param name="obj">要转化的值</param>
        /// <returns>返回转换后值 可空</returns>
        public static DateTime? ToDateTime(this string obj)
        {
            if (string.IsNullOrEmpty(obj))
            {
                return null;
            }
            DateTime date;
            return DateTime.TryParse(obj, out date) ? date : default(DateTime?);
        }
        /// <summary>
        /// 字符串转化成 DateTime
        /// </summary>
        /// <param name="obj">要转化的值</param>
        /// <param name="defaultValue">如果转化失败，返回的默认值</param>
        /// <returns>返回转换后值 可空</returns>
        public static DateTime ToDateTime(this string s, DateTime defaultValue)
        {
            DateTime result;
            return string.IsNullOrWhiteSpace(s) || !DateTime.TryParse(s, out result) ? defaultValue : result;
        }
        /// <summary>
        /// 转化成布尔类型
        /// </summary>
        /// <param name="str">要转化的值</param>
        /// <returns></returns>
        public static bool ToBoolean(this string str)
        {
            bool isOkay = false;
            Boolean.TryParse(str, out isOkay);
            return isOkay;
        }
        /// <summary>
        /// 根据指定编码转化成对应的64位编码
        /// </summary>
        /// <param name="source">要转化的值</param>
        /// <param name="encoding">编码类型</param>
        /// <returns></returns>
        public static string ToBase64(this string source, Encoding encoding)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentNullException("source", "转化Base64字符串不能为空");
            }
            byte[] bytes = encoding.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }
        /// <summary>
        ///  从base64编码解码出正常的值
        /// </summary>
        /// <param name="baseString">base64编码</param>
        /// <param name="encoding">编码类型</param>
        /// <returns></returns>
        public static string FromBase64(this string baseString, Encoding encoding)
        {
            if (string.IsNullOrEmpty(baseString))
            {
                throw new ArgumentNullException("baseString", "解码Base64字符串不能为空");
            }
            byte[] bytes = Convert.FromBase64String(baseString);
            return encoding.GetString(bytes);
        }
        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="sourceStr"></param>
        /// <returns></returns>
        public static bool IsNull(this string sourceStr)
        {
           return string.IsNullOrEmpty(sourceStr);
        }

        public static int IndexOf(this string sourceStr, int order)
        {
            return StringExtention.IndexOf(sourceStr, '-', order);
        }

        public static int IndexOf(this string sourceStr, char c, int order)
        {
            int length = sourceStr.Length;
            int num;
            for (int index = 0; index < length; ++index)
            {
                if ((int)c == (int)sourceStr[index])
                {
                    if (order != 1)
                    {
                        --order;
                    }
                    else
                    {
                        num = index;
                        goto label_8;
                    }
                }
            }
            num = -1;
            label_8:
            return num;
        }

    }
}
