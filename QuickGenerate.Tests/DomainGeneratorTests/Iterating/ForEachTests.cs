using System.Linq;
using Xunit;

namespace QuickGenerate.Tests.DomainGeneratorTests.Iterating
{
    public class ForEachTests
    {
        [Fact]
        public void IsApplied()
        {
            var spy = new ForEachSpy();

            var domainGenerator =
                new DomainGenerator()
                    .ForEach<Something>(spy.Check);

            var things = domainGenerator.Many<Something>(2).ToArray();
            Assert.True(spy.Checked.Contains(things[0]));
            Assert.True(spy.Checked.Contains(things[1]));
        }

        public class Something { }       
    }
}