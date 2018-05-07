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
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            try
            {
                await context.Invoke();
            }
            catch (Exception ex)
            {
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
