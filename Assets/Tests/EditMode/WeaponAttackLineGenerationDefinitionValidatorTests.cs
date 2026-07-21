using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        WeaponAttackLineGenerationDefinitionValidatorTests
    {
        [Test]
        public void Validate_NullDefinition_Throws()
        {
            Assert.That(
                () => CreateValidator().Validate(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Validate_EmptyLineId_Throws()
        {
            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    lineId: string.Empty);

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_NegativeMinimumDamage_Throws()
        {
            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    minDamage: -1);

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_MaximumBelowMinimum_Throws()
        {
            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    minDamage: 5,
                    maxDamage: 4);

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_EmptyAllowedElements_Throws()
        {
            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    allowedElements:
                        Array.Empty<WeaponAttackElement>());

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_UnsupportedElement_Throws()
        {
            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    allowedElements:
                        new[]
                        {
                            (WeaponAttackElement)999
                        });

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_DuplicateElement_Throws()
        {
            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    allowedElements:
                        new[]
                        {
                            WeaponAttackElement.Fire,
                            WeaponAttackElement.Fire
                        });

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_ValidDefinition_DoesNotThrow()
        {
            Assert.That(
                () => CreateValidator().Validate(
                    CreateDefinition()),
                Throws.Nothing);
        }

        private static void AssertInvalid(
            WeaponAttackLineGenerationDefinition definition)
        {
            Assert.That(
                () => CreateValidator().Validate(definition),
                Throws.TypeOf<InvalidOperationException>());
        }

        private static
            WeaponAttackLineGenerationDefinition
            CreateDefinition(
                string lineId = "primary_damage",
                int minDamage = 4,
                int maxDamage = 8,
                WeaponAttackElement[] allowedElements = null)
        {
            return new WeaponAttackLineGenerationDefinition(
                lineId,
                minDamage,
                maxDamage,
                allowedElements ??
                new[]
                {
                    WeaponAttackElement.Neutral,
                    WeaponAttackElement.Fire
                });
        }

        private static
            WeaponAttackLineGenerationDefinitionValidator
            CreateValidator()
        {
            return new
                WeaponAttackLineGenerationDefinitionValidator();
        }
    }
}