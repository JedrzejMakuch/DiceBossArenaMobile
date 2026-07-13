using System.Reflection;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightDeploymentRegistryTests
    {
        private GameObject managerObject;
        private GameObject registryObject;
        private GameObject unitObject;

        private FightDeploymentManager deploymentManager;
        private FightUnitRegistry registry;
        private FightUnit unit;

        [SetUp]
        public void SetUp()
        {
            managerObject =
                new GameObject("FightDeploymentManager");

            deploymentManager =
                managerObject.AddComponent<FightDeploymentManager>();

            registryObject =
                new GameObject("FightUnitRegistry");

            registry =
                registryObject.AddComponent<FightUnitRegistry>();

            unitObject =
                new GameObject("PlayerUnit");

            unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                "Test Player",
                FightTeam.Player,
                10,
                2,
                10);

            SetPrivateField(
                deploymentManager,
                "unitRegistry",
                registry);

            SetPrivateField(
                deploymentManager,
                "playerUnit",
                unit);
        }

        [TearDown]
        public void TearDown()
        {
            if (unitObject != null)
            {
                Object.DestroyImmediate(unitObject);
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
        public void TryRegisterPlayerUnit_RegistersConfiguredPlayer()
        {
            bool result =
                deploymentManager.TryRegisterPlayerUnit();

            Assert.That(result, Is.True);
            Assert.That(registry.Units, Has.Count.EqualTo(1));
            Assert.That(registry.Units[0], Is.SameAs(unit));
        }

        [Test]
        public void TryRegisterPlayerUnit_CalledTwice_DoesNotDuplicatePlayer()
        {
            bool firstResult =
                deploymentManager.TryRegisterPlayerUnit();

            bool secondResult =
                deploymentManager.TryRegisterPlayerUnit();

            Assert.That(firstResult, Is.True);
            Assert.That(secondResult, Is.True);
            Assert.That(registry.Units, Has.Count.EqualTo(1));
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