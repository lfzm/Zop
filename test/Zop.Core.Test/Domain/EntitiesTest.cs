using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Zop.Domain.Entities;

namespace Zop.Core.Test.Domain.Entities
{

    public class EntitiesTest
    {
        [Fact]
        public void EntityTest()
        {
            var entity = new TestEntity();
            Assert.False(entity.IsTransient);
        }
    }

    public class TestEntity : Entity<int>
    {
        public TestEntity()
        {
        }
        public override int Id { get => base.Id; protected set => base.Id = value; }
    }
}
