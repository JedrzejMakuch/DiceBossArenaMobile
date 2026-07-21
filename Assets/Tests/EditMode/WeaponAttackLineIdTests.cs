using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class WeaponAttackLineIdTests
    {
        [Test]
        public void Constructor_ValidValue_StoresValue()
        {
            WeaponAttackLineId id =
                new WeaponAttackLineId(
                    "primary_damage");

            Assert.That(
                id.Value,
                Is.EqualTo("primary_damage"));

            Assert.That(
                id.ToString(),
                Is.EqualTo("primary_damage"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Constructor_InvalidValue_Throws(
            string value)
        {
            Assert.That(
                () => new WeaponAttackLineId(value),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void EqualValues_AreEqual()
        {
            WeaponAttackLineId first =
                new WeaponAttackLineId(
                    "primary_damage");

            WeaponAttackLineId second =
                new WeaponAttackLineId(
                    "primary_damage");

            Assert.That(first, Is.EqualTo(second));
            Assert.That(first == second, Is.True);
            Assert.That(first != second, Is.False);
            Assert.That(
                first.GetHashCode(),
                Is.EqualTo(second.GetHashCode()));
        }

        [Test]
        public void DifferentValues_AreNotEqual()
        {
            WeaponAttackLineId first =
                new WeaponAttackLineId(
                    "primary_damage");

            WeaponAttackLineId second =
                new WeaponAttackLineId(
                    "secondary_damage");

            Assert.That(first, Is.Not.EqualTo(second));
            Assert.That(first == second, Is.False);
            Assert.That(first != second, Is.True);
        }

        [Test]
        public void Equality_IsCaseSensitive()
        {
            WeaponAttackLineId first =
                new WeaponAttackLineId(
                    "primary_damage");

            WeaponAttackLineId second =
                new WeaponAttackLineId(
                    "Primary_Damage");

            Assert.That(first, Is.Not.EqualTo(second));
        }
    }
}