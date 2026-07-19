using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        ItemBaseTypeCompatibilityValidatorTests
    {
        [Test]
        public void Validate_NullItem_Throws()
        {
            ItemBaseTypeCompatibilityValidator validator =
                new ItemBaseTypeCompatibilityValidator();

            Assert.That(
                () => validator.Validate(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Validate_ItemWithoutBaseType_DoesNotThrow()
        {
            ItemDefinition item =
                CreateItem(
                    EquipmentSlotType.MainHand);

            try
            {
                ItemBaseTypeCompatibilityValidator validator =
                    new ItemBaseTypeCompatibilityValidator();

                Assert.That(
                    () => validator.Validate(item),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(item);
            }
        }

        [Test]
        public void Validate_MatchingSlot_DoesNotThrow()
        {
            EquipmentBaseTypeDefinition baseType =
                CreateBaseType(
                    EquipmentSlotType.MainHand);

            ItemDefinition item =
                CreateItem(
                    EquipmentSlotType.MainHand,
                    baseType);

            try
            {
                ItemBaseTypeCompatibilityValidator validator =
                    new ItemBaseTypeCompatibilityValidator();

                Assert.That(
                    () => validator.Validate(item),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(item);
                UnityEngine.Object.DestroyImmediate(baseType);
            }
        }

        [Test]
        public void Validate_MismatchingSlot_Throws()
        {
            EquipmentBaseTypeDefinition baseType =
                CreateBaseType(
                    EquipmentSlotType.MainHand);

            ItemDefinition item =
                CreateItem(
                    EquipmentSlotType.Head,
                    baseType);

            try
            {
                ItemBaseTypeCompatibilityValidator validator =
                    new ItemBaseTypeCompatibilityValidator();

                Assert.That(
                    () => validator.Validate(item),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(item);
                UnityEngine.Object.DestroyImmediate(baseType);
            }
        }

        private static EquipmentBaseTypeDefinition
            CreateBaseType(
                EquipmentSlotType slotType)
        {
            EquipmentBaseTypeDefinition definition =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            definition.InitializeForTests(
                "iron_sword",
                slotType,
                EquipmentBaseTypeCategory.Sword);

            return definition;
        }

        private static ItemDefinition CreateItem(
            EquipmentSlotType slotType,
            EquipmentBaseTypeDefinition baseType = null)
        {
            ItemDefinition item =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();

            item.InitializeForTests(
                "iron_sword_item",
                slotType,
                newBaseType: baseType);

            return item;
        }
    }
}