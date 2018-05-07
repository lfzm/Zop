using System;
using System.Collections.Generic;
using System.Text;

namespace Zop
{
    /// <summary>
    /// 结果返回码
    /// </summary>
    public static class ResultCodes
    {
        /// <summary>
        /// 处理成功
        /// </summary>
        public const string HandlerSuccess = "success";
        /// <summary>
        /// 参数无效
        /// </summary>
        public const string InvalidParameter = "invalid_parameter";
        /// <summary>
        /// 系统繁忙 
        /// </summary>
        public const string UnknowError = "unknow_error";
        /// <summary>
        /// 处理错误
        /// </summary>
        public const string HandlerError = "handler_error";
        /// <summary>
        /// 处理错误
        /// </summary>
        public const string HandlerFailure = "handler_failure";
        /// <summary>
        /// 未找到服务
        /// </summary>
        public const string NotFound = "not_found";
        /// <summary>
        /// 拒绝请求
        /// </summary>
        public const string NotAcceptable = "not_acceptable";
        /// <summary>
        /// 无效授权令牌
        /// </summary>
        public const string InvalidAuthToken = "invalid_auth_token";
        /// <summary>
        /// 访问令牌已过期
        /// </summary>
        public const string AuthTokenTimeOut = "auth_token_time_out";
        /// <summary>
        /// 无效的应用授权令牌
        /// </summary>
        public const string InvalidAppAuthToken = "invalid_app_auth_token";
        /// <summary>
        /// 应用授权令牌已过期
        /// </summary>
        public const string AppAuthTokenTimeOut = "app-auth-token-time-out";
        /// <summary>
        /// 不支持的Context-Type
        /// </summary>
        public const string UnsupportedMediaType = "unsupported_media_type";
    }
}
