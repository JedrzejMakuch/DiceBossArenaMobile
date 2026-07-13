using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitReferenceTests
    {
        [Test]
        public void TestAssembly_CanCreateFightUnit()
        {
            GameObject gameObject = new GameObject("Test FightUnit");

            try
            {
                FightUnit unit = gameObject.AddComponent<FightUnit>();

                Assert.That(unit, Is.Not.Null);
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }
    }
}