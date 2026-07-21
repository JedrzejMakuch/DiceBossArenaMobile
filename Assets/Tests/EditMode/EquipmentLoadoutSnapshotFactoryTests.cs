using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    EquipmentLoadoutSnapshotFactoryTests
{
    [Test]
    public void Create_PreservesEquippedWeaponProfile()
    {
        RolledWeaponProfile profile =
            CreateProfile();

        CharacterItemInstance item =
            CreateWeapon(profile);

        CharacterInventory inventory =
            CreateInventory(item);

        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    new EquippedItemInstance(
                        EquipmentSlotType.MainHand,
                        item.InstanceId)
                });

        EquipmentLoadoutSnapshot snapshot =
            EquipmentLoadoutSnapshotFactory.Create(
                inventory,
                loadout);

        Assert.That(
            snapshot.Items.Count,
            Is.EqualTo(1));

        Assert.That(
            snapshot.Items[0].SlotType,
            Is.EqualTo(
                EquipmentSlotType.MainHand));

        Assert.That(
            snapshot.Items[0].ItemId,
            Is.EqualTo(item.ItemId));

        Assert.That(
            snapshot.Items[0].WeaponProfile,
            Is.SameAs(profile));
    }

    [Test]
    public void Create_EmptyLoadoutCreatesEmptySnapshot()
    {
        EquipmentLoadoutSnapshot snapshot =
            EquipmentLoadoutSnapshotFactory.Create(
                CreateInventory(),
                new CharacterEquipmentLoadout());

        Assert.That(
            snapshot.Items,
            Is.Empty);
    }

    [Test]
    public void Create_MissingInventoryItemThrowsException()
    {
        CharacterEquipmentLoadout loadout =
            new CharacterEquipmentLoadout(
                new[]
                {
                    new EquippedItemInstance(
                        EquipmentSlotType.MainHand,
                        new CharacterItemInstanceId(
                            "missing_instance"))
                });

        Assert.Throws<InvalidOperationException>(
            () =>
                EquipmentLoadoutSnapshotFactory.Create(
                    CreateInventory(),
                    loadout));
    }

    [Test]
    public void Create_NullArgumentsThrowExceptions()
    {
        Assert.Throws<ArgumentNullException>(
            () =>
                EquipmentLoadoutSnapshotFactory.Create(
                    null,
                    new CharacterEquipmentLoadout()));

        Assert.Throws<ArgumentNullException>(
            () =>
                EquipmentLoadoutSnapshotFactory.Create(
                    CreateInventory(),
                    null));
    }

    private static CharacterInventory CreateInventory(
        params CharacterItemInstance[] items)
    {
        return new CharacterInventory(
            10,
            new ItemDefinitionCatalog(
                Array.Empty<ItemDefinition>()),
            items);
    }

    private static CharacterItemInstance CreateWeapon(
        RolledWeaponProfile profile)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "weapon_instance"),
            new CharacterItemId(
                "iron_sword"),
            new EquipmentBaseTypeId(
                "sword"),
            10,
            0,
            1,
            EquipmentItemRarity.Common,
            Array.Empty<RolledEquipmentAffix>(),
            profile);
    }

    private static RolledWeaponProfile CreateProfile()
    {
        return new RolledWeaponProfile(
            new[]
            {
                new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Fire,
                    4,
                    8)
            });
    }
}