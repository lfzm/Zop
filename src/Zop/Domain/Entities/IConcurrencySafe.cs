namespace Zop.Domain.Entities
{
    /// <summary>
    /// 线程安全接口
    /// </summary>
    public interface IConcurrencySafe
    {
        /// <summary>
        /// 乐观并发的版本号
        /// </summary>
        int VersionNo { get; }
    }
}
