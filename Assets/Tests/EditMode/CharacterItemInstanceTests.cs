using System;
using DiceBossArena.Game;
using NUnit.Framework;

public class CharacterItemInstanceTests
{
    [Test]
    public void Constructor_PreservesValues()
    {
        CharacterItemInstance item =
            CreateItem();

        Assert.That(
            item.InstanceId.Value,
            Is.EqualTo("item_instance_001"));

        Assert.That(
            item.ItemId.Value,
            Is.EqualTo("iron_sword"));

        Assert.That(
            item.Level,
            Is.EqualTo(5));

        Assert.That(
            item.UpgradeLevel,
            Is.EqualTo(2));

        Assert.That(
            item.Quantity,
            Is.EqualTo(1));
    }

    [Test]
    public void Constructor_InvalidInstanceIdThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstance(
                    new CharacterItemInstanceId(" "),
                    new CharacterItemId("iron_sword"),
                    1,
                    0,
                    1),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_InvalidItemIdThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstance(
                    new CharacterItemInstanceId(
                        "item_instance_001"),
                    new CharacterItemId(" "),
                    1,
                    0,
                    1),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_LevelBelowOneThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstance(
                    new CharacterItemInstanceId(
                        "item_instance_001"),
                    new CharacterItemId("iron_sword"),
                    0,
                    0,
                    1),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void Constructor_NegativeUpgradeLevelThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstance(
                    new CharacterItemInstanceId(
                        "item_instance_001"),
                    new CharacterItemId("iron_sword"),
                    1,
                    -1,
                    1),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void Constructor_QuantityBelowOneThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstance(
                    new CharacterItemInstanceId(
                        "item_instance_001"),
                    new CharacterItemId("iron_sword"),
                    1,
                    0,
                    0),
            Throws.TypeOf<ArgumentOutOfRangeException>());
    }

    [Test]
    public void InstancesWithSameValues_AreEqual()
    {
        CharacterItemInstance first =
            CreateItem();

        CharacterItemInstance second =
            CreateItem();

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(second.GetHashCode()));
    }

    [Test]
    public void InstancesWithDifferentInstanceIds_AreNotEqual()
    {
        CharacterItemInstance first =
            CreateItem();

        CharacterItemInstance second =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_002"),
                new CharacterItemId("iron_sword"),
                5,
                2,
                1);

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }

    [Test]
    public void CanStackWith_SameBuildDataReturnsTrue()
    {
        CharacterItemInstance first =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "health_potion"),
                1,
                0,
                3);

        CharacterItemInstance second =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_002"),
                new CharacterItemId(
                    "health_potion"),
                1,
                0,
                2);

        Assert.That(
            first.CanStackWith(second),
            Is.True);
    }

    [Test]
    public void CanStackWith_DifferentLevelReturnsFalse()
    {
        CharacterItemInstance first =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "health_potion"),
                1,
                0,
                1);

        CharacterItemInstance second =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_002"),
                new CharacterItemId(
                    "health_potion"),
                2,
                0,
                1);

        Assert.That(
            first.CanStackWith(second),
            Is.False);
    }

    [Test]
    public void CanStackWith_DifferentUpgradeReturnsFalse()
    {
        CharacterItemInstance first =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                1,
                0,
                1);

        CharacterItemInstance second =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_002"),
                new CharacterItemId(
                    "iron_sword"),
                1,
                1,
                1);

        Assert.That(
            first.CanStackWith(second),
            Is.False);
    }

    [Test]
    public void WithQuantity_ReturnsUpdatedIndependentInstance()
    {
        CharacterItemInstance original =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "health_potion"),
                1,
                0,
                2);

        CharacterItemInstance updated =
            original.WithQuantity(5);

        Assert.That(
            original.Quantity,
            Is.EqualTo(2));

        Assert.That(
            updated.Quantity,
            Is.EqualTo(5));

        Assert.That(
            updated.InstanceId,
            Is.EqualTo(original.InstanceId));

        Assert.That(
            updated.ItemId,
            Is.EqualTo(original.ItemId));
    }

    [Test]
    public void WithQuantity_InvalidQuantityThrows()
    {
        CharacterItemInstance item =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "health_potion"),
                1,
                0,
                2);

        Assert.That(
            () => item.WithQuantity(0),
            Throws.TypeOf<
                ArgumentOutOfRangeException>());
    }

    private static CharacterItemInstance CreateItem()
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "item_instance_001"),
            new CharacterItemId("iron_sword"),
            5,
            2,
            1);
    }
}