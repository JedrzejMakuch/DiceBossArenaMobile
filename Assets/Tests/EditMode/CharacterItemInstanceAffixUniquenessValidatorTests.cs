using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    CharacterItemInstanceAffixUniquenessValidatorTests
{
    [Test]
    public void Validate_InvalidItemThrows()
    {
        CharacterItemInstanceAffixUniquenessValidator validator =
            new CharacterItemInstanceAffixUniquenessValidator();

        Assert.That(
            () => validator.Validate(default),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Validate_ItemWithoutAffixes_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Common,
                Array.Empty<RolledEquipmentAffix>());

        CharacterItemInstanceAffixUniquenessValidator validator =
            new CharacterItemInstanceAffixUniquenessValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_UniqueAffixes_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateAffix(
                        "strength_flat",
                        FightStatType.Strength,
                        3),

                    CreateAffix(
                        "health_flat",
                        FightStatType.MaxHealth,
                        5)
                });

        CharacterItemInstanceAffixUniquenessValidator validator =
            new CharacterItemInstanceAffixUniquenessValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_DuplicateAffixIdThrows()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateAffix(
                        "strength_flat",
                        FightStatType.Strength,
                        3),

                    CreateAffix(
                        "strength_flat",
                        FightStatType.Strength,
                        5)
                });

        CharacterItemInstanceAffixUniquenessValidator validator =
            new CharacterItemInstanceAffixUniquenessValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_DifferentIdsWithSameStat_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateAffix(
                        "strength_flat_low",
                        FightStatType.Strength,
                        3),

                    CreateAffix(
                        "strength_flat_high",
                        FightStatType.Strength,
                        5)
                });

        CharacterItemInstanceAffixUniquenessValidator validator =
            new CharacterItemInstanceAffixUniquenessValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    private static CharacterItemInstance CreateItem(
        EquipmentItemRarity rarity,
        RolledEquipmentAffix[] affixes)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "instance_001"),
            new CharacterItemId(
                "iron_sword"),
            5,
            0,
            1,
            rarity,
            affixes);
    }

    private static RolledEquipmentAffix CreateAffix(
        string id,
        FightStatType statType,
        int value)
    {
        return new RolledEquipmentAffix(
            new EquipmentAffixId(id),
            statType,
            FightStatModifierType.Flat,
            value);
    }
}