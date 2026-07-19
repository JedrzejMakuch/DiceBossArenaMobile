using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolCatalogValidatorTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () => new EquipmentAffixPoolCatalogValidator(
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Validate_NullPool_Throws()
        {
            EquipmentAffixPoolCatalogValidator validator =
                new EquipmentAffixPoolCatalogValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Validate_AllEntriesExistInCatalog_DoesNotThrow()
        {
            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            10),

                        new EquipmentAffixPoolEntryDefinition(
                            "vitality_flat",
                            5)
                    });

            EquipmentAffixPoolCatalogValidator validator =
                new EquipmentAffixPoolCatalogValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(pool),
                Throws.Nothing);
        }

        [Test]
        public void Validate_UnknownAffixId_Throws()
        {
            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            10),

                        new EquipmentAffixPoolEntryDefinition(
                            "unknown_affix",
                            5)
                    });

            EquipmentAffixPoolCatalogValidator validator =
                new EquipmentAffixPoolCatalogValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(pool),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_NullEntry_Throws()
        {
            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new EquipmentAffixPoolEntryDefinition[]
                    {
                        null
                    });

            EquipmentAffixPoolCatalogValidator validator =
                new EquipmentAffixPoolCatalogValidator(
                    CreateCatalog());

            Assert.That(
                () => validator.Validate(pool),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        private static EquipmentAffixDefinitionCatalog
            CreateCatalog()
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