using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitDefinitionIdTests
    {
        [Test]
        public void Constructor_TrimsValue()
        {
            FightUnitDefinitionId id =
                new FightUnitDefinitionId(
                    "  enemy_slime  ");

            Assert.That(
                id.Value,
                Is.EqualTo("enemy_slime"));
        }

        [Test]
        public void EmptyValue_IsInvalid()
        {
            FightUnitDefinitionId id =
                new FightUnitDefinitionId(
                    "   ");

            Assert.That(
                id.IsValid,
                Is.False);
        }

        [Test]
        public void EqualValues_AreEqual()
        {
            FightUnitDefinitionId first =
                new FightUnitDefinitionId(
                    "enemy_slime");

            FightUnitDefinitionId second =
                new FightUnitDefinitionId(
                    "enemy_slime");

            Assert.That(
                first,
                Is.EqualTo(second));

            Assert.That(
                first == second,
                Is.True);
        }

        [Test]
        public void Comparison_IsCaseSensitive()
        {
            FightUnitDefinitionId first =
                new FightUnitDefinitionId(
                    "enemy_slime");

            FightUnitDefinitionId second =
                new FightUnitDefinitionId(
                    "Enemy_Slime");

            Assert.That(
                first,
                Is.Not.EqualTo(second));
        }
    }
}