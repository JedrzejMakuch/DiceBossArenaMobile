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
            WeaponAttackLineGenerationDefinition definition =
                new WeaponAttackLineGenerationDefinition(
                    "primary_damage",
                    4,
                    8,
                    new[]
                    {
                        WeaponAttackElement.Fire,
                        WeaponAttackElement.Water
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
    }
}