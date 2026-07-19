using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolRarityCapacityValidatorTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () =>
                    new EquipmentAffixPoolRarityCapacityValidator(
                        null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Validate_NullPool_Throws()
        {
            EquipmentAffixPoolRarityCapacityValidator validator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(
                    null,
                    1,
                    EquipmentItemRarity.Common),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_InvalidItemLevel_Throws(
            int itemLevel)
        {
            EquipmentAffixPoolRarityCapacityValidator validator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(
                    CreatePool(),
                    itemLevel,
                    EquipmentItemRarity.Common),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Validate_CommonWithNoAvailableAffixes_DoesNotThrow()
        {
            EquipmentAffixPoolRarityCapacityValidator validator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(
                    CreatePool(),
                    1,
                    EquipmentItemRarity.Common),
                Throws.Nothing);
        }

        [Test]
        public void Validate_MagicWithTwoAvailableAffixes_DoesNotThrow()
        {
            EquipmentAffixPoolRarityCapacityValidator validator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(
                    CreatePool(),
                    5,
                    EquipmentItemRarity.Magic),
                Throws.Nothing);
        }

        [Test]
        public void Validate_MagicWithOneAvailableAffix_Throws()
        {
            EquipmentAffixPoolRarityCapacityValidator validator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(
                    CreatePool(),
                    3,
                    EquipmentItemRarity.Magic),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_RareWithFourAvailableAffixes_DoesNotThrow()
        {
            EquipmentAffixPoolRarityCapacityValidator validator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(
                    CreatePool(),
                    10,
                    EquipmentItemRarity.Rare),
                Throws.Nothing);
        }

        [Test]
        public void Validate_RareWithThreeAvailableAffixes_Throws()
        {
            EquipmentAffixPoolRarityCapacityValidator validator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(
                    CreatePool(),
                    7,
                    EquipmentItemRarity.Rare),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_UnsupportedRarity_Throws()
        {
            EquipmentAffixPoolRarityCapacityValidator validator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(
                    CreatePool(),
                    10,
                    (EquipmentItemRarity)999),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        private static EquipmentAffixPoolDefinition
            CreatePool()
        {
            return new EquipmentAffixPoolDefinition(
                new[]
                {
                    new EquipmentAffixPoolEntryDefinition(
                        "strength_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "vitality_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "dexterity_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "intelligence_flat",
                        10)
                });
        }

        private static EquipmentAffixDefinitionCatalog
            CreateCatalog()
        {
            return new EquipmentAffixDefinitionCatalog(
                new[]
                {
                    CreateDefinition(
                        "strength_flat",
                        FightStatType.Strength,
                        2),

                    CreateDefinition(
                        "vitality_flat",
                        FightStatType.MaxHealth,
                        5),

                    CreateDefinition(
                        "dexterity_flat",
                        FightStatType.Dexterity,
                        7),

                    CreateDefinition(
                        "intelligence_flat",
                        FightStatType.Intelligence,
                        10)
                });
        }

        private static EquipmentAffixDefinition
            CreateDefinition(
                string id,
                FightStatType statType,
                int minimumItemLevel)
        {
            return new EquipmentAffixDefinition(
                id,
                statType,
                FightStatModifierType.Flat,
                new[]
                {
                    new EquipmentAffixTierDefinition(
                        minimumItemLevel,
                        1,
                        5)
                });
        }
    }
}