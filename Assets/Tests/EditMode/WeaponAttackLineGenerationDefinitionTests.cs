using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        WeaponAttackLineGenerationDefinitionTests
    {
        [Test]
        public void Constructor_StoresGenerationRules()
        {
            WeaponAttackEffectDefinition effect =
                CreateEffect();

            WeaponAttackLineGenerationDefinition definition =
                new WeaponAttackLineGenerationDefinition(
                    "primary_damage",
                    4,
                    8,
                    new[]
                    {
                        WeaponAttackElement.Fire,
                        WeaponAttackElement.Water
                    },
                    new[]
                    {
                        effect
                    });

            Assert.That(
                definition.LineId,
                Is.EqualTo(
                    new WeaponAttackLineId(
                        "primary_damage")));

            Assert.That(
                definition.MinDamage,
                Is.EqualTo(4));

            Assert.That(
                definition.MaxDamage,
                Is.EqualTo(8));

            Assert.That(
                definition.AllowedElements,
                Is.EqualTo(
                    new[]
                    {
                        WeaponAttackElement.Fire,
                        WeaponAttackElement.Water
                    }));

            Assert.That(
                definition.Effects.Count,
                Is.EqualTo(1));

            Assert.That(
                definition.Effects[0],
                Is.SameAs(effect));
        }

        [Test]
        public void Constructor_CopiesAllowedElements()
        {
            WeaponAttackElement[] elements =
            {
                WeaponAttackElement.Fire
            };

            WeaponAttackLineGenerationDefinition definition =
                new WeaponAttackLineGenerationDefinition(
                    "primary_damage",
                    4,
                    8,
                    elements);

            elements[0] =
                WeaponAttackElement.Water;

            Assert.That(
                definition.AllowedElements[0],
                Is.EqualTo(
                    WeaponAttackElement.Fire));
        }

        [Test]
        public void Constructor_CopiesEffects()
        {
            WeaponAttackEffectDefinition firstEffect =
                CreateEffect();

            WeaponAttackEffectDefinition secondEffect =
                new WeaponAttackEffectDefinition(
                    WeaponAttackEffectType.LifeSteal,
                    75,
                    null,
                    40);

            WeaponAttackEffectDefinition[] effects =
            {
                firstEffect
            };

            WeaponAttackLineGenerationDefinition definition =
                new WeaponAttackLineGenerationDefinition(
                    "primary_damage",
                    4,
                    8,
                    new[]
                    {
                        WeaponAttackElement.Fire
                    },
                    effects);

            effects[0] =
                secondEffect;

            Assert.That(
                definition.Effects.Count,
                Is.EqualTo(1));

            Assert.That(
                definition.Effects[0],
                Is.SameAs(firstEffect));
        }

        [Test]
        public void Constructor_NullElements_CreatesEmptyCollection()
        {
            WeaponAttackLineGenerationDefinition definition =
                new WeaponAttackLineGenerationDefinition(
                    "primary_damage",
                    4,
                    8,
                    null);

            Assert.That(
                definition.AllowedElements,
                Is.Empty);
        }

        [Test]
        public void Constructor_NullEffects_CreatesEmptyCollection()
        {
            WeaponAttackLineGenerationDefinition definition =
                new WeaponAttackLineGenerationDefinition(
                    "primary_damage",
                    4,
                    8,
                    new[]
                    {
                        WeaponAttackElement.Fire
                    },
                    null);

            Assert.That(
                definition.Effects,
                Is.Not.Null);

            Assert.That(
                definition.Effects,
                Is.Empty);
        }

        [Test]
        public void Constructor_OmittedEffects_CreatesEmptyCollection()
        {
            WeaponAttackLineGenerationDefinition definition =
                new WeaponAttackLineGenerationDefinition(
                    "primary_damage",
                    4,
                    8,
                    new[]
                    {
                        WeaponAttackElement.Fire
                    });

            Assert.That(
                definition.Effects,
                Is.Not.Null);

            Assert.That(
                definition.Effects,
                Is.Empty);
        }

        private static WeaponAttackEffectDefinition CreateEffect()
        {
            return new WeaponAttackEffectDefinition(
                WeaponAttackEffectType.LifeSteal,
                50,
                null,
                25);
        }
    }
}