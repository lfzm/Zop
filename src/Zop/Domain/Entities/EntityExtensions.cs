using Microsoft.Extensions.Logging;
using Zop.DTO;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// Some useful extension methods for Entities.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// 检查这个实体是否被标记为已删除
        /// </summary>
        public static bool IsNullOrDeleted(this ISoftDelete entity)
        {
            return entity == null || entity.IsDeleted;
        }

        /// <summary>
        ///取消删除实体
        /// </summary>
        public static void UnDelete(this ISoftDelete entity)
        {
            entity.IsDeleted = false;
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        public static void Delete(this ISoftDelete entity)
        {
            entity.IsDeleted = true;
        }
        /// <summary>
        ///获取原来的乐观版本号
        /// </summary>
        public static int OldVersionNo(this IConcurrencySafe entity)
        {
            return entity.VersionNo - 1;
        }
        /// <summary>
        /// 验证是否成功
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool IsValid<TPrimaryKey>(this IEntity<TPrimaryKey> entity)
        {
            return entity.IsValid(null);
        }
        /// <summary>
        /// 验证是否成功
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="logger">日志记录</param>
        /// <param name="logLevel">日志记录级别</param>
        /// <returns></returns>
        public static bool IsValid<TPrimaryKey>(this IEntity<TPrimaryKey> entity, ILogger logger, LogLevel logLevel = LogLevel.Information)
        {
            return ValidationHelper.IsValid(entity, logger, logLevel);
        }
    }
}
