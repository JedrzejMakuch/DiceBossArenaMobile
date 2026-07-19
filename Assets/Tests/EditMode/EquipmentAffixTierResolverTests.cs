using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixTierResolverTests
    {
        [Test]
        public void Resolve_NullAffix_Throws()
        {
            EquipmentAffixTierResolver resolver =
                new EquipmentAffixTierResolver();

            Assert.That(
                () => resolver.Resolve(
                    null,
                    1),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Resolve_InvalidItemLevel_Throws(
            int itemLevel)
        {
            EquipmentAffixTierResolver resolver =
                new EquipmentAffixTierResolver();

            EquipmentAffixDefinition affix =
                CreateAffix();

            Assert.That(
                () => resolver.Resolve(
                    affix,
                    itemLevel),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Resolve_LevelBelowFirstTier_ReturnsNull()
        {
            EquipmentAffixTierResolver resolver =
                new EquipmentAffixTierResolver();

            EquipmentAffixDefinition affix =
                CreateAffix();

            EquipmentAffixTierDefinition result =
                resolver.Resolve(
                    affix,
                    4);

            Assert.That(
                result,
                Is.Null);
        }

        [Test]
        public void Resolve_LevelEqualToFirstTier_ReturnsFirstTier()
        {
            EquipmentAffixTierResolver resolver =
                new EquipmentAffixTierResolver();

            EquipmentAffixDefinition affix =
                CreateAffix();

            EquipmentAffixTierDefinition result =
                resolver.Resolve(
                    affix,
                    5);

            Assert.That(
                result,
                Is.SameAs(
                    affix.Tiers[0]));
        }

        [Test]
        public void Resolve_LevelBetweenTiers_ReturnsHighestAvailableTier()
        {
            EquipmentAffixTierResolver resolver =
                new EquipmentAffixTierResolver();

            EquipmentAffixDefinition affix =
                CreateAffix();

            EquipmentAffixTierDefinition result =
                resolver.Resolve(
                    affix,
                    15);

            Assert.That(
                result,
                Is.SameAs(
                    affix.Tiers[1]));
        }

        [Test]
        public void Resolve_LevelAboveAllTiers_ReturnsLastTier()
        {
            EquipmentAffixTierResolver resolver =
                new EquipmentAffixTierResolver();

            EquipmentAffixDefinition affix =
                CreateAffix();

            EquipmentAffixTierDefinition result =
                resolver.Resolve(
                    affix,
                    99);

            Assert.That(
                result,
                Is.SameAs(
                    affix.Tiers[2]));
        }

        private static EquipmentAffixDefinition
            CreateAffix()
        {
            return new EquipmentAffixDefinition(
                "strength_flat",
                FightStatType.Strength,
                FightStatModifierType.Flat,
                new[]
                {
                    new EquipmentAffixTierDefinition(
                        5,
                        1,
                        3),

                    new EquipmentAffixTierDefinition(
                        10,
                        4,
                        7),

                    new EquipmentAffixTierDefinition(
                        20,
                        8,
                        12)
                });
        }
    }
}