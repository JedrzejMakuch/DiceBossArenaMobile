using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class TestInfrastructureSmokeTests
    {
        [Test]
        public void TestRunner_CanExecuteEditModeTest()
        {
            Assert.That(1 + 1, Is.EqualTo(2));
        }
    }
}