using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.DTO;

namespace Zop.Mvc.Exceptions
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ExceptionHandler
    {
        private readonly ExceptionHandlerOptions _options;
        private readonly ILogger _logger;
        private readonly ExceptionHandlerModel model;

        public ExceptionHandler(ILogger logger, ExceptionHandlerOptions options, HttpContext context, Exception error = null)
        {
            this._logger = logger;
            this._options = options;
            this.model = new ExceptionHandlerModel(context, options.ExceptionHandlingPath, error);
        }
        public Task Handler()
        {
            //如果答复已经开始，我们什么也做不了，只是中止。
            if (this.model.Context.Response.HasStarted)
                return Task.FromResult(0);

            if (this.model.Error != null)
            {
                //异常抛出处理
                this.model.Result = this.BuildExcptionResult();
                this._logger.LogError(this.model.Error, "Message:{Message}->{Path}", this.model.Error.Message, this.model.OriginalPath);
            }
            else
            {
                //获取响应错误消息
                this.model.Result = this.BuildResponseResult();
                if (this.model.Result == null)
                    return Task.FromResult(0);
                //请求favicon.ico 不记录日志
                if (!this.model.Context.Request.Host.Host.Contains("favicon.ico"))
                    this._logger.LogError("Message:{Message};{SubCode}->{Path}", this.model.Result.SubMsg, this.model.Result.SubCode, this.model.OriginalPath);
            }
            this._logger.LogDebug("Response Result:{Result}", this.model.Result);
            //组装响应消息
            this.model.Context.Response.StatusCode = 200;//返回处理成功 统一异常消息
            this.model.Context.Response.ContentLength = null;//清除之前所有返回数据
            var IsAjax = this.model.Context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (!this.model.Path.HasValue
                || IsAjax
                || this.model.Context.Response.ContentType.Contains("application/json"))
            {
                //WebAPI不需要设置错误地址，或者Ajax请求也一样返回参数
                var result = this.model.Result.ToJsonString();
                this.model.Context.Response.ContentType = "application/json;charset=utf-8";
                this.model.Context.Response.WriteAsync(result);

            }
            else
            {
                //Mvc框架,直接跳转至错误页面
                this.model.Context.Request.Path = _options.ExceptionHandlingPath;

            }
            return Task.FromResult(0);
        }

        /// <summary>
        /// 组装异常返回结果对象
        /// </summary>
        /// <returns></returns>
        private Result BuildExcptionResult()
        {
            if (this.model.Error is DTOVerifyException)
                return Result.ReFailure(this.model.Error.Message, ResultCodes.InvalidParameter);
            else if (this.model.Error is ResultNullException)
                return Result.ReFailure(this.model.Error.Message, ResultCodes.HandlerFailure);
            else
                return Result.ReFailure("系统繁忙", ResultCodes.UnknowError);
        }
        /// <summary>
        /// 组装错误响应返回结果对象
        /// </summary>
        /// <returns></returns>
        private Result BuildResponseResult()
        {
            var statusCode = this.model.Context.Response.StatusCode;
            if (statusCode == 401)
                return Result.ReFailure("无效授权令牌", ResultCodes.InvalidAuthToken);
            else if (statusCode == 404)
                return Result.ReFailure("服务暂不可用", ResultCodes.NotFound);
            else if (statusCode == 403)
                return Result.ReFailure("禁止请求", ResultCodes.NotAcceptable);
            else if (statusCode == 406)
                return Result.ReFailure("不接受请求", ResultCodes.NotAcceptable);
            else if (statusCode == 415)
                return Result.ReFailure("不支持的Context-Type", ResultCodes.UnsupportedMediaType);
            else if (statusCode == 502)
                return Result.ReFailure("请求错误", ResultCodes.HandlerError);
            else if (statusCode == 204)
                return Result.ReFailure("未找到数据", ResultCodes.HandlerFailure);
       
            else return null;
        }
    }
}
