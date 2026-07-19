using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixDefinitionValidatorTests
    {
        [Test]
        public void Validate_NullDefinition_Throws()
        {
            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Validate_ValidDefinition_DoesNotThrow()
        {
            EquipmentAffixDefinition definition =
                CreateDefinition(
                    FightStatType.Strength);

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.Nothing);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Validate_InvalidId_Throws(
            string id)
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    id,
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            1,
                            1,
                            3)
                    });

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Validate_EmptyTiers_Throws()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat);

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_NullTier_Throws()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new EquipmentAffixTierDefinition[]
                    {
                null
                    });

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_InvalidTier_Throws()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                new EquipmentAffixTierDefinition(
                    0,
                    1,
                    3)
                    });

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_UnsupportedStatType_Throws()
        {
            EquipmentAffixDefinition definition =
                CreateDefinition(
                    (FightStatType)999);

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [TestCase(FightStatType.MaxActionPoints)]
        [TestCase(FightStatType.MaxMovementPoints)]
        public void Validate_ActionOrMovementPoints_Throws(
            FightStatType statType)
        {
            EquipmentAffixDefinition definition =
                CreateDefinition(statType);

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_UnsupportedModifierType_Throws()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_invalid_modifier",
                    FightStatType.Strength,
                    (FightStatModifierType)999);

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_IncreasingTierLevels_DoesNotThrow()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                new EquipmentAffixTierDefinition(
                    1,
                    1,
                    3),

                new EquipmentAffixTierDefinition(
                    10,
                    4,
                    7),

                new EquipmentAffixTierDefinition(
                    20,
                    8,
                    12)
                    });

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.Nothing);
        }

        [Test]
        public void Validate_DecreasingTierLevels_Throws()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                new EquipmentAffixTierDefinition(
                    10,
                    4,
                    7),

                new EquipmentAffixTierDefinition(
                    1,
                    1,
                    3)
                    });

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_DuplicateTierLevels_Throws()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                new EquipmentAffixTierDefinition(
                    1,
                    1,
                    3),

                new EquipmentAffixTierDefinition(
                    1,
                    4,
                    7)
                    });

            EquipmentAffixDefinitionValidator validator =
                new EquipmentAffixDefinitionValidator();

            Assert.That(
                () => validator.Validate(definition),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        private static EquipmentAffixDefinition
    CreateDefinition(
        FightStatType statType)
        {
            return new EquipmentAffixDefinition(
                "test_affix",
                statType,
                FightStatModifierType.Flat,
                new[]
                {
            new EquipmentAffixTierDefinition(
                1,
                1,
                3)
                });
        }
    }
}