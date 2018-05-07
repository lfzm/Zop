using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.DTO
{
    /// <summary>
    /// 错误页面显示信息
    /// </summary>
    public class ViewErrorDto : ViewDto
    {
        /// <summary>
        /// 错误页面显示信息
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="code">错误代码</param>
        public ViewErrorDto(string message,string code)
        {
            this.ErrorMessage = message;
            this.ErrorCode = code;
        }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get;  }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string ErrorCode { get; }
    }
}
