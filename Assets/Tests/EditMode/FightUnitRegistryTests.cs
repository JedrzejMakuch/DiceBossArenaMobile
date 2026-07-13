using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitRegistryTests
    {
        private GameObject registryObject;
        private GameObject unitObject;
        private FightUnitRegistry registry;
        private FightUnit unit;

        [SetUp]
        public void SetUp()
        {
            registryObject =
                new GameObject("FightUnitRegistry");

            registry =
                registryObject.AddComponent<FightUnitRegistry>();

            unitObject =
                new GameObject("FightUnit");

            unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                "Test Unit",
                FightTeam.Player,
                10,
                2,
                10);
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
        }

        [Test]
        public void Register_AddsUnitAndRaisesEventOnce()
        {
            int eventCount = 0;
            FightUnit registeredUnit = null;

            registry.UnitRegistered += registered =>
            {
                eventCount++;
                registeredUnit = registered;
            };

            bool result =
                registry.Register(unit);

            Assert.That(result, Is.True);
            Assert.That(registry.Units, Has.Count.EqualTo(1));
            Assert.That(registry.Units[0], Is.SameAs(unit));
            Assert.That(eventCount, Is.EqualTo(1));
            Assert.That(registeredUnit, Is.SameAs(unit));
        }

        [Test]
        public void Register_SameUnitTwice_AddsItOnlyOnce()
        {
            int eventCount = 0;

            registry.UnitRegistered += _ =>
                eventCount++;

            bool firstResult =
                registry.Register(unit);

            bool secondResult =
                registry.Register(unit);

            Assert.That(firstResult, Is.True);
            Assert.That(secondResult, Is.False);
            Assert.That(registry.Units, Has.Count.EqualTo(1));
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void Register_Null_DoesNothing()
        {
            bool result =
                registry.Register(null);

            Assert.That(result, Is.False);
            Assert.That(registry.Units, Is.Empty);
        }

        [Test]
        public void Unregister_RemovesUnitAndRaisesEventOnce()
        {
            registry.Register(unit);

            int eventCount = 0;
            FightUnit unregisteredUnit = null;

            registry.UnitUnregistered += unregistered =>
            {
                eventCount++;
                unregisteredUnit = unregistered;
            };

            bool result =
                registry.Unregister(unit);

            Assert.That(result, Is.True);
            Assert.That(registry.Units, Is.Empty);
            Assert.That(eventCount, Is.EqualTo(1));
            Assert.That(unregisteredUnit, Is.SameAs(unit));
        }

        [Test]
        public void Unregister_NotRegisteredUnit_DoesNothing()
        {
            int eventCount = 0;

            registry.UnitUnregistered += _ =>
                eventCount++;

            bool result =
                registry.Unregister(unit);

            Assert.That(result, Is.False);
            Assert.That(registry.Units, Is.Empty);
            Assert.That(eventCount, Is.Zero);
        }

        [Test]
        public void RegisteredUnit_WhenItDies_IsAutomaticallyUnregistered()
        {
            int eventCount = 0;
            FightUnit unregisteredUnit = null;

            registry.Register(unit);

            registry.UnitUnregistered += unregistered =>
            {
                eventCount++;
                unregisteredUnit = unregistered;
            };

            unit.TakeDamage(unit.MaxHealth);

            Assert.That(registry.Units, Is.Empty);
            Assert.That(eventCount, Is.EqualTo(1));
            Assert.That(unregisteredUnit, Is.SameAs(unit));
        }

        [Test]
        public void ManuallyUnregisteredUnit_WhenItDies_DoesNotRaiseAnotherEvent()
        {
            int eventCount = 0;

            registry.Register(unit);

            registry.UnitUnregistered += _ =>
                eventCount++;

            registry.Unregister(unit);

            unit.TakeDamage(unit.MaxHealth);

            Assert.That(registry.Units, Is.Empty);
            Assert.That(eventCount, Is.EqualTo(1));
        }

        [Test]
        public void DetachAllUnits_RemovesDeathSubscriptions()
        {
            int eventCount = 0;

            registry.Register(unit);

            registry.UnitUnregistered += _ =>
                eventCount++;

            registry.DetachAllUnits();

            unit.TakeDamage(unit.MaxHealth);

            Assert.That(registry.Units, Is.Empty);
            Assert.That(eventCount, Is.Zero);
        }
    }
}