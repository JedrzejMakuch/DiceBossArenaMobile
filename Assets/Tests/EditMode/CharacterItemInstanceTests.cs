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

        Assert.That(
            updated.Rarity,
            Is.EqualTo(original.Rarity));
    }

    [Test]
    public void Constructor_WithoutAffixes_CreatesEmptyCollection()
    {
        CharacterItemInstance item =
            CreateItem();

        Assert.That(
            item.Affixes,
            Is.Not.Null);

        Assert.That(
            item.Affixes,
            Is.Empty);
    }

    [Test]
    public void Constructor_PreservesAffixes()
    {
        RolledEquipmentAffix strength =
            new RolledEquipmentAffix(
                new EquipmentAffixId(
                    "strength_flat"),
                FightStatType.Strength,
                FightStatModifierType.Flat,
                5);

        RolledEquipmentAffix vitality =
            new RolledEquipmentAffix(
                new EquipmentAffixId(
                    "vitality_flat"),
                FightStatType.MaxHealth,
                FightStatModifierType.Flat,
                3);

        CharacterItemInstance item =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                5,
                2,
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                strength,
                vitality
                });

        Assert.That(
            item.Affixes.Count,
            Is.EqualTo(2));

        Assert.That(
            item.Affixes[0],
            Is.SameAs(strength));

        Assert.That(
            item.Affixes[1],
            Is.SameAs(vitality));
    }

    [Test]
    public void Constructor_PreservesExplicitRarity()
    {
        CharacterItemInstance item =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                5,
                2,
                1,
                EquipmentItemRarity.Rare);

        Assert.That(
            item.Rarity,
            Is.EqualTo(
                EquipmentItemRarity.Rare));
    }

    [Test]
    public void Constructor_NullAffixEntryThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstance(
                    new CharacterItemInstanceId(
                        "item_instance_001"),
                    new CharacterItemId(
                        "iron_sword"),
                    1,
                    0,
                    1,
                    EquipmentItemRarity.Magic,
                    new RolledEquipmentAffix[]
                    {
                    null
                    }),
            Throws.TypeOf<
                ArgumentException>());
    }

    [Test]
    public void CanStackWith_SameAffixesReturnsTrue()
    {
        CharacterItemInstance first =
            CreateItemWithAffixValue(3);

        CharacterItemInstance second =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_002"),
                new CharacterItemId(
                    "iron_sword"),
                5,
                2,
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                CreateStrengthAffix(3)
                });

        Assert.That(
            first.CanStackWith(second),
            Is.True);
    }

    [Test]
    public void InstancesWithDifferentRarity_AreNotEqual()
    {
        CharacterItemInstance first =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                5,
                2,
                1,
                EquipmentItemRarity.Magic);

        CharacterItemInstance second =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                5,
                2,
                1,
                EquipmentItemRarity.Rare);

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }

    [Test]
    public void CanStackWith_DifferentAffixValueReturnsFalse()
    {
        CharacterItemInstance first =
            CreateItemWithAffixValue(3);

        CharacterItemInstance second =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_002"),
                new CharacterItemId(
                    "iron_sword"),
                5,
                2,
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                CreateStrengthAffix(5)
                });

        Assert.That(
            first.CanStackWith(second),
            Is.False);
    }

    [Test]
    public void Constructor_NullAffixesThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstance(
                    new CharacterItemInstanceId(
                        "item_instance_001"),
                    new CharacterItemId(
                        "iron_sword"),
                    1,
                    0,
                    1,
                    EquipmentItemRarity.Magic,
                    null),
            Throws.TypeOf<
                ArgumentNullException>());
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

    private static CharacterItemInstance
        CreateItemWithAffixValue(
            int value)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "item_instance_001"),
            new CharacterItemId(
                "iron_sword"),
            5,
            2,
            1,
            EquipmentItemRarity.Magic,
            new[]
            {
            CreateStrengthAffix(value)
            });
    }

    private static RolledEquipmentAffix
        CreateStrengthAffix(
            int value)
    {
        return new RolledEquipmentAffix(
            new EquipmentAffixId(
                "strength_flat"),
            FightStatType.Strength,
            FightStatModifierType.Flat,
            value);
    }
}