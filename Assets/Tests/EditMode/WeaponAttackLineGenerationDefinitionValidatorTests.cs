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
        public void Constructor_NullEffectValidator_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                    new WeaponAttackLineGenerationDefinitionValidator(
                        null));
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
        public void Validate_ValidEffect_DoesNotThrow()
        {
            WeaponAttackEffectDefinition effect =
                new WeaponAttackEffectDefinition(
                    WeaponAttackEffectType.LifeSteal,
                    50,
                    null,
                    25);

            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    effects:
                        new[]
                        {
                            effect
                        });

            Assert.That(
                () => CreateValidator().Validate(definition),
                Throws.Nothing);
        }

        [Test]
        public void Validate_NullEffectEntry_Throws()
        {
            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    effects:
                        new WeaponAttackEffectDefinition[]
                        {
                            null
                        });

            AssertInvalid(definition);
        }

        [Test]
        public void Validate_InvalidEffect_Throws()
        {
            WeaponAttackEffectDefinition effect =
                new WeaponAttackEffectDefinition(
                    WeaponAttackEffectType.LifeSteal,
                    50,
                    null,
                    0);

            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    effects:
                        new[]
                        {
                            effect
                        });

            InvalidOperationException exception =
                Assert.Throws<InvalidOperationException>(
                    () =>
                        CreateValidator().Validate(
                            definition));

            Assert.That(
                exception.InnerException,
                Is.Not.Null);

            Assert.That(
                exception.InnerException,
                Is.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Validate_EmptyEffects_DoesNotThrow()
        {
            WeaponAttackLineGenerationDefinition definition =
                CreateDefinition(
                    effects:
                        Array.Empty<
                            WeaponAttackEffectDefinition>());

            Assert.That(
                () => CreateValidator().Validate(definition),
                Throws.Nothing);
        }

        [Test]
        public void Validate_ValidDefinition_DoesNotThrow()
        {
            Assert.That(
                () =>
                    CreateValidator().Validate(
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
                WeaponAttackElement[] allowedElements = null,
                WeaponAttackEffectDefinition[] effects = null)
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
                },
                effects ??
                Array.Empty<
                    WeaponAttackEffectDefinition>());
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