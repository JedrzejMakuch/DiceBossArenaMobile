using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolDefinitionValidatorTests
    {
        [Test]
        public void Validate_NullDefinition_Throws()
        {
            EquipmentAffixPoolDefinitionValidator validator =
                new EquipmentAffixPoolDefinitionValidator();

            Assert.That(
                () => validator.Validate(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Validate_ValidDefinition_DoesNotThrow()
        {
            EquipmentAffixPoolDefinition definition =
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

            EquipmentAffixPoolDefinitionValidator validator =
                new EquipmentAffixPoolDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.Nothing);
        }

        [Test]
        public void Validate_EmptyEntries_Throws()
        {
            EquipmentAffixPoolDefinition definition =
                new EquipmentAffixPoolDefinition();

            EquipmentAffixPoolDefinitionValidator validator =
                new EquipmentAffixPoolDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_NullEntry_Throws()
        {
            EquipmentAffixPoolDefinition definition =
                new EquipmentAffixPoolDefinition(
                    new EquipmentAffixPoolEntryDefinition[]
                    {
                        null
                    });

            EquipmentAffixPoolDefinitionValidator validator =
                new EquipmentAffixPoolDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_InvalidEntry_Throws()
        {
            EquipmentAffixPoolDefinition definition =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            0)
                    });

            EquipmentAffixPoolDefinitionValidator validator =
                new EquipmentAffixPoolDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_DuplicateAffixIds_Throws()
        {
            EquipmentAffixPoolDefinition definition =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            10),

                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            5)
                    });

            EquipmentAffixPoolDefinitionValidator validator =
                new EquipmentAffixPoolDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }
    }
}