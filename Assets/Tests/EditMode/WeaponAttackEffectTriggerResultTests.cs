using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackEffectTriggerResultTests
{
    [Test]
    public void Constructor_AssignsValues()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition();

        WeaponAttackEffectTriggerResult result =
            new WeaponAttackEffectTriggerResult(
                definition,
                true);

        Assert.That(
            result.Definition,
            Is.SameAs(definition));

        Assert.That(
            result.IsTriggered,
            Is.True);
    }

    [Test]
    public void Triggered_CreatesTriggeredResult()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition();

        WeaponAttackEffectTriggerResult result =
            WeaponAttackEffectTriggerResult.Triggered(
                definition);

        Assert.That(
            result.Definition,
            Is.SameAs(definition));

        Assert.That(
            result.IsTriggered,
            Is.True);
    }

    [Test]
    public void NotTriggered_CreatesNotTriggeredResult()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition();

        WeaponAttackEffectTriggerResult result =
            WeaponAttackEffectTriggerResult.NotTriggered(
                definition);

        Assert.That(
            result.Definition,
            Is.SameAs(definition));

        Assert.That(
            result.IsTriggered,
            Is.False);
    }

    [Test]
    public void Constructor_AllowsNullDefinition()
    {
        WeaponAttackEffectTriggerResult result =
            new WeaponAttackEffectTriggerResult(
                null,
                false);

        Assert.That(
            result.Definition,
            Is.Null);

        Assert.That(
            result.IsTriggered,
            Is.False);
    }

    private static WeaponAttackEffectDefinition CreateDefinition()
    {
        return new WeaponAttackEffectDefinition(
            WeaponAttackEffectType.LifeSteal,
            50,
            null,
            25);
    }
}