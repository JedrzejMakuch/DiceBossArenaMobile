using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackEffectLineResultTests
{
    [Test]
    public void Constructor_NullDamageLine_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackEffectLineResult(
                null,
                Array.Empty<
                    WeaponAttackEffectTriggerResult>()));
    }

    [Test]
    public void Constructor_StoresDamageLine()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine();

        WeaponAttackEffectLineResult result =
            new WeaponAttackEffectLineResult(
                damageLine,
                null);

        Assert.That(
            result.DamageLine,
            Is.SameAs(damageLine));
    }

    [Test]
    public void Constructor_StoresEffectResults()
    {
        WeaponAttackEffectTriggerResult effectResult =
            CreateTriggeredEffectResult();

        WeaponAttackEffectLineResult result =
            new WeaponAttackEffectLineResult(
                CreateDamageLine(),
                new[]
                {
                    effectResult
                });

        Assert.That(
            result.EffectResults.Count,
            Is.EqualTo(1));

        Assert.That(
            result.EffectResults[0],
            Is.SameAs(effectResult));
    }

    [Test]
    public void Constructor_CopiesEffectResultsCollection()
    {
        WeaponAttackEffectTriggerResult firstResult =
            CreateTriggeredEffectResult();

        WeaponAttackEffectTriggerResult secondResult =
            WeaponAttackEffectTriggerResult.NotTriggered(
                CreateEffectDefinition());

        WeaponAttackEffectTriggerResult[] effectResults =
        {
            firstResult
        };

        WeaponAttackEffectLineResult result =
            new WeaponAttackEffectLineResult(
                CreateDamageLine(),
                effectResults);

        effectResults[0] =
            secondResult;

        Assert.That(
            result.EffectResults[0],
            Is.SameAs(firstResult));
    }

    [Test]
    public void Constructor_NullEffectResults_CreatesEmptyCollection()
    {
        WeaponAttackEffectLineResult result =
            new WeaponAttackEffectLineResult(
                CreateDamageLine(),
                null);

        Assert.That(
            result.EffectResults,
            Is.Not.Null);

        Assert.That(
            result.EffectResults,
            Is.Empty);
    }

    private static WeaponAttackDamageLineResult
        CreateDamageLine()
    {
        return new WeaponAttackDamageLineResult(
            new WeaponAttackLineId(
                "primary_damage"),
            WeaponAttackElement.Fire,
            6);
    }

    private static WeaponAttackEffectTriggerResult
        CreateTriggeredEffectResult()
    {
        return WeaponAttackEffectTriggerResult.Triggered(
            CreateEffectDefinition());
    }

    private static WeaponAttackEffectDefinition
        CreateEffectDefinition()
    {
        return new WeaponAttackEffectDefinition(
            WeaponAttackEffectType.LifeSteal,
            50,
            null,
            25);
    }
}