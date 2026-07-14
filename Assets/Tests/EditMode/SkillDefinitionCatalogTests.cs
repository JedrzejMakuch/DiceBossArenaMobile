using NUnit.Framework;
using UnityEngine;

public class SkillDefinitionCatalogTests
{
    [Test]
    public void TryResolve_ReturnsDefinitionBySkillId()
    {
        SkillDefinition definition =
            CreateDefinition(
                "basic_attack");

        SkillDefinitionCatalog catalog =
            new SkillDefinitionCatalog(
                new[]
                {
                    definition
                });

        bool resolved =
            catalog.TryResolve(
                "basic_attack",
                out SkillDefinition result);

        Assert.That(
            resolved,
            Is.True);

        Assert.That(
            result,
            Is.SameAs(definition));

        Object.DestroyImmediate(definition);
    }

    [Test]
    public void TryResolve_TrimsRequestedSkillId()
    {
        SkillDefinition definition =
            CreateDefinition(
                "basic_attack");

        SkillDefinitionCatalog catalog =
            new SkillDefinitionCatalog(
                new[]
                {
                    definition
                });

        Assert.That(
            catalog.TryResolve(
                "  basic_attack  ",
                out SkillDefinition result),
            Is.True);

        Assert.That(
            result,
            Is.SameAs(definition));

        Object.DestroyImmediate(definition);
    }

    [Test]
    public void TryResolve_UnknownIdReturnsFalse()
    {
        SkillDefinition definition =
            CreateDefinition(
                "basic_attack");

        SkillDefinitionCatalog catalog =
            new SkillDefinitionCatalog(
                new[]
                {
                    definition
                });

        Assert.That(
            catalog.TryResolve(
                "missing_skill",
                out SkillDefinition result),
            Is.False);

        Assert.That(
            result,
            Is.Null);

        Object.DestroyImmediate(definition);
    }

    [Test]
    public void Constructor_DuplicateSkillIdThrowsException()
    {
        SkillDefinition first =
            CreateDefinition(
                "basic_attack");

        SkillDefinition second =
            CreateDefinition(
                "basic_attack");

        Assert.That(
            () =>
                new SkillDefinitionCatalog(
                    new[]
                    {
                        first,
                        second
                    }),
            Throws.ArgumentException);

        Object.DestroyImmediate(first);
        Object.DestroyImmediate(second);
    }

    [Test]
    public void Constructor_InvalidSkillIdThrowsException()
    {
        SkillDefinition definition =
            CreateDefinition(
                "   ");

        Assert.That(
            () =>
                new SkillDefinitionCatalog(
                    new[]
                    {
                        definition
                    }),
            Throws.ArgumentException);

        Object.DestroyImmediate(definition);
    }

    [Test]
    public void Constructor_NullDefinitionThrowsException()
    {
        Assert.That(
            () =>
                new SkillDefinitionCatalog(
                    new SkillDefinition[]
                    {
                        null
                    }),
            Throws.ArgumentException);
    }

    [Test]
    public void NullCollectionCreatesEmptyCatalog()
    {
        SkillDefinitionCatalog catalog =
            new SkillDefinitionCatalog(null);

        Assert.That(
            catalog.TryResolve(
                "basic_attack",
                out SkillDefinition result),
            Is.False);

        Assert.That(
            result,
            Is.Null);
    }

    private static SkillDefinition CreateDefinition(
        string skillId)
    {
        SkillDefinition definition =
            ScriptableObject.CreateInstance<
                SkillDefinition>();

        definition.InitializeForTests(
            skillId);

        return definition;
    }
}