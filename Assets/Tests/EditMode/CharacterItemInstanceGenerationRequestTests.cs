using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        CharacterItemInstanceGenerationRequestTests
    {
        private ItemDefinition itemDefinition;
        private EquipmentBaseTypeDefinition baseType;
        private EquipmentAffixPoolDefinition affixPool;

        [SetUp]
        public void SetUp()
        {
            baseType =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            baseType.InitializeForTests(
                "sword",
                EquipmentSlotType.MainHand,
                EquipmentBaseTypeCategory.Sword);

            itemDefinition =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();

            itemDefinition.InitializeForTests(
                "iron_sword",
                EquipmentSlotType.MainHand,
                newBaseType: baseType);

            affixPool =
                new EquipmentAffixPoolDefinition();
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(
                itemDefinition);

            UnityEngine.Object.DestroyImmediate(
                baseType);
        }

        [Test]
        public void Constructor_ValidValues_StoresValues()
        {
            CharacterItemInstanceGenerationRequest request =
                new CharacterItemInstanceGenerationRequest(
                    itemDefinition,
                    affixPool,
                    10,
                    2,
                    1);

            Assert.That(
                request.ItemDefinition,
                Is.SameAs(itemDefinition));

            Assert.That(
                request.AffixPool,
                Is.SameAs(affixPool));

            Assert.That(
                request.Level,
                Is.EqualTo(10));

            Assert.That(
                request.UpgradeLevel,
                Is.EqualTo(2));

            Assert.That(
                request.Quantity,
                Is.EqualTo(1));
        }

        [Test]
        public void Constructor_NullItemDefinition_Throws()
        {
            Assert.That(
                () =>
                    new CharacterItemInstanceGenerationRequest(
                        null,
                        affixPool,
                        1,
                        0,
                        1),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_ItemWithoutBaseType_Throws()
        {
            ItemDefinition itemWithoutBaseType =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();

            try
            {
                itemWithoutBaseType.InitializeForTests(
                    "invalid_item",
                    EquipmentSlotType.MainHand);

                Assert.That(
                    () =>
                        new CharacterItemInstanceGenerationRequest(
                            itemWithoutBaseType,
                            affixPool,
                            1,
                            0,
                            1),
                    Throws.TypeOf<ArgumentException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(
                    itemWithoutBaseType);
            }
        }

        [Test]
        public void Constructor_NullAffixPool_Throws()
        {
            Assert.That(
                () =>
                    new CharacterItemInstanceGenerationRequest(
                        itemDefinition,
                        null,
                        1,
                        0,
                        1),
                Throws.TypeOf<ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_InvalidLevel_Throws(
            int level)
        {
            Assert.That(
                () =>
                    new CharacterItemInstanceGenerationRequest(
                        itemDefinition,
                        affixPool,
                        level,
                        0,
                        1),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_NegativeUpgradeLevel_Throws()
        {
            Assert.That(
                () =>
                    new CharacterItemInstanceGenerationRequest(
                        itemDefinition,
                        affixPool,
                        1,
                        -1,
                        1),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Constructor_InvalidQuantity_Throws(
            int quantity)
        {
            Assert.That(
                () =>
                    new CharacterItemInstanceGenerationRequest(
                        itemDefinition,
                        affixPool,
                        1,
                        0,
                        quantity),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }
    }
}