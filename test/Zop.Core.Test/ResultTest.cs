using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Newtonsoft.Json;

namespace Zop.Test
{
    public class ResultTest
    {
        [Fact]
        public void ResultJsonSerialization()
        {
            var r = Result.ReSuccess();
            var json = JsonConvert.SerializeObject(r);
        }
        [Fact]
        public void ResultSuccess()
        {
            var res = Result<string>.ReSuccess("test");
            Assert.Equal(200, res.Code);
        }
    }
}
