using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class ItemDefinitionContentValidatorTests
{
    private ItemDefinition definition;
    private ItemDefinitionContentValidator validator;

    [SetUp]
    public void SetUp()
    {
        validator =
            new ItemDefinitionContentValidator();
    }

    [TearDown]
    public void TearDown()
    {
        if (definition != null)
        {
            UnityEngine.Object.DestroyImmediate(definition);
        }
    }

    [Test]
    public void Validate_OneHandedWeaponSucceeds()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.MainHand,
                EquipmentItemCategory.Weapon,
                WeaponHandedness.OneHanded);

        Assert.That(
            () => validator.Validate(definition),
            Throws.Nothing);
    }

    [Test]
    public void Validate_TwoHandedWeaponSucceeds()
    {
        definition =
            CreateDefinition(
                "battle_axe",
                EquipmentSlotType.MainHand,
                EquipmentItemCategory.Weapon,
                WeaponHandedness.TwoHanded);

        Assert.That(
            () => validator.Validate(definition),
            Throws.Nothing);
    }

    [Test]
    public void Validate_WeaponInWrongSlotThrows()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.OffHand,
                EquipmentItemCategory.Weapon,
                WeaponHandedness.OneHanded);

        Assert.That(
            () => validator.Validate(definition),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Validate_ShieldInOffHandSucceeds()
    {
        definition =
            CreateDefinition(
                "wooden_shield",
                EquipmentSlotType.OffHand,
                EquipmentItemCategory.Shield,
                WeaponHandedness.NotApplicable);

        Assert.That(
            () => validator.Validate(definition),
            Throws.Nothing);
    }

    [Test]
    public void Validate_ArmorWithHandednessThrows()
    {
        definition =
            CreateDefinition(
                "leather_armor",
                EquipmentSlotType.Armor,
                EquipmentItemCategory.Armor,
                WeaponHandedness.TwoHanded);

        Assert.That(
            () => validator.Validate(definition),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Validate_StackedEquipmentThrows()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.MainHand,
                EquipmentItemCategory.Weapon,
                WeaponHandedness.OneHanded,
                2);

        Assert.That(
            () => validator.Validate(definition),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Validate_StackableConsumableSucceeds()
    {
        definition =
            CreateDefinition(
                "health_potion",
                EquipmentSlotType.None,
                EquipmentItemCategory.Consumable,
                WeaponHandedness.NotApplicable,
                10);

        Assert.That(
            () => validator.Validate(definition),
            Throws.Nothing);

        Assert.That(
            definition.IsEquippable,
            Is.False);
    }

    [Test]
    public void Validate_ConsumableWithSlotThrows()
    {
        definition =
            CreateDefinition(
                "health_potion",
                EquipmentSlotType.Accessory,
                EquipmentItemCategory.Consumable,
                WeaponHandedness.NotApplicable,
                10);

        Assert.That(
            () => validator.Validate(definition),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Validate_NullDefinitionThrows()
    {
        Assert.That(
            () => validator.Validate(null),
            Throws.TypeOf<ArgumentNullException>());
    }

    private static ItemDefinition CreateDefinition(
        string itemId,
        EquipmentSlotType slotType,
        EquipmentItemCategory category,
        WeaponHandedness handedness,
        int maxStackSize = 1)
    {
        ItemDefinition result =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        result.InitializeForTests(
            itemId,
            slotType,
            maxStackSize,
            category,
            handedness);

        return result;
    }
}