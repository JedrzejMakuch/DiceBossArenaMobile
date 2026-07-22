using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackDamageLineResolverTests
{
    private WeaponAttackDamageLineResolver resolver;

    [SetUp]
    public void SetUp()
    {
        resolver =
            new WeaponAttackDamageLineResolver();
    }

    [Test]
    public void Resolve_NullDamageLine_ReturnsZero()
    {
        int result = resolver.Resolve(
            null,
            CreateStats());

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
            CreateStats());

        Assert.That(result, Is.Zero);
    }

    [Test]
    public void Resolve_Damage_AppliesArmor()
    {
        FightUnitStats stats =
            CreateStats(
                FightStatType.Armor,
                25);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Neutral,
                100);

        int result = resolver.Resolve(
            damageLine,
            stats);

        Assert.That(result, Is.EqualTo(75));
    }

    [Test]
    public void Resolve_ElementalDamage_AppliesResistanceBeforeArmor()
    {
        Dictionary<FightStatType, int> values =
            new()
            {
            {
                FightStatType.FireResistance,
                25
            },
            {
                FightStatType.Armor,
                10
            }
            };

        FightUnitStats stats =
            new(values);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                100);

        int result = resolver.Resolve(
            damageLine,
            stats);

        Assert.That(result, Is.EqualTo(65));
    }

    [Test]
    public void Resolve_NeutralDamage_IgnoresResistances()
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
            stats);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Resolve_ElementalDamage_AppliesMatchingResistance()
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
            stats);

        Assert.That(result, Is.EqualTo(75));
    }

    [Test]
    public void Resolve_ElementalDamage_IgnoresOtherResistance()
    {
        FightUnitStats stats =
            CreateStats(
                FightStatType.WaterResistance,
                100);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                100);

        int result = resolver.Resolve(
            damageLine,
            stats);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Resolve_TrueDamage_IgnoresResistance()
    {
        FightUnitStats stats =
            CreateStats(
                FightStatType.FireResistance,
                100);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                100,
                true);

        int result = resolver.Resolve(
            damageLine,
            stats);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Resolve_TrueDamage_IgnoresArmor()
    {
        FightUnitStats stats =
            CreateStats(
                FightStatType.Armor,
                100);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Neutral,
                100,
                true);

        int result = resolver.Resolve(
            damageLine,
            stats);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Resolve_TrueDamage_IgnoresResistanceAndArmor()
    {
        Dictionary<FightStatType, int> values =
            new()
            {
            {
                FightStatType.FireResistance,
                100
            },
            {
                FightStatType.Armor,
                100
            }
            };

        FightUnitStats stats =
            new(values);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                100,
                true);

        int result = resolver.Resolve(
            damageLine,
            stats);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Resolve_NullTargetStats_ReturnsRawDamage()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                100);

        int result = resolver.Resolve(
            damageLine,
            null);

        Assert.That(result, Is.EqualTo(100));
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