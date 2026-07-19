using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EquipmentAffixIdTests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Constructor_InvalidValue_Throws(
            string value)
        {
            Assert.That(
                () => new EquipmentAffixId(value),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Constructor_ValidValue_AssignsValue()
        {
            EquipmentAffixId id =
                new EquipmentAffixId(
                    "strength_flat");

            Assert.That(
                id.Value,
                Is.EqualTo("strength_flat"));
        }

        [Test]
        public void EqualValues_AreEqual()
        {
            EquipmentAffixId first =
                new EquipmentAffixId(
                    "strength_flat");

            EquipmentAffixId second =
                new EquipmentAffixId(
                    "strength_flat");

            Assert.That(
                first,
                Is.EqualTo(second));

            Assert.That(
                first == second,
                Is.True);
        }

        [Test]
        public void DifferentValues_AreNotEqual()
        {
            EquipmentAffixId first =
                new EquipmentAffixId(
                    "strength_flat");

            EquipmentAffixId second =
                new EquipmentAffixId(
                    "dexterity_flat");

            Assert.That(
                first,
                Is.Not.EqualTo(second));

            Assert.That(
                first != second,
                Is.True);
        }

        [Test]
        public void Equality_IsCaseSensitive()
        {
            EquipmentAffixId first =
                new EquipmentAffixId(
                    "strength_flat");

            EquipmentAffixId second =
                new EquipmentAffixId(
                    "Strength_Flat");

            Assert.That(
                first,
                Is.Not.EqualTo(second));
        }

        [Test]
        public void ToString_ReturnsValue()
        {
            EquipmentAffixId id =
                new EquipmentAffixId(
                    "strength_flat");

            Assert.That(
                id.ToString(),
                Is.EqualTo("strength_flat"));
        }
    }
}