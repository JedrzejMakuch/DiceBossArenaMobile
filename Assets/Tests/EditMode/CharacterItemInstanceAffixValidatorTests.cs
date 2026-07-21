using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    CharacterItemInstanceAffixValidatorTests
{
    [Test]
    public void Constructor_NullCatalogThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstanceAffixValidator(
                    null),
            Throws.TypeOf<
                ArgumentNullException>());
    }

    [Test]
    public void Validate_InvalidItemThrows()
    {
        CharacterItemInstanceAffixValidator validator =
            new CharacterItemInstanceAffixValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(default),
            Throws.TypeOf<
                ArgumentException>());
    }

    [Test]
    public void Validate_ValidCommonItem_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Common,
                Array.Empty<RolledEquipmentAffix>());

        CharacterItemInstanceAffixValidator validator =
            new CharacterItemInstanceAffixValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_ValidMagicItem_DoesNotThrow()
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

        CharacterItemInstanceAffixValidator validator =
            new CharacterItemInstanceAffixValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_WrongAffixCountThrows()
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

        CharacterItemInstanceAffixValidator validator =
            new CharacterItemInstanceAffixValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<
                InvalidOperationException>());
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
                        4)
                });

        CharacterItemInstanceAffixValidator validator =
            new CharacterItemInstanceAffixValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Validate_UnknownAffixIdThrows()
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
                        "unknown_affix",
                        FightStatType.MaxHealth,
                        5)
                });

        CharacterItemInstanceAffixValidator validator =
            new CharacterItemInstanceAffixValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Validate_ValueOutsideTierThrows()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateAffix(
                        "strength_flat",
                        FightStatType.Strength,
                        99),

                    CreateAffix(
                        "health_flat",
                        FightStatType.MaxHealth,
                        5)
                });

        CharacterItemInstanceAffixValidator validator =
            new CharacterItemInstanceAffixValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    private static EquipmentAffixDefinitionCatalog
        CreateCatalog()
    {
        return new EquipmentAffixDefinitionCatalog(
            new[]
            {
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            1,
                            1,
                            5)
                    }),

                new EquipmentAffixDefinition(
                    "health_flat",
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            1,
                            2,
                            8)
                    })
            });
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