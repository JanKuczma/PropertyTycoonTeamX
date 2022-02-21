using NUnit.Framework;

namespace LogicScripts.UnitTests
{
    [TestFixture]
    public class Tests
    {
        /*
         * Tests the Card constructor initialises the object correctly
         */
        [Test]
        public void Test1()
        {
            Card c = new Card("Hello", "world");
            Assert.True(c.GetName() == "Hello" && c.GetDescription() == "world");
        }
    }
}