using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        StatusEffectStatModifierDefinitionTests
    {
        [Test]
        public void Constructor_AssignsValues()
        {
            StatusEffectStatModifierDefinition definition =
                new StatusEffectStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    -2);

            Assert.That(
                definition.StatType,
                Is.EqualTo(
                    FightStatType.AttackPower));

            Assert.That(
                definition.ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));

            Assert.That(
                definition.ValuePerStack,
                Is.EqualTo(-2));
        }

        [Test]
        public void CreateModifier_UsesSingleStack()
        {
            StatusEffectStatModifierDefinition definition =
                new StatusEffectStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    3);

            FightStatModifier modifier =
                definition.CreateModifier(
                    1);

            Assert.That(
                modifier.StatType,
                Is.EqualTo(
                    FightStatType.AttackPower));

            Assert.That(
                modifier.ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));

            Assert.That(
                modifier.Value,
                Is.EqualTo(3));
        }

        [Test]
        public void CreateModifier_MultipliesValueByStacks()
        {
            StatusEffectStatModifierDefinition definition =
                new StatusEffectStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    -2);

            FightStatModifier modifier =
                definition.CreateModifier(
                    3);

            Assert.That(
                modifier.Value,
                Is.EqualTo(-6));
        }

        [Test]
        public void CreateModifier_ZeroStacksUsesOneStack()
        {
            StatusEffectStatModifierDefinition definition =
                new StatusEffectStatModifierDefinition(
                    FightStatType.Initiative,
                    FightStatModifierType.Flat,
                    4);

            FightStatModifier modifier =
                definition.CreateModifier(
                    0);

            Assert.That(
                modifier.Value,
                Is.EqualTo(4));
        }
    }
}