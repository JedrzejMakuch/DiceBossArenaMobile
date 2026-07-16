using DiceBossArena.Game;
using NUnit.Framework;
using System;

public class CharacterItemInstanceIdTests
{
    [Test]
    public void Constructor_TrimsValue()
    {
        CharacterItemInstanceId id =
            new CharacterItemInstanceId(
                "  item_instance_001  ");

        Assert.That(
            id.Value,
            Is.EqualTo("item_instance_001"));
    }

    [Test]
    public void EmptyValue_IsInvalid()
    {
        CharacterItemInstanceId id =
            new CharacterItemInstanceId("   ");

        Assert.That(
            id.IsValid,
            Is.False);
    }

    [Test]
    public void SameValues_AreEqual()
    {
        CharacterItemInstanceId first =
            new CharacterItemInstanceId(
                "item_instance_001");

        CharacterItemInstanceId second =
            new CharacterItemInstanceId(
                "item_instance_001");

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(second.GetHashCode()));
    }

    [Test]
    public void DifferentValues_AreNotEqual()
    {
        CharacterItemInstanceId first =
            new CharacterItemInstanceId(
                "item_instance_001");

        CharacterItemInstanceId second =
            new CharacterItemInstanceId(
                "item_instance_002");

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }

    [Test]
    public void Comparison_IsCaseSensitive()
    {
        CharacterItemInstanceId first =
            new CharacterItemInstanceId(
                "item_instance_001");

        CharacterItemInstanceId second =
            new CharacterItemInstanceId(
                "ITEM_INSTANCE_001");

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
}