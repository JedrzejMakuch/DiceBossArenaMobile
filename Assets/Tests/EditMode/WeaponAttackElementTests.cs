using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class WeaponAttackElementTests
    {
        [TestCase(WeaponAttackElement.Neutral)]
        [TestCase(WeaponAttackElement.Fire)]
        [TestCase(WeaponAttackElement.Water)]
        [TestCase(WeaponAttackElement.Earth)]
        [TestCase(WeaponAttackElement.Air)]
        public void SupportedElement_IsDefined(
            WeaponAttackElement element)
        {
            Assert.That(
                Enum.IsDefined(
                    typeof(WeaponAttackElement),
                    element),
                Is.True);
        }

        [Test]
        public void UnsupportedElement_IsNotDefined()
        {
            WeaponAttackElement element =
                (WeaponAttackElement)999;

            Assert.That(
                Enum.IsDefined(
                    typeof(WeaponAttackElement),
                    element),
                Is.False);
        }
    }
}