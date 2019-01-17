
using System.Linq;
using System.Reflection;

namespace System.Collections.Generic
{
    /// <summary>
    /// Dictionary类型扩展表
    /// </summary>
    public static class DictionaryExtention
    {
        /// <summary>
        ///  拷贝字典
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Copy(this Dictionary<string, string> source)
        {
            return source.ToDictionary(sou => sou.Key, sou => sou.Value);
        }

        /// <summary>
        /// 获取字典的值
        /// </summary>
        /// <typeparam name="T">取值类型</typeparam>
        /// <param name="dirs">键值集合</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static T GetValue<T>(this IDictionary<string, T> dirs, string key)
        {
            T t;
            dirs.TryGetValue(key, out t);
            return t;
        }
        /// <summary>
        /// 对象转换为字典 （反射）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dictionary<string, string > ObjectConvertDictionary<T>(this T obj) where T : class, new()
        {
            Dictionary<string, string> map = new Dictionary<string, string>();

            Type t = obj.GetType();

            PropertyInfo[] pi = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo p in pi)
            {
                MethodInfo mi = p.GetGetMethod();

                if (mi != null && mi.IsPublic)
                {
                    map.Add(p.Name, mi.Invoke((object)obj, new object[] { }).ToString());
                }
            }
            return map;

        }
    }
}
