using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace Zop.DTO
{
    /// <summary>
    /// 验证帮助根据扩展类
    /// </summary>
    public static class ValidationHelperExtensions
    {
        /// <summary>
        /// 验证IP4格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsIP4(this string value)
        {
            return IP4Attribute.Verify(value);
        }


        /// <summary>
        /// 验证域名格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDomainUrl(this string value)
        {
           string _regex = @"^[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+$";
            Regex reg = new Regex(_regex);
            if (!reg.IsMatch(value))
            {
                if (value == "localhost")
                    return true;
                else
                    return false;
            }
            else
                return true;
        }

        /// <summary>
        /// 验证URL格式
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsUrl(this string value)
        {
            string _regex = @"^((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?$";
            Regex reg = new Regex(_regex);
            if (!reg.IsMatch(value))
                return false;
            else
                return true;
        }
            
    }
}
