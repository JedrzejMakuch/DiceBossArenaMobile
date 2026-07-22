using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public sealed class WeaponAttackDamageApplierTests
{
    private GameObject attackerObject;
    private GameObject targetObject;

    private FightUnit attacker;
    private FightUnit target;

    private WeaponAttackDamageApplier applier;

    [SetUp]
    public void SetUp()
    {
        attacker =
            CreateUnit(
                "Attacker",
                FightTeam.Player,
                out attackerObject);

        target =
            CreateUnit(
                "Target",
                FightTeam.Enemy,
                out targetObject);

        applier =
            new WeaponAttackDamageApplier();
    }

    [TearDown]
    public void TearDown()
    {
        if (attackerObject != null)
        {
            Object.DestroyImmediate(
                attackerObject);
        }

        if (targetObject != null)
        {
            Object.DestroyImmediate(
                targetObject);
        }
    }

    [Test]
    public void Apply_NullResultReturnsInvalidAttack()
    {
        WeaponAttackApplyResult result =
            applier.Apply(null);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackApplyResult.InvalidAttack));
    }

    [Test]
    public void Apply_ValidResultDamagesTarget()
    {
        int healthBefore =
            target.CurrentHealth;

        WeaponAttackRollResult attackResult =
            CreateAttackResult(
                damage: 7);

        WeaponAttackApplyResult result =
            applier.Apply(
                attackResult);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackApplyResult.Success));

        Assert.That(
            target.CurrentHealth,
            Is.EqualTo(
                healthBefore - 7));
    }

    [Test]
    public void Apply_ValidResultRaisesHealthChangedOnce()
    {
        int eventCount = 0;

        target.HealthChanged +=
            unit =>
            {
                eventCount++;
            };

        WeaponAttackRollResult attackResult =
            CreateAttackResult(
                damage: 5);

        WeaponAttackApplyResult result =
            applier.Apply(
                attackResult);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackApplyResult.Success));

        Assert.That(
            eventCount,
            Is.EqualTo(1));
    }

    [Test]
    public void Apply_LethalDamageKillsTarget()
    {
        WeaponAttackRollResult attackResult =
            CreateAttackResult(
                damage: target.CurrentHealth);

        WeaponAttackApplyResult result =
            applier.Apply(
                attackResult);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackApplyResult.Success));

        Assert.That(
            target.IsAlive,
            Is.False);
    }

    [Test]
    public void Apply_AllDamageLinesResolvedTogether()
    {
        int healthBefore =
            target.CurrentHealth;

        WeaponAttackRollResult attackResult =
            new WeaponAttackRollResult(
                attacker,
                target,
                new[]
                {
                    new WeaponAttackDamageLineResult(
                        new WeaponAttackLineId(
                            "neutral"),
                        WeaponAttackElement.Neutral,
                        4),

                    new WeaponAttackDamageLineResult(
                        new WeaponAttackLineId(
                            "fire"),
                        WeaponAttackElement.Fire,
                        3)
                });

        WeaponAttackApplyResult result =
            applier.Apply(
                attackResult);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackApplyResult.Success));

        Assert.That(
            target.CurrentHealth,
            Is.EqualTo(
                healthBefore - 7));
    }

    [Test]
    public void Apply_DeadTargetReturnsInvalidTarget()
    {
        target.TakeDamage(
            target.CurrentHealth);

        WeaponAttackRollResult attackResult =
            CreateAttackResult(
                damage: 5);

        WeaponAttackApplyResult result =
            applier.Apply(
                attackResult);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackApplyResult.InvalidTarget));
    }

    [Test]
    public void Apply_ZeroDamageReturnsNoDamage()
    {
        WeaponAttackRollResult attackResult =
            CreateAttackResult(
                damage: 0);

        WeaponAttackApplyResult result =
            applier.Apply(
                attackResult);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackApplyResult.NoDamage));
    }

    private WeaponAttackRollResult CreateAttackResult(
        int damage)
    {
        return new WeaponAttackRollResult(
            attacker,
            target,
            new[]
            {
                new WeaponAttackDamageLineResult(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Neutral,
                    damage)
            });
    }

    private static FightUnit CreateUnit(
        string unitName,
        FightTeam team,
        out GameObject unitObject)
    {
        unitObject =
            new GameObject(unitName);

        unitObject.AddComponent<
            FightUnitTurnResources>();

        unitObject.AddComponent<
            FightUnitSkills>();

        FightUnit unit =
            unitObject.AddComponent<
                FightUnit>();

        unit.Initialize(
            newUnitName: unitName,
            newTeam: team,
            newMaxHealth: 20,
            newAttackPower: 5,
            newInitiative: 10);

        return unit;
    }
}