using Haulage.Model;

namespace HaulageTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Warehouse test = new Warehouse();
            Assert.AreEqual(test.Name, string.Empty);
        }
    }
}