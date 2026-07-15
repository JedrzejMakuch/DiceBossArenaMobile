using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        SpecializationDefinitionCatalogTests
    {
        private SpecializationDefinition
            firstDefinition;

        private SpecializationDefinition
            secondDefinition;

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
                    "berserker",
                    "companion");

            SpecializationDefinitionCatalog catalog =
                new SpecializationDefinitionCatalog(
                    new[]
                    {
                        firstDefinition
                    });

            bool resolved =
                catalog.TryResolve(
                    new CharacterSpecializationId(
                        "berserker"),
                    out SpecializationDefinition result);

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
                    "berserker",
                    "companion");

            SpecializationDefinitionCatalog catalog =
                new SpecializationDefinitionCatalog(
                    new[]
                    {
                        firstDefinition
                    });

            bool resolved =
                catalog.TryResolve(
                    new CharacterSpecializationId(
                        "guardian"),
                    out SpecializationDefinition result);

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
                    "berserker",
                    "companion");

            secondDefinition =
                CreateDefinition(
                    "berserker",
                    "companion");

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new SpecializationDefinitionCatalog(
                            new[]
                            {
                                firstDefinition,
                                secondDefinition
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "duplicate specialization id: " +
                    "berserker"));
        }

        [Test]
        public void Constructor_InvalidIdThrows()
        {
            firstDefinition =
                CreateDefinition(
                    string.Empty,
                    "companion");

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new SpecializationDefinitionCatalog(
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
        public void Constructor_InvalidRequiredClassIdThrows()
        {
            firstDefinition =
                CreateDefinition(
                    "berserker",
                    string.Empty);

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new SpecializationDefinitionCatalog(
                            new[]
                            {
                                firstDefinition
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "invalid required class id"));
        }

        [Test]
        public void Constructor_NullDefinitionThrows()
        {
            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new SpecializationDefinitionCatalog(
                            new SpecializationDefinition[]
                            {
                                null
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "null specialization definition"));
        }

        [Test]
        public void EmptyCatalog_CannotResolveDefinition()
        {
            SpecializationDefinitionCatalog catalog =
                new SpecializationDefinitionCatalog(
                    null);

            bool resolved =
                catalog.TryResolve(
                    new CharacterSpecializationId(
                        "berserker"),
                    out SpecializationDefinition result);

            Assert.That(
                resolved,
                Is.False);

            Assert.That(
                result,
                Is.Null);
        }

        private SpecializationDefinition
            CreateDefinition(
                string specializationId,
                string requiredClassId)
        {
            SpecializationDefinition definition =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();

            definition.InitializeForTests(
                specializationId,
                requiredClassId);

            return definition;
        }
    }
}