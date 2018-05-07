using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.DTO
{
    /// <summary>
    /// 结果为空异常
    /// </summary>
    public class ResultNullException : ZopException
    {
        public ResultNullException(string message = "未找到结果") : base(message)
        {

        }
    }
}
