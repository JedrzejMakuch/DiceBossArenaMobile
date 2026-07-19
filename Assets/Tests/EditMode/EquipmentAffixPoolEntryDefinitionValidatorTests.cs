using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolEntryDefinitionValidatorTests
    {
        [Test]
        public void Validate_NullDefinition_Throws()
        {
            EquipmentAffixPoolEntryDefinitionValidator validator =
                new EquipmentAffixPoolEntryDefinitionValidator();

            Assert.That(
                () => validator.Validate(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Validate_ValidDefinition_DoesNotThrow()
        {
            EquipmentAffixPoolEntryDefinition definition =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10);

            EquipmentAffixPoolEntryDefinitionValidator validator =
                new EquipmentAffixPoolEntryDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.Nothing);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Validate_InvalidAffixId_Throws(
            string affixId)
        {
            EquipmentAffixPoolEntryDefinition definition =
                new EquipmentAffixPoolEntryDefinition(
                    affixId,
                    10);

            EquipmentAffixPoolEntryDefinitionValidator validator =
                new EquipmentAffixPoolEntryDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void Validate_NonPositiveWeight_Throws(
            int weight)
        {
            EquipmentAffixPoolEntryDefinition definition =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    weight);

            EquipmentAffixPoolEntryDefinitionValidator validator =
                new EquipmentAffixPoolEntryDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }
    }
}