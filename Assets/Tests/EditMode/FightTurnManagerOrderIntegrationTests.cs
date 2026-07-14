using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightTurnManagerOrderIntegrationTests
    {
        private readonly List<GameObject> createdObjects = new();

        [TearDown]
        public void TearDown()
        {
            foreach (GameObject createdObject in createdObjects)
            {
                if (createdObject != null)
                {
                    Object.DestroyImmediate(createdObject);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void BuildTurnOrder_UsesOnlyUnitsFromRegistry()
        {
            FightUnit slowUnit =
                CreateUnit(
                    "Slow Unit",
                    FightTeam.Player,
                    4);

            FightUnit fastUnit =
                CreateUnit(
                    "Fast Unit",
                    FightTeam.Enemy,
                    12);

            FightUnit mediumUnit =
                CreateUnit(
                    "Medium Unit",
                    FightTeam.Enemy,
                    8);

            FightUnit excludedUnit =
                CreateUnit(
                    "Excluded Unit",
                    FightTeam.Player,
                    100);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(slowUnit);
            registry.Register(fastUnit);
            registry.Register(mediumUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            InvokePrivateMethod(
                turnManager,
                "BuildTurnOrder");

            List<FightUnit> turnOrder =
                GetPrivateField<List<FightUnit>>(
                    turnManager,
                    "turnOrder");

            Assert.That(turnOrder, Has.Count.EqualTo(3));
            Assert.That(turnOrder[0], Is.SameAs(fastUnit));
            Assert.That(turnOrder[1], Is.SameAs(mediumUnit));
            Assert.That(turnOrder[2], Is.SameAs(slowUnit));

            CollectionAssert.DoesNotContain(
                turnOrder,
                excludedUnit);
        }

        [Test]
        public void StartCombat_StartsHighestInitiativeUnitExactlyOnce()
        {
            FightUnit slowUnit =
                CreateUnit(
                    "Slow Unit",
                    FightTeam.Player,
                    4);

            FightUnit fastUnit =
                CreateUnit(
                    "Fast Unit",
                    FightTeam.Enemy,
                    12);

            FightUnit mediumUnit =
                CreateUnit(
                    "Medium Unit",
                    FightTeam.Enemy,
                    8);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(slowUnit);
            registry.Register(fastUnit);
            registry.Register(mediumUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            int turnStartedCount = 0;
            FightUnit startedUnit = null;
            int startedRound = 0;

            turnManager.TurnStarted +=
                (unit, round) =>
                {
                    turnStartedCount++;
                    startedUnit = unit;
                    startedRound = round;
                };

            turnManager.StartCombat();

            Assert.That(
                turnManager.CombatRunning,
                Is.True);

            Assert.That(
                turnManager.RoundNumber,
                Is.EqualTo(1));

            Assert.That(
                turnStartedCount,
                Is.EqualTo(1));

            Assert.That(
                startedUnit,
                Is.SameAs(fastUnit));

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(fastUnit));

            Assert.That(
                startedRound,
                Is.EqualTo(1));
        }

        [Test]
        public void EndCurrentTurn_AfterFullCycle_StartsNextRoundWithoutDuplicateTurns()
        {
            FightUnit slowUnit =
                CreateUnit(
                    "Slow Unit",
                    FightTeam.Player,
                    4);

            FightUnit fastUnit =
                CreateUnit(
                    "Fast Unit",
                    FightTeam.Enemy,
                    12);

            FightUnit mediumUnit =
                CreateUnit(
                    "Medium Unit",
                    FightTeam.Enemy,
                    8);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(slowUnit);
            registry.Register(fastUnit);
            registry.Register(mediumUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            List<FightUnit> startedUnits = new();
            List<int> startedRounds = new();

            turnManager.TurnStarted +=
                (unit, round) =>
                {
                    startedUnits.Add(unit);
                    startedRounds.Add(round);
                };

            turnManager.StartCombat();

            turnManager.EndCurrentTurn();
            turnManager.EndCurrentTurn();
            turnManager.EndCurrentTurn();

            Assert.That(startedUnits, Has.Count.EqualTo(4));

            Assert.That(startedUnits[0], Is.SameAs(fastUnit));
            Assert.That(startedUnits[1], Is.SameAs(mediumUnit));
            Assert.That(startedUnits[2], Is.SameAs(slowUnit));
            Assert.That(startedUnits[3], Is.SameAs(fastUnit));

            Assert.That(startedRounds[0], Is.EqualTo(1));
            Assert.That(startedRounds[1], Is.EqualTo(1));
            Assert.That(startedRounds[2], Is.EqualTo(1));
            Assert.That(startedRounds[3], Is.EqualTo(2));

            Assert.That(turnManager.RoundNumber, Is.EqualTo(2));
            Assert.That(turnManager.ActiveUnit, Is.SameAs(fastUnit));
        }

        [Test]
        public void ActiveUnitDies_StartsNextLivingUnitExactlyOnce()
        {
            FightUnit fastUnit =
                CreateUnit(
                    "Fast Unit",
                    FightTeam.Enemy,
                    12);

            FightUnit mediumUnit =
                CreateUnit(
                    "Medium Unit",
                    FightTeam.Player,
                    8);

            FightUnit slowUnit =
                CreateUnit(
                    "Slow Unit",
                    FightTeam.Enemy,
                    4);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(fastUnit);
            registry.Register(mediumUnit);
            registry.Register(slowUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            List<FightUnit> startedUnits = new();

            turnManager.TurnStarted +=
                (unit, round) =>
                {
                    startedUnits.Add(unit);
                };

            turnManager.StartCombat();

            fastUnit.TakeDamage(
                fastUnit.MaxHealth);

            Assert.That(startedUnits, Has.Count.EqualTo(2));
            Assert.That(startedUnits[0], Is.SameAs(fastUnit));
            Assert.That(startedUnits[1], Is.SameAs(mediumUnit));

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(mediumUnit));

            Assert.That(
                turnManager.RoundNumber,
                Is.EqualTo(1));

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(2));

            CollectionAssert.DoesNotContain(
                registry.Units,
                fastUnit);
        }

        [Test]
        public void WaitingUnitDies_IsSkippedWithoutBreakingTurnOrder()
        {
            FightUnit fastUnit =
                CreateUnit(
                    "Fast Unit",
                    FightTeam.Player,
                    12);

            FightUnit mediumUnit =
                CreateUnit(
                    "Medium Unit",
                    FightTeam.Enemy,
                    8);

            FightUnit slowUnit =
                CreateUnit(
                    "Slow Unit",
                    FightTeam.Enemy,
                    4);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(fastUnit);
            registry.Register(mediumUnit);
            registry.Register(slowUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            List<FightUnit> startedUnits = new();

            turnManager.TurnStarted +=
                (unit, round) =>
                {
                    startedUnits.Add(unit);
                };

            turnManager.StartCombat();

            mediumUnit.TakeDamage(
                mediumUnit.MaxHealth);

            turnManager.EndCurrentTurn();

            Assert.That(startedUnits, Has.Count.EqualTo(2));
            Assert.That(startedUnits[0], Is.SameAs(fastUnit));
            Assert.That(startedUnits[1], Is.SameAs(slowUnit));

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(slowUnit));

            Assert.That(
                turnManager.RoundNumber,
                Is.EqualTo(1));

            CollectionAssert.DoesNotContain(
                startedUnits,
                mediumUnit);

            CollectionAssert.DoesNotContain(
                registry.Units,
                mediumUnit);
        }

        [Test]
        public void LastUnitDies_StopsCombatExactlyOnce()
        {
            FightUnit onlyUnit =
                CreateUnit(
                    "Only Unit",
                    FightTeam.Player,
                    10);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(onlyUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            int combatStoppedCount = 0;
            string stopReason = null;

            turnManager.CombatStopped +=
                reason =>
                {
                    combatStoppedCount++;
                    stopReason = reason;
                };

            turnManager.StartCombat();

            onlyUnit.TakeDamage(
                onlyUnit.MaxHealth);

            Assert.That(
                turnManager.CombatRunning,
                Is.False);

            Assert.That(
                turnManager.ActiveUnit,
                Is.Null);

            Assert.That(
                combatStoppedCount,
                Is.EqualTo(1));

            Assert.That(
                stopReason,
                Is.EqualTo("No units remaining."));

            Assert.That(
                registry.Units,
                Is.Empty);
        }

        [Test]
        public void UnitRegisteredDuringRound_StartsTurnInNextRound()
        {
            FightUnit fastUnit =
                CreateUnit(
                    "Fast Unit",
                    FightTeam.Player,
                    12);

            FightUnit slowUnit =
                CreateUnit(
                    "Slow Unit",
                    FightTeam.Enemy,
                    4);

            FightUnit reinforcement =
                CreateUnit(
                    "Reinforcement",
                    FightTeam.Enemy,
                    20);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(fastUnit);
            registry.Register(slowUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            List<FightUnit> startedUnits = new();
            List<int> startedRounds = new();

            turnManager.TurnStarted +=
                (unit, round) =>
                {
                    startedUnits.Add(unit);
                    startedRounds.Add(round);
                };

            turnManager.StartCombat();

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(fastUnit));

            registry.Register(reinforcement);

            turnManager.EndCurrentTurn();

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(slowUnit));

            CollectionAssert.DoesNotContain(
    startedUnits,
    reinforcement);

            turnManager.EndCurrentTurn();

            Assert.That(
                turnManager.RoundNumber,
                Is.EqualTo(2));

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(reinforcement));

            Assert.That(
                startedUnits,
                Is.EqualTo(
                    new[]
                    {
                fastUnit,
                slowUnit,
                reinforcement
                    }));

            Assert.That(
                startedRounds,
                Is.EqualTo(
                    new[]
                    {
                1,
                1,
                2
                    }));
        }

        [Test]
        public void UnitRegisteredAndRemovedDuringRound_NeverStartsTurn()
        {
            FightUnit fastUnit =
                CreateUnit(
                    "Fast Unit",
                    FightTeam.Player,
                    12);

            FightUnit slowUnit =
                CreateUnit(
                    "Slow Unit",
                    FightTeam.Enemy,
                    4);

            FightUnit temporaryUnit =
                CreateUnit(
                    "Temporary Unit",
                    FightTeam.Enemy,
                    20);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(fastUnit);
            registry.Register(slowUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            List<FightUnit> startedUnits = new();

            turnManager.TurnStarted +=
                (unit, round) =>
                {
                    startedUnits.Add(unit);
                };

            turnManager.StartCombat();

            registry.Register(temporaryUnit);

            Assert.That(
                registry.Unregister(temporaryUnit),
                Is.True);

            Assert.DoesNotThrow(
                () =>
                {
                    turnManager.EndCurrentTurn();
                    turnManager.EndCurrentTurn();
                });

            Assert.That(
                turnManager.RoundNumber,
                Is.EqualTo(2));

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(fastUnit));

            CollectionAssert.DoesNotContain(
                startedUnits,
                temporaryUnit);

            Assert.That(
                startedUnits,
                Is.EqualTo(
                    new[]
                    {
                fastUnit,
                slowUnit,
                fastUnit
                    }));
        }

        [Test]
        public void EndCurrentTurn_RaisesTurnEndedExactlyOnceForActiveUnit()
        {
            FightUnit fastUnit =
                CreateUnit(
                    "Fast Unit",
                    FightTeam.Player,
                    12);

            FightUnit slowUnit =
                CreateUnit(
                    "Slow Unit",
                    FightTeam.Enemy,
                    4);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(fastUnit);
            registry.Register(slowUnit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            int turnEndedCount = 0;
            FightUnit endedUnit = null;

            turnManager.TurnEnded +=
                unit =>
                {
                    turnEndedCount++;
                    endedUnit = unit;
                };

            turnManager.StartCombat();
            turnManager.EndCurrentTurn();

            Assert.That(
                turnEndedCount,
                Is.EqualTo(1));

            Assert.That(
                endedUnit,
                Is.SameAs(fastUnit));

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(slowUnit));
        }

        [Test]
        public void EndCombat_CalledMultipleTimes_RaisesCombatStoppedOnlyOnce()
        {
            FightUnit unit =
                CreateUnit(
                    "Test Unit",
                    FightTeam.Player,
                    10);

            FightUnitRegistry registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            FightTurnManager turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            registry.Register(unit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            int combatStoppedCount = 0;
            string stopReason = null;

            turnManager.CombatStopped +=
                reason =>
                {
                    combatStoppedCount++;
                    stopReason = reason;
                };

            turnManager.StartCombat();

            turnManager.EndCombat("First reason");
            turnManager.EndCombat("Second reason");
            turnManager.EndCombat("Third reason");

            Assert.That(
                turnManager.CombatRunning,
                Is.False);

            Assert.That(
                turnManager.ActiveUnit,
                Is.Null);

            Assert.That(
                combatStoppedCount,
                Is.EqualTo(1));

            Assert.That(
                stopReason,
                Is.EqualTo("First reason"));
        }

        private FightUnit CreateUnit(
            string unitName,
            FightTeam team,
            int initiative)
        {
            GameObject unitObject =
                new GameObject(unitName);

            createdObjects.Add(unitObject);

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                unitName,
                team,
                10,
                2,
                initiative);

            return unit;
        }

        private T CreateComponent<T>(
            string objectName)
            where T : Component
        {
            GameObject gameObject =
                new GameObject(objectName);

            createdObjects.Add(gameObject);

            return gameObject.AddComponent<T>();
        }

        private static void SetPrivateField<T>(
            object target,
            string fieldName,
            T value)
        {
            FieldInfo field =
                target.GetType().GetField(
                    fieldName,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.That(
                field,
                Is.Not.Null,
                $"Field '{fieldName}' was not found.");

            field.SetValue(target, value);
        }

        private static T GetPrivateField<T>(
            object target,
            string fieldName)
        {
            FieldInfo field =
                target.GetType().GetField(
                    fieldName,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.That(
                field,
                Is.Not.Null,
                $"Field '{fieldName}' was not found.");

            return (T)field.GetValue(target);
        }

        private static void InvokePrivateMethod(
            object target,
            string methodName)
        {
            MethodInfo method =
                target.GetType().GetMethod(
                    methodName,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.That(
                method,
                Is.Not.Null,
                $"Method '{methodName}' was not found.");

            method.Invoke(target, null);
        }
    }
}