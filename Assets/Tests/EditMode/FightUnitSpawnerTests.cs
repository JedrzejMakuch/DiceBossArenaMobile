using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitSpawnerTests
    {
        private readonly List<Object> createdObjects = new();

        private FightUnitRegistry registry;
        private FightUnitSpawner spawner;
        private FightUnit prefab;
        private FightUnitDefinition definition;
        private FightUnitOwnership ownership;
        private FightGridTile tile;

        [SetUp]
        public void SetUp()
        {
            registry = CreateComponent<FightUnitRegistry>(
                "FightUnitRegistry");

            spawner = CreateComponent<FightUnitSpawner>(
                "FightUnitSpawner");

            SetPrivateField(
                spawner,
                "unitRegistry",
                registry);

            prefab = CreateComponent<FightUnit>(
                "FightUnitPrefab");

            definition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            createdObjects.Add(definition);

            definition.InitializeForTests(
                newUnitName: "Test Warrior",
                newTeam: FightTeam.Player,
                newMaxHealth: 20,
                newAttackPower: 5,
                newInitiative: 8,
                newStartingSkills: null);

            ownership = new FightUnitOwnership(
                FightTeamId.TeamA,
                new FightParticipantId("player-one"),
                FightControllerType.LocalPlayer);

            tile = CreateComponent<FightGridTile>(
                "SpawnTile");

            tile.Initialize(2, 3);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = createdObjects.Count - 1; i >= 0; i--)
            {
                Object createdObject = createdObjects[i];

                if (createdObject != null)
                {
                    Object.DestroyImmediate(createdObject);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void Spawn_WithValidRequest_CreatesInitializedUnit()
        {
            FightUnitSpawnRequest request =
                CreateRequest("Spawned Warrior");

            FightUnit result =
                spawner.Spawn(request);

            TrackSpawnedUnit(result);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.name, Is.EqualTo("Spawned Warrior"));
            Assert.That(result.Definition, Is.SameAs(definition));
            Assert.That(result.UnitName, Is.EqualTo("Test Warrior"));
            Assert.That(result.MaxHealth, Is.EqualTo(20));
            Assert.That(result.CurrentHealth, Is.EqualTo(20));
            Assert.That(result.AttackPower, Is.EqualTo(5));
            Assert.That(result.Initiative, Is.EqualTo(8));
        }

        [Test]
        public void Spawn_WithValidRequest_AssignsOwnership()
        {
            FightUnit result =
                spawner.Spawn(CreateRequest());

            TrackSpawnedUnit(result);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Ownership, Is.Not.Null);
            Assert.That(
                result.TeamId,
                Is.EqualTo(FightTeamId.TeamA));

            Assert.That(
                result.ParticipantId,
                Is.EqualTo(
                    new FightParticipantId("player-one")));

            Assert.That(
                result.ControllerType,
                Is.EqualTo(
                    FightControllerType.LocalPlayer));
        }

        [Test]
        public void Spawn_WithValidRequest_AssignsTileAndRegistersOnce()
        {
            FightUnit result =
                spawner.Spawn(CreateRequest());

            TrackSpawnedUnit(result);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CurrentTile, Is.SameAs(tile));
            Assert.That(tile.IsOccupied, Is.True);
            Assert.That(tile.OccupyingUnit, Is.SameAs(result));

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(1));

            Assert.That(
                registry.Units[0],
                Is.SameAs(result));
        }

        [Test]
        public void Spawn_OnOccupiedTile_ReturnsNullAndDoesNotRegisterUnit()
        {
            FightUnit blockingUnit =
                CreateComponent<FightUnit>(
                    "BlockingUnit");

            blockingUnit.Initialize(
                "Blocking Unit",
                FightTeam.Enemy,
                10,
                2,
                5);

            Assert.That(
                tile.TryOccupy(blockingUnit),
                Is.True);

            FightUnit result =
                spawner.Spawn(CreateRequest());

            Assert.That(result, Is.Null);
            Assert.That(registry.Units, Is.Empty);
            Assert.That(tile.OccupyingUnit, Is.SameAs(blockingUnit));
        }

        [Test]
        public void Despawn_RegisteredUnit_UnregistersReleasesTileAndDestroysUnit()
        {
            FightUnit result =
                spawner.Spawn(CreateRequest());

            Assert.That(result, Is.Not.Null);
            Assert.That(registry.Units, Has.Count.EqualTo(1));
            Assert.That(tile.IsOccupied, Is.True);

            spawner.Despawn(result);

            Assert.That(registry.Units, Is.Empty);
            Assert.That(tile.IsOccupied, Is.False);
            Assert.That(tile.OccupyingUnit, Is.Null);
            Assert.That(result == null, Is.True);
        }

        private FightUnitSpawnRequest CreateRequest(
            string objectName = "SpawnedUnit")
        {
            return new FightUnitSpawnRequest(
                prefab,
                definition,
                ownership,
                tile,
                spawner.transform,
                objectName);
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
                createdObjects.Add(unit.gameObject);
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

            field.SetValue(target, value);
        }
    }
}