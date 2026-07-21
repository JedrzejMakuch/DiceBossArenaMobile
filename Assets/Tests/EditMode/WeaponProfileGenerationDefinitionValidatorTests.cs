using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        WeaponProfileGenerationDefinitionValidatorTests
    {
        [Test]
        public void Constructor_NullLineValidator_Throws()
        {
            Assert.That(
                () => new
                    WeaponProfileGenerationDefinitionValidator(
                        null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Validate_NullDefinition_Throws()
        {
            Assert.That(
                () => CreateValidator().Validate(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Validate_EmptyLines_Throws()
        {
            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    Array.Empty<
                        WeaponAttackLineGenerationDefinition>());

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_NullLine_Throws()
        {
            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    new WeaponAttackLineGenerationDefinition[]
                    {
                        null
                    });

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_InvalidLine_Throws()
        {
            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    new[]
                    {
                        new WeaponAttackLineGenerationDefinition(
                            "primary_damage",
                            4,
                            8,
                            Array.Empty<WeaponAttackElement>())
                    });

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_DuplicateLineId_Throws()
        {
            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    new[]
                    {
                        CreateLine("primary_damage"),
                        CreateLine("primary_damage")
                    });

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_ValidDefinition_DoesNotThrow()
        {
            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    new[]
                    {
                        CreateLine("primary_damage"),
                        CreateLine("secondary_damage")
                    });

            Assert.That(
                () => CreateValidator().Validate(definition),
                Throws.Nothing);
        }

        private static void AssertInvalid(
            WeaponProfileGenerationDefinition definition)
        {
            Assert.That(
                () => CreateValidator().Validate(definition),
                Throws.TypeOf<InvalidOperationException>());
        }

        private static
            WeaponAttackLineGenerationDefinition
            CreateLine(string lineId)
        {
            return new WeaponAttackLineGenerationDefinition(
                lineId,
                4,
                8,
                new[]
                {
                    WeaponAttackElement.Neutral,
                    WeaponAttackElement.Fire
                });
        }

        private static
            WeaponProfileGenerationDefinitionValidator
            CreateValidator()
        {
            return new
                WeaponProfileGenerationDefinitionValidator(
                    new
                        WeaponAttackLineGenerationDefinitionValidator());
        }
    }
}