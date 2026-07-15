using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        CharacterStatModifierDefinitionTests
    {
        [Test]
        public void CreateModifier_CopiesDefinitionValues()
        {
            CharacterStatModifierDefinition definition =
                new CharacterStatModifierDefinition(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10);

            FightStatModifier modifier =
                definition.CreateModifier();

            Assert.That(
                modifier.StatType,
                Is.EqualTo(
                    FightStatType.MaxHealth));

            Assert.That(
                modifier.ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));

            Assert.That(
                modifier.Value,
                Is.EqualTo(10));
        }

        [Test]
        public void CreateModifier_PreservesNegativeValue()
        {
            CharacterStatModifierDefinition definition =
                new CharacterStatModifierDefinition(
                    FightStatType.Initiative,
                    FightStatModifierType.Percent,
                    -15);

            FightStatModifier modifier =
                definition.CreateModifier();

            Assert.That(
                modifier.Value,
                Is.EqualTo(-15));
        }
    }
}