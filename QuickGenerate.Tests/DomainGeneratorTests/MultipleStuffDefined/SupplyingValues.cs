using Xunit;

namespace QuickGenerate.Tests.DomainGeneratorTests.MultipleStuffDefined
{
    public class SupplyingValues
    {
        [Fact]
        public void FirstOneWins()
        {
            var generator =
                new DomainGenerator()
                    .With(42)
                    .With(666);

            var is42 = false;
            var is666 = false;
            var isSomethingElse = false;

            100.Times(
                () =>
                    {
                        var thing = generator.One<SomethingToGenerate>();
                        is42 = thing.MyProperty == 42;
                        is666 = thing.MyProperty == 666;
                        isSomethingElse = thing.MyProperty != 42 && thing.MyProperty != 666;
                    });

            Assert.True(is42);
            Assert.False(is666);
            Assert.False(isSomethingElse);
        }

        public class SomethingToGenerate
        {
            public int MyProperty { get; set; }
        }
    }
}
