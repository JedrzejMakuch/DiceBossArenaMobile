using DiceBossArena.Game;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class WeaponAttackEffectsApplierTests
{
    private readonly List<UnityEngine.Object>
        createdObjects =
            new List<UnityEngine.Object>();

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
    public void Constructor_WhenAppliersAreNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackEffectsApplier(
                null));
    }

    [Test]
    public void Constructor_WhenAppliersContainNull_Throws()
    {
        IReadOnlyList<IWeaponAttackEffectApplier>
            appliers =
                new IWeaponAttackEffectApplier[]
                {
                    new RecordingEffectApplier(),
                    null
                };

        Assert.Throws<ArgumentException>(
            () => new WeaponAttackEffectsApplier(
                appliers));
    }

    [Test]
    public void Apply_WhenAttackResultIsNull_Throws()
    {
        WeaponAttackEffectsApplier effectsApplier =
            new WeaponAttackEffectsApplier(
                Array.Empty<
                    IWeaponAttackEffectApplier>());

        Assert.Throws<ArgumentNullException>(
            () => effectsApplier.Apply(
                null));
    }

    [Test]
    public void Apply_CallsEveryApplierInRegistrationOrder()
    {
        List<int> callOrder =
            new List<int>();

        RecordingEffectApplier first =
            new RecordingEffectApplier(
                () => callOrder.Add(1));

        RecordingEffectApplier second =
            new RecordingEffectApplier(
                () => callOrder.Add(2));

        WeaponAttackEffectsApplier effectsApplier =
            new WeaponAttackEffectsApplier(
                new IWeaponAttackEffectApplier[]
                {
                    first,
                    second
                });

        WeaponAttackRollResult attackResult =
            CreateAttackResult();

        effectsApplier.Apply(
            attackResult);

        CollectionAssert.AreEqual(
            new[]
            {
                1,
                2
            },
            callOrder);

        Assert.AreSame(
            attackResult,
            first.ReceivedAttackResult);

        Assert.AreSame(
            attackResult,
            second.ReceivedAttackResult);
    }

    [Test]
    public void Apply_WhenNoAppliersExist_DoesNotThrow()
    {
        WeaponAttackEffectsApplier effectsApplier =
            new WeaponAttackEffectsApplier(
                Array.Empty<
                    IWeaponAttackEffectApplier>());

        WeaponAttackRollResult attackResult =
            CreateAttackResult();

        Assert.DoesNotThrow(
            () => effectsApplier.Apply(
                attackResult));
    }

    private WeaponAttackRollResult
    CreateAttackResult()
    {
        FightUnit attacker =
            CreateUnit(
                "Attacker",
                FightTeam.Player);

        FightUnit target =
            CreateUnit(
                "Target",
                FightTeam.Enemy);

        WeaponAttackDamageLineResult damageLine =
            new WeaponAttackDamageLineResult(
                new WeaponAttackLineId(
                    "test_damage"),
                WeaponAttackElement.Neutral,
                5);

        WeaponAttackEffectLineResult effectLine =
            new WeaponAttackEffectLineResult(
                damageLine,
                Array.Empty<
                    WeaponAttackEffectTriggerResult>());

        WeaponAttackDamageLineResult[] damageLines =
        {
        damageLine
    };

        WeaponAttackEffectLineResult[] effectLines =
        {
        effectLine
    };

        Assert.That(
            damageLines.Length,
            Is.EqualTo(1));

        return new WeaponAttackRollResult(
            attacker,
            target,
            damageLines,
            effectLines);
    }

    private FightUnit CreateUnit(
    string unitName,
    FightTeam team)
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
            newTeam: team,
            newMaxHealth: 20,
            newAttackPower: 5,
            newInitiative: 10);

        return unit;
    }

    private sealed class RecordingEffectApplier :
        IWeaponAttackEffectApplier
    {
        private readonly Action onApply;

        public RecordingEffectApplier(
            Action onApply = null)
        {
            this.onApply =
                onApply;
        }

        public WeaponAttackRollResult
            ReceivedAttackResult
        {
            get;
            private set;
        }

        public void Apply(
            WeaponAttackRollResult attackResult)
        {
            ReceivedAttackResult =
                attackResult;

            onApply?.Invoke();
        }
    }
}