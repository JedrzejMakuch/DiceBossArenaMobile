using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        CharacterBuildCompositionRequestTests
    {
        private ClassDefinition classDefinition;

        private SpecializationDefinition
            specializationDefinition;

        [SetUp]
        public void SetUp()
        {
            classDefinition =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();

            classDefinition.InitializeForTests(
                "companion");

            specializationDefinition =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();

            specializationDefinition
                .InitializeForTests(
                    "berserker",
                    "companion");
        }

        [TearDown]
        public void TearDown()
        {
            if (specializationDefinition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    specializationDefinition);
            }

            if (classDefinition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    classDefinition);
            }
        }

        [Test]
        public void Constructor_OnlyInventoryThrows()
        {
            ItemDefinitionCatalog catalog =
                new ItemDefinitionCatalog(
                    Array.Empty<ItemDefinition>());

            CharacterInventory inventory =
                new CharacterInventory(
                    capacity: 10,
                    definitionResolver: catalog);

            Assert.Throws<ArgumentException>(
                () =>
                    new CharacterBuildCompositionRequest(
                        classDefinition,
                        inventory: inventory));
        }

        [Test]
        public void Constructor_OnlyRuntimeLoadoutThrows()
        {
            ItemDefinitionCatalog catalog =
    new ItemDefinitionCatalog(
        Array.Empty<ItemDefinition>());

            CharacterEquipmentLoadout loadout =
                new CharacterEquipmentLoadout();

            Assert.Throws<ArgumentException>(
                () =>
                    new CharacterBuildCompositionRequest(
                        classDefinition,
                        runtimeEquipmentLoadout: loadout));
        }

        [Test]
        public void Constructor_RuntimeEquipmentSetsFlag()
        {
            ItemDefinitionCatalog catalog =
                new ItemDefinitionCatalog(
                    Array.Empty<ItemDefinition>());

            CharacterInventory inventory =
                new CharacterInventory(
                    capacity: 10,
                    definitionResolver: catalog);

            CharacterEquipmentLoadout loadout =
                new CharacterEquipmentLoadout();

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    inventory: inventory,
                    runtimeEquipmentLoadout: loadout);

            Assert.That(
                request.HasRuntimeEquipment,
                Is.True);

            Assert.That(
                request.Inventory,
                Is.SameAs(inventory));

            Assert.That(
                request.RuntimeEquipmentLoadout,
                Is.SameAs(loadout));
        }

        [Test]
        public void Constructor_MissingClassThrows()
        {
            Assert.That(
                () =>
                    new CharacterBuildCompositionRequest(
                        null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Constructor_AllowsMissingSpecialization()
        {
            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            Assert.That(
                request.ClassDefinition,
                Is.SameAs(
                    classDefinition));

            Assert.That(
                request.SpecializationDefinition,
                Is.Null);
        }

        [Test]
        public void Constructor_AssignsSpecialization()
        {
            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                request.SpecializationDefinition,
                Is.SameAs(
                    specializationDefinition));
        }

        [Test]
        public void Constructor_CopiesSelectedSkills()
        {
            List<CharacterBuildSkill> source =
                new()
                {
                    new CharacterBuildSkill(
                        "basic_attack",
                        1)
                };

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition,
                    source);

            source.Add(
                new CharacterBuildSkill(
                    "rage",
                    1));

            Assert.That(
                request.SelectedSkills.Count,
                Is.EqualTo(1));

            Assert.That(
                request.SelectedSkills[0].SkillId,
                Is.EqualTo(
                    "basic_attack"));
        }

        [Test]
        public void Constructor_NullSkillsCreatesEmptyCollection()
        {
            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            Assert.That(
                request.SelectedSkills,
                Is.Empty);
        }
    }
}