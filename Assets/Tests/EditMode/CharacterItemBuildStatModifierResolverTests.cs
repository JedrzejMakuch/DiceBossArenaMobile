using DiceBossArena.Game;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CharacterItemBuildStatModifierResolverTests
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
                new CharacterItemBuildStatModifierResolver(
                    null));
    }

    [Test]
    public void Resolve_InvalidItemThrows()
    {
        ItemDefinitionCatalog catalog =
            CreateCatalog();

        CharacterItemBuildStatModifierResolver resolver =
            new CharacterItemBuildStatModifierResolver(
                catalog);

        Assert.Throws<ArgumentException>(
            () => resolver.Resolve(default));
    }

    [Test]
    public void Resolve_MissingItemDefinitionThrows()
    {
        ItemDefinitionCatalog catalog =
            CreateCatalog();

        CharacterItemBuildStatModifierResolver resolver =
            new CharacterItemBuildStatModifierResolver(
                catalog);

        CharacterItemInstance item =
            CreateItem(
                itemId: "missing_item",
                baseTypeId: string.Empty);

        Assert.Throws<InvalidOperationException>(
            () => resolver.Resolve(item));
    }

    [Test]
    public void Resolve_BaseTypeMismatchThrows()
    {
        EquipmentBaseTypeDefinition baseType =
            CreateBaseType(
                "sword");

        ItemDefinition definition =
            CreateItemDefinition(
                "test_item",
                baseType);

        ItemDefinitionCatalog catalog =
            CreateCatalog(definition);

        CharacterItemBuildStatModifierResolver resolver =
            new CharacterItemBuildStatModifierResolver(
                catalog);

        CharacterItemInstance item =
            CreateItem(
                itemId: "test_item",
                baseTypeId: "axe");

        Assert.Throws<InvalidOperationException>(
            () => resolver.Resolve(item));
    }

    [Test]
    public void Resolve_ReturnsBaseDefinitionAndAffixModifiersInOrder()
    {
        CharacterStatModifierDefinition baseModifier =
            CreateModifierDefinition(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                4);

        CharacterStatModifierDefinition itemModifier =
            CreateModifierDefinition(
                FightStatType.Initiative,
                FightStatModifierType.Flat,
                2);

        EquipmentBaseTypeDefinition baseType =
            CreateBaseType(
                "sword",
                new[]
                {
                    baseModifier
                });

        ItemDefinition definition =
            CreateItemDefinition(
                "test_item",
                baseType,
                new[]
                {
                    itemModifier
                });

        ItemDefinitionCatalog catalog =
            CreateCatalog(definition);

        CharacterItemBuildStatModifierResolver resolver =
            new CharacterItemBuildStatModifierResolver(
                catalog);

        CharacterItemInstance item =
            CreateItem(
                itemId: "test_item",
                baseTypeId: "sword",
                affixes: new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "strength_flat"),
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        7),

                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "fire_resistance_percent"),
                        FightStatType.FireResistance,
                        FightStatModifierType.Percent,
                        10)
                });

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(item);

        Assert.That(
            result.Count,
            Is.EqualTo(4));

        Assert.That(
            result[0],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    4)));

        Assert.That(
            result[1],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.Initiative,
                    FightStatModifierType.Flat,
                    2)));

        Assert.That(
            result[2],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    7)));

        Assert.That(
            result[3],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.FireResistance,
                    FightStatModifierType.Percent,
                    10)));
    }

    [Test]
    public void Resolve_ItemWithoutModifiersReturnsEmptyCollection()
    {
        ItemDefinition definition =
            CreateItemDefinition(
                "test_item",
                null);

        ItemDefinitionCatalog catalog =
            CreateCatalog(definition);

        CharacterItemBuildStatModifierResolver resolver =
            new CharacterItemBuildStatModifierResolver(
                catalog);

        CharacterItemInstance item =
            CreateItem(
                itemId: "test_item",
                baseTypeId: string.Empty);

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(item);

        Assert.That(
            result,
            Is.Empty);
    }

    private EquipmentBaseTypeDefinition CreateBaseType(
        string baseTypeId,
        IReadOnlyList<CharacterStatModifierDefinition>
            modifiers = null)
    {
        EquipmentBaseTypeDefinition definition =
            ScriptableObject.CreateInstance<
                EquipmentBaseTypeDefinition>();

        createdObjects.Add(definition);

        definition.InitializeForTests(
            baseTypeId,
            EquipmentSlotType.MainHand,
            EquipmentBaseTypeCategory.Sword,
            modifiers);

        return definition;
    }

    private ItemDefinition CreateItemDefinition(
        string itemId,
        EquipmentBaseTypeDefinition baseType,
        IReadOnlyList<CharacterStatModifierDefinition>
            modifiers = null)
    {
        ItemDefinition definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        createdObjects.Add(definition);

        definition.InitializeForTests(
            newItemId: itemId,
            newSlotType: EquipmentSlotType.MainHand,
            newStatModifiers: modifiers,
            newBaseType: baseType);

        return definition;
    }

    private CharacterStatModifierDefinition
        CreateModifierDefinition(
            FightStatType statType,
            FightStatModifierType modifierType,
            int value)
    {
        CharacterStatModifierDefinition definition =
            new CharacterStatModifierDefinition(
                statType,
                modifierType,
                value);

        return definition;
    }

    private static ItemDefinitionCatalog CreateCatalog(
    params ItemDefinition[] definitions)
    {
        return new ItemDefinitionCatalog(
            definitions);
    }

    private static CharacterItemInstance CreateItem(
        string itemId,
        string baseTypeId,
        IReadOnlyList<RolledEquipmentAffix>
            affixes = null)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "test_instance"),
            new CharacterItemId(itemId),
            new EquipmentBaseTypeId(baseTypeId),
            level: 1,
            upgradeLevel: 0,
            quantity: 1,
            rarity: EquipmentItemRarity.Common,
            newAffixes:
                affixes ??
                Array.Empty<RolledEquipmentAffix>());
    }
}