using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        FightTurnManagerStatusEffectIntegrationTests
    {
        private GameObject rootObject;
        private GameObject firstUnitObject;
        private GameObject secondUnitObject;

        private FightUnitRegistry registry;
        private FightTurnManager turnManager;

        private FightUnit firstUnit;
        private FightUnit secondUnit;

        private StatusEffectDefinition definition;

        private PeriodicDamageStatusEffectBehaviorDefinition
            damageBehavior;

        [SetUp]
        public void SetUp()
        {
            rootObject =
                new GameObject(
                    "Fight Root");

            registry =
                rootObject.AddComponent<
                    FightUnitRegistry>();

            turnManager =
                rootObject.AddComponent<
                    FightTurnManager>();

            turnManager.InitializeForTests(
                registry);

            firstUnit =
                CreateUnit(
                    ref firstUnitObject,
                    "First",
                    FightTeam.Player,
                    initiative:
                        20);

            secondUnit =
                CreateUnit(
                    ref secondUnitObject,
                    "Second",
                    FightTeam.Enemy,
                    initiative:
                        10);

            registry.Register(
                firstUnit);

            registry.Register(
                secondUnit);
        }

        [TearDown]
        public void TearDown()
        {
            if (damageBehavior != null)
            {
                Object.DestroyImmediate(
                    damageBehavior);
            }

            if (definition != null)
            {
                Object.DestroyImmediate(
                    definition);
            }

            if (firstUnitObject != null)
            {
                Object.DestroyImmediate(
                    firstUnitObject);
            }

            if (secondUnitObject != null)
            {
                Object.DestroyImmediate(
                    secondUnitObject);
            }

            if (rootObject != null)
            {
                Object.DestroyImmediate(
                    rootObject);
            }
        }

        [Test]
        public void StartCombat_ProcessesStartOfTurnEffects()
        {
            CountingStatusEffectBehavior behavior =
                ScriptableObject.CreateInstance<
                    CountingStatusEffectBehavior>();

            try
            {
                definition =
                    CreateDefinition(
                        id:
                            "regeneration_test",
                        tickTiming:
                            StatusEffectTickTiming
                                .StartOfOwnerTurn,
                        duration:
                            2,
                        behavior:
                            behavior);

                firstUnit.ApplyStatusEffect(
                    definition);

                turnManager.StartCombat();

                Assert.That(
                    behavior.ExecutionCount,
                    Is.EqualTo(1));

                Assert.That(
                    turnManager.ActiveUnit,
                    Is.SameAs(firstUnit));
            }
            finally
            {
                Object.DestroyImmediate(
                    behavior);
            }
        }

        [Test]
        public void EndCurrentTurn_ProcessesEndOfTurnEffects()
        {
            damageBehavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();

            damageBehavior.InitializeForTests(
                3);

            definition =
                CreateDefinition(
                    id:
                        "bleed",
                    tickTiming:
                        StatusEffectTickTiming
                            .EndOfOwnerTurn,
                    duration:
                        2,
                    behavior:
                        damageBehavior);

            firstUnit.ApplyStatusEffect(
                definition);

            turnManager.StartCombat();

            turnManager.EndCurrentTurn();

            Assert.That(
                firstUnit.CurrentHealth,
                Is.EqualTo(17));

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(secondUnit));
        }

        [Test]
        public void DeathFromEndOfTurnEffect_DoesNotSkipNextUnit()
        {
            damageBehavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();

            damageBehavior.InitializeForTests(
                25);

            definition =
                CreateDefinition(
                    id:
                        "bleed",
                    tickTiming:
                        StatusEffectTickTiming
                            .EndOfOwnerTurn,
                    duration:
                        1,
                    behavior:
                        damageBehavior);

            firstUnit.ApplyStatusEffect(
                definition);

            int secondUnitTurnStartedCount = 0;

            turnManager.TurnStarted +=
                (unit, _) =>
                {
                    if (unit == secondUnit)
                    {
                        secondUnitTurnStartedCount++;
                    }
                };

            turnManager.StartCombat();

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(firstUnit));

            turnManager.EndCurrentTurn();

            Assert.That(
                firstUnit.IsAlive,
                Is.False);

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(secondUnit));

            Assert.That(
                secondUnitTurnStartedCount,
                Is.EqualTo(1));
        }

        [Test]
        public void EndOfTurnEffect_ExecutesBeforeTurnEndedEvent()
        {
            damageBehavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();

            damageBehavior.InitializeForTests(
                3);

            definition =
                CreateDefinition(
                    id:
                        "bleed",
                    tickTiming:
                        StatusEffectTickTiming
                            .EndOfOwnerTurn,
                    duration:
                        1,
                    behavior:
                        damageBehavior);

            firstUnit.ApplyStatusEffect(
                definition);

            int healthDuringTurnEnded = -1;

            turnManager.TurnEnded +=
                unit =>
                {
                    if (unit == firstUnit)
                    {
                        healthDuringTurnEnded =
                            unit.CurrentHealth;
                    }
                };

            turnManager.StartCombat();
            turnManager.EndCurrentTurn();

            Assert.That(
                healthDuringTurnEnded,
                Is.EqualTo(17));
        }

        private FightUnit CreateUnit(
            ref GameObject unitGameObject,
            string unitName,
            FightTeam team,
            int initiative)
        {
            unitGameObject =
                new GameObject(
                    unitName);

            FightUnit unit =
                unitGameObject.AddComponent<
                    FightUnit>();

            unit.Initialize(
                newUnitName:
                    unitName,
                newTeam:
                    team,
                newMaxHealth:
                    20,
                newAttackPower:
                    5,
                newInitiative:
                    initiative);

            return unit;
        }

        private StatusEffectDefinition CreateDefinition(
            string id,
            StatusEffectTickTiming tickTiming,
            int duration,
            StatusEffectBehaviorDefinition behavior)
        {
            StatusEffectDefinition result =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            result.InitializeForTests(
                newStatusEffectId:
                    id,
                newDisplayName:
                    id,
                newBaseDurationTurns:
                    duration,
                newMaxStacks:
                    1,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .RefreshDuration,
                newTickTiming:
                    tickTiming,
                newBehaviors:
                    new[]
                    {
                        behavior
                    });

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