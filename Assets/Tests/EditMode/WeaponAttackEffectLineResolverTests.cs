using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackEffectLineResolverTests
{
    [Test]
    public void Constructor_NullEffectsTriggerResolver_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackEffectLineResolver(
                null));
    }

    [Test]
    public void Resolve_NullAttackLine_Throws()
    {
        WeaponAttackEffectLineResolver resolver =
            CreateResolver(0);

        Assert.Throws<ArgumentNullException>(
            () => resolver.Resolve(
                null,
                CreateDamageLine()));
    }

    [Test]
    public void Resolve_NullDamageLine_Throws()
    {
        WeaponAttackEffectLineResolver resolver =
            CreateResolver(0);

        Assert.Throws<ArgumentNullException>(
            () => resolver.Resolve(
                CreateAttackLine(),
                null));
    }

    [Test]
    public void Resolve_DifferentLineIds_Throws()
    {
        RolledWeaponAttackLine attackLine =
            CreateAttackLine();

        WeaponAttackDamageLineResult damageLine =
            new WeaponAttackDamageLineResult(
                new WeaponAttackLineId(
                    "secondary_damage"),
                WeaponAttackElement.Fire,
                6);

        WeaponAttackEffectLineResolver resolver =
            CreateResolver(0);

        Assert.Throws<InvalidOperationException>(
            () => resolver.Resolve(
                attackLine,
                damageLine));
    }

    [Test]
    public void Resolve_ReturnsDamageLine()
    {
        RolledWeaponAttackLine attackLine =
            CreateAttackLine();

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine();

        WeaponAttackEffectLineResolver resolver =
            CreateResolver(0);

        WeaponAttackEffectLineResult result =
            resolver.Resolve(
                attackLine,
                damageLine);

        Assert.That(
            result.DamageLine,
            Is.SameAs(damageLine));
    }

    [Test]
    public void Resolve_ResolvesAllAttackLineEffects()
    {
        WeaponAttackEffectDefinition triggeredEffect =
            new WeaponAttackEffectDefinition(
                WeaponAttackEffectType.LifeSteal,
                100,
                null,
                25);

        WeaponAttackEffectDefinition notTriggeredEffect =
            new WeaponAttackEffectDefinition(
                WeaponAttackEffectType.LifeSteal,
                0,
                null,
                25);

        RolledWeaponAttackLine attackLine =
            new RolledWeaponAttackLine(
                new WeaponAttackLineId(
                    "primary_damage"),
                WeaponAttackElement.Fire,
                4,
                8,
                new[]
                {
                    triggeredEffect,
                    notTriggeredEffect
                });

        WeaponAttackEffectLineResolver resolver =
            CreateResolver(50);

        WeaponAttackEffectLineResult result =
            resolver.Resolve(
                attackLine,
                CreateDamageLine());

        Assert.That(
            result.EffectResults.Count,
            Is.EqualTo(2));

        Assert.That(
            result.EffectResults[0].Definition,
            Is.SameAs(triggeredEffect));

        Assert.That(
            result.EffectResults[0].IsTriggered,
            Is.True);

        Assert.That(
            result.EffectResults[1].Definition,
            Is.SameAs(notTriggeredEffect));

        Assert.That(
            result.EffectResults[1].IsTriggered,
            Is.False);
    }

    [Test]
    public void Resolve_NoEffects_ReturnsEmptyEffectResults()
    {
        RolledWeaponAttackLine attackLine =
            new RolledWeaponAttackLine(
                new WeaponAttackLineId(
                    "primary_damage"),
                WeaponAttackElement.Fire,
                4,
                8);

        WeaponAttackEffectLineResolver resolver =
            CreateResolver(0);

        WeaponAttackEffectLineResult result =
            resolver.Resolve(
                attackLine,
                CreateDamageLine());

        Assert.That(
            result.EffectResults,
            Is.Empty);
    }

    private static WeaponAttackEffectLineResolver
        CreateResolver(
            int randomValue)
    {
        WeaponAttackEffectTriggerResolver effectResolver =
            new WeaponAttackEffectTriggerResolver(
                new FakeRandomSource(
                    randomValue));

        WeaponAttackEffectsTriggerResolver
            effectsResolver =
                new WeaponAttackEffectsTriggerResolver(
                    effectResolver);

        return new WeaponAttackEffectLineResolver(
            effectsResolver);
    }

    private static RolledWeaponAttackLine
        CreateAttackLine()
    {
        return new RolledWeaponAttackLine(
            new WeaponAttackLineId(
                "primary_damage"),
            WeaponAttackElement.Fire,
            4,
            8,
            new[]
            {
                new WeaponAttackEffectDefinition(
                    WeaponAttackEffectType.LifeSteal,
                    100,
                    null,
                    25)
            });
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