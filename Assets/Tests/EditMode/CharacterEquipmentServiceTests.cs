using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class CharacterEquipmentServiceTests
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
    public void TryEquip_ValidItemEquipsAndStaysInInventory()
    {
        AddDefinition(
            "iron_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        CharacterItemInstance item =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterInventory inventory =
            CreateInventory(item);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        EquipmentOperationResult result =
            CreateService().TryEquip(
                inventory,
                loadout,
                item.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult.Equipped));

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));

        Assert.That(
            loadout.Items[0].InstanceId,
            Is.EqualTo(item.InstanceId));
    }

    [Test]
    public void TryEquip_MissingItemDoesNotChangeLoadout()
    {
        CharacterInventory inventory =
            CreateInventory();

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        EquipmentOperationResult result =
            CreateService().TryEquip(
                inventory,
                loadout,
                new CharacterItemInstanceId(
                    "missing_instance"),
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult
                    .ItemNotFound));

        Assert.That(
            loadout.Items,
            Is.Empty);
    }

    [Test]
    public void TryEquip_WrongSlotIsAtomic()
    {
        AddDefinition(
            "iron_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        CharacterItemInstance item =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterInventory inventory =
            CreateInventory(item);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        EquipmentOperationResult result =
            CreateService().TryEquip(
                inventory,
                loadout,
                item.InstanceId,
                EquipmentSlotType.OffHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult.InvalidSlot));

        Assert.That(
            loadout.Items,
            Is.Empty);

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));
    }

    [Test]
    public void TryEquip_OccupiedSlotIsAtomic()
    {
        AddDefinition(
            "iron_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        AddDefinition(
            "steel_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        CharacterItemInstance first =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterItemInstance second =
            CreateItem(
                "instance_002",
                "steel_sword");

        CharacterInventory inventory =
            CreateInventory(
                first,
                second);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    new EquippedItemInstance(
                        EquipmentSlotType.MainHand,
                        first.InstanceId)
                });

        EquipmentOperationResult result =
            CreateService().TryEquip(
                inventory,
                loadout,
                second.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult
                    .SlotOccupied));

        Assert.That(
            loadout.Items[0].InstanceId,
            Is.EqualTo(first.InstanceId));
    }

    [Test]
    public void TryEquip_TwoHandedWeaponWithOffHandIsAtomic()
    {
        AddDefinition(
            "battle_axe",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.TwoHanded);

        AddDefinition(
            "wooden_shield",
            EquipmentSlotType.OffHand,
            EquipmentItemCategory.Shield,
            WeaponHandedness.NotApplicable);

        CharacterItemInstance axe =
            CreateItem(
                "instance_001",
                "battle_axe");

        CharacterItemInstance shield =
            CreateItem(
                "instance_002",
                "wooden_shield");

        CharacterInventory inventory =
            CreateInventory(
                axe,
                shield);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    new EquippedItemInstance(
                        EquipmentSlotType.OffHand,
                        shield.InstanceId)
                });

        EquipmentOperationResult result =
            CreateService().TryEquip(
                inventory,
                loadout,
                axe.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult
                    .LoadoutConflict));

        Assert.That(
            loadout.Count,
            Is.EqualTo(1));
    }

    [Test]
    public void TryUnequip_OccupiedSlotRemovesReference()
    {
        CharacterItemInstance item =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterInventory inventory =
            CreateInventory(item);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                new EquippedItemInstance(
                    EquipmentSlotType.MainHand,
                    item.InstanceId)
                });

        EquipmentOperationResult result =
            CreateService().TryUnequip(
                loadout,
                EquipmentSlotType.MainHand);

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult.Unequipped));

        Assert.That(
            loadout.Items,
            Is.Empty);

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));

        Assert.That(
            inventory.Items[0].InstanceId,
            Is.EqualTo(item.InstanceId));
    }

    [Test]
    public void TryUnequip_EmptySlotReturnsFailure()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        EquipmentOperationResult result =
            CreateService().TryUnequip(
                loadout,
                EquipmentSlotType.OffHand);

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult.SlotEmpty));

        Assert.That(
            loadout.Items,
            Is.Empty);
    }

    [Test]
    public void TryUnequip_NoneSlotReturnsInvalidSlot()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        EquipmentOperationResult result =
            CreateService().TryUnequip(
                loadout,
                EquipmentSlotType.None);

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult.InvalidSlot));

        Assert.That(
            loadout.Items,
            Is.Empty);
    }

    [Test]
    public void TrySwap_ValidItemReplacesSlot()
    {
        AddDefinition(
            "iron_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        AddDefinition(
            "steel_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        CharacterItemInstance first =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterItemInstance second =
            CreateItem(
                "instance_002",
                "steel_sword");

        CharacterInventory inventory =
            CreateInventory(
                first,
                second);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                new EquippedItemInstance(
                    EquipmentSlotType.MainHand,
                    first.InstanceId)
                });

        EquipmentOperationResult result =
            CreateService().TrySwap(
                inventory,
                loadout,
                second.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult.Swapped));

        Assert.That(
            loadout.Items[0].InstanceId,
            Is.EqualTo(second.InstanceId));

        Assert.That(
            inventory.Count,
            Is.EqualTo(2));
    }

    [Test]
    public void TrySwap_WrongSlotDoesNotReplaceItem()
    {
        AddDefinition(
            "iron_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        AddDefinition(
            "wooden_shield",
            EquipmentSlotType.OffHand,
            EquipmentItemCategory.Shield,
            WeaponHandedness.NotApplicable);

        CharacterItemInstance sword =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterItemInstance shield =
            CreateItem(
                "instance_002",
                "wooden_shield");

        CharacterInventory inventory =
            CreateInventory(
                sword,
                shield);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                new EquippedItemInstance(
                    EquipmentSlotType.MainHand,
                    sword.InstanceId)
                });

        EquipmentOperationResult result =
            CreateService().TrySwap(
                inventory,
                loadout,
                shield.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult.InvalidSlot));

        Assert.That(
            loadout.Items[0].InstanceId,
            Is.EqualTo(sword.InstanceId));
    }

    [Test]
    public void TrySwap_TwoHandedConflictIsAtomic()
    {
        AddDefinition(
            "iron_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        AddDefinition(
            "battle_axe",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.TwoHanded);

        AddDefinition(
            "wooden_shield",
            EquipmentSlotType.OffHand,
            EquipmentItemCategory.Shield,
            WeaponHandedness.NotApplicable);

        CharacterItemInstance sword =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterItemInstance axe =
            CreateItem(
                "instance_002",
                "battle_axe");

        CharacterItemInstance shield =
            CreateItem(
                "instance_003",
                "wooden_shield");

        CharacterInventory inventory =
            CreateInventory(
                sword,
                axe,
                shield);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                new EquippedItemInstance(
                    EquipmentSlotType.MainHand,
                    sword.InstanceId),
                new EquippedItemInstance(
                    EquipmentSlotType.OffHand,
                    shield.InstanceId)
                });

        EquipmentOperationResult result =
            CreateService().TrySwap(
                inventory,
                loadout,
                axe.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult
                    .LoadoutConflict));

        Assert.That(
            loadout.Items[0].InstanceId,
            Is.EqualTo(sword.InstanceId));

        Assert.That(
            loadout.Items[1].InstanceId,
            Is.EqualTo(shield.InstanceId));
    }

    [Test]
    public void TrySwap_SameInstanceReturnsAlreadyEquipped()
    {
        AddDefinition(
            "iron_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        CharacterItemInstance sword =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterInventory inventory =
            CreateInventory(sword);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                new EquippedItemInstance(
                    EquipmentSlotType.MainHand,
                    sword.InstanceId)
                });

        EquipmentOperationResult result =
            CreateService().TrySwap(
                inventory,
                loadout,
                sword.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult
                    .AlreadyEquipped));

        Assert.That(
            loadout.Items[0].InstanceId,
            Is.EqualTo(sword.InstanceId));
    }

    [Test]
    public void Constructor_NullDefinitionResolverThrows()
    {
        Assert.That(
            () =>
                new CharacterEquipmentService(
                    null,
                    new EquipmentSlotCompatibilityValidator(
                        new ItemDefinitionContentValidator()),
                    new ItemRequirementValidator()),
            Throws.TypeOf<
                System.ArgumentNullException>());
    }

    [Test]
    public void Constructor_NullSlotValidatorThrows()
    {
        Assert.That(
            () =>
                new CharacterEquipmentService(
                    new ItemDefinitionCatalog(null),
                    null,
                    new ItemRequirementValidator()),
            Throws.TypeOf<
                System.ArgumentNullException>());
    }

    [Test]
    public void Constructor_NullRequirementValidatorThrows()
    {
        Assert.That(
            () =>
                new CharacterEquipmentService(
                    new ItemDefinitionCatalog(null),
                    new EquipmentSlotCompatibilityValidator(
                        new ItemDefinitionContentValidator()),
                    null),
            Throws.TypeOf<
                System.ArgumentNullException>());
    }

    [Test]
    public void TryEquip_UnknownDefinitionIsAtomic()
    {
        CharacterItemInstance item =
            CreateItem(
                "instance_001",
                "missing_definition");

        CharacterInventory inventory =
            new CharacterInventory(
                10,
                new ItemDefinitionCatalog(null),
                new[]
                {
                item
                });

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        EquipmentOperationResult result =
            CreateService().TryEquip(
                inventory,
                loadout,
                item.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult
                    .UnknownDefinition));

        Assert.That(
            loadout.Items,
            Is.Empty);

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));
    }

    [Test]
    public void TrySwap_EmptySlotIsAtomic()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        CharacterInventory inventory =
            CreateInventory();

        EquipmentOperationResult result =
            CreateService().TrySwap(
                inventory,
                loadout,
                new CharacterItemInstanceId(
                    "missing_instance"),
                EquipmentSlotType.MainHand,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            result,
            Is.EqualTo(
                EquipmentOperationResult.SlotEmpty));

        Assert.That(
            loadout.Items,
            Is.Empty);

        Assert.That(
            inventory.Items,
            Is.Empty);
    }

    [Test]
    public void TryEquip_TwoAccessoriesCanOccupySeparateSlots()
    {
        AddDefinition(
            "wolf_charm",
            EquipmentSlotType.Accessory,
            EquipmentItemCategory.Accessory,
            WeaponHandedness.NotApplicable);

        AddDefinition(
            "raven_ring",
            EquipmentSlotType.Accessory,
            EquipmentItemCategory.Accessory,
            WeaponHandedness.NotApplicable);

        CharacterItemInstance first =
            CreateItem(
                "instance_001",
                "wolf_charm");

        CharacterItemInstance second =
            CreateItem(
                "instance_002",
                "raven_ring");

        CharacterInventory inventory =
            CreateInventory(
                first,
                second);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        CharacterEquipmentService service =
            CreateService();

        EquipmentOperationResult firstResult =
            service.TryEquip(
                inventory,
                loadout,
                first.InstanceId,
                EquipmentSlotType.Accessory,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        EquipmentOperationResult secondResult =
            service.TryEquip(
                inventory,
                loadout,
                second.InstanceId,
                EquipmentSlotType.AccessoryTwo,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            firstResult,
            Is.EqualTo(
                EquipmentOperationResult.Equipped));

        Assert.That(
            secondResult,
            Is.EqualTo(
                EquipmentOperationResult.Equipped));

        Assert.That(
            loadout.Count,
            Is.EqualTo(2));

        Assert.That(
            loadout.TryGet(
                EquipmentSlotType.Accessory,
                out EquippedItemInstance firstEquipped),
            Is.True);

        Assert.That(
            firstEquipped.InstanceId,
            Is.EqualTo(first.InstanceId));

        Assert.That(
            loadout.TryGet(
                EquipmentSlotType.AccessoryTwo,
                out EquippedItemInstance secondEquipped),
            Is.True);

        Assert.That(
            secondEquipped.InstanceId,
            Is.EqualTo(second.InstanceId));
    }

    [Test]
    public void TryEquipAndUnequip_WeaponProfileRemainsUnchanged()
    {
        AddDefinition(
            "iron_sword",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.OneHanded);

        RolledWeaponProfile profile =
            new RolledWeaponProfile(
                new[]
                {
                new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Fire,
                    4,
                    8)
                });

        CharacterItemInstance item =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                new EquipmentBaseTypeId(
                    "sword"),
                10,
                0,
                1,
                EquipmentItemRarity.Common,
                System.Array.Empty<
                    RolledEquipmentAffix>(),
                profile);

        CharacterInventory inventory =
            CreateInventory(item);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        CharacterEquipmentService service =
            CreateService();

        EquipmentOperationResult equipResult =
            service.TryEquip(
                inventory,
                loadout,
                item.InstanceId,
                EquipmentSlotType.MainHand,
                new CharacterClassId(
                    "companion"),
                new CharacterSpecializationId(
                    "berserker"));

        Assert.That(
            equipResult,
            Is.EqualTo(
                EquipmentOperationResult.Equipped));

        Assert.That(
            inventory.TryGet(
                item.InstanceId,
                out CharacterItemInstance equippedItem),
            Is.True);

        Assert.That(
            equippedItem.WeaponProfile,
            Is.SameAs(profile));

        EquipmentOperationResult unequipResult =
            service.TryUnequip(
                loadout,
                EquipmentSlotType.MainHand);

        Assert.That(
            unequipResult,
            Is.EqualTo(
                EquipmentOperationResult.Unequipped));

        Assert.That(
            inventory.TryGet(
                item.InstanceId,
                out CharacterItemInstance unequippedItem),
            Is.True);

        Assert.That(
            unequippedItem.WeaponProfile,
            Is.SameAs(profile));

        Assert.That(
            unequippedItem,
            Is.EqualTo(item));
    }

    private CharacterEquipmentService CreateService()
    {
        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(
                definitions);

        return new CharacterEquipmentService(
            catalog,
            new EquipmentSlotCompatibilityValidator(
                new ItemDefinitionContentValidator()),
            new ItemRequirementValidator());
    }

    private CharacterInventory CreateInventory(
        params CharacterItemInstance[] items)
    {
        return new CharacterInventory(
            10,
            new ItemDefinitionCatalog(
                definitions),
            items);
    }

    private void AddDefinition(
        string itemId,
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
            1,
            category,
            handedness);

        definitions.Add(definition);
    }

    private static CharacterItemInstance CreateItem(
        string instanceId,
        string itemId)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                instanceId),
            new CharacterItemId(itemId),
            1,
            0,
            1);
    }
}