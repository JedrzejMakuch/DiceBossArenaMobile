using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class CharacterBuildResolverTests
{
    [Test]
    public void Resolve_ConvertsSkillIdsToDefinitions()
    {
        SkillDefinition basicAttack =
            CreateDefinition(
                "basic_attack",
                3);

        SkillDefinition shieldBash =
            CreateDefinition(
                "shield_bash",
                5);

        SkillDefinitionCatalog catalog =
            new SkillDefinitionCatalog(
                new[]
                {
                    basicAttack,
                    shieldBash
                });

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                    new CharacterBuildSkill(
                        "basic_attack",
                        2),
                    new CharacterBuildSkill(
                        "shield_bash",
                        4)
                },
                null);

        CharacterBuildResolver resolver =
            new CharacterBuildResolver(catalog);

        ResolvedCharacterBuild result =
            resolver.Resolve(snapshot);

        Assert.That(
            result.Skills.Count,
            Is.EqualTo(2));

        Assert.That(
            result.Skills[0].Definition,
            Is.SameAs(basicAttack));

        Assert.That(
            result.Skills[0].Level,
            Is.EqualTo(2));

        Assert.That(
            result.Skills[1].Definition,
            Is.SameAs(shieldBash));

        Assert.That(
            result.Skills[1].Level,
            Is.EqualTo(4));

        Object.DestroyImmediate(basicAttack);
        Object.DestroyImmediate(shieldBash);
    }

    [Test]
    public void Resolve_PreservesSkillOrder()
    {
        SkillDefinition first =
            CreateDefinition("first_skill");

        SkillDefinition second =
            CreateDefinition("second_skill");

        CharacterBuildResolver resolver =
            new CharacterBuildResolver(
                new SkillDefinitionCatalog(
                    new[]
                    {
                        first,
                        second
                    }));

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                CharacterBuildSnapshot.Empty.ClassId,
                CharacterBuildSnapshot.Empty
                    .SpecializationId,
                new[]
                {
                    new CharacterBuildSkill(
                        "second_skill",
                        1),
                    new CharacterBuildSkill(
                        "first_skill",
                        1)
                },
                null);

        ResolvedCharacterBuild result =
            resolver.Resolve(snapshot);

        Assert.That(
            result.Skills[0].Definition,
            Is.SameAs(second));

        Assert.That(
            result.Skills[1].Definition,
            Is.SameAs(first));

        Object.DestroyImmediate(first);
        Object.DestroyImmediate(second);
    }

    [Test]
    public void Resolve_CopiesStatModifiers()
    {
        SkillDefinition definition =
            CreateDefinition("basic_attack");

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                new[]
                {
                    new FightStatModifier(
                        FightStatType.MaxHealth,
                        FightStatModifierType.Flat,
                        10)
                });

        CharacterBuildResolver resolver =
            new CharacterBuildResolver(
                new SkillDefinitionCatalog(
                    new[]
                    {
                        definition
                    }));

        ResolvedCharacterBuild result =
            resolver.Resolve(snapshot);

        Assert.That(
            result.StatModifiers.Count,
            Is.EqualTo(1));

        Assert.That(
            result.StatModifiers[0],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10)));

        Assert.That(
            ReferenceEquals(
                snapshot.StatModifiers,
                result.StatModifiers),
            Is.False);

        Object.DestroyImmediate(definition);
    }

    [Test]
    public void Resolve_PreservesEquipmentAndPassives()
    {
        SkillDefinition definition =
            CreateDefinition("basic_attack");

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                null,
                new EquipmentLoadoutSnapshot(
                    new[]
                    {
                        new EquippedItemSnapshot(
                            EquipmentSlotType.Weapon,
                            new CharacterItemId(
                                "iron_sword"))
                    }),
                new[]
                {
                    new CharacterPassiveId(
                        "shield_mastery")
                });

        CharacterBuildResolver resolver =
            new CharacterBuildResolver(
                new SkillDefinitionCatalog(
                    new[]
                    {
                        definition
                    }));

        ResolvedCharacterBuild result =
            resolver.Resolve(snapshot);

        Assert.That(
            result.EquipmentLoadout,
            Is.EqualTo(
                snapshot.EquipmentLoadout));

        Assert.That(
            result.PassiveIds.Count,
            Is.EqualTo(1));

        Assert.That(
            result.PassiveIds[0],
            Is.EqualTo(
                new CharacterPassiveId(
                    "shield_mastery")));

        Object.DestroyImmediate(definition);
    }

    [Test]
    public void Resolve_UnknownSkillThrowsException()
    {
        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                    new CharacterBuildSkill(
                        "missing_skill",
                        1)
                },
                null);

        CharacterBuildResolver resolver =
            new CharacterBuildResolver(
                new SkillDefinitionCatalog(null));

        Assert.That(
            () => resolver.Resolve(snapshot),
            Throws.InvalidOperationException);
    }

    [Test]
    public void Resolve_NullSnapshotThrowsException()
    {
        CharacterBuildResolver resolver =
            new CharacterBuildResolver(
                new SkillDefinitionCatalog(null));

        Assert.That(
            () => resolver.Resolve(null),
            Throws.ArgumentNullException);
    }

    [Test]
    public void Constructor_NullResolverThrowsException()
    {
        Assert.That(
            () =>
                new CharacterBuildResolver(null),
            Throws.ArgumentNullException);
    }

    [Test]
    public void ResolvingSameSnapshotTwiceCreatesIndependentResults()
    {
        SkillDefinition definition =
            CreateDefinition("basic_attack");

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                    new CharacterBuildSkill(
                        "basic_attack",
                        2)
                },
                null);

        CharacterBuildResolver resolver =
            new CharacterBuildResolver(
                new SkillDefinitionCatalog(
                    new[]
                    {
                        definition
                    }));

        ResolvedCharacterBuild first =
            resolver.Resolve(snapshot);

        ResolvedCharacterBuild second =
            resolver.Resolve(snapshot);

        Assert.That(
            ReferenceEquals(first, second),
            Is.False);

        Assert.That(
            ReferenceEquals(
                first.Skills,
                second.Skills),
            Is.False);

        Assert.That(
            ReferenceEquals(
                first.Skills[0],
                second.Skills[0]),
            Is.False);

        Assert.That(
            first.Skills[0].Definition,
            Is.SameAs(
                second.Skills[0].Definition));

        Object.DestroyImmediate(definition);
    }

    private static SkillDefinition CreateDefinition(
        string skillId,
        int maxLevel = 1)
    {
        SkillDefinition definition =
            ScriptableObject.CreateInstance<
                SkillDefinition>();

        definition.InitializeForTests(
            skillId,
            skillId,
            maxLevel);

        return definition;
    }
}