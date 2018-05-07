using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Mvc.Exceptions
{
    /// <summary>
    /// 异常处理对象
    /// </summary>
    public class ExceptionHandlerModel
    {
        public ExceptionHandlerModel(HttpContext context, string path)
        {
            this.Context = context;
            this.Path = path;
        }
        public ExceptionHandlerModel(HttpContext context, string path, Exception error):this(context,path)
        {
            this.Error = error;
        }
        public HttpContext Context { get; }
        /// <summary>
        /// 错误对象
        /// </summary>
        public Exception Error { get; set; }
        /// <summary>
        /// 异常跳转路径
        /// </summary>
        public PathString Path { get;  }
        /// <summary>
        /// 返回状态
        /// </summary>
        public int StatusCode { get => this.Context.Response.StatusCode; }
        /// <summary>
        /// 原来请求路径
        /// </summary>
        public PathString OriginalPath { get => Context.Request.Path; }
        /// <summary>
        /// 结果对象
        /// </summary>
        public Result Result { get; set; }
    }
}
