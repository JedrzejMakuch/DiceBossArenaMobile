using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    WeaponAttackLifeStealCalculatorTests
{
    [Test]
    public void Calculate_NullEffectResult_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => CreateCalculator().Calculate(
                null,
                CreateDamageLine(8)));
    }

    [Test]
    public void Calculate_NullDamageLine_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => CreateCalculator().Calculate(
                CreateTriggeredResult(25),
                null));
    }

    [Test]
    public void Calculate_NotTriggered_ReturnsZero()
    {
        WeaponAttackEffectDefinition definition =
            CreateLifeStealDefinition(25);

        WeaponAttackEffectTriggerResult result =
            WeaponAttackEffectTriggerResult
                .NotTriggered(
                    definition);

        Assert.That(
            CreateCalculator().Calculate(
                result,
                CreateDamageLine(8)),
            Is.EqualTo(0));
    }

    [Test]
    public void Calculate_NonLifeStealEffect_Throws()
    {
        WeaponAttackEffectDefinition definition =
            new WeaponAttackEffectDefinition(
                WeaponAttackEffectType.None,
                0,
                null,
                0);

        WeaponAttackEffectTriggerResult result =
            WeaponAttackEffectTriggerResult
                .Triggered(
                    definition);

        Assert.Throws<InvalidOperationException>(
            () => CreateCalculator().Calculate(
                result,
                CreateDamageLine(8)));
    }

    [Test]
    public void Calculate_ZeroDamage_ReturnsZero()
    {
        Assert.That(
            CreateCalculator().Calculate(
                CreateTriggeredResult(25),
                CreateDamageLine(0)),
            Is.EqualTo(0));
    }

    [Test]
    public void Calculate_ReturnsPercentageOfDamage()
    {
        Assert.That(
            CreateCalculator().Calculate(
                CreateTriggeredResult(25),
                CreateDamageLine(8)),
            Is.EqualTo(2));
    }

    [Test]
    public void Calculate_RoundsDown()
    {
        Assert.That(
            CreateCalculator().Calculate(
                CreateTriggeredResult(25),
                CreateDamageLine(7)),
            Is.EqualTo(1));
    }

    [Test]
    public void Calculate_OneHundredPercent_ReturnsFullDamage()
    {
        Assert.That(
            CreateCalculator().Calculate(
                CreateTriggeredResult(100),
                CreateDamageLine(7)),
            Is.EqualTo(7));
    }

    private static WeaponAttackLifeStealCalculator
        CreateCalculator()
    {
        return new WeaponAttackLifeStealCalculator();
    }

    private static WeaponAttackEffectTriggerResult
        CreateTriggeredResult(
            int lifeStealPercent)
    {
        return WeaponAttackEffectTriggerResult
            .Triggered(
                CreateLifeStealDefinition(
                    lifeStealPercent));
    }

    private static WeaponAttackEffectDefinition
        CreateLifeStealDefinition(
            int lifeStealPercent)
    {
        return new WeaponAttackEffectDefinition(
            WeaponAttackEffectType.LifeSteal,
            100,
            null,
            lifeStealPercent);
    }

    private static WeaponAttackDamageLineResult
        CreateDamageLine(
            int damage)
    {
        return new WeaponAttackDamageLineResult(
            new WeaponAttackLineId(
                "primary_damage"),
            WeaponAttackElement.Neutral,
            damage);
    }
}