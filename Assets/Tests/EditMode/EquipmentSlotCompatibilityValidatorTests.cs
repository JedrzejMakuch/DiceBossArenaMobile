using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class EquipmentSlotCompatibilityValidatorTests
{
    private ItemDefinition definition;

    private EquipmentSlotCompatibilityValidator
        validator;

    [SetUp]
    public void SetUp()
    {
        validator =
            new EquipmentSlotCompatibilityValidator(
                new ItemDefinitionContentValidator());
    }

    [TearDown]
    public void TearDown()
    {
        if (definition != null)
        {
            Object.DestroyImmediate(definition);
        }
    }

    [Test]
    public void CanEquip_WeaponInMainHandReturnsTrue()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.MainHand,
                EquipmentItemCategory.Weapon,
                WeaponHandedness.OneHanded);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.MainHand),
            Is.True);
    }

    [Test]
    public void CanEquip_WeaponInOffHandReturnsFalse()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.MainHand,
                EquipmentItemCategory.Weapon,
                WeaponHandedness.OneHanded);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.OffHand),
            Is.False);
    }

    [Test]
    public void CanEquip_ShieldInOffHandReturnsTrue()
    {
        definition =
            CreateDefinition(
                "wooden_shield",
                EquipmentSlotType.OffHand,
                EquipmentItemCategory.Shield,
                WeaponHandedness.NotApplicable);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.OffHand),
            Is.True);
    }

    [Test]
    public void CanEquip_ArmorInArmorSlotReturnsTrue()
    {
        definition =
            CreateDefinition(
                "leather_armor",
                EquipmentSlotType.Armor,
                EquipmentItemCategory.Armor,
                WeaponHandedness.NotApplicable);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.Armor),
            Is.True);
    }

    [Test]
    public void CanEquip_AccessoryInArmorSlotReturnsFalse()
    {
        definition =
            CreateDefinition(
                "wolf_charm",
                EquipmentSlotType.Accessory,
                EquipmentItemCategory.Accessory,
                WeaponHandedness.NotApplicable);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.Armor),
            Is.False);
    }

    [Test]
    public void CanEquip_ConsumableReturnsFalse()
    {
        definition =
            CreateDefinition(
                "health_potion",
                EquipmentSlotType.None,
                EquipmentItemCategory.Consumable,
                WeaponHandedness.NotApplicable,
                10);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.Accessory),
            Is.False);
    }

    [Test]
    public void CanEquip_NoneSlotReturnsFalse()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.MainHand,
                EquipmentItemCategory.Weapon,
                WeaponHandedness.OneHanded);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.None),
            Is.False);
    }

    [Test]
    public void CanEquip_NullDefinitionReturnsFalse()
    {
        Assert.That(
            validator.CanEquip(
                null,
                EquipmentSlotType.MainHand),
            Is.False);
    }

    [Test]
    public void CanEquip_InvalidDefinitionReturnsFalse()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.OffHand,
                EquipmentItemCategory.Weapon,
                WeaponHandedness.OneHanded);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.OffHand),
            Is.False);
    }

    [Test]
    public void CanEquip_AccessoryInSecondAccessorySlotReturnsTrue()
    {
        definition =
            CreateDefinition(
                "wolf_charm",
                EquipmentSlotType.Accessory,
                EquipmentItemCategory.Accessory,
                WeaponHandedness.NotApplicable);

        Assert.That(
            validator.CanEquip(
                definition,
                EquipmentSlotType.AccessoryTwo),
            Is.True);
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