using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EquipmentBaseTypeDefinitionValidatorTests
    {
        [Test]
        public void Validate_ValidDefinition_DoesNotThrow()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Validate_InvalidId_Throws()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "   ",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Validate_MainHandSword_DoesNotThrow()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Validate_HeadSword_Throws()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "head_sword",
                    EquipmentSlotType.Head,
                    EquipmentBaseTypeCategory.Sword);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Validate_OffHandShield_DoesNotThrow()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "wooden_shield",
                    EquipmentSlotType.OffHand,
                    EquipmentBaseTypeCategory.Shield);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Validate_MainHandShield_Throws()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "main_hand_shield",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Shield);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [TestCase(
            EquipmentSlotType.Accessory,
            EquipmentBaseTypeCategory.Ring)]
        [TestCase(
            EquipmentSlotType.Accessory,
            EquipmentBaseTypeCategory.Amulet)]
        [TestCase(
            EquipmentSlotType.AccessoryTwo,
            EquipmentBaseTypeCategory.Ring)]
        [TestCase(
            EquipmentSlotType.AccessoryTwo,
            EquipmentBaseTypeCategory.Amulet)]
        public void Validate_ValidAccessoryCategory_DoesNotThrow(
            EquipmentSlotType slotType,
            EquipmentBaseTypeCategory category)
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "accessory",
                    slotType,
                    category);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Validate_NoneSlot_Throws()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "iron_sword",
                    EquipmentSlotType.None,
                    EquipmentBaseTypeCategory.Sword);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [TestCase(
    EquipmentSlotType.MainHand,
    EquipmentBaseTypeCategory.Sword)]
        [TestCase(
    EquipmentSlotType.MainHand,
    EquipmentBaseTypeCategory.Axe)]
        [TestCase(
    EquipmentSlotType.MainHand,
    EquipmentBaseTypeCategory.Hammer)]
        [TestCase(
    EquipmentSlotType.MainHand,
    EquipmentBaseTypeCategory.Dagger)]
        [TestCase(
    EquipmentSlotType.MainHand,
    EquipmentBaseTypeCategory.Bow)]
        [TestCase(
    EquipmentSlotType.MainHand,
    EquipmentBaseTypeCategory.Staff)]
        [TestCase(
    EquipmentSlotType.MainHand,
    EquipmentBaseTypeCategory.Wand)]
        [TestCase(
    EquipmentSlotType.OffHand,
    EquipmentBaseTypeCategory.Shield)]
        [TestCase(
    EquipmentSlotType.Head,
    EquipmentBaseTypeCategory.Helmet)]
        [TestCase(
    EquipmentSlotType.Armor,
    EquipmentBaseTypeCategory.ChestArmor)]
        [TestCase(
    EquipmentSlotType.Hands,
    EquipmentBaseTypeCategory.Gloves)]
        [TestCase(
    EquipmentSlotType.Feet,
    EquipmentBaseTypeCategory.Boots)]
        [TestCase(
    EquipmentSlotType.Accessory,
    EquipmentBaseTypeCategory.Ring)]
        [TestCase(
    EquipmentSlotType.Accessory,
    EquipmentBaseTypeCategory.Amulet)]
        [TestCase(
    EquipmentSlotType.AccessoryTwo,
    EquipmentBaseTypeCategory.Ring)]
        [TestCase(
    EquipmentSlotType.AccessoryTwo,
    EquipmentBaseTypeCategory.Amulet)]
        public void Validate_AllAllowedSlotCategoryPairs_DoNotThrow(
    EquipmentSlotType slotType,
    EquipmentBaseTypeCategory category)
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "valid_base_type",
                    slotType,
                    category);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [TestCase(
    EquipmentSlotType.OffHand,
    EquipmentBaseTypeCategory.Sword)]
        [TestCase(
    EquipmentSlotType.Head,
    EquipmentBaseTypeCategory.Boots)]
        [TestCase(
    EquipmentSlotType.Armor,
    EquipmentBaseTypeCategory.Helmet)]
        [TestCase(
    EquipmentSlotType.Hands,
    EquipmentBaseTypeCategory.Shield)]
        [TestCase(
    EquipmentSlotType.Feet,
    EquipmentBaseTypeCategory.Gloves)]
        [TestCase(
    EquipmentSlotType.Accessory,
    EquipmentBaseTypeCategory.Bow)]
        [TestCase(
    EquipmentSlotType.AccessoryTwo,
    EquipmentBaseTypeCategory.ChestArmor)]
        public void Validate_InvalidSlotCategoryPairs_Throw(
    EquipmentSlotType slotType,
    EquipmentBaseTypeCategory category)
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "invalid_base_type",
                    slotType,
                    category);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Validate_NoneCategory_Throws()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.None);

            try
            {
                EquipmentBaseTypeDefinitionValidator validator =
                    new EquipmentBaseTypeDefinitionValidator();

                Assert.That(
                    () => validator.Validate(definition),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        private static EquipmentBaseTypeDefinition
            CreateDefinition(
                string id,
                EquipmentSlotType slotType,
                EquipmentBaseTypeCategory category)
        {
            EquipmentBaseTypeDefinition definition =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            definition.InitializeForTests(
                id,
                slotType,
                category);

            return definition;
        }
    }
}