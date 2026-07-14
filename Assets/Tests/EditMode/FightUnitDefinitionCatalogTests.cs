using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitDefinitionCatalogTests
    {
        private FightUnitDefinition firstDefinition;
        private FightUnitDefinition secondDefinition;

        [TearDown]
        public void TearDown()
        {
            if (firstDefinition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    firstDefinition);
            }

            if (secondDefinition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    secondDefinition);
            }
        }

        [Test]
        public void TryResolve_KnownIdReturnsDefinition()
        {
            firstDefinition =
                CreateDefinition(
                    "enemy_slime",
                    "Slime");

            FightUnitDefinitionCatalog catalog =
                new FightUnitDefinitionCatalog(
                    new[]
                    {
                        firstDefinition
                    });

            bool resolved =
                catalog.TryResolve(
                    new FightUnitDefinitionId(
                        "enemy_slime"),
                    out FightUnitDefinition result);

            Assert.That(
                resolved,
                Is.True);

            Assert.That(
                result,
                Is.SameAs(firstDefinition));
        }

        [Test]
        public void TryResolve_UnknownIdReturnsFalse()
        {
            firstDefinition =
                CreateDefinition(
                    "enemy_slime",
                    "Slime");

            FightUnitDefinitionCatalog catalog =
                new FightUnitDefinitionCatalog(
                    new[]
                    {
                        firstDefinition
                    });

            bool resolved =
                catalog.TryResolve(
                    new FightUnitDefinitionId(
                        "enemy_goblin"),
                    out FightUnitDefinition result);

            Assert.That(
                resolved,
                Is.False);

            Assert.That(
                result,
                Is.Null);
        }

        [Test]
        public void Constructor_DuplicateIdThrows()
        {
            firstDefinition =
                CreateDefinition(
                    "enemy_slime",
                    "Slime");

            secondDefinition =
                CreateDefinition(
                    "enemy_slime",
                    "Green Slime");

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new FightUnitDefinitionCatalog(
                            new[]
                            {
                                firstDefinition,
                                secondDefinition
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "duplicate unit id: enemy_slime"));
        }

        [Test]
        public void Constructor_InvalidIdThrows()
        {
            firstDefinition =
                CreateDefinition(
                    string.Empty,
                    "Slime");

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new FightUnitDefinitionCatalog(
                            new[]
                            {
                                firstDefinition
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "invalid id"));
        }

        [Test]
        public void Constructor_NullDefinitionThrows()
        {
            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new FightUnitDefinitionCatalog(
                            new FightUnitDefinition[]
                            {
                                null
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "null unit definition"));
        }

        [Test]
        public void EmptyCatalog_CannotResolveDefinition()
        {
            FightUnitDefinitionCatalog catalog =
                new FightUnitDefinitionCatalog(
                    null);

            bool resolved =
                catalog.TryResolve(
                    new FightUnitDefinitionId(
                        "enemy_slime"),
                    out FightUnitDefinition result);

            Assert.That(
                resolved,
                Is.False);

            Assert.That(
                result,
                Is.Null);
        }

        private FightUnitDefinition CreateDefinition(
            string unitId,
            string unitName)
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName:
                    unitName,
                newTeam:
                    FightTeam.Enemy,
                newMaxHealth:
                    10,
                newAttackPower:
                    2,
                newInitiative:
                    5,
                newUnitId:
                    unitId);

            return definition;
        }
    }
}