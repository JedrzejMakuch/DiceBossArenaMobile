using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightOutcomeManagerTests
    {
        private readonly List<GameObject> createdObjects = new();

        private FightUnitRegistry registry;
        private FightTurnManager turnManager;
        private FightOutcomeManager outcomeManager;
        private FightUnit unit;

        [SetUp]
        public void SetUp()
        {
            registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            outcomeManager =
                CreateComponent<FightOutcomeManager>(
                    "FightOutcomeManager");

            unit =
                CreateComponent<FightUnit>(
                    "TestUnit");

            unit.Initialize(
                "Test Unit",
                FightTeam.Player,
                10,
                2,
                10);

            registry.Register(unit);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            SetPrivateField(
                outcomeManager,
                "turnManager",
                turnManager);

            turnManager.StartCombat();
        }

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
        public void ForceVictory_ResolvesVictoryExactlyOnce()
        {
            int resolvedCount = 0;
            FightOutcome resolvedOutcome =
                FightOutcome.None;

            outcomeManager.OutcomeResolved +=
                outcome =>
                {
                    resolvedCount++;
                    resolvedOutcome = outcome;
                };

            outcomeManager.ForceVictory();
            outcomeManager.ForceVictory();

            Assert.That(
                outcomeManager.FightFinished,
                Is.True);

            Assert.That(
                outcomeManager.CurrentOutcome,
                Is.EqualTo(FightOutcome.Victory));

            Assert.That(
                resolvedCount,
                Is.EqualTo(1));

            Assert.That(
                resolvedOutcome,
                Is.EqualTo(FightOutcome.Victory));

            Assert.That(
                turnManager.CombatRunning,
                Is.False);

            Assert.That(
                turnManager.ActiveUnit,
                Is.Null);
        }

        [Test]
        public void ForceDefeat_ResolvesDefeatExactlyOnce()
        {
            int resolvedCount = 0;
            FightOutcome resolvedOutcome =
                FightOutcome.None;

            outcomeManager.OutcomeResolved +=
                outcome =>
                {
                    resolvedCount++;
                    resolvedOutcome = outcome;
                };

            outcomeManager.ForceDefeat();
            outcomeManager.ForceDefeat();

            Assert.That(
                outcomeManager.FightFinished,
                Is.True);

            Assert.That(
                outcomeManager.CurrentOutcome,
                Is.EqualTo(FightOutcome.Defeat));

            Assert.That(
                resolvedCount,
                Is.EqualTo(1));

            Assert.That(
                resolvedOutcome,
                Is.EqualTo(FightOutcome.Defeat));

            Assert.That(
                turnManager.CombatRunning,
                Is.False);

            Assert.That(
                turnManager.ActiveUnit,
                Is.Null);
        }

        [Test]
        public void VictoryResolved_First_CannotBeOverwrittenByDefeat()
        {
            List<FightOutcome> resolvedOutcomes =
                new();

            outcomeManager.OutcomeResolved +=
                outcome =>
                {
                    resolvedOutcomes.Add(outcome);
                };

            outcomeManager.ForceVictory();
            outcomeManager.ForceDefeat();

            Assert.That(
                outcomeManager.CurrentOutcome,
                Is.EqualTo(FightOutcome.Victory));

            Assert.That(
                resolvedOutcomes,
                Is.EqualTo(
                    new[]
                    {
                        FightOutcome.Victory
                    }));
        }

        [Test]
        public void DefeatResolved_First_CannotBeOverwrittenByVictory()
        {
            List<FightOutcome> resolvedOutcomes =
                new();

            outcomeManager.OutcomeResolved +=
                outcome =>
                {
                    resolvedOutcomes.Add(outcome);
                };

            outcomeManager.ForceDefeat();
            outcomeManager.ForceVictory();

            Assert.That(
                outcomeManager.CurrentOutcome,
                Is.EqualTo(FightOutcome.Defeat));

            Assert.That(
                resolvedOutcomes,
                Is.EqualTo(
                    new[]
                    {
                        FightOutcome.Defeat
                    }));
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
    }
}