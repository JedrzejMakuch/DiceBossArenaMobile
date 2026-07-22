using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackEffectTriggerResolverTests
{
    [Test]
    public void Constructor_NullRandomSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackEffectTriggerResolver(
                null));
    }

    [Test]
    public void Resolve_NullDefinition_ReturnsNotTriggeredResult()
    {
        FakeRandomSource randomSource =
            new FakeRandomSource(0);

        WeaponAttackEffectTriggerResolver resolver =
            new WeaponAttackEffectTriggerResolver(
                randomSource);

        WeaponAttackEffectTriggerResult result =
            resolver.Resolve(null);

        Assert.That(
            result,
            Is.Not.Null);

        Assert.That(
            result.IsTriggered,
            Is.False);

        Assert.That(
            result.Definition,
            Is.Null);

        Assert.That(
            randomSource.CallCount,
            Is.Zero);
    }

    [Test]
    public void Resolve_ZeroChance_ReturnsNotTriggeredWithoutRoll()
    {
        FakeRandomSource randomSource =
            new FakeRandomSource(0);

        WeaponAttackEffectTriggerResolver resolver =
            new WeaponAttackEffectTriggerResolver(
                randomSource);

        WeaponAttackEffectDefinition definition =
            CreateDefinition(0);

        WeaponAttackEffectTriggerResult result =
            resolver.Resolve(definition);

        Assert.That(
            result,
            Is.Not.Null);

        Assert.That(
            result.IsTriggered,
            Is.False);

        Assert.That(
            result.Definition,
            Is.SameAs(definition));

        Assert.That(
            randomSource.CallCount,
            Is.Zero);
    }

    [Test]
    public void Resolve_OneHundredChance_ReturnsTriggeredWithoutRoll()
    {
        FakeRandomSource randomSource =
            new FakeRandomSource(99);

        WeaponAttackEffectTriggerResolver resolver =
            new WeaponAttackEffectTriggerResolver(
                randomSource);

        WeaponAttackEffectDefinition definition =
            CreateDefinition(100);

        WeaponAttackEffectTriggerResult result =
            resolver.Resolve(definition);

        Assert.That(
            result,
            Is.Not.Null);

        Assert.That(
            result.IsTriggered,
            Is.True);

        Assert.That(
            result.Definition,
            Is.SameAs(definition));

        Assert.That(
            randomSource.CallCount,
            Is.Zero);
    }

    [TestCase(0)]
    [TestCase(34)]
    public void Resolve_RollBelowChance_ReturnsTriggeredResult(
        int roll)
    {
        FakeRandomSource randomSource =
            new FakeRandomSource(roll);

        WeaponAttackEffectTriggerResolver resolver =
            new WeaponAttackEffectTriggerResolver(
                randomSource);

        WeaponAttackEffectDefinition definition =
            CreateDefinition(35);

        WeaponAttackEffectTriggerResult result =
            resolver.Resolve(definition);

        Assert.That(
            result,
            Is.Not.Null);

        Assert.That(
            result.IsTriggered,
            Is.True);

        Assert.That(
            result.Definition,
            Is.SameAs(definition));

        Assert.That(
            randomSource.CallCount,
            Is.EqualTo(1));

        Assert.That(
            randomSource.LastMinimum,
            Is.Zero);

        Assert.That(
            randomSource.LastMaximum,
            Is.EqualTo(100));
    }

    [TestCase(35)]
    [TestCase(99)]
    public void Resolve_RollAtOrAboveChance_ReturnsNotTriggeredResult(
        int roll)
    {
        FakeRandomSource randomSource =
            new FakeRandomSource(roll);

        WeaponAttackEffectTriggerResolver resolver =
            new WeaponAttackEffectTriggerResolver(
                randomSource);

        WeaponAttackEffectDefinition definition =
            CreateDefinition(35);

        WeaponAttackEffectTriggerResult result =
            resolver.Resolve(definition);

        Assert.That(
            result,
            Is.Not.Null);

        Assert.That(
            result.IsTriggered,
            Is.False);

        Assert.That(
            result.Definition,
            Is.SameAs(definition));

        Assert.That(
            randomSource.CallCount,
            Is.EqualTo(1));

        Assert.That(
            randomSource.LastMinimum,
            Is.Zero);

        Assert.That(
            randomSource.LastMaximum,
            Is.EqualTo(100));
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

        public int CallCount { get; private set; }

        public int LastMinimum { get; private set; }

        public int LastMaximum { get; private set; }

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
            CallCount++;

            LastMinimum =
                minimumInclusive;

            LastMaximum =
                maximumExclusive;

            return value;
        }
    }
}