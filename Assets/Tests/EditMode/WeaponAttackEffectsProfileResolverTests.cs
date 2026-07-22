using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    WeaponAttackEffectsProfileResolverTests
{
    [Test]
    public void Constructor_NullLineResolver_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () =>
                new WeaponAttackEffectsProfileResolver(
                    null));
    }

    [Test]
    public void Resolve_NullWeaponProfile_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => CreateResolver().Resolve(
                null,
                Array.Empty<
                    WeaponAttackDamageLineResult>()));
    }

    [Test]
    public void Resolve_NullDamageLines_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => CreateResolver().Resolve(
                CreateProfile(),
                null));
    }

    [Test]
    public void Resolve_DifferentLineCounts_Throws()
    {
        Assert.Throws<InvalidOperationException>(
            () => CreateResolver().Resolve(
                CreateProfile(),
                Array.Empty<
                    WeaponAttackDamageLineResult>()));
    }

    [Test]
    public void Resolve_NullDamageLine_Throws()
    {
        WeaponAttackDamageLineResult[] damageLines =
        {
            null,
            CreateDamageLine(
                "secondary_damage",
                WeaponAttackElement.Water,
                4)
        };

        Assert.Throws<InvalidOperationException>(
            () => CreateResolver().Resolve(
                CreateProfile(),
                damageLines));
    }

    [Test]
    public void Resolve_DuplicateDamageLineIds_Throws()
    {
        WeaponAttackDamageLineResult[] damageLines =
        {
            CreateDamageLine(
                "primary_damage",
                WeaponAttackElement.Fire,
                6),

            CreateDamageLine(
                "primary_damage",
                WeaponAttackElement.Water,
                4)
        };

        Assert.Throws<InvalidOperationException>(
            () => CreateResolver().Resolve(
                CreateProfile(),
                damageLines));
    }

    [Test]
    public void Resolve_MissingMatchingLine_Throws()
    {
        WeaponAttackDamageLineResult[] damageLines =
        {
            CreateDamageLine(
                "primary_damage",
                WeaponAttackElement.Fire,
                6),

            CreateDamageLine(
                "different_damage",
                WeaponAttackElement.Water,
                4)
        };

        Assert.Throws<InvalidOperationException>(
            () => CreateResolver().Resolve(
                CreateProfile(),
                damageLines));
    }

    [Test]
    public void Resolve_MatchesLinesById()
    {
        RolledWeaponProfile profile =
            CreateProfile();

        WeaponAttackDamageLineResult secondaryDamage =
            CreateDamageLine(
                "secondary_damage",
                WeaponAttackElement.Water,
                4);

        WeaponAttackDamageLineResult primaryDamage =
            CreateDamageLine(
                "primary_damage",
                WeaponAttackElement.Fire,
                6);

        WeaponAttackEffectLineResult[] results =
            ToArray(
                CreateResolver().Resolve(
                    profile,
                    new[]
                    {
                        secondaryDamage,
                        primaryDamage
                    }));

        Assert.That(
            results.Length,
            Is.EqualTo(2));

        Assert.That(
            results[0].DamageLine,
            Is.SameAs(primaryDamage));

        Assert.That(
            results[1].DamageLine,
            Is.SameAs(secondaryDamage));
    }

    [Test]
    public void Resolve_ResolvesEffectsForEveryLine()
    {
        WeaponAttackEffectDefinition firstEffect =
            CreateEffect(100);

        WeaponAttackEffectDefinition secondEffect =
            CreateEffect(0);

        RolledWeaponProfile profile =
            new RolledWeaponProfile(
                new[]
                {
                    CreateAttackLine(
                        "primary_damage",
                        WeaponAttackElement.Fire,
                        firstEffect),

                    CreateAttackLine(
                        "secondary_damage",
                        WeaponAttackElement.Water,
                        secondEffect)
                });

        WeaponAttackEffectLineResult[] results =
            ToArray(
                CreateResolver().Resolve(
                    profile,
                    new[]
                    {
                        CreateDamageLine(
                            "primary_damage",
                            WeaponAttackElement.Fire,
                            6),

                        CreateDamageLine(
                            "secondary_damage",
                            WeaponAttackElement.Water,
                            4)
                    }));

        Assert.That(
            results[0].EffectResults[0].IsTriggered,
            Is.True);

        Assert.That(
            results[1].EffectResults[0].IsTriggered,
            Is.False);
    }

    private static WeaponAttackEffectsProfileResolver
        CreateResolver()
    {
        WeaponAttackEffectTriggerResolver triggerResolver =
            new WeaponAttackEffectTriggerResolver(
                new FakeRandomSource(50));

        WeaponAttackEffectsTriggerResolver
            effectsTriggerResolver =
                new WeaponAttackEffectsTriggerResolver(
                    triggerResolver);

        WeaponAttackEffectLineResolver lineResolver =
            new WeaponAttackEffectLineResolver(
                effectsTriggerResolver);

        return new WeaponAttackEffectsProfileResolver(
            lineResolver);
    }

    private static RolledWeaponProfile CreateProfile()
    {
        return new RolledWeaponProfile(
            new[]
            {
                CreateAttackLine(
                    "primary_damage",
                    WeaponAttackElement.Fire),

                CreateAttackLine(
                    "secondary_damage",
                    WeaponAttackElement.Water)
            });
    }

    private static RolledWeaponAttackLine
        CreateAttackLine(
            string lineId,
            WeaponAttackElement element,
            WeaponAttackEffectDefinition effect = null)
    {
        return new RolledWeaponAttackLine(
            new WeaponAttackLineId(lineId),
            element,
            4,
            8,
            effect == null
                ? Array.Empty<
                    WeaponAttackEffectDefinition>()
                : new[]
                {
                    effect
                });
    }

    private static WeaponAttackDamageLineResult
        CreateDamageLine(
            string lineId,
            WeaponAttackElement element,
            int damage)
    {
        return new WeaponAttackDamageLineResult(
            new WeaponAttackLineId(lineId),
            element,
            damage);
    }

    private static WeaponAttackEffectDefinition
        CreateEffect(
            int triggerChance)
    {
        return new WeaponAttackEffectDefinition(
            WeaponAttackEffectType.LifeSteal,
            triggerChance,
            null,
            25);
    }

    private static WeaponAttackEffectLineResult[]
        ToArray(
            System.Collections.Generic.IReadOnlyList<
                WeaponAttackEffectLineResult> results)
    {
        WeaponAttackEffectLineResult[] copy =
            new WeaponAttackEffectLineResult[
                results.Count];

        for (int index = 0;
             index < results.Count;
             index++)
        {
            copy[index] =
                results[index];
        }

        return copy;
    }

    private sealed class FakeRandomSource :
        IWeaponAttackRandomSource
    {
        private readonly int value;

        public FakeRandomSource(int newValue)
        {
            value = newValue;
        }

        public int Next(
            int minimumInclusive,
            int maximumExclusive)
        {
            return value;
        }
    }
}