using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackResistanceResolverTests
{
    private WeaponAttackResistanceResolver resolver;

    [SetUp]
    public void SetUp()
    {
        resolver =
            new WeaponAttackResistanceResolver();
    }

    [Test]
    public void Resolve_FireDamage_AppliesFireResistance()
    {
        FightUnitStats stats =
            CreateStats(
                FightStatType.FireResistance,
                25);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                100);

        int result = resolver.Resolve(
            damageLine,
            stats,
            damageLine.Damage);

        Assert.That(result, Is.EqualTo(75));
    }

    [Test]
    public void Resolve_NeutralDamage_ReturnsRawDamage()
    {
        FightUnitStats stats =
            CreateStats(
                FightStatType.FireResistance,
                100);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Neutral,
                100);

        int result = resolver.Resolve(
            damageLine,
            stats,
            damageLine.Damage);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Resolve_NullStats_ReturnsRawDamage()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                100);

        int result = resolver.Resolve(
            damageLine,
            null,
            100);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Resolve_NullDamageLine_ReturnsZero()
    {
        int result = resolver.Resolve(
            null,
            CreateStats(),
            100);

        Assert.That(result, Is.Zero);
    }

    [Test]
    public void Resolve_NonPositiveDamage_ReturnsZero()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                0);

        int result = resolver.Resolve(
            damageLine,
            CreateStats(),
            0);

        Assert.That(result, Is.Zero);
    }

    private static FightUnitStats CreateStats(
        FightStatType? statType = null,
        int value = 0)
    {
        Dictionary<FightStatType, int> values =
            new();

        if (statType.HasValue)
        {
            values.Add(
                statType.Value,
                value);
        }

        return new FightUnitStats(values);
    }

    private static WeaponAttackDamageLineResult CreateDamageLine(
    WeaponAttackElement element,
    int damage,
    bool isTrueDamage = false)
    {
        return new WeaponAttackDamageLineResult(
            new WeaponAttackLineId("test_line"),
            element,
            damage,
            isTrueDamage);
    }
}