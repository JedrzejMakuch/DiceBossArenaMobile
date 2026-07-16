using DiceBossArena.Assets._Project.Scripts.Characters.Equipment.Definitions;
using DiceBossArena.Game;
using NUnit.Framework;
using System;
using UnityEngine;

public class CharacterItemInstanceValidatorTests
{
    private ItemDefinition definition;

    [TearDown]
    public void TearDown()
    {
        if (definition != null)
        {
            UnityEngine.Object.DestroyImmediate(definition);
        }
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