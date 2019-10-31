using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 结果对象
    /// </summary>
    public class Result
    {
        public Result() { }
        public Result(string message, int status)
        {
            this.Code = status;
            this.Msg = message;
        }
        public Result(ValueTuple<int, string> result)
        {
            this.Code = result.Item1;
            this.Msg = result.Item2;
        }
        /// <summary>
        /// 执行是否成功
        /// </summary>
        [IgnoreDataMember, JsonIgnore]
        public bool Success
        {
            get
            {
                return this.Code == 200;
            }
        }
        /// <summary>
        /// 执行是否失败
        /// </summary>
        [IgnoreDataMember, JsonIgnore]
        public bool Failure
        {
            get
            {
                return this.Code != 200;
            }
        }
        /// <summary>
        /// 业务返回码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 执行返回消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result"></param>
        protected void To(Result result)
        {
            this.Code = result.Code;
            this.Msg = result.Msg;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="code">结果代码</param>
        protected void To(string message, int code)
        {
            this.Code = code;
            this.Msg = message;
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="result">结果对象</param>
        protected void To(ValueTuple<int, string> result)
        {
            this.Code = result.Item1;
            this.Msg = result.Item2;
        }

        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="message">结果消息</param>
        /// <param name="code">结果状态</param>
        /// <returns></returns>
        public static Result ReFailure(string message, int code)
        {
            return new Result(message, code);
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public static Result ReFailure(ValueTuple<int, string> result)
        {
            return new Result(result.Item2, result.Item1);
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
        /// <param name="code">结果状态</param>
        /// <returns></returns>
        public static T ReFailure<T>(string message, int code) where T : Result, new()
        {
            T result = new T();
            result.To(message, code);
            return result;
        }
        /// <summary>
        /// 创建返回信息（返回处理失败）
        /// </summary>
        /// <param name="result">结果消息</param>
        /// <returns></returns>
        public static T ReFailure<T>(ValueTuple<int, string> result) where T : Result, new()
        {
            T ret = new T();
            ret.To(result);
            return ret;
        }

        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static Result ReSuccess()
        {
            return new Result(BaseResultCodes.Success);
        }
        /// <summary>
        /// 创建成功的返回消息
        /// </summary>
        /// <returns></returns>
        public static T ReSuccess<T>() where T : Result, new()
        {
            T result = new T();
            result.To(BaseResultCodes.Success);
            return result;
        }

        /// <summary>
        /// 转换为 <see cref="Task<Result>"/>
        /// </summary>
        /// <returns></returns>
        public Task<Result> AsTask()
        {
            return Task.FromResult(this);
        }
    }

}
