using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Mvc.Exceptions;

namespace Microsoft.AspNetCore.Builder
{
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder UseZopExceptionHandler(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<ExceptionHandlerMiddieware>();
        }

        public static IApplicationBuilder UseZopExceptionHandler(this IApplicationBuilder app, string errorHandlingPath)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseZopExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandlingPath = new PathString(errorHandlingPath)
            });
        }
        public static IApplicationBuilder UseZopExceptionHandler(this IApplicationBuilder app, ExceptionHandlerOptions options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return app.UseMiddleware<ExceptionHandlerMiddieware>(Options.Create(options));
        }
    }
}
