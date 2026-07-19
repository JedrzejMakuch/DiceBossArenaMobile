using System;
using DiceBossArena.Game;
using NUnit.Framework;

public class CharacterEquipmentLoadoutTests
{
    [Test]
    public void Constructor_CreatesEmptyLoadout()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        Assert.That(
            loadout.Items,
            Is.Empty);

        Assert.That(
            loadout.Count,
            Is.EqualTo(0));
    }

    [Test]
    public void Constructor_CopiesInitialItems()
    {
        EquippedItemInstance[] source =
        {
            CreateItem(
                EquipmentSlotType.MainHand,
                "instance_001")
        };

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                source);

        source[0] =
            CreateItem(
                EquipmentSlotType.OffHand,
                "instance_002");

        Assert.That(
            loadout.Count,
            Is.EqualTo(1));

        Assert.That(
            loadout.Items[0].SlotType,
            Is.EqualTo(
                EquipmentSlotType.MainHand));

        Assert.That(
            loadout.Items[0]
                .InstanceId.Value,
            Is.EqualTo("instance_001"));
    }

    [Test]
    public void TryGet_OccupiedSlotReturnsItem()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    CreateItem(
                        EquipmentSlotType.MainHand,
                        "instance_001")
                });

        bool found =
            loadout.TryGet(
                EquipmentSlotType.MainHand,
                out EquippedItemInstance item);

        Assert.That(
            found,
            Is.True);

        Assert.That(
            item.InstanceId.Value,
            Is.EqualTo("instance_001"));
    }

    [Test]
    public void TryGet_EmptySlotReturnsFalse()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        bool found =
            loadout.TryGet(
                EquipmentSlotType.OffHand,
                out EquippedItemInstance item);

        Assert.That(
            found,
            Is.False);

        Assert.That(
            item.IsValid,
            Is.False);
    }

    [Test]
    public void Constructor_DuplicateSlotThrows()
    {
        Assert.That(
            () =>
                new CharacterEquipmentLoadout(
                    new[]
                    {
                        CreateItem(
                            EquipmentSlotType.MainHand,
                            "instance_001"),
                        CreateItem(
                            EquipmentSlotType.MainHand,
                            "instance_002")
                    }),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_DuplicateInstanceIdThrows()
    {
        Assert.That(
            () =>
                new CharacterEquipmentLoadout(
                    new[]
                    {
                        CreateItem(
                            EquipmentSlotType.MainHand,
                            "instance_001"),
                        CreateItem(
                            EquipmentSlotType.OffHand,
                            "instance_001")
                    }),
            Throws.ArgumentException);
    }

    [Test]
    public void EquippedItem_NoneSlotThrows()
    {
        Assert.That(
            () =>
                CreateItem(
                    EquipmentSlotType.None,
                    "instance_001"),
            Throws.ArgumentException);
    }

    [Test]
    public void EquippedItem_InvalidInstanceIdThrows()
    {
        Assert.That(
            () =>
                CreateItem(
                    EquipmentSlotType.MainHand,
                    " "),
            Throws.ArgumentException);
    }

    [Test]
    public void Set_EmptySlotAddsItem()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        bool replaced =
            loadout.Set(
                CreateItem(
                    EquipmentSlotType.MainHand,
                    "instance_001"),
                out EquippedItemInstance previous);

        Assert.That(
            replaced,
            Is.False);

        Assert.That(
            previous.IsValid,
            Is.False);

        Assert.That(
            loadout.Count,
            Is.EqualTo(1));
    }

    [Test]
    public void Set_OccupiedSlotReturnsPreviousItem()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                CreateItem(
                    EquipmentSlotType.MainHand,
                    "instance_001")
                });

        bool replaced =
            loadout.Set(
                CreateItem(
                    EquipmentSlotType.MainHand,
                    "instance_002"),
                out EquippedItemInstance previous);

        Assert.That(
            replaced,
            Is.True);

        Assert.That(
            previous.InstanceId.Value,
            Is.EqualTo("instance_001"));

        Assert.That(
            loadout.Items[0]
                .InstanceId.Value,
            Is.EqualTo("instance_002"));
    }

    [Test]
    public void TryRemove_OccupiedSlotReturnsItem()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                CreateItem(
                    EquipmentSlotType.OffHand,
                    "instance_001")
                });

        bool removed =
            loadout.TryRemove(
                EquipmentSlotType.OffHand,
                out EquippedItemInstance item);

        Assert.That(
            removed,
            Is.True);

        Assert.That(
            item.InstanceId.Value,
            Is.EqualTo("instance_001"));

        Assert.That(
            loadout.Items,
            Is.Empty);
    }

    [Test]
    public void TryRemove_EmptySlotDoesNotChangeLoadout()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout();

        bool removed =
            loadout.TryRemove(
                EquipmentSlotType.OffHand,
                out EquippedItemInstance item);

        Assert.That(
            removed,
            Is.False);

        Assert.That(
            item.IsValid,
            Is.False);

        Assert.That(
            loadout.Items,
            Is.Empty);
    }

    [Test]
    public void Constructor_AllowsEightDifferentEquipmentSlots()
    {
        EquippedItemInstance[] items =
        {
        CreateItem(
            EquipmentSlotType.MainHand,
            "main-hand"),

        CreateItem(
            EquipmentSlotType.OffHand,
            "off-hand"),

        CreateItem(
            EquipmentSlotType.Head,
            "head"),

        CreateItem(
            EquipmentSlotType.Armor,
            "armor"),

        CreateItem(
            EquipmentSlotType.Hands,
            "hands"),

        CreateItem(
            EquipmentSlotType.Feet,
            "feet"),

        CreateItem(
            EquipmentSlotType.Accessory,
            "accessory-one"),

        CreateItem(
            EquipmentSlotType.AccessoryTwo,
            "accessory-two")
    };

        CharacterEquipmentLoadout loadout =
            new(items);

        Assert.That(
            loadout.Count,
            Is.EqualTo(8));

        Assert.That(
            loadout.IsSlotOccupied(
                EquipmentSlotType.MainHand),
            Is.True);

        Assert.That(
            loadout.IsSlotOccupied(
                EquipmentSlotType.OffHand),
            Is.True);

        Assert.That(
            loadout.IsSlotOccupied(
                EquipmentSlotType.Head),
            Is.True);

        Assert.That(
            loadout.IsSlotOccupied(
                EquipmentSlotType.Armor),
            Is.True);

        Assert.That(
            loadout.IsSlotOccupied(
                EquipmentSlotType.Hands),
            Is.True);

        Assert.That(
            loadout.IsSlotOccupied(
                EquipmentSlotType.Feet),
            Is.True);

        Assert.That(
            loadout.IsSlotOccupied(
                EquipmentSlotType.Accessory),
            Is.True);

        Assert.That(
            loadout.IsSlotOccupied(
                EquipmentSlotType.AccessoryTwo),
            Is.True);
    }

    private static EquippedItemInstance CreateItem(
        EquipmentSlotType slotType,
        string instanceId)
    {
        return new EquippedItemInstance(
            slotType,
            new CharacterItemInstanceId(
                instanceId));
    }
}