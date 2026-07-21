using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentBaseTypeWeaponProfileValidatorTests
    {
        [Test]
        public void Constructor_NullProfileValidator_Throws()
        {
            Assert.That(
                () => new
                    EquipmentBaseTypeWeaponProfileValidator(
                        null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Validate_NullDefinition_Throws()
        {
            Assert.That(
                () => CreateValidator().Validate(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Validate_MainHandWithoutProfile_Throws()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword,
                    null);

            AssertInvalidAndDestroy(definition);
        }

        [Test]
        public void Validate_NonMainHandWithProfile_Throws()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    EquipmentSlotType.OffHand,
                    EquipmentBaseTypeCategory.Shield,
                    CreateValidProfile());

            AssertInvalidAndDestroy(definition);
        }

        [Test]
        public void Validate_InvalidWeaponProfile_Throws()
        {
            WeaponProfileGenerationDefinition profile =
                new WeaponProfileGenerationDefinition(
                    Array.Empty<
                        WeaponAttackLineGenerationDefinition>());

            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword,
                    profile);

            AssertInvalidAndDestroy(definition);
        }

        [Test]
        public void Validate_MainHandWithValidProfile_DoesNotThrow()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword,
                    CreateValidProfile());

            try
            {
                Assert.That(
                    () => CreateValidator().Validate(
                        definition),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Validate_NonMainHandWithoutProfile_DoesNotThrow()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    EquipmentSlotType.OffHand,
                    EquipmentBaseTypeCategory.Shield,
                    null);

            try
            {
                Assert.That(
                    () => CreateValidator().Validate(
                        definition),
                    Throws.Nothing);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        private static void AssertInvalidAndDestroy(
            EquipmentBaseTypeDefinition definition)
        {
            try
            {
                Assert.That(
                    () => CreateValidator().Validate(
                        definition),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        private static EquipmentBaseTypeDefinition
            CreateDefinition(
                EquipmentSlotType slotType,
                EquipmentBaseTypeCategory category,
                WeaponProfileGenerationDefinition profile)
        {
            EquipmentBaseTypeDefinition definition =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            definition.InitializeForTests(
                "test_base_type",
                slotType,
                category,
                newStatModifiers: null,
                newWeaponProfileGeneration: profile);

            return definition;
        }

        private static WeaponProfileGenerationDefinition
            CreateValidProfile()
        {
            return new WeaponProfileGenerationDefinition(
                new[]
                {
                    new WeaponAttackLineGenerationDefinition(
                        "primary_damage",
                        4,
                        8,
                        new[]
                        {
                            WeaponAttackElement.Neutral,
                            WeaponAttackElement.Fire
                        })
                });
        }

        private static
            EquipmentBaseTypeWeaponProfileValidator
            CreateValidator()
        {
            return new
                EquipmentBaseTypeWeaponProfileValidator(
                    new
                        WeaponProfileGenerationDefinitionValidator(
                            new
                                WeaponAttackLineGenerationDefinitionValidator()));
        }
    }
}