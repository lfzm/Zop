using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Extensions.Security
{
    /// <summary>
    /// Base64 8Bit字节码的编码方式
    /// </summary>
    public static class Base64
    {
        private static readonly char[] TwoPads = { '=', '=' };
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string Encode(byte[] bytes)
        {
            return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="encoded">编码字符串</param>
        /// <returns></returns>
        public static byte[] Decode(string encoded)
        {
            var chars = new List<char>(encoded.ToCharArray());

            for (int i = 0; i < chars.Count; ++i)
            {
                if (chars[i] == '_')
                {
                    chars[i] = '/';
                }
                else if (chars[i] == '-')
                {
                    chars[i] = '+';
                }
            }

            switch (encoded.Length % 4)
            {
                case 2:
                    chars.Add('=');
                    chars.AddRange(TwoPads);
                    break;
                case 3:
                    chars.Add('=');
                    break;
            }

            var array = chars.ToArray();

            return Convert.FromBase64CharArray(array, 0, array.Length);
        }
    }
}
