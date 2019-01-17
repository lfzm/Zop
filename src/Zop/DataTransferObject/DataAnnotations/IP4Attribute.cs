using System.Text.RegularExpressions;

namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// IP4
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class IP4Attribute : RegularExpressionAttribute
    {
        /// <summary>
        /// Ip 正则表达式
        /// </summary>
        static string _ipregex = "^(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])\\.(\\d{1,2}|1\\d\\d|2[0-4]\\d|25[0-5])$";//Ip
        public IP4Attribute() : base(_ipregex)
        {
            base.ErrorMessage = "IP4格式不正确";
        }

        /// <summary>
        /// 验证Ip4格式
        /// </summary>
        /// <param name="ip4"></param>
        /// <returns></returns>
        public static bool Verify(string ip4)
        {
            Regex r = new Regex(_ipregex);
            return r.IsMatch(ip4);
        }
    }
}
