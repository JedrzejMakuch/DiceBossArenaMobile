using DiceBossArena.Game;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CharacterEquipmentStatModifierResolverTests
{
    private readonly List<UnityEngine.Object>
        createdObjects = new();

    [TearDown]
    public void TearDown()
    {
        for (int index = 0;
             index < createdObjects.Count;
             index++)
        {
            UnityEngine.Object.DestroyImmediate(
                createdObjects[index]);
        }

        createdObjects.Clear();
    }

    [Test]
    public void Constructor_NullCatalogThrows()
    {
        Assert.Throws<ArgumentNullException>(
            () =>
                new CharacterEquipmentStatModifierResolver(
                    null));
    }

    [Test]
    public void Resolve_NullInventoryThrows()
    {
        ItemDefinitionCatalog catalog =
            CreateCatalog();

        CharacterEquipmentStatModifierResolver resolver =
            new CharacterEquipmentStatModifierResolver(
                catalog);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        Assert.Throws<ArgumentNullException>(
            () => resolver.Resolve(
                null,
                loadout));
    }

    [Test]
    public void Resolve_NullLoadoutThrows()
    {
        ItemDefinitionCatalog catalog =
            CreateCatalog();

        CharacterInventory inventory =
            new CharacterInventory(
                capacity: 10,
                definitionResolver: catalog);

        CharacterEquipmentStatModifierResolver resolver =
            new CharacterEquipmentStatModifierResolver(
                catalog);

        Assert.Throws<ArgumentNullException>(
            () => resolver.Resolve(
                inventory,
                null));
    }

    [Test]
    public void Resolve_EmptyLoadoutReturnsEmptyCollection()
    {
        ItemDefinitionCatalog catalog =
            CreateCatalog();

        CharacterInventory inventory =
            new CharacterInventory(
                capacity: 10,
                definitionResolver: catalog);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        CharacterEquipmentStatModifierResolver resolver =
            new CharacterEquipmentStatModifierResolver(
                catalog);

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(
                inventory,
                loadout);

        Assert.That(
            result,
            Is.Not.Null);

        Assert.That(
            result,
            Is.Empty);
    }

    [Test]
    public void Resolve_EquippedInstanceMissingFromInventoryThrows()
    {
        ItemDefinitionCatalog catalog =
            CreateCatalog();

        CharacterInventory inventory =
            new CharacterInventory(
                capacity: 10,
                definitionResolver: catalog);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    new EquippedItemInstance(
                        EquipmentSlotType.MainHand,
                        new CharacterItemInstanceId(
                            "missing_instance"))
                });

        CharacterEquipmentStatModifierResolver resolver =
            new CharacterEquipmentStatModifierResolver(
                catalog);

        Assert.Throws<InvalidOperationException>(
            () => resolver.Resolve(
                inventory,
                loadout));
    }

    [Test]
    public void Resolve_OneEquippedItemReturnsItsModifiers()
    {
        CharacterStatModifierDefinition baseModifier =
            CreateModifierDefinition(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                5);

        EquipmentBaseTypeDefinition baseType =
            CreateBaseType(
                baseTypeId: "sword",
                slotType: EquipmentSlotType.MainHand,
                modifiers: new[]
                {
                    baseModifier
                });

        ItemDefinition itemDefinition =
            CreateItemDefinition(
                itemId: "iron_sword",
                slotType: EquipmentSlotType.MainHand,
                baseType: baseType);

        ItemDefinitionCatalog catalog =
            CreateCatalog(
                itemDefinition);

        CharacterItemInstance item =
            CreateItem(
                instanceId: "sword_instance",
                itemId: "iron_sword",
                baseTypeId: "sword",
                affixes: new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "strength_flat"),
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        3)
                });

        CharacterInventory inventory =
            CreateInventory(
                catalog,
                item);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    new EquippedItemInstance(
                        EquipmentSlotType.MainHand,
                        item.InstanceId)
                });

        CharacterEquipmentStatModifierResolver resolver =
            new CharacterEquipmentStatModifierResolver(
                catalog);

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(
                inventory,
                loadout);

        Assert.That(
            result.Count,
            Is.EqualTo(2));

        Assert.That(
            result[0],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5)));

        Assert.That(
            result[1],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    3)));
    }

    [Test]
    public void Resolve_MultipleItemsPreservesLoadoutOrder()
    {
        CharacterStatModifierDefinition swordModifier =
            CreateModifierDefinition(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                4);

        CharacterStatModifierDefinition helmetModifier =
            CreateModifierDefinition(
                FightStatType.Armor,
                FightStatModifierType.Flat,
                8);

        EquipmentBaseTypeDefinition swordBaseType =
            CreateBaseType(
                baseTypeId: "sword",
                slotType: EquipmentSlotType.MainHand,
                modifiers: new[]
                {
                    swordModifier
                });

        EquipmentBaseTypeDefinition helmetBaseType =
            CreateBaseType(
                baseTypeId: "helmet",
                slotType: EquipmentSlotType.Head,
                modifiers: new[]
                {
                    helmetModifier
                });

        ItemDefinition swordDefinition =
            CreateItemDefinition(
                itemId: "iron_sword",
                slotType: EquipmentSlotType.MainHand,
                baseType: swordBaseType);

        ItemDefinition helmetDefinition =
            CreateItemDefinition(
                itemId: "iron_helmet",
                slotType: EquipmentSlotType.Head,
                baseType: helmetBaseType);

        ItemDefinitionCatalog catalog =
            CreateCatalog(
                swordDefinition,
                helmetDefinition);

        CharacterItemInstance helmet =
            CreateItem(
                instanceId: "helmet_instance",
                itemId: "iron_helmet",
                baseTypeId: "helmet",
                affixes: new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "health_flat"),
                        FightStatType.MaxHealth,
                        FightStatModifierType.Flat,
                        12)
                });

        CharacterItemInstance sword =
            CreateItem(
                instanceId: "sword_instance",
                itemId: "iron_sword",
                baseTypeId: "sword",
                affixes: new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "initiative_flat"),
                        FightStatType.Initiative,
                        FightStatModifierType.Flat,
                        2)
                });

        CharacterInventory inventory =
            CreateInventory(
                catalog,
                helmet,
                sword);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    new EquippedItemInstance(
                        EquipmentSlotType.Head,
                        helmet.InstanceId),

                    new EquippedItemInstance(
                        EquipmentSlotType.MainHand,
                        sword.InstanceId)
                });

        CharacterEquipmentStatModifierResolver resolver =
            new CharacterEquipmentStatModifierResolver(
                catalog);

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(
                inventory,
                loadout);

        Assert.That(
            result.Count,
            Is.EqualTo(4));

        Assert.That(
            result[0],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.Armor,
                    FightStatModifierType.Flat,
                    8)));

        Assert.That(
            result[1],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    12)));

        Assert.That(
            result[2],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    4)));

        Assert.That(
            result[3],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.Initiative,
                    FightStatModifierType.Flat,
                    2)));
    }

    [Test]
    public void Resolve_UnequippedInventoryItemIsIgnored()
    {
        CharacterStatModifierDefinition equippedModifier =
            CreateModifierDefinition(
                FightStatType.Armor,
                FightStatModifierType.Flat,
                6);

        CharacterStatModifierDefinition unequippedModifier =
            CreateModifierDefinition(
                FightStatType.MaxHealth,
                FightStatModifierType.Flat,
                50);

        EquipmentBaseTypeDefinition equippedBaseType =
            CreateBaseType(
                baseTypeId: "helmet",
                slotType: EquipmentSlotType.Head,
                modifiers: new[]
                {
                    equippedModifier
                });

        EquipmentBaseTypeDefinition unequippedBaseType =
            CreateBaseType(
                baseTypeId: "boots",
                slotType: EquipmentSlotType.Feet,
                modifiers: new[]
                {
                    unequippedModifier
                });

        ItemDefinition helmetDefinition =
            CreateItemDefinition(
                itemId: "helmet_item",
                slotType: EquipmentSlotType.Head,
                baseType: equippedBaseType);

        ItemDefinition bootsDefinition =
            CreateItemDefinition(
                itemId: "boots_item",
                slotType: EquipmentSlotType.Feet,
                baseType: unequippedBaseType);

        ItemDefinitionCatalog catalog =
            CreateCatalog(
                helmetDefinition,
                bootsDefinition);

        CharacterItemInstance helmet =
            CreateItem(
                instanceId: "helmet_instance",
                itemId: "helmet_item",
                baseTypeId: "helmet");

        CharacterItemInstance boots =
            CreateItem(
                instanceId: "boots_instance",
                itemId: "boots_item",
                baseTypeId: "boots");

        CharacterInventory inventory =
            CreateInventory(
                catalog,
                helmet,
                boots);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    new EquippedItemInstance(
                        EquipmentSlotType.Head,
                        helmet.InstanceId)
                });

        CharacterEquipmentStatModifierResolver resolver =
            new CharacterEquipmentStatModifierResolver(
                catalog);

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(
                inventory,
                loadout);

        Assert.That(
            result.Count,
            Is.EqualTo(1));

        Assert.That(
            result[0],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.Armor,
                    FightStatModifierType.Flat,
                    6)));
    }

    private CharacterStatModifierDefinition
        CreateModifierDefinition(
            FightStatType statType,
            FightStatModifierType modifierType,
            int value)
    {
        return new CharacterStatModifierDefinition(
            statType,
            modifierType,
            value);
    }

    private EquipmentBaseTypeDefinition CreateBaseType(
        string baseTypeId,
        EquipmentSlotType slotType,
        IReadOnlyList<CharacterStatModifierDefinition>
            modifiers = null)
    {
        EquipmentBaseTypeDefinition definition =
            ScriptableObject.CreateInstance<
                EquipmentBaseTypeDefinition>();

        createdObjects.Add(definition);

        definition.InitializeForTests(
            baseTypeId,
            slotType,
            EquipmentBaseTypeCategory.ChestArmor,
            modifiers);

        return definition;
    }

    private ItemDefinition CreateItemDefinition(
        string itemId,
        EquipmentSlotType slotType,
        EquipmentBaseTypeDefinition baseType)
    {
        ItemDefinition definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        createdObjects.Add(definition);

        definition.InitializeForTests(
            newItemId: itemId,
            newSlotType: slotType,
            newBaseType: baseType);

        return definition;
    }

    private static ItemDefinitionCatalog CreateCatalog(
        params ItemDefinition[] definitions)
    {
        return new ItemDefinitionCatalog(
            definitions);
    }

    private static CharacterInventory CreateInventory(
        ItemDefinitionCatalog catalog,
        params CharacterItemInstance[] items)
    {
        return new CharacterInventory(
            capacity: 10,
            definitionResolver: catalog,
            initialItems: items);
    }

    private static CharacterItemInstance CreateItem(
        string instanceId,
        string itemId,
        string baseTypeId,
        IReadOnlyList<RolledEquipmentAffix>
            affixes = null)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                instanceId),
            new CharacterItemId(
                itemId),
            new EquipmentBaseTypeId(
                baseTypeId),
            level: 1,
            upgradeLevel: 0,
            quantity: 1,
            rarity: EquipmentItemRarity.Common,
            newAffixes:
                affixes ??
                Array.Empty<RolledEquipmentAffix>());
    }
}