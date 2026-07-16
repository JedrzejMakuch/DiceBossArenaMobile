using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.TestTools;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EnemySpawnManagerTests
    {
        private readonly List<Object> createdObjects =
            new();

        private EnemySpawnManager spawnManager;
        private FightUnitSpawner unitSpawner;
        private FightUnitRegistry registry;
        private FightArenaGenerator arenaGenerator;
        private FightUnit enemyPrefab;
        private FightUnitDefinition enemyDefinition;
        private FightGridTile spawnTile;

        [SetUp]
        public void SetUp()
        {
            spawnManager =
                CreateComponent<EnemySpawnManager>(
                    "EnemySpawnManager");

            unitSpawner =
                CreateComponent<FightUnitSpawner>(
                    "FightUnitSpawner");

            registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            arenaGenerator =
                CreateComponent<FightArenaGenerator>(
                    "FightArenaGenerator");

            enemyPrefab =
                CreateComponent<FightUnit>(
                    "EnemyPrefab");

            enemyPrefab.gameObject
                .AddComponent<FightUnitSkills>();

            enemyDefinition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            createdObjects.Add(enemyDefinition);

            enemyDefinition.InitializeForTests(
                newUnitName:
                    "Test Enemy",
                newTeam:
                    FightTeam.Enemy,
                newMaxHealth:
                    10,
                newAttackPower:
                    2,
                newInitiative:
                    10,
                newStartingSkills:
                    null);

            enemyPrefab.Initialize(
                enemyDefinition);

            spawnTile =
                CreateComponent<FightGridTile>(
                    "EnemySpawnTile");

            spawnTile.Initialize(3, 4);

            SetPrivateField(
                unitSpawner,
                "unitRegistry",
                registry);

            SetPrivateField(
                spawnManager,
                "arenaGenerator",
                arenaGenerator);

            SetPrivateField(
                spawnManager,
                "enemyUnitPrefab",
                enemyPrefab);

            SetPrivateField(
                spawnManager,
                "unitSpawner",
                unitSpawner);

            SetPrivateField(
                spawnManager,
                "enemyCount",
                1);

            SetGeneratedTiles(
                arenaGenerator,
                spawnTile);
        }

        [TearDown]
        public void TearDown()
        {
            for (
                int i = createdObjects.Count - 1;
                i >= 0;
                i--)
            {
                Object createdObject =
                    createdObjects[i];

                if (createdObject != null)
                {
                    Object.DestroyImmediate(
                        createdObject);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void SpawnEnemies_UsesSharedSpawnerAndRegistersEnemy()
        {
            spawnManager.SpawnEnemies();

            Assert.That(
                spawnManager.SpawnedEnemies,
                Has.Count.EqualTo(1));

            FightUnit spawnedEnemy =
                spawnManager.SpawnedEnemies[0];

            TrackSpawnedUnit(spawnedEnemy);

            Assert.That(
                spawnedEnemy,
                Is.Not.Null);

            Assert.That(
                spawnedEnemy.Definition,
                Is.SameAs(enemyDefinition));

            Assert.That(
                spawnedEnemy.TeamId,
                Is.EqualTo(
                    FightTeamId.TeamB));

            Assert.That(
                spawnedEnemy.ParticipantId,
                Is.EqualTo(
                    new FightParticipantId(
                        "enemy-ai")));

            Assert.That(
                spawnedEnemy.ControllerType,
                Is.EqualTo(
                    FightControllerType.AI));

            Assert.That(
                spawnedEnemy.CurrentTile,
                Is.SameAs(spawnTile));

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(1));

            Assert.That(
                registry.Units[0],
                Is.SameAs(spawnedEnemy));
        }

        [Test]
        public void SpawnEnemies_WithoutConfiguredDataUsesFreshEmptyState()
        {
            spawnManager.SpawnEnemies();

            Assert.That(
                spawnManager.SpawnedEnemies,
                Has.Count.EqualTo(1));

            FightUnit spawnedEnemy =
                spawnManager.SpawnedEnemies[0];

            TrackSpawnedUnit(spawnedEnemy);

            Assert.That(
                spawnedEnemy.ClassId.IsValid,
                Is.False);

            Assert.That(
                spawnedEnemy
                    .SpecializationId
                    .IsValid,
                Is.False);

            Assert.That(
                spawnedEnemy.PassiveIds,
                Is.Empty);

            Assert.That(
                spawnedEnemy.CurrentHealth,
                Is.EqualTo(
                    spawnedEnemy.MaxHealth));
        }

        [Test]
        public void SpawnEnemies_AppliesConfiguredBuildAndRuntimeState()
        {
            CharacterBuildSnapshot buildSnapshot =
                new CharacterBuildSnapshot(
                    new CharacterClassId(
                        "enemy_warrior"),
                    new CharacterSpecializationId(
                        "brute"),
                    null,
                    new[]
                    {
                        new FightStatModifier(
                            FightStatType.MaxHealth,
                            FightStatModifierType.Flat,
                            10),
                        new FightStatModifier(
                            FightStatType.AttackPower,
                            FightStatModifierType.Flat,
                            3)
                    },
                    new EquipmentLoadoutSnapshot(
                        new[]
                        {
                            new EquippedItemSnapshot(
                                EquipmentSlotType.MainHand,
                                new CharacterItemId(
                                    "rusty_axe"))
                        }),
                    new[]
                    {
                        new CharacterPassiveId(
                            "enemy_rage")
                    });

            spawnManager.ConfigureSpawnData(
                buildSnapshot,
                new FightUnitRuntimeSnapshot(12));

            spawnManager.SpawnEnemies();

            Assert.That(
                spawnManager.SpawnedEnemies,
                Has.Count.EqualTo(1));

            FightUnit spawnedEnemy =
                spawnManager.SpawnedEnemies[0];

            TrackSpawnedUnit(spawnedEnemy);

            Assert.That(
                spawnedEnemy.ClassId.Value,
                Is.EqualTo(
                    "enemy_warrior"));

            Assert.That(
                spawnedEnemy
                    .SpecializationId
                    .Value,
                Is.EqualTo("brute"));

            Assert.That(
                spawnedEnemy.MaxHealth,
                Is.EqualTo(20));

            Assert.That(
                spawnedEnemy.CurrentHealth,
                Is.EqualTo(12));

            Assert.That(
                spawnedEnemy.AttackPower,
                Is.EqualTo(5));

            Assert.That(
                spawnedEnemy
                    .EquipmentLoadout
                    .Items,
                Has.Count.EqualTo(1));

            Assert.That(
                spawnedEnemy
                    .EquipmentLoadout
                    .Items[0]
                    .ItemId
                    .Value,
                Is.EqualTo(
                    "rusty_axe"));

            Assert.That(
                spawnedEnemy.PassiveIds,
                Has.Count.EqualTo(1));

            Assert.That(
                spawnedEnemy
                    .PassiveIds[0]
                    .Value,
                Is.EqualTo(
                    "enemy_rage"));
        }

        [Test]
        public void ConfigureSpawnData_CopiesBuildSnapshot()
        {
            CharacterBuildSnapshot source =
                new CharacterBuildSnapshot(
                    new CharacterClassId(
                        "enemy_warrior"),
                    new CharacterSpecializationId(
                        "brute"),
                    null,
                    null);

            spawnManager.ConfigureSpawnData(
                source,
                FightUnitRuntimeSnapshot.Fresh);

            Assert.That(
                spawnManager
                    .EnemyBuildSnapshot,
                Is.EqualTo(source));

            Assert.That(
                ReferenceEquals(
                    spawnManager
                        .EnemyBuildSnapshot,
                    source),
                Is.False);
        }

        [Test]
        public void SpawnEnemies_CalledAgainReusesConfiguredSpawnData()
        {
            CharacterBuildSnapshot buildSnapshot =
                new CharacterBuildSnapshot(
                    new CharacterClassId(
                        "enemy_warrior"),
                    new CharacterSpecializationId(
                        "brute"),
                    null,
                    new[]
                    {
                        new FightStatModifier(
                            FightStatType.AttackPower,
                            FightStatModifierType.Flat,
                            3)
                    });

            spawnManager.ConfigureSpawnData(
                buildSnapshot,
                new FightUnitRuntimeSnapshot(6));

            spawnManager.SpawnEnemies();
            spawnManager.SpawnEnemies();

            FightUnit secondEnemy =
                spawnManager.SpawnedEnemies[0];

            TrackSpawnedUnit(secondEnemy);

            Assert.That(
                secondEnemy.ClassId.Value,
                Is.EqualTo(
                    "enemy_warrior"));

            Assert.That(
                secondEnemy.AttackPower,
                Is.EqualTo(5));

            Assert.That(
                secondEnemy.CurrentHealth,
                Is.EqualTo(6));
        }

        [Test]
        public void SpawnEnemies_WithoutSpawnerDoesNotCreateEnemy()
        {
            SetPrivateField<FightUnitSpawner>(
                spawnManager,
                "unitSpawner",
                null);

            LogAssert.Expect(
                LogType.Error,
                "EnemySpawnManager: " +
                "FightUnitSpawner is not assigned.");

            spawnManager.SpawnEnemies();

            Assert.That(
                spawnManager.SpawnedEnemies,
                Is.Empty);

            Assert.That(
                registry.Units,
                Is.Empty);
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
                $"Field '{fieldName}' " +
                "was not found.");

            field.SetValue(
                target,
                value);
        }

        private static void SetGeneratedTiles(
            FightArenaGenerator generator,
            params FightGridTile[] tiles)
        {
            FieldInfo field =
                typeof(FightArenaGenerator)
                    .GetField(
                        "generatedTiles",
                        BindingFlags.Instance |
                        BindingFlags.NonPublic);

            Assert.That(
                field,
                Is.Not.Null,
                "Field 'generatedTiles' " +
                "was not found.");

            List<FightGridTile> generatedTiles =
                field.GetValue(generator)
                    as List<FightGridTile>;

            Assert.That(
                generatedTiles,
                Is.Not.Null);

            generatedTiles.Clear();
            generatedTiles.AddRange(tiles);
        }
    }
}