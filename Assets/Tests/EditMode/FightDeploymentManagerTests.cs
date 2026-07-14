using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightDeploymentManagerTests
    {
        private readonly List<Object> createdObjects = new();

        private FightDeploymentManager deploymentManager;
        private FightUnitSpawner unitSpawner;
        private FightUnitRegistry registry;
        private FightArenaGenerator arenaGenerator;
        private FightUnit playerPrefab;
        private FightUnitDefinition playerDefinition;
        private FightGridTile firstTile;
        private FightGridTile secondTile;
        private Button startFightButton;

        [SetUp]
        public void SetUp()
        {
            deploymentManager =
                CreateComponent<FightDeploymentManager>(
                    "FightDeploymentManager");

            unitSpawner =
                CreateComponent<FightUnitSpawner>(
                    "FightUnitSpawner");

            registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            arenaGenerator =
                CreateComponent<FightArenaGenerator>(
                    "FightArenaGenerator");

            playerPrefab =
                CreateComponent<FightUnit>(
                    "PlayerPrefab");

            playerPrefab.gameObject
                .AddComponent<FightUnitSkills>();

            playerDefinition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            createdObjects.Add(playerDefinition);

            playerDefinition.InitializeForTests(
                newUnitName: "Test Player",
                newTeam: FightTeam.Player,
                newMaxHealth: 15,
                newAttackPower: 4,
                newInitiative: 11,
                newStartingSkills: null);

            playerPrefab.Initialize(playerDefinition);

            firstTile =
                CreateComponent<FightGridTile>(
                    "FirstSpawnTile");

            firstTile.Initialize(1, 2);

            secondTile =
                CreateComponent<FightGridTile>(
                    "SecondSpawnTile");

            secondTile.Initialize(3, 4);

            startFightButton =
                CreateComponent<Button>(
                    "StartFightButton");

            SetPrivateField(
                unitSpawner,
                "unitRegistry",
                registry);

            SetPrivateField(
                deploymentManager,
                "arenaGenerator",
                arenaGenerator);

            SetPrivateField(
                deploymentManager,
                "unitSpawner",
                unitSpawner);

            SetPrivateField(
                deploymentManager,
                "playerUnitPrefab",
                playerPrefab);

            SetPrivateField(
                deploymentManager,
                "startFightButton",
                startFightButton);

            SetPrivateField(
                deploymentManager,
                "playerSpawnCount",
                2);

            SetGeneratedTiles(
                arenaGenerator,
                firstTile,
                secondTile);

            deploymentManager.PrepareDeployment();
        }

        [TearDown]
        public void TearDown()
        {
            FightUnit spawnedPlayer =
                deploymentManager != null
                    ? deploymentManager.PlayerUnit
                    : null;

            if (spawnedPlayer != null)
            {
                createdObjects.Add(spawnedPlayer.gameObject);
            }

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
        public void SelectPlayerSpawnTile_SpawnsAndRegistersPlayer()
        {
            InvokePrivateMethod(
                deploymentManager,
                "SelectPlayerSpawnTile",
                firstTile);

            FightUnit player =
                deploymentManager.PlayerUnit;

            Assert.That(player, Is.Not.Null);
            Assert.That(player.Definition, Is.SameAs(playerDefinition));
            Assert.That(player.CurrentTile, Is.SameAs(firstTile));

            Assert.That(
                player.TeamId,
                Is.EqualTo(FightTeamId.TeamA));

            Assert.That(
                player.ParticipantId,
                Is.EqualTo(
                    new FightParticipantId("local-player")));

            Assert.That(
                player.ControllerType,
                Is.EqualTo(
                    FightControllerType.LocalPlayer));

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(1));

            Assert.That(
                registry.Units[0],
                Is.SameAs(player));

            Assert.That(
                startFightButton.interactable,
                Is.True);
        }

        [Test]
        public void SelectSecondTile_MovesExistingPlayerWithoutDuplicateRegistration()
        {
            InvokePrivateMethod(
                deploymentManager,
                "SelectPlayerSpawnTile",
                firstTile);

            FightUnit firstPlayer =
                deploymentManager.PlayerUnit;

            InvokePrivateMethod(
                deploymentManager,
                "SelectPlayerSpawnTile",
                secondTile);

            FightUnit secondPlayer =
                deploymentManager.PlayerUnit;

            Assert.That(secondPlayer, Is.SameAs(firstPlayer));
            Assert.That(secondPlayer.CurrentTile, Is.SameAs(secondTile));

            Assert.That(firstTile.IsOccupied, Is.False);
            Assert.That(secondTile.OccupyingUnit, Is.SameAs(secondPlayer));

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(1));

            Assert.That(
                registry.Units[0],
                Is.SameAs(secondPlayer));
        }

        [Test]
        public void PrepareDeployment_WhenPlayerExists_DespawnsPreviousPlayer()
        {
            InvokePrivateMethod(
                deploymentManager,
                "SelectPlayerSpawnTile",
                firstTile);

            FightUnit firstPlayer =
                deploymentManager.PlayerUnit;

            Assert.That(firstPlayer, Is.Not.Null);
            Assert.That(registry.Units, Has.Count.EqualTo(1));

            deploymentManager.PrepareDeployment();

            Assert.That(
                deploymentManager.PlayerUnit,
                Is.Null);

            Assert.That(
                registry.Units,
                Is.Empty);

            Assert.That(
                firstTile.IsOccupied,
                Is.False);

            Assert.That(
                firstPlayer == null,
                Is.True);
        }


        [Test]
        public void PrepareDeployment_AgainReusesConfiguredPlayerData()
        {
            CharacterBuildSnapshot buildSnapshot =
                new CharacterBuildSnapshot(
                    new CharacterClassId(
                        "warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    null,
                    new[]
                    {
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    3)
                    });

            deploymentManager.ConfigurePlayerSpawnData(
                buildSnapshot,
                new FightUnitRuntimeSnapshot(8));

            InvokePrivateMethod(
                deploymentManager,
                "SelectPlayerSpawnTile",
                firstTile);

            FightUnit firstPlayer =
                deploymentManager.PlayerUnit;

            Assert.That(
                firstPlayer,
                Is.Not.Null);

            deploymentManager.PrepareDeployment();

            Assert.That(
                firstPlayer == null,
                Is.True);

            InvokePrivateMethod(
                deploymentManager,
                "SelectPlayerSpawnTile",
                secondTile);

            FightUnit secondPlayer =
                deploymentManager.PlayerUnit;

            Assert.That(
                secondPlayer,
                Is.Not.Null);

            Assert.That(
                secondPlayer.ClassId.Value,
                Is.EqualTo("warrior"));

            Assert.That(
                secondPlayer.SpecializationId.Value,
                Is.EqualTo("guardian"));

            Assert.That(
                secondPlayer.CurrentHealth,
                Is.EqualTo(8));

            Assert.That(
                secondPlayer.AttackPower,
                Is.EqualTo(
                    playerDefinition.AttackPower + 3));

            Assert.That(
                secondPlayer.CurrentTile,
                Is.SameAs(secondTile));

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(1));

            Assert.That(
                registry.Units[0],
                Is.SameAs(secondPlayer));
        }

        [Test]
        public void ConfigurePlayerSpawnData_CopiesBuildSnapshot()
        {
            CharacterBuildSnapshot source =
                new CharacterBuildSnapshot(
                    new CharacterClassId(
                        "warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    null,
                    null);

            deploymentManager.ConfigurePlayerSpawnData(
                source,
                FightUnitRuntimeSnapshot.Fresh);

            Assert.That(
                deploymentManager.PlayerBuildSnapshot,
                Is.EqualTo(source));

            Assert.That(
                ReferenceEquals(
                    deploymentManager.PlayerBuildSnapshot,
                    source),
                Is.False);
        }

        [Test]
        public void SelectPlayerSpawnTile_WithoutConfiguredDataUsesFreshEmptyState()
        {
            InvokePrivateMethod(
                deploymentManager,
                "SelectPlayerSpawnTile",
                firstTile);

            FightUnit player =
                deploymentManager.PlayerUnit;

            Assert.That(
                player,
                Is.Not.Null);

            Assert.That(
                player.ClassId.IsValid,
                Is.False);

            Assert.That(
                player.SpecializationId.IsValid,
                Is.False);

            Assert.That(
                player.PassiveIds,
                Is.Empty);

            Assert.That(
                player.CurrentHealth,
                Is.EqualTo(
                    player.MaxHealth));
        }

        [Test]
        public void SelectPlayerSpawnTile_AppliesConfiguredBuildAndRuntimeState()
        {
            CharacterBuildSnapshot buildSnapshot =
                new CharacterBuildSnapshot(
                    new CharacterClassId(
                        "warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
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
                        EquipmentSlotType.Weapon,
                        new CharacterItemId(
                            "iron_sword"))
                        }),
                    new[]
                    {
                new CharacterPassiveId(
                    "shield_mastery")
                    });

            deploymentManager.ConfigurePlayerSpawnData(
                buildSnapshot,
                new FightUnitRuntimeSnapshot(17));

            InvokePrivateMethod(
                deploymentManager,
                "SelectPlayerSpawnTile",
                firstTile);

            FightUnit player =
                deploymentManager.PlayerUnit;

            Assert.That(
                player,
                Is.Not.Null);

            Assert.That(
                player.ClassId.Value,
                Is.EqualTo("warrior"));

            Assert.That(
                player.SpecializationId.Value,
                Is.EqualTo("guardian"));

            Assert.That(
                player.MaxHealth,
                Is.EqualTo(
                    playerDefinition.MaxHealth + 10));

            Assert.That(
                player.CurrentHealth,
                Is.EqualTo(17));

            Assert.That(
                player.AttackPower,
                Is.EqualTo(
                    playerDefinition.AttackPower + 3));

            Assert.That(
                player.EquipmentLoadout.Items,
                Has.Count.EqualTo(1));

            Assert.That(
                player.EquipmentLoadout.Items[0]
                    .ItemId.Value,
                Is.EqualTo("iron_sword"));

            Assert.That(
                player.PassiveIds,
                Has.Count.EqualTo(1));

            Assert.That(
                player.PassiveIds[0].Value,
                Is.EqualTo(
                    "shield_mastery"));

            Assert.That(
                player.CurrentTile,
                Is.SameAs(firstTile));

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(1));
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

        private static void SetGeneratedTiles(
            FightArenaGenerator generator,
            params FightGridTile[] tiles)
        {
            FieldInfo field =
                typeof(FightArenaGenerator).GetField(
                    "generatedTiles",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.That(
                field,
                Is.Not.Null,
                "Field 'generatedTiles' was not found.");

            List<FightGridTile> generatedTiles =
                field.GetValue(generator)
                    as List<FightGridTile>;

            Assert.That(
                generatedTiles,
                Is.Not.Null);

            generatedTiles.Clear();
            generatedTiles.AddRange(tiles);
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

        private static void InvokePrivateMethod(
            object target,
            string methodName,
            params object[] arguments)
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

            method.Invoke(
                target,
                arguments);
        }
    }
}