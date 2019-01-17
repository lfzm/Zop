using Newtonsoft.Json;
using System;

namespace Microsoft.AspNetCore.Http
{
    public static class SesstionExtension
    {
        /// <summary>
        /// 获取Sesstion的对象
        /// </summary>
        /// <typeparam name="T">获取类型</typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="key">key</param>
        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            if (string.IsNullOrEmpty(value))
                return default(T);

            return JsonConvert.DeserializeObject<T>(value);
        }
        /// <summary>
        /// 获取Sesstion的对象 (默认取对象的名称作为Key)
        /// </summary>
        /// <typeparam name="T">获取类型</typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        public static T Get<T>(this ISession session)
        {
            return session.Get<T>(typeof(T).FullName);
        }

        /// <summary>
        /// 设置Sesstion的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public static void Set<T>(this ISession session, string key, T value)
        {
            if (value == null)
                throw new ArgumentNullException("值不能为空");
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 设置Sesstion的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"><see cref="ISession"/></param>
        /// <param name="value">value</param>
        public static void Set<T>(this ISession session, T value)
        {
            session.Set(typeof(T).FullName, value);
        }
    }
}
