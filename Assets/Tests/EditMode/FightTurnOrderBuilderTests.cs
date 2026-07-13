using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightTurnOrderBuilderTests
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
        public void Build_SortsLivingUnitsByDescendingInitiative()
        {
            FightUnit slowUnit =
                CreateUnit("Slow", 4);

            FightUnit fastUnit =
                CreateUnit("Fast", 12);

            FightUnit mediumUnit =
                CreateUnit("Medium", 8);

            List<FightUnit> result =
                FightTurnOrderBuilder.Build(
                    new[]
                    {
                        slowUnit,
                        fastUnit,
                        mediumUnit
                    });

            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result[0], Is.SameAs(fastUnit));
            Assert.That(result[1], Is.SameAs(mediumUnit));
            Assert.That(result[2], Is.SameAs(slowUnit));
        }

        [Test]
        public void Build_ExcludesNullAndDeadUnits()
        {
            FightUnit livingUnit =
                CreateUnit("Living", 5);

            FightUnit deadUnit =
                CreateUnit("Dead", 10);

            deadUnit.TakeDamage(
                deadUnit.MaxHealth);

            List<FightUnit> result =
                FightTurnOrderBuilder.Build(
                    new FightUnit[]
                    {
                        null,
                        deadUnit,
                        livingUnit
                    });

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0], Is.SameAs(livingUnit));
        }

        [Test]
        public void Build_DuplicateUnit_AddsItOnlyOnce()
        {
            FightUnit unit =
                CreateUnit("Unit", 7);

            List<FightUnit> result =
                FightTurnOrderBuilder.Build(
                    new[]
                    {
                        unit,
                        unit
                    });

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0], Is.SameAs(unit));
        }

        [Test]
        public void Build_NullCollection_ReturnsEmptyOrder()
        {
            List<FightUnit> result =
                FightTurnOrderBuilder.Build(null);

            Assert.That(result, Is.Empty);
        }

        private FightUnit CreateUnit(
            string unitName,
            int initiative)
        {
            GameObject unitObject =
                new GameObject(unitName);

            createdObjects.Add(unitObject);

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                unitName,
                FightTeam.Player,
                10,
                2,
                initiative);

            return unit;
        }
    }
}