using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixConfigurationValidatorTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () => new EquipmentAffixConfigurationValidator(
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Validate_NullPool_Throws()
        {
            EquipmentAffixConfigurationValidator validator =
                new EquipmentAffixConfigurationValidator(
                    CreateValidCatalog());

            Assert.That(
                () => validator.Validate(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Validate_ValidConfiguration_DoesNotThrow()
        {
            EquipmentAffixConfigurationValidator validator =
                new EquipmentAffixConfigurationValidator(
                    CreateValidCatalog());

            Assert.That(
                () => validator.Validate(
                    CreateValidPool()),
                Throws.Nothing);
        }

        [Test]
        public void Validate_InvalidCatalogDefinition_Throws()
        {
            EquipmentAffixDefinition invalidDefinition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat);

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        invalidDefinition
                    });

            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            10)
                    });

            EquipmentAffixConfigurationValidator validator =
                new EquipmentAffixConfigurationValidator(
                    catalog);

            Assert.That(
                () => validator.Validate(pool),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_InvalidPoolStructure_Throws()
        {
            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            0)
                    });

            EquipmentAffixConfigurationValidator validator =
                new EquipmentAffixConfigurationValidator(
                    CreateValidCatalog());

            Assert.That(
                () => validator.Validate(pool),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_UnknownPoolAffixId_Throws()
        {
            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "unknown_affix",
                            10)
                    });

            EquipmentAffixConfigurationValidator validator =
                new EquipmentAffixConfigurationValidator(
                    CreateValidCatalog());

            Assert.That(
                () => validator.Validate(pool),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        private static EquipmentAffixDefinitionCatalog
            CreateValidCatalog()
        {
            return new EquipmentAffixDefinitionCatalog(
                new[]
                {
                    CreateDefinition(
                        "strength_flat",
                        FightStatType.Strength),

                    CreateDefinition(
                        "vitality_flat",
                        FightStatType.MaxHealth)
                });
        }

        private static EquipmentAffixPoolDefinition
            CreateValidPool()
        {
            return new EquipmentAffixPoolDefinition(
                new[]
                {
                    new EquipmentAffixPoolEntryDefinition(
                        "strength_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "vitality_flat",
                        5)
                });
        }

        private static EquipmentAffixDefinition
            CreateDefinition(
                string id,
                FightStatType statType)
        {
            return new EquipmentAffixDefinition(
                id,
                statType,
                FightStatModifierType.Flat,
                new[]
                {
                    new EquipmentAffixTierDefinition(
                        1,
                        1,
                        5)
                });
        }
    }
}