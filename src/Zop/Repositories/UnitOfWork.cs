using Microsoft.Extensions.Logging;
using System;
using System.Data;

namespace Zop.Repositories
{
    /// <summary>
    /// 工作单元抽象类
    /// </summary>
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected readonly ILogger logger;
        /// <summary>
        /// 工作单元
        /// </summary>
        /// <param name="_logger"></param>
        public UnitOfWork(ILogger<UnitOfWork> _logger)
        {
            logger = _logger;
        }
        /// <summary>
        /// 数据库事务
        /// </summary>
        public IDbTransaction Transaction { get; protected set; }

        /// <summary>
        /// 工作单元是否提交
        /// </summary>
        public bool Committed { get; private set; } = false;

        /// <summary>
        /// 开始工作单元
        /// </summary>
        /// <returns></returns>
        public abstract IDbTransaction Begin();

        /// <summary>
        /// 提交工作单元
        /// </summary>
        public bool Commit()
        {
            try
            {
                if (Committed)
                    return true;
                this.CommitTran();
                this.Committed = true;
                return true;
            }
            catch (Exception ex)
            {
                this.RollbackTran();
                this.logger.LogError(ex, "UnitOfWork Commit failed ,error message:{message}", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 回滚当前事务
        /// </summary>
        protected virtual void RollbackTran()
        {
            try
            {
                this.Transaction.Rollback();
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "UnitOfWork RollbackTran failed ,error message:{message}", ex.Message);
            }
        }
        /// <summary>
        /// 提交当前事务
        /// </summary>
        protected virtual void CommitTran()
        {
            this.Transaction.Commit();
        }

    }
}
