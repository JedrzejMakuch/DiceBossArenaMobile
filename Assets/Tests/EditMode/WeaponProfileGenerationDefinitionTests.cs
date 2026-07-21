using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        WeaponProfileGenerationDefinitionTests
    {
        [Test]
        public void Constructor_StoresLinesInOrder()
        {
            WeaponAttackLineGenerationDefinition primary =
                CreateLine("primary_damage");

            WeaponAttackLineGenerationDefinition secondary =
                CreateLine("secondary_damage");

            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    new[]
                    {
                        primary,
                        secondary
                    });

            Assert.That(
                definition.Lines,
                Is.EqualTo(
                    new[]
                    {
                        primary,
                        secondary
                    }));
        }

        [Test]
        public void Constructor_CopiesLines()
        {
            WeaponAttackLineGenerationDefinition primary =
                CreateLine("primary_damage");

            WeaponAttackLineGenerationDefinition secondary =
                CreateLine("secondary_damage");

            WeaponAttackLineGenerationDefinition[] lines =
            {
                primary
            };

            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    lines);

            lines[0] =
                secondary;

            Assert.That(
                definition.Lines[0],
                Is.SameAs(primary));
        }

        [Test]
        public void Constructor_NullLines_CreatesEmptyCollection()
        {
            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    null);

            Assert.That(
                definition.Lines,
                Is.Empty);
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
    }
}