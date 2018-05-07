using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.DataAnnotations;

namespace Zop.DTO
{
    /// <summary>
    /// Dto实体扩展
    /// </summary>
    public static class DTOExtensions
    {
        /// <summary>
        /// 验证是否成功
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsValid(this RequestDto entity)
        {
            return entity.IsValid(null);
        }
        /// <summary>
        /// 验证实体对象
        /// </summary>
        /// <param name="entity"></param>
        public static Result ValidResult(this RequestDto entity)
        {
            return ValidationHelper.ValidResult(entity);
        }
        /// <summary>
        /// 验证是否成功
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="logger">日志记录</param>
        /// <param name="logLevel">日志记录级别</param>
        /// <returns></returns>
        public static bool IsValid(this RequestDto entity, ILogger logger, LogLevel logLevel = LogLevel.Information)
        {
            return ValidationHelper.IsValid(entity, logger, logLevel);
        }
    }
}
