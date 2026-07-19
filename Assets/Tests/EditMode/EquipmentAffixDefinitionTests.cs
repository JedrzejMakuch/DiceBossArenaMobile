using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EquipmentAffixDefinitionTests
    {
        [Test]
        public void Constructor_AssignsTiers()
        {
            EquipmentAffixTierDefinition firstTier =
                new EquipmentAffixTierDefinition(
                    1,
                    1,
                    3);

            EquipmentAffixTierDefinition secondTier =
                new EquipmentAffixTierDefinition(
                    10,
                    4,
                    7);

            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new List<EquipmentAffixTierDefinition>
                    {
                firstTier,
                secondTier
                    });

            Assert.That(
                definition.Tiers,
                Has.Count.EqualTo(2));

            Assert.That(
                definition.Tiers[0],
                Is.SameAs(firstTier));

            Assert.That(
                definition.Tiers[1],
                Is.SameAs(secondTier));
        }

        [Test]
        public void Constructor_WithoutTiers_CreatesEmptyCollection()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat);

            Assert.That(
                definition.Tiers,
                Is.Not.Null);

            Assert.That(
                definition.Tiers,
                Is.Empty);
        }

        [Test]
        public void Constructor_AssignsDefinitionData()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat);

            Assert.That(
                definition.Id,
                Is.EqualTo(
                    new EquipmentAffixId(
                        "strength_flat")));

            Assert.That(
                definition.StatType,
                Is.EqualTo(
                    FightStatType.Strength));

            Assert.That(
                definition.ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Id_InvalidValue_Throws(
            string id)
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    id,
                    FightStatType.Strength,
                    FightStatModifierType.Flat);

            Assert.That(
                () =>
                {
                    EquipmentAffixId result =
                        definition.Id;
                },
                Throws.TypeOf<
                    System.ArgumentException>());
        }
    }
}