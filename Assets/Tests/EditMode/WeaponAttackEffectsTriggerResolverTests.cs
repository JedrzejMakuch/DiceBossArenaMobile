using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackEffectsTriggerResolverTests
{
    [Test]
    public void Constructor_NullEffectResolver_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackEffectsTriggerResolver(
                null));
    }

    [Test]
    public void Resolve_NullDefinitions_ReturnsEmptyList()
    {
        WeaponAttackEffectsTriggerResolver resolver =
            CreateResolver(0);

        IReadOnlyList<WeaponAttackEffectTriggerResult> results =
            resolver.Resolve(null);

        Assert.That(
            results,
            Is.Not.Null);

        Assert.That(
            results.Count,
            Is.Zero);
    }

    [Test]
    public void Resolve_EmptyDefinitions_ReturnsEmptyList()
    {
        WeaponAttackEffectsTriggerResolver resolver =
            CreateResolver(0);

        IReadOnlyList<WeaponAttackEffectTriggerResult> results =
            resolver.Resolve(
                Array.Empty<
                    WeaponAttackEffectDefinition>());

        Assert.That(
            results,
            Is.Not.Null);

        Assert.That(
            results.Count,
            Is.Zero);
    }

    [Test]
    public void Resolve_ReturnsResultForEveryDefinition()
    {
        WeaponAttackEffectDefinition firstDefinition =
            CreateDefinition(100);

        WeaponAttackEffectDefinition secondDefinition =
            CreateDefinition(0);

        WeaponAttackEffectsTriggerResolver resolver =
            CreateResolver(50);

        IReadOnlyList<WeaponAttackEffectTriggerResult> results =
            resolver.Resolve(
                new[]
                {
                    firstDefinition,
                    secondDefinition
                });

        Assert.That(
            results.Count,
            Is.EqualTo(2));

        Assert.That(
            results[0].Definition,
            Is.SameAs(firstDefinition));

        Assert.That(
            results[0].IsTriggered,
            Is.True);

        Assert.That(
            results[1].Definition,
            Is.SameAs(secondDefinition));

        Assert.That(
            results[1].IsTriggered,
            Is.False);
    }

    [Test]
    public void Resolve_PreservesDefinitionOrder()
    {
        WeaponAttackEffectDefinition firstDefinition =
            CreateDefinition(0);

        WeaponAttackEffectDefinition secondDefinition =
            CreateDefinition(100);

        WeaponAttackEffectDefinition thirdDefinition =
            CreateDefinition(0);

        WeaponAttackEffectsTriggerResolver resolver =
            CreateResolver(50);

        IReadOnlyList<WeaponAttackEffectTriggerResult> results =
            resolver.Resolve(
                new[]
                {
                    firstDefinition,
                    secondDefinition,
                    thirdDefinition
                });

        Assert.That(
            results[0].Definition,
            Is.SameAs(firstDefinition));

        Assert.That(
            results[1].Definition,
            Is.SameAs(secondDefinition));

        Assert.That(
            results[2].Definition,
            Is.SameAs(thirdDefinition));
    }

    [Test]
    public void Resolve_NullEntry_ReturnsNotTriggeredResult()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(100);

        WeaponAttackEffectsTriggerResolver resolver =
            CreateResolver(0);

        IReadOnlyList<WeaponAttackEffectTriggerResult> results =
            resolver.Resolve(
                new WeaponAttackEffectDefinition[]
                {
                    definition,
                    null
                });

        Assert.That(
            results.Count,
            Is.EqualTo(2));

        Assert.That(
            results[0].IsTriggered,
            Is.True);

        Assert.That(
            results[1].Definition,
            Is.Null);

        Assert.That(
            results[1].IsTriggered,
            Is.False);
    }

    private static WeaponAttackEffectsTriggerResolver CreateResolver(
        int randomValue)
    {
        FakeRandomSource randomSource =
            new FakeRandomSource(randomValue);

        WeaponAttackEffectTriggerResolver effectResolver =
            new WeaponAttackEffectTriggerResolver(
                randomSource);

        return new WeaponAttackEffectsTriggerResolver(
            effectResolver);
    }

    private static WeaponAttackEffectDefinition CreateDefinition(
        int triggerChancePercent)
    {
        return new WeaponAttackEffectDefinition(
            WeaponAttackEffectType.LifeSteal,
            triggerChancePercent,
            null,
            25);
    }

    private sealed class FakeRandomSource :
        IWeaponAttackRandomSource
    {
        private readonly int value;

        public FakeRandomSource(
            int newValue)
        {
            value =
                newValue;
        }

        public int Next(
            int minimumInclusive,
            int maximumExclusive)
        {
            return value;
        }
    }
}