using DiceBossArena.Game;
using NUnit.Framework;
using System;
using UnityEngine;

public class CharacterItemInstanceValidatorTests
{
    private ItemDefinition definition;

    private EquipmentBaseTypeDefinition
    baseTypeDefinition;

    [TearDown]
    public void TearDown()
    {
        if (definition != null)
        {
            UnityEngine.Object.DestroyImmediate(
                definition);
        }

        if (baseTypeDefinition != null)
        {
            UnityEngine.Object.DestroyImmediate(
                baseTypeDefinition);
        }
    }

    [Test]
    public void ValidateAndResolve_MissingBaseTypeThrows()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                1);

        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(
                new[]
                {
                definition
                });

        EquipmentBaseTypeDefinitionCatalog
            baseTypeCatalog =
                new EquipmentBaseTypeDefinitionCatalog(
                    null);

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                itemCatalog,
                CreateAffixCatalog(),
                baseTypeCatalog);

        CharacterItemInstance instance =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                1,
                0,
                1,
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

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    instance),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void ValidateAndResolve_UnknownBaseTypeThrows()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                1);

        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(
                new[]
                {
                definition
                });

        EquipmentBaseTypeDefinitionCatalog
            baseTypeCatalog =
                new EquipmentBaseTypeDefinitionCatalog(
                    null);

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                itemCatalog,
                CreateAffixCatalog(),
                baseTypeCatalog);

        CharacterItemInstance instance =
            CreateCompleteMagicInstance(
                "unknown_base_type");

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    instance),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void ValidateAndResolve_ValidCompleteItemReturnsDefinition()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                1);

        baseTypeDefinition =
            CreateBaseTypeDefinition(
                "one_handed_sword");

        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(
                new[]
                {
                definition
                });

        EquipmentBaseTypeDefinitionCatalog
            baseTypeCatalog =
                new EquipmentBaseTypeDefinitionCatalog(
                    new[]
                    {
                    baseTypeDefinition
                    });

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                itemCatalog,
                CreateAffixCatalog(),
                baseTypeCatalog);

        CharacterItemInstance instance =
            CreateCompleteMagicInstance(
                "one_handed_sword");

        ItemDefinition result =
            validator.ValidateAndResolve(
                instance);

        Assert.That(
            result,
            Is.SameAs(definition));
    }

    [Test]
    public void Constructor_NullBaseTypeResolverThrows()
    {
        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(null);

        Assert.That(
            () =>
                new CharacterItemInstanceValidator(
                    itemCatalog,
                    CreateAffixCatalog(),
                    null),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void Constructor_NullResolverThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstanceValidator(null),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void ValidateAndResolve_ValidInstanceReturnsDefinition()
    {
        definition =
            CreateDefinition(
                "health_potion",
                10);

        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(
                new[]
                {
                    definition
                });

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                catalog);

        CharacterItemInstance instance =
            CreateInstance(
                "health_potion",
                5);

        ItemDefinition result =
            validator.ValidateAndResolve(
                instance);

        Assert.That(
            result,
            Is.SameAs(definition));
    }

    [Test]
    public void ValidateAndResolve_UnknownDefinitionThrows()
    {
        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(null);

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                catalog);

        CharacterItemInstance instance =
            CreateInstance(
                "unknown_item",
                1);

        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(
                () =>
                    validator.ValidateAndResolve(
                        instance));

        Assert.That(
            exception.Message,
            Does.Contain("unknown_item"));
    }

    [Test]
    public void ValidateAndResolve_QuantityAboveMaximumThrows()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                1);

        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(
                new[]
                {
                    definition
                });

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                catalog);

        CharacterItemInstance instance =
            CreateInstance(
                "iron_sword",
                2);

        InvalidOperationException exception =
            Assert.Throws<InvalidOperationException>(
                () =>
                    validator.ValidateAndResolve(
                        instance));

        Assert.That(
            exception.Message,
            Does.Contain("quantity 2"));

        Assert.That(
            exception.Message,
            Does.Contain("maximum stack of 1"));
    }

    [Test]
    public void ValidateAndResolve_QuantityAtMaximumSucceeds()
    {
        definition =
            CreateDefinition(
                "health_potion",
                10);

        ItemDefinitionCatalog catalog =
            new ItemDefinitionCatalog(
                new[]
                {
                    definition
                });

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                catalog);

        CharacterItemInstance instance =
            CreateInstance(
                "health_potion",
                10);

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    instance),
            Throws.Nothing);
    }

    [Test]
    public void Constructor_NullAffixCatalogThrows()
    {
        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(null);

        Assert.That(
            () =>
                new CharacterItemInstanceValidator(
                    itemCatalog,
                    null),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void ConstructorWithAffixes_NullResolverThrows()
    {
        EquipmentAffixDefinitionCatalog affixCatalog =
            CreateAffixCatalog();

        Assert.That(
            () =>
                new CharacterItemInstanceValidator(
                    null,
                    affixCatalog),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void ValidateAndResolve_InvalidInstanceThrows()
    {
        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(null);

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                itemCatalog);

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    default),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void ValidateAndResolve_ValidMagicItemReturnsDefinition()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                1);

        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(
                new[]
                {
                definition
                });

        EquipmentAffixDefinitionCatalog affixCatalog =
            CreateAffixCatalog();

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                itemCatalog,
                affixCatalog);

        CharacterItemInstance instance =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                1,
                0,
                1,
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

        ItemDefinition result =
            validator.ValidateAndResolve(
                instance);

        Assert.That(
            result,
            Is.SameAs(definition));
    }

    [Test]
    public void ValidateAndResolve_InvalidAffixCountThrows()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                1);

        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(
                new[]
                {
                definition
                });

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                itemCatalog,
                CreateAffixCatalog());

        CharacterItemInstance instance =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                1,
                0,
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                CreateAffix(
                    "strength_flat",
                    FightStatType.Strength,
                    3)
                });

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    instance),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void ValidateAndResolve_InvalidAffixValueThrows()
    {
        definition =
            CreateDefinition(
                "iron_sword",
                1);

        ItemDefinitionCatalog itemCatalog =
            new ItemDefinitionCatalog(
                new[]
                {
                definition
                });

        CharacterItemInstanceValidator validator =
            new CharacterItemInstanceValidator(
                itemCatalog,
                CreateAffixCatalog());

        CharacterItemInstance instance =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "item_instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                1,
                0,
                1,
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

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    instance),
            Throws.TypeOf<InvalidOperationException>());
    }

    private static EquipmentAffixDefinitionCatalog
    CreateAffixCatalog()
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

    private static CharacterItemInstance
    CreateCompleteMagicInstance(
        string baseTypeId)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "item_instance_001"),
            new CharacterItemId(
                "iron_sword"),
            new EquipmentBaseTypeId(
                baseTypeId),
            1,
            0,
            1,
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
    }

    private static EquipmentBaseTypeDefinition
    CreateBaseTypeDefinition(
        string baseTypeId)
    {
        EquipmentBaseTypeDefinition result =
            ScriptableObject.CreateInstance<
                EquipmentBaseTypeDefinition>();

        result.InitializeForTests(
            baseTypeId,
            default,
            default);

        return result;
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

    private static ItemDefinition CreateDefinition(
        string itemId,
        int maxStackSize)
    {
        ItemDefinition result =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        result.InitializeForTests(
            itemId,
            EquipmentSlotType.Accessory,
            maxStackSize);

        return result;
    }

    private static CharacterItemInstance CreateInstance(
        string itemId,
        int quantity)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "item_instance_001"),
            new CharacterItemId(itemId),
            1,
            0,
            quantity);
    }
}