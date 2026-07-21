using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class RolledWeaponAttackLineTests
    {
        [Test]
        public void Constructor_ValidValues_StoresValues()
        {
            RolledWeaponAttackLine line =
                CreateLine();

            Assert.That(
                line.LineId,
                Is.EqualTo(
                    new WeaponAttackLineId(
                        "primary_damage")));

            Assert.That(
                line.Element,
                Is.EqualTo(WeaponAttackElement.Fire));

            Assert.That(
                line.MinDamage,
                Is.EqualTo(4));

            Assert.That(
                line.MaxDamage,
                Is.EqualTo(8));
        }

        [Test]
        public void Constructor_DefaultLineId_Throws()
        {
            Assert.That(
                () => new RolledWeaponAttackLine(
                    default,
                    WeaponAttackElement.Neutral,
                    4,
                    8),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Constructor_UnsupportedElement_Throws()
        {
            Assert.That(
                () => new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    (WeaponAttackElement)999,
                    4,
                    8),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_NegativeMinDamage_Throws()
        {
            Assert.That(
                () => new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Fire,
                    -1,
                    8),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_MaxDamageLowerThanMinDamage_Throws()
        {
            Assert.That(
                () => new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Fire,
                    8,
                    7),
                Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void EqualValues_AreEqual()
        {
            RolledWeaponAttackLine first =
                CreateLine();

            RolledWeaponAttackLine second =
                CreateLine();

            Assert.That(first, Is.EqualTo(second));

            Assert.That(
                first.GetHashCode(),
                Is.EqualTo(second.GetHashCode()));
        }

        [Test]
        public void DifferentElement_AreNotEqual()
        {
            RolledWeaponAttackLine first =
                CreateLine();

            RolledWeaponAttackLine second =
                new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Water,
                    4,
                    8);

            Assert.That(first, Is.Not.EqualTo(second));
        }

        [Test]
        public void DifferentDamageRange_AreNotEqual()
        {
            RolledWeaponAttackLine first =
                CreateLine();

            RolledWeaponAttackLine second =
                new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Fire,
                    5,
                    9);

            Assert.That(first, Is.Not.EqualTo(second));
        }

        [Test]
        public void Equals_Null_ReturnsFalse()
        {
            Assert.That(
                CreateLine().Equals(null),
                Is.False);
        }

        private static RolledWeaponAttackLine CreateLine()
        {
            return new RolledWeaponAttackLine(
                new WeaponAttackLineId(
                    "primary_damage"),
                WeaponAttackElement.Fire,
                4,
                8);
        }
    }
}