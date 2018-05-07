using System;

namespace Zop.Toolkit.IDGenerator.Snowflake
{
    public class InvalidSystemClock : Exception
    {      
        public InvalidSystemClock(string message) : base(message) { }
    }
}