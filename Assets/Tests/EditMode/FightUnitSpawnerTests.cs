using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.TestTools;

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

            prefab.gameObject.AddComponent<FightUnitSkills>();

            definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

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

        [Test]
        public void Spawn_AppliesBuildSnapshot()
        {
            SkillDefinition skillDefinition =
                ScriptableObject.CreateInstance<
                    SkillDefinition>();

            skillDefinition.InitializeForTests(
                "shield_bash",
                "Shield Bash",
                3);

            spawner.InitializeBuildResolverForTests(
                new[]
                {
            skillDefinition
                });

            CharacterBuildSnapshot build =
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    new[]
                    {
                new CharacterBuildSkill(
                    "shield_bash",
                    2)
                    },
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

            FightUnitSpawnRequest request =
                new FightUnitSpawnRequest(
                    prefab,
                    definition,
                    ownership,
                    tile,
                    buildSnapshot: build);

            FightUnit unit =
                spawner.Spawn(request);

            Assert.That(
                unit,
                Is.Not.Null);

            Assert.That(
                unit.ClassId.Value,
                Is.EqualTo("warrior"));

            Assert.That(
                unit.SpecializationId.Value,
                Is.EqualTo("guardian"));

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(
                    definition.MaxHealth + 10));

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(
                    definition.AttackPower + 3));

            Assert.That(
                unit.Skills.Skills.Count,
                Is.EqualTo(1));

            Assert.That(
                unit.Skills.Skills[0].Definition,
                Is.SameAs(skillDefinition));

            Assert.That(
                unit.Skills.Skills[0].Level,
                Is.EqualTo(2));

            Assert.That(
                unit.EquipmentLoadout.Items[0]
                    .ItemId.Value,
                Is.EqualTo("iron_sword"));

            Assert.That(
                unit.PassiveIds[0].Value,
                Is.EqualTo("shield_mastery"));

            Object.DestroyImmediate(skillDefinition);
        }

        [Test]
        public void Spawn_UnknownBuildSkillReturnsNull()
        {
            spawner.InitializeBuildResolverForTests(
                null);

            CharacterBuildSnapshot build =
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    new[]
                    {
                new CharacterBuildSkill(
                    "missing_skill",
                    1)
                    },
                    null);

            FightUnitSpawnRequest request =
                new FightUnitSpawnRequest(
                    prefab,
                    definition,
                    ownership,
                    tile,
                    buildSnapshot: build);

            LogAssert.Expect(
                LogType.Error,
                "FightUnitSpawner could not resolve build. " +
                "Could not resolve skill id: missing_skill.");

            FightUnit result =
                spawner.Spawn(request);

            Assert.That(
                result,
                Is.Null);

            Assert.That(
                tile.IsOccupied,
                Is.False);

            Assert.That(
                tile.OccupyingUnit,
                Is.Null);
        }

        [Test]
        public void Spawn_WithoutExplicitBuildStillWorks()
        {
            FightUnitSpawnRequest request =
                new FightUnitSpawnRequest(
                    prefab,
                    definition,
                    ownership,
                    tile);

            FightUnit unit =
                spawner.Spawn(request);

            Assert.That(
                unit,
                Is.Not.Null);

            Assert.That(
                unit.ClassId.IsValid,
                Is.False);

            Assert.That(
                unit.SpecializationId.IsValid,
                Is.False);

            Assert.That(
                unit.PassiveIds,
                Is.Empty);

            Assert.That(
                unit.EquipmentLoadout.Items,
                Is.Empty);
        }

        [Test]
        public void Spawn_RestoresCurrentHealthAfterApplyingBuild()
        {
            CharacterBuildSnapshot build =
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    null,
                    new[]
                    {
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10)
                    });

            FightUnitSpawnRequest request =
                new FightUnitSpawnRequest(
                    prefab,
                    definition,
                    ownership,
                    tile,
                    runtimeSnapshot:
                        new FightUnitRuntimeSnapshot(24),
                    buildSnapshot:
                        build);

            FightUnit unit =
                spawner.Spawn(request);

            TrackSpawnedUnit(unit);

            Assert.That(
                unit,
                Is.Not.Null);

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(30));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(24));
        }

        [Test]
        public void Spawn_ClampsRestoredHealthToBuildModifiedMaximum()
        {
            CharacterBuildSnapshot build =
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    null,
                    new[]
                    {
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10)
                    });

            FightUnitSpawnRequest request =
                new FightUnitSpawnRequest(
                    prefab,
                    definition,
                    ownership,
                    tile,
                    runtimeSnapshot:
                        new FightUnitRuntimeSnapshot(100),
                    buildSnapshot:
                        build);

            FightUnit unit =
                spawner.Spawn(request);

            TrackSpawnedUnit(unit);

            Assert.That(
                unit,
                Is.Not.Null);

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(30));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(30));
        }

        [Test]
        public void Spawn_FreshRuntimeStartsAtFinalMaximumHealth()
        {
            CharacterBuildSnapshot build =
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    null,
                    new[]
                    {
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10)
                    });

            FightUnitSpawnRequest request =
                new FightUnitSpawnRequest(
                    prefab,
                    definition,
                    ownership,
                    tile,
                    buildSnapshot:
                        build);

            FightUnit unit =
                spawner.Spawn(request);

            TrackSpawnedUnit(unit);

            Assert.That(
                unit,
                Is.Not.Null);

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(30));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(30));
        }

        [Test]
        public void Spawn_ExplicitBuildReplacesDefinitionStartingSkills()
        {
            SkillDefinition definitionSkill =
                ScriptableObject.CreateInstance<
                    SkillDefinition>();

            createdObjects.Add(definitionSkill);

            definitionSkill.InitializeForTests(
                "basic_attack",
                "Basic Attack",
                0);

            SkillDefinition buildSkill =
                ScriptableObject.CreateInstance<
                    SkillDefinition>();

            createdObjects.Add(buildSkill);

            buildSkill.InitializeForTests(
                "shield_bash",
                "Shield Bash",
                3);

            spawner.InitializeBuildResolverForTests(
                new[]
                {
            definitionSkill,
            buildSkill
                });

            FightUnitDefinition definitionWithSkill =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            createdObjects.Add(
                definitionWithSkill);

            definitionWithSkill.InitializeForTests(
                newUnitName:
                    "Skilled Warrior",
                newTeam:
                    FightTeam.Player,
                newMaxHealth:
                    20,
                newAttackPower:
                    5,
                newInitiative:
                    8,
                newStartingSkills:
                    new[]
                    {
                new UnitStartingSkill(
                    definitionSkill,
                    1)
                    });

            CharacterBuildSnapshot build =
                new CharacterBuildSnapshot(
                    new CharacterClassId(
                        "warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    new[]
                    {
                new CharacterBuildSkill(
                    "shield_bash",
                    2)
                    },
                    null);

            FightUnitSpawnRequest request =
                new FightUnitSpawnRequest(
                    prefab:
                        prefab,
                    definition:
                        definitionWithSkill,
                    ownership:
                        ownership,
                    tile:
                        tile,
                    buildSnapshot:
                        build);

            FightUnit unit =
                spawner.Spawn(request);

            TrackSpawnedUnit(unit);

            Assert.That(
                unit,
                Is.Not.Null);

            Assert.That(
                unit.Skills.Skills,
                Has.Count.EqualTo(1));

            Assert.That(
                unit.Skills.Skills[0]
                    .Definition,
                Is.SameAs(buildSkill));

            Assert.That(
                unit.Skills.Skills[0]
                    .Level,
                Is.EqualTo(2));

            Assert.That(
                unit.Skills.GetSkillById(
                    "basic_attack"),
                Is.Null);
        }

        [Test]
        public void Spawn_SameSnapshotsCreateIndependentDeterministicUnits()
        {
            FightGridTile secondTile =
                CreateComponent<FightGridTile>(
                    "SecondSpawnTile");

            secondTile.Initialize(4, 5);

            CharacterBuildSnapshot sharedBuild =
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

            FightUnitRuntimeSnapshot sharedRuntime =
                new FightUnitRuntimeSnapshot(24);

            FightUnitSpawnRequest firstRequest =
                new FightUnitSpawnRequest(
                    prefab:
                        prefab,
                    definition:
                        definition,
                    ownership:
                        ownership,
                    tile:
                        tile,
                    parent:
                        spawner.transform,
                    objectName:
                        "FirstUnit",
                    runtimeSnapshot:
                        sharedRuntime,
                    buildSnapshot:
                        sharedBuild);

            FightUnitSpawnRequest secondRequest =
                new FightUnitSpawnRequest(
                    prefab:
                        prefab,
                    definition:
                        definition,
                    ownership:
                        ownership,
                    tile:
                        secondTile,
                    parent:
                        spawner.transform,
                    objectName:
                        "SecondUnit",
                    runtimeSnapshot:
                        sharedRuntime,
                    buildSnapshot:
                        sharedBuild);

            FightUnit firstUnit =
                spawner.Spawn(firstRequest);

            FightUnit secondUnit =
                spawner.Spawn(secondRequest);

            TrackSpawnedUnit(firstUnit);
            TrackSpawnedUnit(secondUnit);

            Assert.That(
                firstUnit,
                Is.Not.Null);

            Assert.That(
                secondUnit,
                Is.Not.Null);

            Assert.That(
                firstUnit,
                Is.Not.SameAs(secondUnit));

            Assert.That(
                firstUnit.gameObject,
                Is.Not.SameAs(
                    secondUnit.gameObject));

            Assert.That(
                registry.Units,
                Has.Count.EqualTo(2));

            Assert.That(
                firstUnit.ClassId,
                Is.EqualTo(
                    secondUnit.ClassId));

            Assert.That(
                firstUnit.SpecializationId,
                Is.EqualTo(
                    secondUnit.SpecializationId));

            Assert.That(
                firstUnit.MaxHealth,
                Is.EqualTo(30));

            Assert.That(
                secondUnit.MaxHealth,
                Is.EqualTo(30));

            Assert.That(
                firstUnit.AttackPower,
                Is.EqualTo(8));

            Assert.That(
                secondUnit.AttackPower,
                Is.EqualTo(8));

            Assert.That(
                firstUnit.CurrentHealth,
                Is.EqualTo(24));

            Assert.That(
                secondUnit.CurrentHealth,
                Is.EqualTo(24));

            Assert.That(
                firstUnit.CurrentTile,
                Is.SameAs(tile));

            Assert.That(
                secondUnit.CurrentTile,
                Is.SameAs(secondTile));

            Assert.That(
                firstUnit.EquipmentLoadout.Items[0]
                    .ItemId.Value,
                Is.EqualTo("iron_sword"));

            Assert.That(
                secondUnit.EquipmentLoadout.Items[0]
                    .ItemId.Value,
                Is.EqualTo("iron_sword"));

            Assert.That(
                firstUnit.PassiveIds[0].Value,
                Is.EqualTo("shield_mastery"));

            Assert.That(
                secondUnit.PassiveIds[0].Value,
                Is.EqualTo("shield_mastery"));

            firstUnit.TakeDamage(5);

            Assert.That(
                firstUnit.CurrentHealth,
                Is.EqualTo(19));

            Assert.That(
                secondUnit.CurrentHealth,
                Is.EqualTo(24));

            Assert.That(
                secondTile.OccupyingUnit,
                Is.SameAs(secondUnit));

            Assert.That(
                secondUnit.IsAlive,
                Is.True);
        }

        [Test]
        public void Spawn_EmptyBuildPreservesDefinitionStartingSkills()
        {
            SkillDefinition startingSkill =
                ScriptableObject.CreateInstance<
                    SkillDefinition>();

            createdObjects.Add(startingSkill);

            startingSkill.InitializeForTests(
                "basic_attack",
                "Basic Attack",
                0);

            FightUnitDefinition definitionWithSkill =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            createdObjects.Add(
                definitionWithSkill);

            definitionWithSkill.InitializeForTests(
                newUnitName:
                    "Skilled Warrior",
                newTeam:
                    FightTeam.Player,
                newMaxHealth:
                    20,
                newAttackPower:
                    5,
                newInitiative:
                    8,
                newStartingSkills:
                    new[]
                    {
                new UnitStartingSkill(
                    startingSkill,
                    1)
                    });

            FightUnitSpawnRequest request =
                new FightUnitSpawnRequest(
                    prefab:
                        prefab,
                    definition:
                        definitionWithSkill,
                    ownership:
                        ownership,
                    tile:
                        tile,
                    buildSnapshot:
                        CharacterBuildSnapshot.Empty);

            FightUnit unit =
                spawner.Spawn(request);

            TrackSpawnedUnit(unit);

            Assert.That(
                unit,
                Is.Not.Null);

            Assert.That(
                unit.Skills,
                Is.Not.Null);

            Assert.That(
                unit.Skills.Skills,
                Has.Count.EqualTo(1));

            Assert.That(
                unit.Skills.Skills[0]
                    .Definition,
                Is.SameAs(startingSkill));

            Assert.That(
                unit.Skills.Skills[0]
                    .Level,
                Is.EqualTo(1));
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