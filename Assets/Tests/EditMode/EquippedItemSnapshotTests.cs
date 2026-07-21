using DiceBossArena.Game;
using NUnit.Framework;

public sealed class EquippedItemSnapshotTests
{
    [Test]
    public void Constructor_WithoutProfile_CreatesItem()
    {
        EquippedItemSnapshot item =
            new EquippedItemSnapshot(
                EquipmentSlotType.Armor,
                new CharacterItemId(
                    "leather_armor"));

        Assert.That(
            item.SlotType,
            Is.EqualTo(EquipmentSlotType.Armor));

        Assert.That(
            item.ItemId.Value,
            Is.EqualTo("leather_armor"));

        Assert.That(
            item.WeaponProfile,
            Is.Null);
    }

    [Test]
    public void Constructor_WithProfile_PreservesProfile()
    {
        RolledWeaponProfile profile =
            CreateProfile(
                WeaponAttackElement.Fire);

        EquippedItemSnapshot item =
            new EquippedItemSnapshot(
                EquipmentSlotType.MainHand,
                new CharacterItemId(
                    "iron_sword"),
                profile);

        Assert.That(
            item.WeaponProfile,
            Is.SameAs(profile));
    }

    [Test]
    public void EqualItemsWithEqualProfiles_AreEqual()
    {
        EquippedItemSnapshot first =
            CreateWeaponSnapshot(
                CreateProfile(
                    WeaponAttackElement.Fire));

        EquippedItemSnapshot second =
            CreateWeaponSnapshot(
                CreateProfile(
                    WeaponAttackElement.Fire));

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(second.GetHashCode()));
    }

    [Test]
    public void ItemsWithDifferentProfiles_AreNotEqual()
    {
        EquippedItemSnapshot first =
            CreateWeaponSnapshot(
                CreateProfile(
                    WeaponAttackElement.Fire));

        EquippedItemSnapshot second =
            CreateWeaponSnapshot(
                CreateProfile(
                    WeaponAttackElement.Water));

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }

    private static EquippedItemSnapshot
        CreateWeaponSnapshot(
            RolledWeaponProfile profile)
    {
        return new EquippedItemSnapshot(
            EquipmentSlotType.MainHand,
            new CharacterItemId(
                "iron_sword"),
            profile);
    }

    private static RolledWeaponProfile
        CreateProfile(
            WeaponAttackElement element)
    {
        return new RolledWeaponProfile(
            new[]
            {
                new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    element,
                    4,
                    8)
            });
    }
}