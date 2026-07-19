using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class ItemDefinitionTests
{
    private ItemDefinition definition;

    [TearDown]
    public void TearDown()
    {
        if (definition != null)
        {
            Object.DestroyImmediate(definition);
        }
    }

    [Test]
    public void InitializeForTests_PreservesIdentity()
    {
        definition =
            CreateDefinition();

        Assert.That(
            definition.ItemId.Value,
            Is.EqualTo("iron_sword"));

        Assert.That(
            definition.ItemId.IsValid,
            Is.True);

        Assert.That(
            definition.DisplayName,
            Is.EqualTo("Iron Sword"));
    }

    [Test]
    public void InitializeForTests_PreservesLocalizationKeys()
    {
        definition =
            CreateDefinition();

        Assert.That(
            definition.NameLocalizationKey.Value,
            Is.EqualTo("item.iron_sword.name"));

        Assert.That(
            definition.DescriptionLocalizationKey.Value,
            Is.EqualTo(
                "item.iron_sword.description"));
    }

    [Test]
    public void InitializeForTests_AssignsBaseType()
    {
        EquipmentBaseTypeDefinition baseType =
            ScriptableObject.CreateInstance<
                EquipmentBaseTypeDefinition>();

        ItemDefinition item =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        try
        {
            baseType.InitializeForTests(
                "iron_sword",
                EquipmentSlotType.MainHand,
                EquipmentBaseTypeCategory.Sword);

            item.InitializeForTests(
                "iron_sword_item",
                EquipmentSlotType.MainHand,
                newBaseType: baseType);

            Assert.That(
                item.BaseType,
                Is.SameAs(baseType));
        }
        finally
        {
            Object.DestroyImmediate(item);
            Object.DestroyImmediate(baseType);
        }
    }

    [Test]
    public void InitializeForTests_WithoutBaseType_UsesNull()
    {
        ItemDefinition item =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        try
        {
            item.InitializeForTests(
                "iron_sword_item",
                EquipmentSlotType.MainHand);

            Assert.That(
                item.BaseType,
                Is.Null);
        }
        finally
        {
            Object.DestroyImmediate(item);
        }
    }

    [Test]
    public void InitializeForTests_PreservesEquipmentData()
    {
        definition =
            CreateDefinition();

        Assert.That(
            definition.SlotType,
            Is.EqualTo(EquipmentSlotType.MainHand));

        Assert.That(
            definition.MaxStackSize,
            Is.EqualTo(1));
    }

    [Test]
    public void InitializeForTests_TrimsTextValues()
    {
        definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
    "  iron_sword  ",
    EquipmentSlotType.MainHand,
    1,
    EquipmentItemCategory.Weapon,
    WeaponHandedness.OneHanded,
    "  Iron Sword  ",
    "  item.iron_sword.name  ",
    "  item.iron_sword.description  ",
    "  A basic sword.  ");

        Assert.That(
            definition.ItemId.Value,
            Is.EqualTo("iron_sword"));

        Assert.That(
            definition.DisplayName,
            Is.EqualTo("Iron Sword"));

        Assert.That(
            definition.Description,
            Is.EqualTo("A basic sword."));
    }

    [Test]
    public void EmptyDisplayName_UsesItemId()
    {
        definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            "iron_sword",
            EquipmentSlotType.MainHand);

        Assert.That(
            definition.DisplayName,
            Is.EqualTo("iron_sword"));
    }

    [Test]
    public void StackSizeBelowOne_IsClamped()
    {
        definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            "iron_sword",
            EquipmentSlotType.MainHand,
            0);

        Assert.That(
            definition.MaxStackSize,
            Is.EqualTo(1));
    }

    [Test]
    public void NullBuildCollections_CreateEmptyCollections()
    {
        definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            "iron_sword",
            EquipmentSlotType.MainHand);

        Assert.That(
            definition.StatModifiers,
            Is.Empty);

        Assert.That(
            definition.GrantedSkills,
            Is.Empty);

        Assert.That(
            definition.GrantedPassives,
            Is.Empty);
    }

    [Test]
    public void InitializeForTests_PreservesCategoryAndHandedness()
    {
        definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            "battle_axe",
            EquipmentSlotType.MainHand,
            1,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.TwoHanded);

        Assert.That(
            definition.Category,
            Is.EqualTo(
                EquipmentItemCategory.Weapon));

        Assert.That(
            definition.Handedness,
            Is.EqualTo(
                WeaponHandedness.TwoHanded));
    }

    [Test]
    public void InitializeForTests_PreservesRequirements()
    {
        definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            "berserker_axe",
            EquipmentSlotType.MainHand,
            1,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.TwoHanded,
            newRequiredClassIds:
                new[]
                {
                "companion"
                },
            newRequiredSpecializationIds:
                new[]
                {
                "berserker"
                });

        Assert.That(
            definition.RequiredClassIds,
            Is.EqualTo(
                new[]
                {
                "companion"
                }));

        Assert.That(
            definition.RequiredSpecializationIds,
            Is.EqualTo(
                new[]
                {
                "berserker"
                }));
    }

    [Test]
    public void InitializeForTests_NormalizesRequirements()
    {
        definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            "berserker_axe",
            EquipmentSlotType.MainHand,
            1,
            EquipmentItemCategory.Weapon,
            WeaponHandedness.TwoHanded,
            newRequiredClassIds:
                new[]
                {
                "  companion  "
                },
            newRequiredSpecializationIds:
                new[]
                {
                "  berserker  "
                });

        Assert.That(
            definition.RequiredClassIds[0],
            Is.EqualTo("companion"));

        Assert.That(
            definition.RequiredSpecializationIds[0],
            Is.EqualTo("berserker"));
    }

    [Test]
    public void NullRequirements_CreateEmptyCollections()
    {
        definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            "iron_sword",
            EquipmentSlotType.MainHand);

        Assert.That(
            definition.RequiredClassIds,
            Is.Empty);

        Assert.That(
            definition.RequiredSpecializationIds,
            Is.Empty);
    }

    private ItemDefinition CreateDefinition()
    {
        ItemDefinition result =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        result.InitializeForTests(
    "iron_sword",
    EquipmentSlotType.MainHand,
    1,
    EquipmentItemCategory.Weapon,
    WeaponHandedness.OneHanded,
    "Iron Sword",
    "item.iron_sword.name",
    "item.iron_sword.description",
    "A basic sword.");

        return result;
    }
}