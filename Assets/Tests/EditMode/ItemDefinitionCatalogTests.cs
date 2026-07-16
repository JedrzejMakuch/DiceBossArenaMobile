using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class ItemDefinitionCatalogTests
{
    private ItemDefinition firstDefinition;
    private ItemDefinition secondDefinition;

    [TearDown]
    public void TearDown()
    {
        DestroyDefinition(firstDefinition);
        DestroyDefinition(secondDefinition);
    }

    [Test]
    public void TryResolve_KnownIdReturnsDefinition()
    {
        firstDefinition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.Weapon);

        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(
                new[]
                {
                    firstDefinition
                });

        bool resolved =
            catalog.TryResolve(
                new CharacterItemId("iron_sword"),
                out ItemDefinition result);

        Assert.That(
            resolved,
            Is.True);

        Assert.That(
            result,
            Is.SameAs(firstDefinition));
    }

    [Test]
    public void TryResolve_UnknownIdReturnsFalse()
    {
        firstDefinition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.Weapon);

        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(
                new[]
                {
                    firstDefinition
                });

        bool resolved =
            catalog.TryResolve(
                new CharacterItemId("wooden_staff"),
                out ItemDefinition result);

        Assert.That(
            resolved,
            Is.False);

        Assert.That(
            result,
            Is.Null);
    }

    [Test]
    public void TryResolve_InvalidIdReturnsFalse()
    {
        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(null);

        bool resolved =
            catalog.TryResolve(
                new CharacterItemId(" "),
                out ItemDefinition result);

        Assert.That(
            resolved,
            Is.False);

        Assert.That(
            result,
            Is.Null);
    }

    [Test]
    public void Constructor_DuplicateIdThrows()
    {
        firstDefinition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.Weapon);

        secondDefinition =
            CreateDefinition(
                "iron_sword",
                EquipmentSlotType.Weapon);

        Assert.That(
            () =>
                new ItemDefinitionCatalog(
                    new[]
                    {
                        firstDefinition,
                        secondDefinition
                    }),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_NullDefinitionThrows()
    {
        Assert.That(
            () =>
                new ItemDefinitionCatalog(
                    new ItemDefinition[]
                    {
                        null
                    }),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_InvalidDefinitionIdThrows()
    {
        firstDefinition =
            CreateDefinition(
                " ",
                EquipmentSlotType.Weapon);

        Assert.That(
            () =>
                new ItemDefinitionCatalog(
                    new[]
                    {
                        firstDefinition
                    }),
            Throws.ArgumentException);
    }

    [Test]
    public void NullCollectionCreatesEmptyCatalog()
    {
        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(null);

        bool resolved =
            catalog.TryResolve(
                new CharacterItemId("iron_sword"),
                out ItemDefinition result);

        Assert.That(
            resolved,
            Is.False);

        Assert.That(
            result,
            Is.Null);
    }

    private static ItemDefinition CreateDefinition(
        string itemId,
        EquipmentSlotType slotType)
    {
        ItemDefinition definition =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        definition.InitializeForTests(
            itemId,
            slotType);

        return definition;
    }

    private static void DestroyDefinition(
        ItemDefinition definition)
    {
        if (definition != null)
        {
            Object.DestroyImmediate(definition);
        }
    }
}