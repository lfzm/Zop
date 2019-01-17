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
        public Result(string message, int status = ResultCodes.HandlerSuccess)
        {
            this.Status = status;
            this.Message = message;
        }
        /// <summary>
        /// 执行是否成功
        /// </summary>
        [JsonIgnore]
        public bool Success
        {
            get
            {
                return this.Status == ResultCodes.HandlerSuccess;
            }
        }
        /// <summary>
        /// 业务返回码
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }
        /// <summary>
        /// 执行返回消息
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(Result result)
        {
            this.Status = result.Status;
            this.Message = result.Message;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(string message, int status)
        {
            this.Status = status;
            this.Message = message;
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="status">结果状态</param>
        /// <returns></returns>
        public static Result ReFailure(string message, int status)
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
            return new Result(message, ResultCodes.HandlerError);
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
        public static T ReFailure<T>(string message, int status) where T : Result, new()
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
            result.To(message, ResultCodes.HandlerError);
            return result;
        }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static Result ReSuccess()
        {
            return new Result("success", ResultCodes.HandlerSuccess);
        }
        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static T ReSuccess<T>() where T : Result, new()
        {
            T result = new T();
            result.To("success", ResultCodes.HandlerSuccess);
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
        public Result(T data) : base("success")
        {
            this.Data = data;
        }
        public Result(T data, string message, int status = ResultCodes.HandlerSuccess) : base(message, status)
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
        public new static Result<T> ReFailure(string message, int status)
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
            result.To(message, ResultCodes.HandlerError);
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
