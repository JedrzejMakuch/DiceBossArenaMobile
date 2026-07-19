using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixDefinitionCatalogValidatorTests
    {
        [Test]
        public void Validate_NullCatalog_Throws()
        {
            EquipmentAffixDefinitionCatalogValidator validator =
                new EquipmentAffixDefinitionCatalogValidator();

            Assert.That(
                () => validator.Validate(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Validate_EmptyCatalog_DoesNotThrow()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    Array.Empty<
                        EquipmentAffixDefinition>());

            EquipmentAffixDefinitionCatalogValidator validator =
                new EquipmentAffixDefinitionCatalogValidator();

            Assert.That(
                () => validator.Validate(catalog),
                Throws.Nothing);
        }

        [Test]
        public void Validate_AllDefinitionsValid_DoesNotThrow()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        CreateValidDefinition(
                            "strength_flat",
                            FightStatType.Strength),

                        CreateValidDefinition(
                            "vitality_flat",
                            FightStatType.MaxHealth)
                    });

            EquipmentAffixDefinitionCatalogValidator validator =
                new EquipmentAffixDefinitionCatalogValidator();

            Assert.That(
                () => validator.Validate(catalog),
                Throws.Nothing);
        }

        [Test]
        public void Validate_InvalidDefinition_Throws()
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

            EquipmentAffixDefinitionCatalogValidator validator =
                new EquipmentAffixDefinitionCatalogValidator();

            Assert.That(
                () => validator.Validate(catalog),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_DefinitionWithInvalidTier_Throws()
        {
            EquipmentAffixDefinition invalidDefinition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            0,
                            1,
                            5)
                    });

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        invalidDefinition
                    });

            EquipmentAffixDefinitionCatalogValidator validator =
                new EquipmentAffixDefinitionCatalogValidator();

            Assert.That(
                () => validator.Validate(catalog),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        private static EquipmentAffixDefinition
            CreateValidDefinition(
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