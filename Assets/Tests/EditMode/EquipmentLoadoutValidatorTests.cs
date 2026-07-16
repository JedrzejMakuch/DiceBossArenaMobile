using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class EquipmentLoadoutValidatorTests
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
                UnityEngine.Object.DestroyImmediate(
                    definitions[i]);
            }
        }

        definitions.Clear();
    }

    [Test]
    public void Validate_OneHandedWeaponAndShieldSucceeds()
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

        EquipmentLoadoutSnapshot loadout =
            CreateLoadout(
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "iron_sword")),
                new EquippedItemSnapshot(
                    EquipmentSlotType.OffHand,
                    new CharacterItemId(
                        "wooden_shield")));

        Assert.That(
            () => CreateValidator().Validate(
                loadout,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker")),
            Throws.Nothing);
    }

    [Test]
    public void Validate_TwoHandedWeaponWithoutOffHandSucceeds()
    {
        AddDefinition(
            "battle_axe",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.TwoHanded);

        EquipmentLoadoutSnapshot loadout =
            CreateLoadout(
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "battle_axe")));

        Assert.That(
            () => CreateValidator().Validate(
                loadout,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker")),
            Throws.Nothing);
    }

    [Test]
    public void Validate_TwoHandedWeaponWithOffHandThrows()
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

        EquipmentLoadoutSnapshot loadout =
            CreateLoadout(
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "battle_axe")),
                new EquippedItemSnapshot(
                    EquipmentSlotType.OffHand,
                    new CharacterItemId(
                        "wooden_shield")));

        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(
                () => CreateValidator().Validate(
                    loadout,
                    new CharacterClassId("companion"),
                    new CharacterSpecializationId(
                        "berserker")));

        Assert.That(
            exception.Message,
            Does.Contain("blocks the OffHand slot"));
    }

    [Test]
    public void Validate_ItemInWrongSlotThrows()
    {
        AddDefinition(
            "wooden_shield",
            EquipmentSlotType.OffHand,
            EquipmentItemCategory.Shield,
            WeaponHandedness.NotApplicable);

        EquipmentLoadoutSnapshot loadout =
            CreateLoadout(
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "wooden_shield")));

        Assert.That(
            () => CreateValidator().Validate(
                loadout,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker")),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Validate_UnknownDefinitionThrows()
    {
        EquipmentLoadoutSnapshot loadout =
            CreateLoadout(
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "missing_weapon")));

        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(
                () => CreateValidator().Validate(
                    loadout,
                    new CharacterClassId("companion"),
                    new CharacterSpecializationId(
                        "berserker")));

        Assert.That(
            exception.Message,
            Does.Contain("missing_weapon"));
    }

    [Test]
    public void Validate_UnmetClassRequirementThrows()
    {
        AddDefinition(
            "berserker_axe",
            EquipmentSlotType.MainHand,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.TwoHanded,
            requiredClassIds:
                new[]
                {
                    "companion"
                },
            requiredSpecializationIds:
                new[]
                {
                    "berserker"
                });

        EquipmentLoadoutSnapshot loadout =
            CreateLoadout(
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "berserker_axe")));

        Assert.That(
            () => CreateValidator().Validate(
                loadout,
                new CharacterClassId("mage"),
                new CharacterSpecializationId(
                    "berserker")),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Validate_NullLoadoutThrows()
    {
        Assert.That(
            () => CreateValidator().Validate(
                null,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker")),
            Throws.TypeOf<
                ArgumentNullException>());
    }

    private ItemDefinition AddDefinition(
        string itemId,
        EquipmentSlotType slotType,
        EquipmentItemCategory category,
        WeaponHandedness handedness,
        string[] requiredClassIds = null,
        string[]
            requiredSpecializationIds = null)
    {
        ItemDefinition definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            itemId,
            slotType,
            1,
            category,
            handedness,
            newRequiredClassIds:
                requiredClassIds,
            newRequiredSpecializationIds:
                requiredSpecializationIds);

        definitions.Add(definition);

        return definition;
    }

    private EquipmentLoadoutValidator CreateValidator()
    {
        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(
                definitions);

        return new EquipmentLoadoutValidator(
            catalog,
            new EquipmentSlotCompatibilityValidator(
                new ItemDefinitionContentValidator()),
            new ItemRequirementValidator());
    }

    private static EquipmentLoadoutSnapshot CreateLoadout(
        params EquippedItemSnapshot[] items)
    {
        return new EquipmentLoadoutSnapshot(
            items);
    }
}