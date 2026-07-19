using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class RolledEquipmentAffixTests
    {
        [Test]
        public void Constructor_AssignsData()
        {
            EquipmentAffixId affixId =
                new EquipmentAffixId(
                    "strength_flat");

            RolledEquipmentAffix affix =
                new RolledEquipmentAffix(
                    affixId,
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    7);

            Assert.That(
                affix.AffixId,
                Is.EqualTo(affixId));

            Assert.That(
                affix.StatType,
                Is.EqualTo(
                    FightStatType.Strength));

            Assert.That(
                affix.ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));

            Assert.That(
                affix.Value,
                Is.EqualTo(7));
        }

        [Test]
        public void Constructor_InvalidAffixId_Throws()
        {
            EquipmentAffixId invalidId =
                default;

            Assert.That(
                () => new RolledEquipmentAffix(
                    invalidId,
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    1),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Constructor_UnsupportedStatType_Throws()
        {
            Assert.That(
                () => new RolledEquipmentAffix(
                    new EquipmentAffixId(
                        "test_affix"),
                    (FightStatType)999,
                    FightStatModifierType.Flat,
                    1),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_UnsupportedModifierType_Throws()
        {
            Assert.That(
                () => new RolledEquipmentAffix(
                    new EquipmentAffixId(
                        "test_affix"),
                    FightStatType.Strength,
                    (FightStatModifierType)999,
                    1),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [TestCase(-5)]
        [TestCase(0)]
        [TestCase(5)]
        public void Constructor_AllowsAnyIntegerValue(
            int value)
        {
            RolledEquipmentAffix affix =
                new RolledEquipmentAffix(
                    new EquipmentAffixId(
                        "test_affix"),
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    value);

            Assert.That(
                affix.Value,
                Is.EqualTo(value));
        }
    }
}