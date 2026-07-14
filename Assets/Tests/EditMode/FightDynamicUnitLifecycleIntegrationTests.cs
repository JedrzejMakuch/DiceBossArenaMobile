using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightDynamicUnitLifecycleIntegrationTests
    {
        private readonly List<Object> createdObjects = new();

        private FightUnitRegistry registry;
        private FightUnitSpawner spawner;
        private FightTurnManager turnManager;

        private FightUnit reinforcementPrefab;
        private FightUnitDefinition reinforcementDefinition;
        private FightGridTile reinforcementTile;

        [SetUp]
        public void SetUp()
        {
            registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            spawner =
                CreateComponent<FightUnitSpawner>(
                    "FightUnitSpawner");

            turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            SetPrivateField(
                spawner,
                "unitRegistry",
                registry);

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            reinforcementPrefab =
                CreateComponent<FightUnit>(
                    "ReinforcementPrefab");

            reinforcementDefinition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            createdObjects.Add(reinforcementDefinition);

            reinforcementDefinition.InitializeForTests(
                newUnitName: "Reinforcement",
                newTeam: FightTeam.Enemy,
                newMaxHealth: 15,
                newAttackPower: 3,
                newInitiative: 20,
                newStartingSkills: null);

            reinforcementTile =
                CreateComponent<FightGridTile>(
                    "ReinforcementTile");

            reinforcementTile.Initialize(5, 5);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = createdObjects.Count - 1; i >= 0; i--)
            {
                Object createdObject =
                    createdObjects[i];

                if (createdObject != null)
                {
                    Object.DestroyImmediate(createdObject);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void ReinforcementSpawnedDuringRound_StartsInNextRound()
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

            registry.Register(fastUnit);
            registry.Register(slowUnit);

            List<FightUnit> startedUnits = new();
            List<int> startedRounds = new();

            turnManager.TurnStarted +=
                (unit, round) =>
                {
                    startedUnits.Add(unit);
                    startedRounds.Add(round);
                };

            turnManager.StartCombat();

            FightUnit reinforcement =
                spawner.Spawn(
                    CreateReinforcementRequest());

            TrackSpawnedUnit(reinforcement);

            Assert.That(
                reinforcement,
                Is.Not.Null);

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(3));

            Assert.That(
                reinforcement.CurrentTile,
                Is.SameAs(reinforcementTile));

            Assert.That(
                reinforcementTile.OccupyingUnit,
                Is.SameAs(reinforcement));

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
        public void ReinforcementDespawnedDuringRound_DoesNotEnterNextRound()
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

            registry.Register(fastUnit);
            registry.Register(slowUnit);

            List<FightUnit> startedUnits = new();

            turnManager.TurnStarted +=
                (unit, round) =>
                {
                    startedUnits.Add(unit);
                };

            turnManager.StartCombat();

            FightUnit reinforcement =
                spawner.Spawn(
                    CreateReinforcementRequest());

            Assert.That(
                reinforcement,
                Is.Not.Null);

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(3));

            spawner.Despawn(reinforcement);

            Assert.That(
                reinforcement == null,
                Is.True);

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(2));

            Assert.That(
                reinforcementTile.IsOccupied,
                Is.False);

            Assert.That(
                reinforcementTile.OccupyingUnit,
                Is.Null);

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

        private FightUnitSpawnRequest CreateReinforcementRequest()
        {
            FightUnitOwnership ownership =
                new FightUnitOwnership(
                    FightTeamId.TeamB,
                    new FightParticipantId("enemy-reinforcement"),
                    FightControllerType.AI);

            return new FightUnitSpawnRequest(
                reinforcementPrefab,
                reinforcementDefinition,
                ownership,
                reinforcementTile,
                spawner.transform,
                "SpawnedReinforcement");
        }

        private FightUnit CreateUnit(
            string unitName,
            FightTeam team,
            int initiative)
        {
            FightUnit unit =
                CreateComponent<FightUnit>(
                    unitName);

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

        private void TrackSpawnedUnit(
            FightUnit unit)
        {
            if (unit != null)
            {
                createdObjects.Add(
                    unit.gameObject);
            }
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

            field.SetValue(
                target,
                value);
        }
    }
}