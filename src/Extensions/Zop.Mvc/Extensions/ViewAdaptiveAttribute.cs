using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 页面自适应
    /// </summary>
    public class ViewAdaptiveAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// action名称
        /// </summary>
        private string ViewName { get; set; }

        public ViewAdaptiveAttribute()
        {
        }

    

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ViewResult)
            {
                this.ViewName = ((ViewResult)context.Result).ViewName;
                if (string.IsNullOrEmpty(this.ViewName))
                    this.ViewName = context.RouteData.Values["action"].ToString();

                var VisitorTerminal = context.HttpContext.GetVisitorTerminal();
                if (VisitorTerminal.IsMobileTerminal && !this.ViewName.Contains("_Moblie"))
                    this.ViewName = this.ViewName + "_Moblie";

                var result = new ViewResult();
                result = (ViewResult)context.Result;
                result.ViewName = this.ViewName;
                context.Result = result;
            }
            else
            {
                base.OnResultExecuting(context);
            }
        }
    }
}
