using DiceBossArena.Game;
using NUnit.Framework;

public class EquipmentLoadoutSnapshotTests
{
    [Test]
    public void Constructor_CopiesItems()
    {
        EquippedItemSnapshot[] source =
        {
            new EquippedItemSnapshot(
                EquipmentSlotType.Weapon,
                new CharacterItemId("iron_sword"))
        };

        EquipmentLoadoutSnapshot loadout =
            new EquipmentLoadoutSnapshot(source);

        source[0] =
            new EquippedItemSnapshot(
                EquipmentSlotType.Weapon,
                new CharacterItemId("wooden_staff"));

        Assert.That(
            loadout.Items.Count,
            Is.EqualTo(1));

        Assert.That(
            loadout.Items[0].ItemId.Value,
            Is.EqualTo("iron_sword"));
    }

    [Test]
    public void Constructor_DuplicateSlotThrowsException()
    {
        EquippedItemSnapshot[] items =
        {
            new EquippedItemSnapshot(
                EquipmentSlotType.Weapon,
                new CharacterItemId("iron_sword")),
            new EquippedItemSnapshot(
                EquipmentSlotType.Weapon,
                new CharacterItemId("wooden_staff"))
        };

        Assert.That(
            () =>
                new EquipmentLoadoutSnapshot(items),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_InvalidItemIdThrowsException()
    {
        EquippedItemSnapshot[] items =
        {
            new EquippedItemSnapshot(
                EquipmentSlotType.Weapon,
                new CharacterItemId("   "))
        };

        Assert.That(
            () =>
                new EquipmentLoadoutSnapshot(items),
            Throws.ArgumentException);
    }

    [Test]
    public void LoadoutsWithSameContent_AreEqual()
    {
        EquipmentLoadoutSnapshot first =
            new EquipmentLoadoutSnapshot(
                new[]
                {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.Weapon,
                        new CharacterItemId(
                            "iron_sword")),
                    new EquippedItemSnapshot(
                        EquipmentSlotType.Armor,
                        new CharacterItemId(
                            "leather_armor"))
                });

        EquipmentLoadoutSnapshot second =
            new EquipmentLoadoutSnapshot(
                new[]
                {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.Weapon,
                        new CharacterItemId(
                            "iron_sword")),
                    new EquippedItemSnapshot(
                        EquipmentSlotType.Armor,
                        new CharacterItemId(
                            "leather_armor"))
                });

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(second.GetHashCode()));
    }

    [Test]
    public void NullCollectionCreatesEmptyLoadout()
    {
        EquipmentLoadoutSnapshot loadout =
            new EquipmentLoadoutSnapshot(null);

        Assert.That(
            loadout.Items,
            Is.Empty);
    }
}