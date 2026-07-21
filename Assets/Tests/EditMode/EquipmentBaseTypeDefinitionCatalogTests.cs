using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public sealed class
    EquipmentBaseTypeDefinitionCatalogTests
{
    private EquipmentBaseTypeDefinition firstDefinition;
    private EquipmentBaseTypeDefinition secondDefinition;

    [TearDown]
    public void TearDown()
    {
        if (firstDefinition != null)
        {
            UnityEngine.Object.DestroyImmediate(
                firstDefinition);
        }

        if (secondDefinition != null)
        {
            UnityEngine.Object.DestroyImmediate(
                secondDefinition);
        }
    }

    [Test]
    public void Constructor_NullCollectionCreatesEmptyCatalog()
    {
        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                null);

        bool resolved =
            catalog.TryResolve(
                new EquipmentBaseTypeId(
                    "one_handed_sword"),
                out EquipmentBaseTypeDefinition definition);

        Assert.That(
            resolved,
            Is.False);

        Assert.That(
            definition,
            Is.Null);
    }

    [Test]
    public void Constructor_NullDefinitionThrows()
    {
        Assert.That(
            () =>
                new EquipmentBaseTypeDefinitionCatalog(
                    new EquipmentBaseTypeDefinition[]
                    {
                        null
                    }),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Constructor_InvalidDefinitionIdThrows()
    {
        firstDefinition =
            CreateDefinition(
                " ");

        Assert.That(
            () =>
                new EquipmentBaseTypeDefinitionCatalog(
                    new[]
                    {
                        firstDefinition
                    }),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void Constructor_DuplicateIdThrows()
    {
        firstDefinition =
            CreateDefinition(
                "one_handed_sword");

        secondDefinition =
            CreateDefinition(
                "one_handed_sword");

        Assert.That(
            () =>
                new EquipmentBaseTypeDefinitionCatalog(
                    new[]
                    {
                        firstDefinition,
                        secondDefinition
                    }),
            Throws.TypeOf<
                InvalidOperationException>());
    }

    [Test]
    public void TryResolve_KnownIdReturnsDefinition()
    {
        firstDefinition =
            CreateDefinition(
                "one_handed_sword");

        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                new[]
                {
                    firstDefinition
                });

        bool resolved =
            catalog.TryResolve(
                new EquipmentBaseTypeId(
                    "one_handed_sword"),
                out EquipmentBaseTypeDefinition definition);

        Assert.That(
            resolved,
            Is.True);

        Assert.That(
            definition,
            Is.SameAs(firstDefinition));
    }

    [Test]
    public void TryResolve_UnknownIdReturnsFalse()
    {
        firstDefinition =
            CreateDefinition(
                "one_handed_sword");

        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                new[]
                {
                    firstDefinition
                });

        bool resolved =
            catalog.TryResolve(
                new EquipmentBaseTypeId(
                    "unknown_type"),
                out EquipmentBaseTypeDefinition definition);

        Assert.That(
            resolved,
            Is.False);

        Assert.That(
            definition,
            Is.Null);
    }

    [Test]
    public void TryResolve_InvalidIdReturnsFalse()
    {
        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                null);

        bool resolved =
            catalog.TryResolve(
                default,
                out EquipmentBaseTypeDefinition definition);

        Assert.That(
            resolved,
            Is.False);

        Assert.That(
            definition,
            Is.Null);
    }

    [Test]
    public void Get_KnownIdReturnsDefinition()
    {
        firstDefinition =
            CreateDefinition(
                "one_handed_sword");

        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                new[]
                {
                    firstDefinition
                });

        EquipmentBaseTypeDefinition result =
            catalog.Get(
                new EquipmentBaseTypeId(
                    "one_handed_sword"));

        Assert.That(
            result,
            Is.SameAs(firstDefinition));
    }

    [Test]
    public void Get_UnknownIdThrows()
    {
        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                null);

        Assert.That(
            () =>
                catalog.Get(
                    new EquipmentBaseTypeId(
                        "unknown_type")),
            Throws.TypeOf<
                KeyNotFoundException>());
    }

    [Test]
    public void Get_InvalidIdThrows()
    {
        EquipmentBaseTypeDefinitionCatalog catalog =
            new EquipmentBaseTypeDefinitionCatalog(
                null);

        Assert.That(
            () => catalog.Get(default),
            Throws.TypeOf<ArgumentException>());
    }

    private EquipmentBaseTypeDefinition
        CreateDefinition(
            string baseTypeId)
    {
        EquipmentBaseTypeDefinition definition =
            ScriptableObject.CreateInstance<
                EquipmentBaseTypeDefinition>();

        definition.InitializeForTests(
            baseTypeId,
            EquipmentSlotType.MainHand,
            EquipmentBaseTypeCategory.Sword);

        return definition;
    }
}