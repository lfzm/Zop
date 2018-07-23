using System;
using System.Collections.Generic;
using System.Text;
using Zop.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// 控制器扩展类
    /// </summary>
    public static class ControllerExtension
    {
        /// <summary>
        /// 错误视图
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="message">错误信息</param>
        /// <param name="viewName">错误视图名称</param>
        /// <returns></returns>
        public static ViewResult ErrorView(this Controller controller, string message, string viewName = "Error")
        {
            return controller.ErrorView(new ViewErrorDto(message, "0000"), viewName);
        }
        /// <summary>
        /// 错误视图
        /// </summary>
        /// <typeparam name="TError">ViewErrorDto</typeparam>
        /// <param name="controller">控制器</param>
        /// <param name="error">错误信息</param>
        /// <param name="viewName">错误视图名称</param>
        /// <returns></returns>
        public static ViewResult ErrorView<TError>(this Controller controller, TError error, string viewName = "Error")
        where TError : ViewErrorDto
        {
            return controller.View(viewName, error);
        }

     

    }
}
