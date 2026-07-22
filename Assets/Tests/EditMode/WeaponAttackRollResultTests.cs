using System;
using NUnit.Framework;
using UnityEngine;

public sealed class WeaponAttackRollResultTests
{
    private GameObject attackerObject;
    private GameObject targetObject;

    private FightUnit attacker;
    private FightUnit target;

    [SetUp]
    public void SetUp()
    {
        attackerObject =
            new GameObject("Attacker");

        attacker =
            attackerObject.AddComponent<FightUnit>();

        attacker.Initialize(
            newUnitName: "Attacker",
            newTeam: FightTeam.Player,
            newMaxHealth: 20,
            newAttackPower: 5,
            newInitiative: 10);

        targetObject =
            new GameObject("Target");

        target =
            targetObject.AddComponent<FightUnit>();

        target.Initialize(
            newUnitName: "Target",
            newTeam: FightTeam.Enemy,
            newMaxHealth: 20,
            newAttackPower: 5,
            newInitiative: 5);
    }

    [TearDown]
    public void TearDown()
    {
        if (attackerObject != null)
        {
            UnityEngine.Object.DestroyImmediate(
                attackerObject);
        }

        if (targetObject != null)
        {
            UnityEngine.Object.DestroyImmediate(
                targetObject);
        }
    }

    [Test]
    public void Constructor_ValidDataStoresAttackResult()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                "primary_damage",
                6);

        WeaponAttackRollResult result =
            new WeaponAttackRollResult(
                attacker,
                target,
                new[]
                {
                    damageLine
                });

        Assert.That(
            result.Attacker,
            Is.SameAs(attacker));

        Assert.That(
            result.Target,
            Is.SameAs(target));

        Assert.That(
            result.DamageLines.Count,
            Is.EqualTo(1));

        Assert.That(
            result.DamageLines[0],
            Is.SameAs(damageLine));
    }

    [Test]
    public void Constructor_CalculatesTotalDamage()
    {
        WeaponAttackRollResult result =
            new WeaponAttackRollResult(
                attacker,
                target,
                new[]
                {
                CreateDamageLine(
                    "primary_damage",
                    6),

                CreateDamageLine(
                    "fire_damage",
                    3)
                });

        Assert.That(
            result.TotalDamage,
            Is.EqualTo(9));
    }

    [Test]
    public void Constructor_NullAttackerThrows()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackRollResult(
                null,
                target,
                new[]
                {
                    CreateDamageLine(
                        "primary_damage",
                        6)
                }));
    }

    [Test]
    public void Constructor_NullTargetThrows()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackRollResult(
                attacker,
                null,
                new[]
                {
                    CreateDamageLine(
                        "primary_damage",
                        6)
                }));
    }

    [Test]
    public void Constructor_NullDamageLinesThrows()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackRollResult(
                attacker,
                target,
                null));
    }

    [Test]
    public void Constructor_EmptyDamageLinesThrows()
    {
        Assert.Throws<ArgumentException>(
            () => new WeaponAttackRollResult(
                attacker,
                target,
                Array.Empty<
                    WeaponAttackDamageLineResult>()));
    }

    [Test]
    public void Constructor_NullDamageLineThrows()
    {
        Assert.Throws<ArgumentException>(
            () => new WeaponAttackRollResult(
                attacker,
                target,
                new WeaponAttackDamageLineResult[]
                {
                    null
                }));
    }

    [Test]
    public void Constructor_CopiesDamageLinesCollection()
    {
        WeaponAttackDamageLineResult originalLine =
            CreateDamageLine(
                "primary_damage",
                6);

        WeaponAttackDamageLineResult[] source =
        {
            originalLine
        };

        WeaponAttackRollResult result =
            new WeaponAttackRollResult(
                attacker,
                target,
                source);

        source[0] =
            CreateDamageLine(
                "replacement_damage",
                99);

        Assert.That(
            result.DamageLines[0],
            Is.SameAs(originalLine));
    }

    private static WeaponAttackDamageLineResult
        CreateDamageLine(
            string lineId,
            int damage)
    {
        return new WeaponAttackDamageLineResult(
            new DiceBossArena.Game
                .WeaponAttackLineId(lineId),
            DiceBossArena.Game
                .WeaponAttackElement.Neutral,
            damage);
    }
}