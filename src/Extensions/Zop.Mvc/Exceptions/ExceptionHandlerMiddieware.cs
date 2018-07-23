using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zop.Mvc.Exceptions
{
    /// <summary>
    /// 异常处理中间件
    /// </summary>
    public class ExceptionHandlerMiddieware
    {
        private readonly RequestDelegate next;
        private readonly ILogger _logger;
        private readonly ExceptionHandlerOptions _options;

        public ExceptionHandlerMiddieware(RequestDelegate next, ILogger<ExceptionHandlerMiddieware> logger, IOptions<ExceptionHandlerOptions> options)
        {
            this.next = next;
            this._logger = logger;
            this._options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            Exception exception=null;
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                //组装异常对象
                exception = ex;
            }
            finally
            {
                var errorHandler = new ExceptionHandler(this._logger, this._options, context, exception);
                await errorHandler.Handler();
            }

        }


    }
}
