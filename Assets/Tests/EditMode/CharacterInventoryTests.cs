using System;
using DiceBossArena.Game;
using NUnit.Framework;

public class CharacterInventoryTests
{
    [Test]
    public void TryGet_UnknownInstanceReturnsFalse()
    {
        CharacterInventory inventory =
            new CharacterInventory(
                5,
                CreateEmptyResolver());

        bool found =
            inventory.TryGet(
                new CharacterItemInstanceId(
                    "missing_instance"),
                out CharacterItemInstance result);

        Assert.That(
            found,
            Is.False);

        Assert.That(
            result.IsValid,
            Is.False);
    }

    [Test]
    public void TryGet_KnownInstanceReturnsItem()
    {
        CharacterItemInstance expected =
            CreateItem(
                "instance_001",
                "iron_sword");

        CharacterInventory inventory =
            new CharacterInventory(
                5,
                CreateEmptyResolver(),
                new[]
                {
                expected
                });

        bool found =
            inventory.TryGet(
                new CharacterItemInstanceId(
                    "instance_001"),
                out CharacterItemInstance result);

        Assert.That(
            found,
            Is.True);

        Assert.That(
            result,
            Is.EqualTo(expected));
    }

    [Test]
    public void Constructor_CreatesEmptyInventory()
    {
        CharacterInventory inventory =
            new CharacterInventory(10, CreateEmptyResolver());

        Assert.That(
            inventory.Capacity,
            Is.EqualTo(10));

        Assert.That(
            inventory.Count,
            Is.EqualTo(0));

        Assert.That(
            inventory.Items,
            Is.Empty);

        Assert.That(
            inventory.IsFull,
            Is.False);
    }

    [Test]
    public void Constructor_CopiesInitialItems()
    {
        CharacterItemInstance[] source =
        {
            CreateItem(
                "instance_001",
                "iron_sword")
        };

        CharacterInventory inventory =
            new CharacterInventory(
                5, CreateEmptyResolver(),
                source);

        source[0] =
            CreateItem(
                "instance_002",
                "wooden_shield");

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));

        Assert.That(
            inventory.Items[0].InstanceId.Value,
            Is.EqualTo("instance_001"));

        Assert.That(
            inventory.Items[0].ItemId.Value,
            Is.EqualTo("iron_sword"));
    }

    [Test]
    public void Constructor_FullInventoryIsMarkedAsFull()
    {
        CharacterInventory inventory =
            new CharacterInventory(
                1, CreateEmptyResolver(),
                new[]
                {
                    CreateItem(
                        "instance_001",
                        "iron_sword")
                });

        Assert.That(
            inventory.IsFull,
            Is.True);
    }

    [Test]
    public void Constructor_CapacityBelowOneThrows()
    {
        Assert.That(
            () => new CharacterInventory(0, CreateEmptyResolver()),
            Throws.TypeOf<
                ArgumentOutOfRangeException>());
    }

    [Test]
    public void Constructor_TooManyItemsThrows()
    {
        Assert.That(
            () =>
                new CharacterInventory(
                    1, CreateEmptyResolver(),
                    new[]
                    {
                        CreateItem(
                            "instance_001",
                            "iron_sword"),
                        CreateItem(
                            "instance_002",
                            "wooden_shield")
                    }),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_DuplicateInstanceIdThrows()
    {
        Assert.That(
            () =>
                new CharacterInventory(
                    5, CreateEmptyResolver(),
                    new[]
                    {
                        CreateItem(
                            "instance_001",
                            "iron_sword"),
                        CreateItem(
                            "instance_001",
                            "wooden_shield")
                    }),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_DefaultItemInstanceThrows()
    {
        Assert.That(
            () =>
                new CharacterInventory(
                    5,
    CreateEmptyResolver(),
                    new[]
                    {
                        default(
                            CharacterItemInstance)
                    }),
            Throws.ArgumentException);
    }



    private static IItemDefinitionResolver
    CreateEmptyResolver()
    {
        return new ItemDefinitionCatalog(null);
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