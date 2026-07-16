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
    public void InitializeForTests_PreservesEquipmentData()
    {
        definition =
            CreateDefinition();

        Assert.That(
            definition.SlotType,
            Is.EqualTo(EquipmentSlotType.Weapon));

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
            EquipmentSlotType.Weapon,
            1,
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
            EquipmentSlotType.Weapon);

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
            EquipmentSlotType.Weapon,
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
            EquipmentSlotType.Weapon);

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

    private ItemDefinition CreateDefinition()
    {
        ItemDefinition result =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        result.InitializeForTests(
            "iron_sword",
            EquipmentSlotType.Weapon,
            1,
            "Iron Sword",
            "item.iron_sword.name",
            "item.iron_sword.description",
            "A basic sword.");

        return result;
    }
}