using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    CharacterItemInstanceAffixCountValidatorTests
{
    [Test]
    public void Validate_InvalidItemThrows()
    {
        CharacterItemInstanceAffixCountValidator validator =
            new CharacterItemInstanceAffixCountValidator();

        Assert.That(
            () => validator.Validate(default),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Validate_CommonWithoutAffixes_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Common,
                Array.Empty<RolledEquipmentAffix>());

        CharacterItemInstanceAffixCountValidator validator =
            new CharacterItemInstanceAffixCountValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_CommonWithAffix_Throws()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Common,
                new[]
                {
                    CreateAffix(
                        "strength_flat",
                        FightStatType.Strength,
                        3)
                });

        CharacterItemInstanceAffixCountValidator validator =
            new CharacterItemInstanceAffixCountValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_MagicWithTwoAffixes_DoesNotThrow()
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

        CharacterItemInstanceAffixCountValidator validator =
            new CharacterItemInstanceAffixCountValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_MagicWithOneAffix_Throws()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateAffix(
                        "strength_flat",
                        FightStatType.Strength,
                        3)
                });

        CharacterItemInstanceAffixCountValidator validator =
            new CharacterItemInstanceAffixCountValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_RareWithFourAffixes_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Rare,
                new[]
                {
                    CreateAffix(
                        "strength_flat",
                        FightStatType.Strength,
                        3),

                    CreateAffix(
                        "health_flat",
                        FightStatType.MaxHealth,
                        5),

                    CreateAffix(
                        "dexterity_flat",
                        FightStatType.Dexterity,
                        4),

                    CreateAffix(
                        "intelligence_flat",
                        FightStatType.Intelligence,
                        2)
                });

        CharacterItemInstanceAffixCountValidator validator =
            new CharacterItemInstanceAffixCountValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_RareWithThreeAffixes_Throws()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Rare,
                new[]
                {
                    CreateAffix(
                        "strength_flat",
                        FightStatType.Strength,
                        3),

                    CreateAffix(
                        "health_flat",
                        FightStatType.MaxHealth,
                        5),

                    CreateAffix(
                        "dexterity_flat",
                        FightStatType.Dexterity,
                        4)
                });

        CharacterItemInstanceAffixCountValidator validator =
            new CharacterItemInstanceAffixCountValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
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