using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ClassDefinitionCatalogTests
    {
        private ClassDefinition firstDefinition;
        private ClassDefinition secondDefinition;

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
                    "companion");

            ClassDefinitionCatalog catalog =
                new ClassDefinitionCatalog(
                    new[]
                    {
                        firstDefinition
                    });

            bool resolved =
                catalog.TryResolve(
                    new CharacterClassId(
                        "companion"),
                    out ClassDefinition result);

            Assert.That(
                resolved,
                Is.True);

            Assert.That(
                result,
                Is.SameAs(
                    firstDefinition));
        }

        [Test]
        public void TryResolve_UnknownIdReturnsFalse()
        {
            firstDefinition =
                CreateDefinition(
                    "companion");

            ClassDefinitionCatalog catalog =
                new ClassDefinitionCatalog(
                    new[]
                    {
                        firstDefinition
                    });

            bool resolved =
                catalog.TryResolve(
                    new CharacterClassId(
                        "mage"),
                    out ClassDefinition result);

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
                    "companion");

            secondDefinition =
                CreateDefinition(
                    "companion");

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new ClassDefinitionCatalog(
                            new[]
                            {
                                firstDefinition,
                                secondDefinition
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "duplicate class id: companion"));
        }

        [Test]
        public void Constructor_InvalidIdThrows()
        {
            firstDefinition =
                CreateDefinition(
                    string.Empty);

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new ClassDefinitionCatalog(
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
                        new ClassDefinitionCatalog(
                            new ClassDefinition[]
                            {
                                null
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "null class definition"));
        }

        [Test]
        public void EmptyCatalog_CannotResolveDefinition()
        {
            ClassDefinitionCatalog catalog =
                new ClassDefinitionCatalog(
                    null);

            bool resolved =
                catalog.TryResolve(
                    new CharacterClassId(
                        "companion"),
                    out ClassDefinition result);

            Assert.That(
                resolved,
                Is.False);

            Assert.That(
                result,
                Is.Null);
        }

        private ClassDefinition CreateDefinition(
            string classId)
        {
            ClassDefinition definition =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();

            definition.InitializeForTests(
                classId);

            return definition;
        }
    }
}