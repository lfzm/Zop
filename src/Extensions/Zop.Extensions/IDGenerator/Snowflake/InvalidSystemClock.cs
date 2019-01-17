using System;

namespace Zop.Extensions.IDGenerator.Snowflake
{
    public class InvalidSystemClock : Exception
    {      
        public InvalidSystemClock(string message) : base(message) { }
    }
}