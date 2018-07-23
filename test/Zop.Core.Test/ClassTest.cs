using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Newtonsoft.Json;

namespace Zop.Core.Test
{
    public class ClassTest
    {
        [Fact]
        public void TestF()
        {
            Test t1 = new Test("1");
            string j = JsonConvert.SerializeObject(t1);

            StringBuilder text = new StringBuilder();
            Dictionary<int, int> hashCode = new Dictionary<int, int>();
            for (int i = 0; i < 1000; i++)
            {
                Test t = new Test("1");
                text.AppendLine($"{i} ; hashCode:{t.GetHashCode()}");
                hashCode.Add(t.GetHashCode(), i);
            }
            string log = text.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        public class Test
        {
            [JsonProperty]
            private string a;
            public Test(string name)
            {
                a = "a";
                this.Name = name;
            }
            public string Name { get; set; }
        }

    }
}
