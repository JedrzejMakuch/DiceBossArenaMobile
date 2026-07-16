using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class CharacterInventoryAddTests
{
    private readonly List<ItemDefinition>
        definitions = new();

    [TearDown]
    public void TearDown()
    {
        for (int i = 0;
             i < definitions.Count;
             i++)
        {
            if (definitions[i] != null)
            {
                Object.DestroyImmediate(
                    definitions[i]);
            }
        }

        definitions.Clear();
    }

    [Test]
    public void TryAdd_NewItemAddsSlot()
    {
        AddDefinition(
            "iron_sword",
            1,
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        CharacterInventory inventory =
            CreateInventory(5);

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_001",
                    "iron_sword"));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult.Added));

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));
    }

    [Test]
    public void TryAdd_CompatibleItemsAreStacked()
    {
        AddDefinition(
            "health_potion",
            10,
            EquipmentSlotType.None,
            EquipmentItemCategory.Consumable,
            WeaponHandedness.NotApplicable);

        CharacterInventory inventory =
            CreateInventory(
                5,
                CreateItem(
                    "instance_001",
                    "health_potion",
                    3));

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_002",
                    "health_potion",
                    2));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult.Stacked));

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(5));

        Assert.That(
            inventory.Items[0]
                .InstanceId.Value,
            Is.EqualTo("instance_001"));
    }

    [Test]
    public void TryAdd_FullInventoryReturnsInventoryFull()
    {
        AddDefinition(
            "iron_sword",
            1,
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        AddDefinition(
            "wooden_shield",
            1,
            EquipmentSlotType.OffHand,
            EquipmentItemCategory.Shield,
            WeaponHandedness.NotApplicable);

        CharacterInventory inventory =
            CreateInventory(
                1,
                CreateItem(
                    "instance_001",
                    "iron_sword"));

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_002",
                    "wooden_shield"));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult.InventoryFull));

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));
    }

    [Test]
    public void TryAdd_DuplicateInstanceIdReturnsFailure()
    {
        AddDefinition(
            "health_potion",
            10,
            EquipmentSlotType.None,
            EquipmentItemCategory.Consumable,
            WeaponHandedness.NotApplicable);

        CharacterInventory inventory =
            CreateInventory(
                5,
                CreateItem(
                    "instance_001",
                    "health_potion"));

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_001",
                    "health_potion"));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult
                    .DuplicateInstanceId));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(1));
    }

    [Test]
    public void TryAdd_UnknownDefinitionReturnsFailure()
    {
        CharacterInventory inventory =
            CreateInventory(5);

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_001",
                    "missing_item"));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult
                    .UnknownDefinition));

        Assert.That(
            inventory.Items,
            Is.Empty);
    }

    [Test]
    public void TryAdd_QuantityAboveStackLimitIsAtomic()
    {
        AddDefinition(
            "health_potion",
            5,
            EquipmentSlotType.None,
            EquipmentItemCategory.Consumable,
            WeaponHandedness.NotApplicable);

        CharacterInventory inventory =
            CreateInventory(5);

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_001",
                    "health_potion",
                    6));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult.InvalidItem));

        Assert.That(
            inventory.Items,
            Is.Empty);
    }

    [Test]
    public void TryAdd_FullInventoryCanExtendExistingStack()
    {
        AddDefinition(
            "health_potion",
            10,
            EquipmentSlotType.None,
            EquipmentItemCategory.Consumable,
            WeaponHandedness.NotApplicable);

        CharacterInventory inventory =
            CreateInventory(
                1,
                CreateItem(
                    "instance_001",
                    "health_potion",
                    3));

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_002",
                    "health_potion",
                    2));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult.Stacked));

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(5));

        Assert.That(
            inventory.IsFull,
            Is.True);
    }

    [Test]
    public void TryAdd_StackOverflowUsesNewSlot()
    {
        AddDefinition(
            "health_potion",
            5,
            EquipmentSlotType.None,
            EquipmentItemCategory.Consumable,
            WeaponHandedness.NotApplicable);

        CharacterInventory inventory =
            CreateInventory(
                2,
                CreateItem(
                    "instance_001",
                    "health_potion",
                    4));

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_002",
                    "health_potion",
                    2));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult.Added));

        Assert.That(
            inventory.Count,
            Is.EqualTo(2));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(4));

        Assert.That(
            inventory.Items[1].Quantity,
            Is.EqualTo(2));
    }

    [Test]
    public void TryAdd_FullInventoryAndStackOverflowIsAtomic()
    {
        AddDefinition(
            "health_potion",
            5,
            EquipmentSlotType.None,
            EquipmentItemCategory.Consumable,
            WeaponHandedness.NotApplicable);

        CharacterInventory inventory =
            CreateInventory(
                1,
                CreateItem(
                    "instance_001",
                    "health_potion",
                    4));

        InventoryAddResult result =
            inventory.TryAdd(
                CreateItem(
                    "instance_002",
                    "health_potion",
                    2));

        Assert.That(
            result,
            Is.EqualTo(
                InventoryAddResult.InventoryFull));

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(4));

        Assert.That(
            inventory.Items[0]
                .InstanceId.Value,
            Is.EqualTo("instance_001"));
    }

    private CharacterInventory CreateInventory(
        int capacity,
        params CharacterItemInstance[] items)
    {
        return new CharacterInventory(
            capacity,
            new ItemDefinitionCatalog(
                definitions),
            items);
    }

    private void AddDefinition(
        string itemId,
        int maxStackSize,
        EquipmentSlotType slotType,
        EquipmentItemCategory category,
        WeaponHandedness handedness)
    {
        ItemDefinition definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            itemId,
            slotType,
            maxStackSize,
            category,
            handedness);

        definitions.Add(definition);
    }

    private static CharacterItemInstance CreateItem(
        string instanceId,
        string itemId,
        int quantity = 1)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                instanceId),
            new CharacterItemId(itemId),
            1,
            0,
            quantity);
    }
}