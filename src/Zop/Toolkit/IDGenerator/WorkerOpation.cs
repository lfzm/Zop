using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Toolkit.IDGenerator
{
    public class WorkerOpation : IWorkerOpation
    {
        public long GetDatacenterId()
        {
            return 1;
        }

        public long GetWorkerId()
        {
            return 1;
        }
    }
}
