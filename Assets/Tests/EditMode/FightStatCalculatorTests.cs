using System.Collections.Generic;
using NUnit.Framework;
using DiceBossArena.Game;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightStatCalculatorTests
    {
        [Test]
        public void Calculate_WithoutModifiers_ReturnsBaseValue()
        {
            int result =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    10,
                    null);

            Assert.That(
                result,
                Is.EqualTo(10));
        }

        [Test]
        public void Calculate_NewFightStatTypes_AreCalculatedCorrectly()
        {
            List<FightStatModifier> modifiers =
                new()
                {
            new FightStatModifier(
                FightStatType.FireResistance,
                FightStatModifierType.Flat,
                15),

            new FightStatModifier(
                FightStatType.FireResistance,
                FightStatModifierType.Percent,
                20)
                };

            int result =
                FightStatCalculator.Calculate(
                    FightStatType.FireResistance,
                    100,
                    modifiers);

            Assert.That(
                result,
                Is.EqualTo(138));
        }

        [Test]
        public void Calculate_DifferentElementStats_DoNotAffectEachOther()
        {
            List<FightStatModifier> modifiers =
                new()
                {
            new FightStatModifier(
                FightStatType.FireDamageBonus,
                FightStatModifierType.Flat,
                25),

            new FightStatModifier(
                FightStatType.WaterDamageBonus,
                FightStatModifierType.Flat,
                50),

            new FightStatModifier(
                FightStatType.FireResistance,
                FightStatModifierType.Flat,
                15)
                };

            Assert.That(
                FightStatCalculator.Calculate(
                    FightStatType.FireDamageBonus,
                    100,
                    modifiers),
                Is.EqualTo(125));

            Assert.That(
                FightStatCalculator.Calculate(
                    FightStatType.WaterDamageBonus,
                    100,
                    modifiers),
                Is.EqualTo(150));

            Assert.That(
                FightStatCalculator.Calculate(
                    FightStatType.FireResistance,
                    100,
                    modifiers),
                Is.EqualTo(115));
        }

        [Test]
        public void Calculate_WithFlatModifier_AddsFlatValue()
        {
            List<FightStatModifier> modifiers =
                new()
                {
                    new FightStatModifier(
                        FightStatType.AttackPower,
                        FightStatModifierType.Flat,
                        5)
                };

            int result =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    10,
                    modifiers);

            Assert.That(
                result,
                Is.EqualTo(15));
        }

        [Test]
        public void Calculate_WithPercentModifier_AppliesPercentValue()
        {
            List<FightStatModifier> modifiers =
                new()
                {
                    new FightStatModifier(
                        FightStatType.AttackPower,
                        FightStatModifierType.Percent,
                        20)
                };

            int result =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    100,
                    modifiers);

            Assert.That(
                result,
                Is.EqualTo(120));
        }

        [Test]
        public void Calculate_AppliesFlatBeforePercent()
        {
            List<FightStatModifier> modifiers =
                new()
                {
                    new FightStatModifier(
                        FightStatType.AttackPower,
                        FightStatModifierType.Flat,
                        20),

                    new FightStatModifier(
                        FightStatType.AttackPower,
                        FightStatModifierType.Percent,
                        50)
                };

            int result =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    100,
                    modifiers);

            Assert.That(
                result,
                Is.EqualTo(180));
        }

        [Test]
        public void Calculate_IgnoresModifiersForOtherStats()
        {
            List<FightStatModifier> modifiers =
                new()
                {
            new FightStatModifier(
                FightStatType.MaxHealth,
                FightStatModifierType.Flat,
                100),

            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                5)
                };

            int result =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    10,
                    modifiers);

            Assert.That(
                result,
                Is.EqualTo(15));
        }

        [Test]
        public void Calculate_ReturnsSameResultRegardlessOfModifierOrder()
        {
            List<FightStatModifier> firstOrder =
                new()
                {
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                20),

            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Percent,
                50),

            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                -5)
                };

            List<FightStatModifier> secondOrder =
                new()
                {
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Percent,
                50),

            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                -5),

            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                20)
                };

            int firstResult =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    100,
                    firstOrder);

            int secondResult =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    100,
                    secondOrder);

            Assert.That(
                firstResult,
                Is.EqualTo(172));

            Assert.That(
                secondResult,
                Is.EqualTo(firstResult));
        }

        [Test]
        public void Calculate_WithNegativePercent_ReducesValue()
        {
            List<FightStatModifier> modifiers =
                new()
                {
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Percent,
                -25)
                };

            int result =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    100,
                    modifiers);

            Assert.That(
                result,
                Is.EqualTo(75));
        }

        [Test]
        public void Calculate_CanReturnNegativeValue()
        {
            List<FightStatModifier> modifiers =
                new()
                {
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                -20)
                };

            int result =
                FightStatCalculator.Calculate(
                    FightStatType.AttackPower,
                    10,
                    modifiers);

            Assert.That(
                result,
                Is.EqualTo(-10));
        }

        [Test]
        public void Calculate_DoesNotModifyModifierCollection()
        {
            List<FightStatModifier> modifiers =
                new()
                {
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                5),

            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Percent,
                20)
                };

            int countBefore = modifiers.Count;

            FightStatCalculator.Calculate(
                FightStatType.AttackPower,
                10,
                modifiers);

            Assert.That(
                modifiers.Count,
                Is.EqualTo(countBefore));

            Assert.That(
                modifiers[0].Value,
                Is.EqualTo(5));

            Assert.That(
                modifiers[1].Value,
                Is.EqualTo(20));
        }
    }
}