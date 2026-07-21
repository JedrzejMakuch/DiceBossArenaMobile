using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public sealed class
    CharacterItemInstanceBaseTypeDefinitionValidatorTests
{
    private EquipmentBaseTypeDefinition definition;

    [TearDown]
    public void TearDown()
    {
        if (definition != null)
        {
            UnityEngine.Object.DestroyImmediate(
                definition);
        }
    }

    [Test]
    public void Constructor_NullResolverThrows()
    {
        Assert.That(
            () =>
                new CharacterItemInstanceBaseTypeDefinitionValidator(
                    null),
            Throws.TypeOf<ArgumentNullException>());
    }

    [Test]
    public void ValidateAndResolve_InvalidItemThrows()
    {
        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                null);

        CharacterItemInstanceBaseTypeDefinitionValidator validator =
            new CharacterItemInstanceBaseTypeDefinitionValidator(
                catalog);

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    default),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void ValidateAndResolve_MissingBaseTypeThrows()
    {
        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                null);

        CharacterItemInstanceBaseTypeDefinitionValidator validator =
            new CharacterItemInstanceBaseTypeDefinitionValidator(
                catalog);

        CharacterItemInstance item =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                1,
                0,
                1);

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void ValidateAndResolve_UnknownBaseTypeThrows()
    {
        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                null);

        CharacterItemInstanceBaseTypeDefinitionValidator validator =
            new CharacterItemInstanceBaseTypeDefinitionValidator(
                catalog);

        CharacterItemInstance item =
            CreateItem(
                "unknown_type");

        Assert.That(
            () =>
                validator.ValidateAndResolve(
                    item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void ValidateAndResolve_KnownBaseTypeReturnsDefinition()
    {
        definition =
            CreateDefinition(
                "one_handed_sword");

        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                new[]
                {
                    definition
                });

        CharacterItemInstanceBaseTypeDefinitionValidator validator =
            new CharacterItemInstanceBaseTypeDefinitionValidator(
                catalog);

        CharacterItemInstance item =
            CreateItem(
                "one_handed_sword");

        EquipmentBaseTypeDefinition result =
            validator.ValidateAndResolve(
                item);

        Assert.That(
            result,
            Is.SameAs(definition));
    }

    private static CharacterItemInstance CreateItem(
        string baseTypeId)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "instance_001"),
            new CharacterItemId(
                "iron_sword"),
            new EquipmentBaseTypeId(
                baseTypeId),
            1,
            0,
            1,
            EquipmentItemRarity.Common,
            Array.Empty<RolledEquipmentAffix>());
    }

    private EquipmentBaseTypeDefinition CreateDefinition(
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
}