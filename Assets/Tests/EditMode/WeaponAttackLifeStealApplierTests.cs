using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public sealed class WeaponAttackLifeStealApplierTests
{
    private readonly List<UnityEngine.Object> createdObjects =
        new();

    [TearDown]
    public void TearDown()
    {
        for (int index = createdObjects.Count - 1;
             index >= 0;
             index--)
        {
            if (createdObjects[index] != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    createdObjects[index]);
            }
        }

        createdObjects.Clear();
    }

    [Test]
    public void Constructor_NullCalculator_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackLifeStealApplier(
                null));
    }

    [Test]
    public void Apply_NullAttackResult_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => CreateApplier().Apply(
                null));
    }

    [Test]
    public void Apply_NoLifeSteal_ReturnsZero()
    {
        FightUnit attacker =
            CreateUnit(
                "Attacker");

        FightUnit target =
            CreateUnit(
                "Target");

        attacker.TakeDamage(
            10);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                8);

        WeaponAttackRollResult attackResult =
            new WeaponAttackRollResult(
                attacker,
                target,
                new[]
                {
                    damageLine
                });

        int healthBefore =
            attacker.CurrentHealth;

        int result =
            CreateApplier().Apply(
                attackResult);

        Assert.That(
            result,
            Is.EqualTo(0));

        Assert.That(
            attacker.CurrentHealth,
            Is.EqualTo(healthBefore));
    }

    [Test]
    public void Apply_LifeSteal_HealsAttacker()
    {
        FightUnit attacker =
            CreateUnit(
                "Attacker");

        FightUnit target =
            CreateUnit(
                "Target");

        attacker.TakeDamage(
            10);

        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                8);

        WeaponAttackEffectDefinition definition =
            new WeaponAttackEffectDefinition(
                WeaponAttackEffectType.LifeSteal,
                100,
                null,
                25);

        WeaponAttackEffectTriggerResult effectResult =
            WeaponAttackEffectTriggerResult
                .Triggered(
                    definition);

        WeaponAttackEffectLineResult effectLine =
            new WeaponAttackEffectLineResult(
                damageLine,
                new[]
                {
                    effectResult
                });

        WeaponAttackRollResult attackResult =
            new WeaponAttackRollResult(
                attacker,
                target,
                new[]
                {
                    damageLine
                },
                new[]
                {
                    effectLine
                });

        int healthBefore =
            attacker.CurrentHealth;

        int result =
            CreateApplier().Apply(
                attackResult);

        Assert.That(
            result,
            Is.EqualTo(2));

        Assert.That(
            attacker.CurrentHealth,
            Is.EqualTo(
                healthBefore + 2));
    }

    private static WeaponAttackLifeStealApplier
        CreateApplier()
    {
        return new WeaponAttackLifeStealApplier(
            new WeaponAttackLifeStealCalculator());
    }

    private FightUnit CreateUnit(
        string unitName)
    {
        GameObject unitObject =
            new GameObject(
                unitName);

        createdObjects.Add(
            unitObject);

        unitObject.AddComponent<
            FightUnitTurnResources>();

        unitObject.AddComponent<
            FightUnitSkills>();

        FightUnit unit =
            unitObject.AddComponent<
                FightUnit>();

        unit.Initialize(
            newUnitName: unitName,
            newTeam: FightTeam.Player,
            newMaxHealth: 20,
            newAttackPower: 5,
            newInitiative: 10);

        return unit;
    }

    private static WeaponAttackDamageLineResult
        CreateDamageLine(
            int damage)
    {
        return new WeaponAttackDamageLineResult(
            new WeaponAttackLineId(
                "primary_damage"),
            WeaponAttackElement.Neutral,
            damage);
    }
}