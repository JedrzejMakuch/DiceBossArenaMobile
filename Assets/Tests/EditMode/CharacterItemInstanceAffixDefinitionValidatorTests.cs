using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    CharacterItemInstanceAffixDefinitionValidatorTests
{
    [Test]
    public void Constructor_NullCatalogThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstanceAffixDefinitionValidator(
                    null),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void Validate_InvalidItemThrows()
    {
        CharacterItemInstanceAffixDefinitionValidator validator =
            new CharacterItemInstanceAffixDefinitionValidator(
                CreateCatalog());

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

        CharacterItemInstanceAffixDefinitionValidator validator =
            new CharacterItemInstanceAffixDefinitionValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_MatchingAffix_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "strength_flat"),
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        3)
                });

        CharacterItemInstanceAffixDefinitionValidator validator =
            new CharacterItemInstanceAffixDefinitionValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_UnknownAffixIdThrows()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "unknown_affix"),
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        3)
                });

        CharacterItemInstanceAffixDefinitionValidator validator =
            new CharacterItemInstanceAffixDefinitionValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_MismatchedStatTypeThrows()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "strength_flat"),
                        FightStatType.Dexterity,
                        FightStatModifierType.Flat,
                        3)
                });

        CharacterItemInstanceAffixDefinitionValidator validator =
            new CharacterItemInstanceAffixDefinitionValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_MismatchedModifierTypeThrows()
    {
        CharacterItemInstance item =
            CreateItem(
                EquipmentItemRarity.Magic,
                new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "strength_flat"),
                        FightStatType.Strength,
                        FightStatModifierType.Percent,
                        3)
                });

        CharacterItemInstanceAffixDefinitionValidator validator =
            new CharacterItemInstanceAffixDefinitionValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
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
}