using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    CharacterItemInstanceAffixValueValidatorTests
{
    [Test]
    public void Constructor_NullCatalogThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstanceAffixValueValidator(
                    null),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void Validate_InvalidItemThrows()
    {
        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
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
                1,
                EquipmentItemRarity.Common,
                Array.Empty<RolledEquipmentAffix>());

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_ValueInsideFirstTier_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateStrengthAffix(3)
                });

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_MinimumValue_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateStrengthAffix(1)
                });

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_MaximumValue_DoesNotThrow()
    {
        CharacterItemInstance item =
            CreateItem(
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateStrengthAffix(5)
                });

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_ValueBelowTierRange_Throws()
    {
        CharacterItemInstance item =
            CreateItem(
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateStrengthAffix(0)
                });

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_ValueAboveTierRange_Throws()
    {
        CharacterItemInstance item =
            CreateItem(
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateStrengthAffix(6)
                });

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_UsesHighestAvailableTier()
    {
        CharacterItemInstance item =
            CreateItem(
                10,
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateStrengthAffix(8)
                });

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }

    [Test]
    public void Validate_ValueFromOldTierAtHigherLevel_Throws()
    {
        CharacterItemInstance item =
            CreateItem(
                10,
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateStrengthAffix(3)
                });

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                CreateCatalog());

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_NoAvailableTierForItemLevel_Throws()
    {
        EquipmentAffixDefinitionCatalog catalog =
            new EquipmentAffixDefinitionCatalog(
                new[]
                {
                    new EquipmentAffixDefinition(
                        "strength_flat",
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        new[]
                        {
                            new EquipmentAffixTierDefinition(
                                5,
                                1,
                                5)
                        })
                });

        CharacterItemInstance item =
            CreateItem(
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                    CreateStrengthAffix(3)
                });

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
                catalog);

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_UnknownAffixIdThrows()
    {
        CharacterItemInstance item =
            CreateItem(
                1,
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

        CharacterItemInstanceAffixValueValidator validator =
            new CharacterItemInstanceAffixValueValidator(
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
                            5),

                        new EquipmentAffixTierDefinition(
                            10,
                            6,
                            10)
                    })
            });
    }

    private static CharacterItemInstance CreateItem(
        int itemLevel,
        EquipmentItemRarity rarity,
        RolledEquipmentAffix[] affixes)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "instance_001"),
            new CharacterItemId(
                "iron_sword"),
            itemLevel,
            0,
            1,
            rarity,
            affixes);
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