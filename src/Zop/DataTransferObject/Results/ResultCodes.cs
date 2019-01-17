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
        public const int HandlerSuccess = 200;
        /// <summary>
        /// 参数无效
        /// </summary>
        public const int InvalidParameter = 501;
        /// <summary>
        /// 系统繁忙 
        /// </summary>
        public const int UnknowError = 500;
        /// <summary>
        /// 处理错误
        /// </summary>
        public const int HandlerError = 502;
        /// <summary>
        /// 未找到服务
        /// </summary>
        public const int NotFound = 404;
        /// <summary>
        /// 拒绝请求
        /// </summary>
        public const int NotAcceptable = 403;
        /// 不支持的Context-Type
        /// </summary>
        public const int UnsupportedMediaType = 415;
    }
}
