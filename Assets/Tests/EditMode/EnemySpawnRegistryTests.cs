using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EnemySpawnRegistryTests
    {
        private GameObject managerObject;
        private GameObject registryObject;
        private GameObject enemyObject;

        private EnemySpawnManager spawnManager;
        private FightUnitRegistry registry;
        private FightUnit enemy;

        [SetUp]
        public void SetUp()
        {
            managerObject =
                new GameObject("EnemySpawnManager");

            spawnManager =
                managerObject.AddComponent<EnemySpawnManager>();

            registryObject =
                new GameObject("FightUnitRegistry");

            registry =
                registryObject.AddComponent<FightUnitRegistry>();

            enemyObject =
                new GameObject("Enemy");

            enemy =
                enemyObject.AddComponent<FightUnit>();

            enemy.Initialize(
                "Test Enemy",
                FightTeam.Enemy,
                10,
                2,
                10);

            SetPrivateField(
                spawnManager,
                "unitRegistry",
                registry);
        }

        [TearDown]
        public void TearDown()
        {
            if (enemyObject != null)
            {
                Object.DestroyImmediate(enemyObject);
            }

            if (registryObject != null)
            {
                Object.DestroyImmediate(registryObject);
            }

            if (managerObject != null)
            {
                Object.DestroyImmediate(managerObject);
            }
        }

        [Test]
        public void TryRegisterEnemy_RegistersConfiguredEnemy()
        {
            bool result =
                spawnManager.TryRegisterEnemy(enemy);

            Assert.That(result, Is.True);
            Assert.That(registry.Units, Has.Count.EqualTo(1));
            Assert.That(registry.Units[0], Is.SameAs(enemy));
        }

        [Test]
        public void TryRegisterEnemy_CalledTwice_DoesNotDuplicateEnemy()
        {
            bool firstResult =
                spawnManager.TryRegisterEnemy(enemy);

            bool secondResult =
                spawnManager.TryRegisterEnemy(enemy);

            Assert.That(firstResult, Is.True);
            Assert.That(secondResult, Is.True);
            Assert.That(registry.Units, Has.Count.EqualTo(1));
        }

        [Test]
        public void TryRegisterEnemy_WithoutRegistry_ReturnsFalse()
        {
            SetPrivateField<FightUnitRegistry>(
                spawnManager,
                "unitRegistry",
                null);

            bool result =
                spawnManager.TryRegisterEnemy(enemy);

            Assert.That(result, Is.False);
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

        [Test]
        public void RegisteredEnemy_WhenUnregistered_CanBeReplacedWithoutStaleReference()
        {
            GameObject secondEnemyObject =
                new GameObject("SecondEnemy");

            FightUnit secondEnemy =
                secondEnemyObject.AddComponent<FightUnit>();

            secondEnemy.Initialize(
                "Second Test Enemy",
                FightTeam.Enemy,
                10,
                2,
                10);

            try
            {
                bool firstRegisterResult =
                    spawnManager.TryRegisterEnemy(enemy);

                bool unregisterResult =
                    registry.Unregister(enemy);

                bool secondRegisterResult =
                    spawnManager.TryRegisterEnemy(secondEnemy);

                Assert.That(firstRegisterResult, Is.True);
                Assert.That(unregisterResult, Is.True);
                Assert.That(secondRegisterResult, Is.True);

                Assert.That(registry.Units, Has.Count.EqualTo(1));
                Assert.That(registry.Units[0], Is.SameAs(secondEnemy));
                CollectionAssert.DoesNotContain(registry.Units, enemy);
            }
            finally
            {
                Object.DestroyImmediate(secondEnemyObject);
            }
        }
    }
}