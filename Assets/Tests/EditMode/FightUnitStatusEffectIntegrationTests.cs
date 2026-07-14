using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        FightUnitStatusEffectIntegrationTests
    {
        private GameObject unitObject;
        private FightUnit unit;
        private StatusEffectDefinition definition;
        private CountingStatusEffectBehavior behavior;
        private PeriodicDamageStatusEffectBehaviorDefinition
    periodicDamageBehavior;

        [SetUp]
        public void SetUp()
        {
            unitObject =
                new GameObject(
                    "Unit");

            unit =
                unitObject.AddComponent<
                    FightUnit>();

            unit.Initialize(
                newUnitName:
                    "Test Unit",
                newTeam:
                    FightTeam.Player,
                newMaxHealth:
                    20,
                newAttackPower:
                    10,
                newInitiative:
                    5);
        }

        [TearDown]
        public void TearDown()
        {
            if (periodicDamageBehavior != null)
            {
                Object.DestroyImmediate(
                    periodicDamageBehavior);
            }

            if (behavior != null)
            {
                Object.DestroyImmediate(
                    behavior);
            }

            if (definition != null)
            {
                Object.DestroyImmediate(
                    definition);
            }

            if (unitObject != null)
            {
                Object.DestroyImmediate(
                    unitObject);
            }
        }

        [Test]
        public void InitializedUnit_HasStatusEffectCollection()
        {
            Assert.That(
                unit.StatusEffects,
                Is.Not.Null);

            Assert.That(
                unit.StatusEffects.Count,
                Is.EqualTo(0));
        }

        [Test]
        public void ApplyStatusEffect_AddsEffectToUnit()
        {
            definition =
                CreateWeakenedDefinition();

            StatusEffectApplyResult result =
                unit.ApplyStatusEffect(
                    definition);

            Assert.That(
                result,
                Is.EqualTo(
                    StatusEffectApplyResult.Added));

            Assert.That(
                unit.HasStatusEffect(
                    new StatusEffectId(
                        "weakened")),
                Is.True);
        }

        [Test]
        public void ApplyStatusEffect_ChangesUnitStat()
        {
            definition =
                CreateWeakenedDefinition(
                    valuePerStack:
                        -2);

            unit.ApplyStatusEffect(
                definition);

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(8));
        }

        [Test]
        public void ApplyingSecondStack_RecalculatesUnitStat()
        {
            definition =
                CreateWeakenedDefinition(
                    valuePerStack:
                        -2,
                    maxStacks:
                        3);

            unit.ApplyStatusEffect(
                definition);

            unit.ApplyStatusEffect(
                definition);

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(6));

            Assert.That(
                unit.StatusEffects.TryGet(
                    new StatusEffectId(
                        "weakened"),
                    out StatusEffectRuntimeState state),
                Is.True);

            Assert.That(
                state.Stacks,
                Is.EqualTo(2));
        }

        [Test]
        public void RemoveStatusEffect_RestoresUnitStat()
        {
            definition =
                CreateWeakenedDefinition(
                    valuePerStack:
                        -2);

            unit.ApplyStatusEffect(
                definition);

            bool removed =
                unit.RemoveStatusEffect(
                    new StatusEffectId(
                        "weakened"));

            Assert.That(
                removed,
                Is.True);

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(10));

            Assert.That(
                unit.HasStatusEffect(
                    new StatusEffectId(
                        "weakened")),
                Is.False);
        }

        [Test]
        public void ReinitializeUnit_ClearsStatusEffects()
        {
            definition =
                CreateWeakenedDefinition(
                    valuePerStack:
                        -2);

            unit.ApplyStatusEffect(
                definition);

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(8));

            unit.Initialize(
                newUnitName:
                    "Reinitialized Unit",
                newTeam:
                    FightTeam.Player,
                newMaxHealth:
                    20,
                newAttackPower:
                    12,
                newInitiative:
                    5);

            Assert.That(
                unit.StatusEffects.Count,
                Is.EqualTo(0));

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(12));
        }

        [Test]
        public void TwoUnits_HaveIndependentStatusCollections()
        {
            definition =
                CreateWeakenedDefinition(
                    valuePerStack:
                        -2);

            GameObject secondObject =
                new GameObject(
                    "Second Unit");

            FightUnit secondUnit =
                secondObject.AddComponent<
                    FightUnit>();

            try
            {
                secondUnit.Initialize(
                    newUnitName:
                        "Second Unit",
                    newTeam:
                        FightTeam.Enemy,
                    newMaxHealth:
                        20,
                    newAttackPower:
                        10,
                    newInitiative:
                        5);

                unit.ApplyStatusEffect(
                    definition);

                Assert.That(
                    unit.AttackPower,
                    Is.EqualTo(8));

                Assert.That(
                    secondUnit.AttackPower,
                    Is.EqualTo(10));

                Assert.That(
                    unit.StatusEffects.Count,
                    Is.EqualTo(1));

                Assert.That(
                    secondUnit.StatusEffects.Count,
                    Is.EqualTo(0));
            }
            finally
            {
                Object.DestroyImmediate(
                    secondObject);
            }
        }

        [Test]
        public void Bleed_DealsDamageAtEndOfOwnerTurn()
        {
            periodicDamageBehavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();

            periodicDamageBehavior.InitializeForTests(
                2);

            definition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            definition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newBaseDurationTurns:
                    3,
                newMaxStacks:
                    5,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .AddStacksAndRefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                newBehaviors:
                    new[]
                    {
                periodicDamageBehavior
                    });

            unit.ApplyStatusEffect(
                definition);

            unit.ProcessStatusEffectsAtEndOfTurn();

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(18));

            Assert.That(
                unit.HasStatusEffect(
                    new StatusEffectId(
                        "bleed")),
                Is.True);
        }

        [Test]
        public void Bleed_MultipleStacksIncreaseDamage()
        {
            periodicDamageBehavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();

            periodicDamageBehavior.InitializeForTests(
                2);

            definition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            definition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newBaseDurationTurns:
                    3,
                newMaxStacks:
                    5,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .AddStacksAndRefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                newBehaviors:
                    new[]
                    {
                periodicDamageBehavior
                    });

            unit.ApplyStatusEffect(
                definition);

            unit.ApplyStatusEffect(
                definition);

            unit.ApplyStatusEffect(
                definition);

            unit.ProcessStatusEffectsAtEndOfTurn();

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(14));
        }

        [Test]
        public void Bleed_DealsLastDamageBeforeExpiration()
        {
            periodicDamageBehavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();

            periodicDamageBehavior.InitializeForTests(
                3);

            definition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            definition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newBaseDurationTurns:
                    1,
                newMaxStacks:
                    1,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .RefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                newBehaviors:
                    new[]
                    {
                periodicDamageBehavior
                    });

            unit.ApplyStatusEffect(
                definition);

            StatusEffectTurnProcessResult result =
                unit.ProcessStatusEffectsAtEndOfTurn();

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(17));

            Assert.That(
                unit.HasStatusEffect(
                    new StatusEffectId(
                        "bleed")),
                Is.False);

            Assert.That(
                result.EffectsExpired,
                Is.EqualTo(1));
        }

        [Test]
        public void Bleed_CanKillUnitAtEndOfTurn()
        {
            periodicDamageBehavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();

            periodicDamageBehavior.InitializeForTests(
                25);

            definition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            definition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newBaseDurationTurns:
                    1,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                newBehaviors:
                    new[]
                    {
                periodicDamageBehavior
                    });

            bool diedRaised = false;

            unit.Died +=
                _ => diedRaised = true;

            unit.ApplyStatusEffect(
                definition);

            unit.ProcessStatusEffectsAtEndOfTurn();

            Assert.That(
                unit.IsAlive,
                Is.False);

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(0));

            Assert.That(
                diedRaised,
                Is.True);
        }

        [Test]
        public void DispelDebuff_RemovesEffectAndRestoresStat()
        {
            definition =
                CreateWeakenedDefinition(
                    valuePerStack:
                        -2);

            unit.ApplyStatusEffect(
                definition);

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(8));

            StatusEffectDispelResult result =
                unit.DispelStatusEffects(
                    StatusEffectCategory.Debuff);

            Assert.That(
                result.RemovedCount,
                Is.EqualTo(1));

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(10));

            Assert.That(
                unit.HasStatusEffect(
                    new StatusEffectId(
                        "weakened")),
                Is.False);
        }

        private StatusEffectDefinition CreateWeakenedDefinition(
                int valuePerStack = -2,
                int maxStacks = 1)
        {
            StatusEffectStatModifierDefinition modifier =
                new StatusEffectStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    valuePerStack);

            StatusEffectDefinition result =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            result.InitializeForTests(
                newStatusEffectId:
                    "weakened",
                newDisplayName:
                    "Weakened",
                newBaseDurationTurns:
                    3,
                newMaxStacks:
                    maxStacks,
                newStackingPolicy:
                    maxStacks > 1
                        ? StatusEffectStackingPolicy
                            .AddStacksAndRefreshDuration
                        : StatusEffectStackingPolicy
                            .RefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming.None,
                newStatModifiers:
                    new[]
                    {
                        modifier
                    },
                newCategory:
    StatusEffectCategory.Debuff);

            return result;
        }

        private sealed class CountingStatusEffectBehavior :
    StatusEffectBehaviorDefinition
        {
            public int ExecutionCount { get; private set; }

            public override void Execute(
                StatusEffectExecutionContext context)
            {
                ExecutionCount++;
            }
        }
    }
}