using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixTierDefinitionValidatorTests
    {
        [Test]
        public void Validate_NullDefinition_Throws()
        {
            EquipmentAffixTierDefinitionValidator validator =
                new EquipmentAffixTierDefinitionValidator();

            Assert.That(
                () => validator.Validate(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Validate_ValidDefinition_DoesNotThrow()
        {
            EquipmentAffixTierDefinition definition =
                new EquipmentAffixTierDefinition(
                    1,
                    2,
                    5);

            EquipmentAffixTierDefinitionValidator validator =
                new EquipmentAffixTierDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.Nothing);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Validate_InvalidMinimumItemLevel_Throws(
            int minimumItemLevel)
        {
            EquipmentAffixTierDefinition definition =
                new EquipmentAffixTierDefinition(
                    minimumItemLevel,
                    1,
                    3);

            EquipmentAffixTierDefinitionValidator validator =
                new EquipmentAffixTierDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_MinimumValueGreaterThanMaximum_Throws()
        {
            EquipmentAffixTierDefinition definition =
                new EquipmentAffixTierDefinition(
                    1,
                    5,
                    3);

            EquipmentAffixTierDefinitionValidator validator =
                new EquipmentAffixTierDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_EqualMinimumAndMaximum_DoesNotThrow()
        {
            EquipmentAffixTierDefinition definition =
                new EquipmentAffixTierDefinition(
                    1,
                    4,
                    4);

            EquipmentAffixTierDefinitionValidator validator =
                new EquipmentAffixTierDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.Nothing);
        }
    }
}