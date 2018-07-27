using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.DTO
{
    /// <summary>
    /// 结果实体类
    /// </summary>
    public class ResultResponseDto : Result, IResponseDto
    {
        /// <summary>
        /// 结果实体类
        /// </summary>
        public ResultResponseDto(): base("success", ResultCodes.HandlerSuccess)
        {

        }
   
    }
}
