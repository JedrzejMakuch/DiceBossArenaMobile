using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackArmorResolverTests
{
    private WeaponAttackArmorResolver resolver;

    [SetUp]
    public void SetUp()
    {
        resolver =
            new WeaponAttackArmorResolver();
    }

    [Test]
    public void Resolve_DamageWithArmor_AppliesFlatReduction()
    {
        FightUnitStats stats =
            CreateStats(
                FightStatType.Armor,
                25);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine();

        int result = resolver.Resolve(
            damageLine,
            stats,
            100);

        Assert.That(result, Is.EqualTo(75));
    }

    [Test]
    public void Resolve_NoArmor_ReturnsRawDamage()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine();

        int result = resolver.Resolve(
            damageLine,
            CreateStats(),
            100);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Resolve_ArmorAboveDamage_ReturnsZero()
    {
        FightUnitStats stats =
            CreateStats(
                FightStatType.Armor,
                150);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine();

        int result = resolver.Resolve(
            damageLine,
            stats,
            100);

        Assert.That(result, Is.Zero);
    }

    [Test]
    public void Resolve_NullStats_ReturnsRawDamage()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine();

        int result = resolver.Resolve(
            damageLine,
            null,
            100);

        Assert.That(result, Is.EqualTo(100));
    }

    [TestCase(0)]
    [TestCase(-10)]
    public void Resolve_NonPositiveDamage_ReturnsZero(
        int damage)
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine();

        int result = resolver.Resolve(
            damageLine,
            CreateStats(),
            damage);

        Assert.That(result, Is.Zero);
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

    private static WeaponAttackDamageLineResult CreateDamageLine()
    {
        return new WeaponAttackDamageLineResult(
            new WeaponAttackLineId("test_line"),
            WeaponAttackElement.Neutral,
            100);
    }
}