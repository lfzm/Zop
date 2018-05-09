using Microsoft.Extensions.Logging;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Zop.Exceptions
{
    /// <summary>
    /// Orleans异常过滤
    /// </summary>
    public class OrleansExceptionsFiltered : IIncomingGrainCallFilter
    {
        private readonly ILogger Logger;

        public OrleansExceptionsFiltered(ILogger<OrleansExceptionsFiltered> _logger)
        {
            this.Logger = _logger;
        }

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            try
            {
                await context.Invoke();
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex, context.Grain.GetType().Name);
                Exception InnerEx= ex.InnerException;
                while (true)
                {
                    if (InnerEx.InnerException == null)
                    {
                        throw new ZopException(InnerEx.Message);
                    }
                    else
                        InnerEx = InnerEx.InnerException;
                }
            }
        }
    }
}
