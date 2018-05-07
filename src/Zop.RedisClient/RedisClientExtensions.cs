using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Zop.RedisClient
{
    /// <summary>
    /// RedisClient扩展类
    /// </summary>
    public static class RedisClientExtensions
    {
        /// <summary>
        /// Redis Key的前缀
        /// </summary>
        public static string KeyPrefix { private get; set; }

        #region String

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static bool StringSet(this IDatabase db, string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            return db.StringSet(key, value, expiry);
        }
        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool StringSet<T>(this IDatabase db, string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return db.StringSet(key, json, expiry);
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static bool StringSet(this IDatabase db, List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(p.Key), p.Value)).ToList();
            return db.StringSet(newkeyValues.ToArray());
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="key">Redis Key</param>
        /// <returns></returns>
        public static string StringGet(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            return db.StringGet(key);
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public static RedisValue[] StringGet(this IDatabase db, List<string> listKey)
        {
            List<string> newKeys = listKey.Select(AddSysCustomKey).ToList();
            return db.StringGet(ConvertRedisKeys(newKeys));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static T StringGet<T>(this IDatabase db, string key)
        {
            if (db.KeyExists(key))
            {
                key = AddSysCustomKey(key);
                return ConvertObj<T>(db.StringGet(key));
            }
            else return default(T);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static double StringIncrement(this IDatabase db, string key, double val = 1)
        {
            key = AddSysCustomKey(key);
            return db.StringIncrement(key, val);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static double StringDecrement(this IDatabase db, string key, double val = 1)
        {
            key = AddSysCustomKey(key);
            return db.StringDecrement(key, val);
        }

        /// <summary>
        /// 保存单个key value
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">Redis Key</param>
        /// <param name="value">保存的值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public static Task<bool> StringSetAsync(this IDatabase db, string key, string value, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            return db.StringSetAsync(key, value, expiry);
        }

        /// <summary>
        /// 保存多个key value
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="keyValues">键值对</param>
        /// <returns></returns>
        public static Task<bool> StringSetAsync(this IDatabase db, List<KeyValuePair<RedisKey, RedisValue>> keyValues)
        {
            List<KeyValuePair<RedisKey, RedisValue>> newkeyValues =
                keyValues.Select(p => new KeyValuePair<RedisKey, RedisValue>(AddSysCustomKey(p.Key), p.Value)).ToList();
            return db.StringSetAsync(newkeyValues.ToArray());
        }

        /// <summary>
        /// 保存一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="obj"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static Task<bool> StringSetAsync<T>(this IDatabase db, string key, T obj, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(obj);
            return db.StringSetAsync(key, json, expiry);
        }

        /// <summary>
        /// 获取单个key的值
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<string> StringGetAsync(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            return await db.StringGetAsync(key);
        }

        /// <summary>
        /// 获取多个Key
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="listKey">Redis Key集合</param>
        /// <returns></returns>
        public static async Task<RedisValue[]> StringGetAsync(this IDatabase db, List<string> listKey)
        {
            List<string> newKeys = listKey.Select(AddSysCustomKey).ToList();
            return await db.StringGetAsync(ConvertRedisKeys(newKeys));
        }

        /// <summary>
        /// 获取一个key的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<T> StringGetAsync<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            string result = await db.StringGetAsync(key);
            if (string.IsNullOrEmpty(result))
                return default(T);
            return ConvertObj<T>(result);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static Task<double> StringIncrementAsync(this IDatabase db, string key, double val = 1)
        {
            key = AddSysCustomKey(key);
            return db.StringIncrementAsync(key, val);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static async Task<double> StringDecrementAsync(this IDatabase db, string key, double val = 1)
        {
            key = AddSysCustomKey(key);
            return await db.StringDecrementAsync(key, val);
        }

        #endregion String

        #region Hash
        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool HashExists(this IDatabase db, string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return db.HashExists(key, dataKey);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool HashSet<T>(this IDatabase db, string key, string dataKey, T t)
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(t);
            return db.HashSet(key, dataKey, json);
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static bool HashDelete(this IDatabase db, string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return db.HashDelete(key, dataKey);
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public static long HashDelete(this IDatabase db, string key, List<RedisValue> dataKeys)
        {
            key = AddSysCustomKey(key);
            return db.HashDelete(key, dataKeys.ToArray());
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static T HashGet<T>(this IDatabase db, string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            string value = db.HashGet(key, dataKey);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static double HashIncrement(this IDatabase db, string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return db.HashIncrement(key, dataKey, val);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static double HashDecrement(this IDatabase db, string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return db.HashDecrement(key, dataKey, val);
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static List<T> HashKeys<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            RedisValue[] values = db.HashKeys(key);
            return ConvetList<T>(values);
        }


        /// <summary>
        /// 判断某个数据是否已经被缓存
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<bool> HashExistsAsync(this IDatabase db, string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return await db.HashExistsAsync(key, dataKey);
        }

        /// <summary>
        /// 存储数据到hash表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Task<bool> HashSetAsync<T>(this IDatabase db, string key, string dataKey, T t)
        {
            key = AddSysCustomKey(key);
            string json = ConvertJson(t);
            return db.HashSetAsync(key, dataKey, json);
        }

        /// <summary>
        /// 移除hash中的某值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static Task<bool> HashDeleteAsync(this IDatabase db, string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            return db.HashDeleteAsync(key, dataKey);
        }

        /// <summary>
        /// 移除hash中的多个值
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKeys"></param>
        /// <returns></returns>
        public static Task<long> HashDeleteAsync(this IDatabase db, string key, List<RedisValue> dataKeys)
        {
            key = AddSysCustomKey(key);
            return db.HashDeleteAsync(key, dataKeys.ToArray());
        }

        /// <summary>
        /// 从hash表获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <returns></returns>
        public static async Task<T> HashGeAsync<T>(this IDatabase db, string key, string dataKey)
        {
            key = AddSysCustomKey(key);
            string value = await db.HashGetAsync(key, dataKey);

            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 为数字增长val
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>增长后的值</returns>
        public static async Task<double> HashIncrementAsync(this IDatabase db, string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return await db.HashIncrementAsync(key, dataKey, val);
        }

        /// <summary>
        /// 为数字减少val
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="dataKey"></param>
        /// <param name="val">可以为负</param>
        /// <returns>减少后的值</returns>
        public static Task<double> HashDecrementAsync(this IDatabase db, string key, string dataKey, double val = 1)
        {
            key = AddSysCustomKey(key);
            return db.HashDecrementAsync(key, dataKey, val);
        }

        /// <summary>
        /// 获取hashkey所有Redis key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<List<T>> HashKeysAsync<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            RedisValue[] values = await db.HashKeysAsync(key);
            return ConvetList<T>(values);
        }


        #endregion Hash

        #region List
        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value"></param>
        public static void ListRemove<T>(this IDatabase db, string key, T value)
        {
            key = AddSysCustomKey(key);
            db.ListRemove(key, ConvertJson(value));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static List<T> ListRange<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            var values = db.ListRange(key);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value"></param>
        public static void ListRightPush<T>(this IDatabase db, string key, T value)
        {
            key = AddSysCustomKey(key);
            db.ListRightPush(key, ConvertJson(value));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static T ListRightPop<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            var value = db.ListRightPop(key);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value"></param>
        public static void ListLeftPush<T>(this IDatabase db, string key, T value)
        {
            key = AddSysCustomKey(key);
            db.ListLeftPush(key, ConvertJson(value));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static T ListLeftPop<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            var value = db.ListLeftPop(key);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static long ListLength(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            return db.ListLength(key);
        }


        /// <summary>
        /// 移除指定ListId的内部List的值
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value"></param>
        public static async Task<long> ListRemoveAsync<T>(this IDatabase db, string key, T value)
        {
            key = AddSysCustomKey(key);
            return await db.ListRemoveAsync(key, ConvertJson(value));
        }

        /// <summary>
        /// 获取指定key的List
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<List<T>> ListRangeAsync<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            var values = await db.ListRangeAsync(key);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 入队
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value"></param>
        public static async Task<long> ListRightPushAsync<T>(this IDatabase db, string key, T value)
        {
            key = AddSysCustomKey(key);
            return await db.ListRightPushAsync(key, ConvertJson(value));
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<T> ListRightPopAsync<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            var value = await db.ListRightPopAsync(key);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 入栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value"></param>
        public static async Task<long> ListLeftPushAsync<T>(this IDatabase db, string key, T value)
        {
            key = AddSysCustomKey(key);
            return await db.ListLeftPushAsync(key, ConvertJson(value));
        }

        /// <summary>
        /// 出栈
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<T> ListLeftPopAsync<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            var value = await db.ListLeftPopAsync(key);
            if (string.IsNullOrEmpty(value))
                return default(T);
            return ConvertObj<T>(value);
        }

        /// <summary>
        /// 获取集合中的数量
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<long> ListLengthAsync(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            return await db.ListLengthAsync(key);
        }


        #endregion List

        #region SortedSet 有序集合
        /// <summary>
        /// 添加有序集合
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value">值</param>
        /// <param name="score"></param>
        public static bool SortedSetAdd<T>(this IDatabase db, string key, T value, double score)
        {
            key = AddSysCustomKey(key);
            return db.SortedSetAdd(key, ConvertJson<T>(value), score);
        }

        /// <summary>
        /// 删除有序集合
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value">值</param>
        public static bool SortedSetRemove<T>(this IDatabase db, string key, T value)
        {
            key = AddSysCustomKey(key);
            return db.SortedSetRemove(key, ConvertJson(value));
        }

        /// <summary>
        /// 获取全部有序集合
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static List<T> SortedSetRangeByRank<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            var values = db.SortedSetRangeByRank(key);
            return ConvetList<T>(values);
        }
        /// <summary>
        /// 获取指定有序集合 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="start">序号开始</param>
        /// <param name="stop">序号结束</param>
        /// <param name="order">排序类型</param>
        /// <returns></returns>
        public static List<T> SortedSetRangeByRank<T>(this IDatabase db,string key, long start = 0, long stop = -1, Order order = Order.Ascending)
        {
            key = AddSysCustomKey(key);
            var values = db.SortedSetRangeByRank(key, start, stop, order);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取有序集合中的数量
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static long SortedSetLength(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            return db.SortedSetLength(key);
        }
        /// <summary>
        /// 添加有序集合
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value">值</param>
        /// <param name="score"></param>
        public static async Task<bool> SortedSetAddAsync<T>(this IDatabase db, string key, T value, double score)
        {
            key = AddSysCustomKey(key);
            return await db.SortedSetAddAsync(key, ConvertJson<T>(value), score);
        }

        /// <summary>
        /// 删除有序集合
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="value">值</param>
        public static async Task<bool> SortedSetRemoveAsync<T>(this IDatabase db, string key, T value)
        {
            key = AddSysCustomKey(key);
            return await db.SortedSetRemoveAsync(key, ConvertJson(value));
        }

        /// <summary>
        /// 获取全部有序集合
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static async Task<List<T>> SortedSetRangeByRankAsync<T>(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            var values = await db.SortedSetRangeByRankAsync(key);
            return ConvetList<T>(values);
        }

        /// <summary>
        /// 获取有序集合中的数量
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static Task<long> SortedSetLengthAsync(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            return db.SortedSetLengthAsync(key);
        }


        #endregion SortedSet 有序集合

        #region Key
        /// <summary>
        /// 删除单个key
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns>是否删除成功</returns>
        public static bool KeyDelete(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            return db.KeyDelete(key);
        }

        /// <summary>
        /// 删除多个key
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="keys">redis key</param>
        /// <returns>成功删除的个数</returns>
        public static long KeyDelete(this IDatabase db, List<string> keys)
        {
            List<string> newKeys = keys.Select(AddSysCustomKey).ToList();
            return db.KeyDelete(ConvertRedisKeys(newKeys));
        }

        /// <summary>
        /// 判断key是否存储
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <returns></returns>
        public static bool KeyExists(this IDatabase db, string key)
        {
            key = AddSysCustomKey(key);
            return db.KeyExists(key);
        }

        /// <summary>
        /// 重新命名key
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">就的redis key</param>
        /// <param name="newKey">新的redis key</param>
        /// <returns></returns>
        public static bool KeyRename(this IDatabase db, string key, string newKey)
        {
            key = AddSysCustomKey(key);
            return db.KeyRename(key, newKey);
        }

        /// <summary>
        /// 设置Key的有效时间
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="key">redis key</param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool KeyExpire(this IDatabase db, string key, TimeSpan? expiry = default(TimeSpan?))
        {
            key = AddSysCustomKey(key);
            return db.KeyExpire(key, expiry);
        }

        #endregion key

        #region 发布订阅
        /// <summary>
        /// Redis发布订阅  订阅
        /// </summary>
        /// <param name="conn">连接器</param>
        /// <param name="subChannel"></param>
        /// <param name="handler"></param>
        public static void Subscribe(this ConnectionMultiplexer conn, string subChannel, Action<RedisChannel, RedisValue> handler = null)
        {
            ISubscriber sub = conn.GetSubscriber();
            sub.Subscribe(subChannel, (channel, message) =>
            {
                if (handler == null)
                {
                    Console.WriteLine(subChannel + " 订阅收到消息：" + message);
                }
                else
                {
                    handler(channel, message);
                }
            });
        }
        /// <summary>
        /// Redis发布订阅  发布
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conn">连接器</param>
        /// <param name="channel"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static long Publish<T>(this ConnectionMultiplexer conn, string channel, T msg)
        {
            ISubscriber sub = conn.GetSubscriber();
            return sub.Publish(channel, ConvertJson(msg));
        }

        /// <summary>
        /// Redis发布订阅  取消订阅
        /// </summary>
        /// <param name="conn">连接器</param>
        /// <param name="channel"></param>
        public static void Unsubscribe(this ConnectionMultiplexer conn, string channel)
        {
            ISubscriber sub = conn.GetSubscriber();
            sub.Unsubscribe(channel);
        }

        /// <summary>
        /// Redis发布订阅  取消全部订阅
        /// </summary>
        /// <param name="conn">连接器</param>
        public static void UnsubscribeAll(this ConnectionMultiplexer conn)
        {
            ISubscriber sub = conn.GetSubscriber();
            sub.UnsubscribeAll();
        }

        #endregion 发布订阅

        #region 其他
        /// <summary>
        /// 获取缓存并且修改（带锁）
        /// </summary>
        /// <typeparam name="T">获取缓存对象</typeparam>
        /// <param name="db">数据库</param>
        /// <param name="key">锁Key</param>
        /// <param name="retrieveDataFunc">读取缓存函数</param>
        /// <param name="timeExpiration"></param>
        /// <param name="modifyEntityFunc"></param>
        /// <param name="lockTimeout"></param>
        /// <param name="isSlidingExpiration"></param>
        /// <returns></returns>
        public static T GetCachedAndModifyWithLock<T>(this IDatabase db,
            string key,
            Func<T> retrieveDataFunc,
            TimeSpan timeExpiration,
            Func<T, bool> modifyEntityFunc,
            TimeSpan? lockTimeout = null,
            bool isSlidingExpiration = false) where T : class
        {

            int lockCounter = 0;//用于获取锁时，如果每个键都有太多锁，计时器
            Exception logException = null;

            var cache = db;
            var lockToken = Guid.NewGuid().ToString(); //当前部分代码的独特令牌
            var lockName = key + "_lock"; //unique lock name. key-relative.
            T tResult = null;

            while (lockCounter < 20)
            {
                //check for access to cache object, trying to lock it
                if (!cache.LockTake(lockName, lockToken, lockTimeout ?? TimeSpan.FromSeconds(10)))
                {
                    lockCounter++;
                    Thread.Sleep(100); //sleep for 100 milliseconds for next lock try. you can play with that
                    continue;
                }

                try
                {
                    RedisValue result = RedisValue.Null;

                    if (isSlidingExpiration)
                    {
                        //in case of sliding expiration - get object with expiry time
                        var exp = cache.StringGetWithExpiry(key);

                        //check ttl.
                        if (exp.Expiry.HasValue && exp.Expiry.Value.TotalSeconds >= 0)
                        {
                            //get only if not expired
                            result = exp.Value;
                        }
                    }
                    else //in absolute expiration case simply get
                    {
                        result = cache.StringGet(key);
                    }

                    //"REDIS_NULL" is for cases when our retrieveDataFunc function returning null (we cannot store null in redis, but can store pre-defined string :) )
                    if (result.HasValue && result == "REDIS_NULL") return null;
                    //in case when cache is epmty
                    if (!result.HasValue)
                    {
                        //retrieving data from caller function (from db from example)
                        tResult = retrieveDataFunc();

                        if (tResult != null)
                        {
                            //trying to modify that entity. if caller modifyEntityFunc returns true, it means that caller wants to resave modified entity.
                            if (modifyEntityFunc(tResult))
                            {
                                //json serialization
                                var json =JsonConvert.SerializeObject(tResult);
                                cache.StringSet(key, json, timeExpiration);
                            }
                        }
                        else
                        {
                            //save pre-defined string in case if source-value is null.
                            cache.StringSet(key, "REDIS_NULL", timeExpiration);
                        }
                    }
                    else
                    {
                        //retrieve from cache and serialize to required object
                        tResult = JsonConvert.DeserializeObject<T>(result);
                        //trying to modify
                        if (modifyEntityFunc(tResult))
                        {
                            //and save if required
                            var json = JsonConvert.SerializeObject(tResult);
                            cache.StringSet(key, json, timeExpiration);
                        }
                    }

                    //refresh exiration in case of sliding expiration flag
                    if (isSlidingExpiration)
                        cache.KeyExpire(key, timeExpiration);
                }
                catch (Exception ex)
                {
                    logException = ex;
                }
                finally
                {
                    cache.LockRelease(lockName, lockToken);
                }
                break;
            }

            if (lockCounter >= 20 || logException != null)
            {
                //log it
            }

            return tResult;
        }

        #endregion 其他

        #region 辅助方法
        /// <summary>
        /// 追加系统缓存Key前缀
        /// </summary>
        /// <param name="oldKey"></param>
        /// <returns></returns>
        private static string AddSysCustomKey(string oldKey)
        {
            return KeyPrefix + oldKey;
        }
        /// <summary>
        /// 转换成JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertJson<T>(T value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject (value);
            return result;
        }

        public static T ConvertObj<T>(RedisValue value)
        {
            return JsonConvert .DeserializeObject<T>(value);
        }

        private static List<T> ConvetList<T>(RedisValue[] values)
        {
            List<T> result = new List<T>();
            foreach (var item in values)
            {
                var model = ConvertObj<T>(item);
                result.Add(model);
            }
            return result;
        }

        private static RedisKey[] ConvertRedisKeys(List<string> redisKeys)
        {
            return redisKeys.Select(redisKey => (RedisKey)redisKey).ToArray();
        }
        #endregion
    }
}
