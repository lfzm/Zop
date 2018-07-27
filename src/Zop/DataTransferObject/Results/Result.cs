using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zop
{
    /// <summary>
    /// 结果对象
    /// </summary>
    public class Result
    {
        public Result() { }
        public Result(string message, string status = ResultCodes.HandlerSuccess)
        {
            this.SubCode = ResultCodes.HandlerSuccess;
            this.SubMsg = message;
        }
        /// <summary>
        /// 执行是否成功
        /// </summary>
        [JsonIgnore]
        public bool Success
        {
            get
            {
                return this.SubCode == ResultCodes.HandlerSuccess;
            }
        }
        /// <summary>
        /// 业务返回码
        /// </summary>
        [JsonProperty(PropertyName = "sub_code")]
        public string SubCode { get; set; }
        /// <summary>
        /// 执行返回消息
        /// </summary>
        [JsonProperty(PropertyName = "sub_msg")]
        public string SubMsg { get; set; }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(Result result)
        {
            this.SubCode = result.SubCode;
            this.SubMsg = result.SubMsg;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(string message, string status)
        {
            this.SubCode = status;
            this.SubMsg = message;
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public static Result ReFailure(string message, string status)
        {
            return new Result(message, status);
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <returns></returns>
        public static Result ReFailure(string message)
        {
            return new Result(message, ResultCodes.HandlerFailure);
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static T ReFailure<T>(Result result) where T : Result, new()
        {
            T r = new T();
            r.To(result);
            return r;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public static T ReFailure<T>(string message, string status) where T : Result, new()
        {
            T result = new T();
            result.To(message, status);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <returns></returns>
        public static T ReFailure<T>(string message) where T : Result, new()
        {
            T result = new T();
            result.To(message, ResultCodes.HandlerFailure);
            return result;
        }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static Result ReSuccess()
        {
            return new Result(ResultCodes.HandlerSuccess, ResultCodes.HandlerSuccess);
        }
        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static T ReSuccess<T>() where T : Result, new()
        {
            T result = new T();
            result.To(ResultCodes.HandlerSuccess, ResultCodes.HandlerSuccess);
            return result;
        }
    }
    /// <summary>
    /// 实体结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 实体结果
        /// </summary>
        public Result() { }
        /// <summary>
        /// 实体结果
        /// </summary>
        /// <param name="data"></param>
        public Result(T data) : base(ResultCodes.HandlerSuccess)
        {
            this.Data = data;
        }
        public Result(T data, string message, string status = ResultCodes.HandlerSuccess) : base(message, status)
        {
            this.Data = data;
        }

        /// <summary>
        /// 返回对象
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public T Data { get; set; }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static Result<T> ReSuccess(T data)
        {
            return new Result<T>(data);
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public new static Result<T> ReFailure(string message, string status)
        {
            Result<T> result = new Result<T>();
            result.To(message, status);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <returns></returns>
        public new static Result<T> ReFailure(string message)
        {
            Result<T> result = new Result<T>();
            result.To(message, ResultCodes.HandlerFailure);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static Result<T> ReFailure(Result result) 
        {
            Result<T> re = new Result<T>();
            re.To(result);
            return re;
        }
    }
}
