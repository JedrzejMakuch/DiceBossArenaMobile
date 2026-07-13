using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace DiceBossArena.Tests.PlayMode
{
    public sealed class PlayModeInfrastructureSmokeTests
    {
        [UnityTest]
        public IEnumerator TestRunner_CanExecutePlayModeTest()
        {
            yield return null;

            Assert.Pass();
        }
    }
}