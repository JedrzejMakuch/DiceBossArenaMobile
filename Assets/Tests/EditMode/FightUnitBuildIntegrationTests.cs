using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class FightUnitBuildIntegrationTests
{
    [Test]
    public void ApplyBuild_InitializesSkillsAndStats()
    {
        SkillDefinition skillDefinition =
            CreateSkillDefinition(
                "shield_bash",
                3);

        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        FightUnitSkills unitSkills =
            unitObject.AddComponent<FightUnitSkills>();

        unit.Initialize(
            newUnitName: "Guardian",
            newTeam: FightTeam.Player,
            newMaxHealth: 10,
            newAttackPower: 4,
            newInitiative: 5);

        ResolvedCharacterBuild build =
            new ResolvedCharacterBuild(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                    new UnitStartingSkill(
                        skillDefinition,
                        2)
                },
                new[]
                {
                    new FightStatModifier(
                        FightStatType.MaxHealth,
                        FightStatModifierType.Flat,
                        10),
                    new FightStatModifier(
                        FightStatType.AttackPower,
                        FightStatModifierType.Flat,
                        3)
                },
                null,
                null);

        Assert.That(
            unit.ApplyBuild(build),
            Is.True);

        Assert.That(
            unit.ClassId.Value,
            Is.EqualTo("warrior"));

        Assert.That(
            unit.SpecializationId.Value,
            Is.EqualTo("guardian"));

        Assert.That(
            unit.MaxHealth,
            Is.EqualTo(20));

        Assert.That(
            unit.AttackPower,
            Is.EqualTo(7));

        Assert.That(
            unitSkills.Skills.Count,
            Is.EqualTo(1));

        Assert.That(
            unitSkills.Skills[0].Definition,
            Is.SameAs(skillDefinition));

        Assert.That(
            unitSkills.Skills[0].Level,
            Is.EqualTo(2));

        Object.DestroyImmediate(unitObject);
        Object.DestroyImmediate(skillDefinition);
    }

    [Test]
    public void ApplyRuntimeSnapshot_RestoresCurrentHealth()
    {
        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        unit.Initialize(
            newUnitName: "Unit",
            newTeam: FightTeam.Player,
            newMaxHealth: 20,
            newAttackPower: 4,
            newInitiative: 5);

        bool applied =
            unit.ApplyRuntimeSnapshot(
                new FightUnitRuntimeSnapshot(7));

        Assert.That(
            applied,
            Is.True);

        Assert.That(
            unit.CurrentHealth,
            Is.EqualTo(7));

        Object.DestroyImmediate(unitObject);
    }

    [Test]
    public void ApplyRuntimeSnapshot_FreshKeepsFullHealth()
    {
        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        unit.Initialize(
            newUnitName: "Unit",
            newTeam: FightTeam.Player,
            newMaxHealth: 20,
            newAttackPower: 4,
            newInitiative: 5);

        Assert.That(
            unit.ApplyRuntimeSnapshot(
                FightUnitRuntimeSnapshot.Fresh),
            Is.True);

        Assert.That(
            unit.CurrentHealth,
            Is.EqualTo(20));

        Object.DestroyImmediate(unitObject);
    }

    [Test]
    public void ApplyRuntimeSnapshot_ClampsHealthToFinalMaximum()
    {
        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        unit.Initialize(
            newUnitName: "Unit",
            newTeam: FightTeam.Player,
            newMaxHealth: 20,
            newAttackPower: 4,
            newInitiative: 5);

        ResolvedCharacterBuild build =
            new ResolvedCharacterBuild(
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
                },
                null,
                null);

        unit.ApplyBuild(build);

        Assert.That(
            unit.MaxHealth,
            Is.EqualTo(30));

        unit.ApplyRuntimeSnapshot(
            new FightUnitRuntimeSnapshot(50));

        Assert.That(
            unit.CurrentHealth,
            Is.EqualTo(30));

        Object.DestroyImmediate(unitObject);
    }

    [Test]
    public void ApplyRuntimeSnapshot_NullReturnsFalse()
    {
        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        unit.Initialize(
            newUnitName: "Unit",
            newTeam: FightTeam.Player,
            newMaxHealth: 20,
            newAttackPower: 4,
            newInitiative: 5);

        Assert.That(
            unit.ApplyRuntimeSnapshot(null),
            Is.False);

        Object.DestroyImmediate(unitObject);
    }

    [Test]
    public void ApplyBuild_CopiesPassiveCollection()
    {
        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        unit.Initialize(
            newUnitName: "Guardian",
            newTeam: FightTeam.Player,
            newMaxHealth: 10,
            newAttackPower: 4,
            newInitiative: 5);

        CharacterPassiveId[] sourcePassives =
        {
            new CharacterPassiveId(
                "shield_mastery")
        };

        ResolvedCharacterBuild build =
            new ResolvedCharacterBuild(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                null,
                null,
                sourcePassives);

        Assert.That(
            unit.ApplyBuild(build),
            Is.True);

        sourcePassives[0] =
            new CharacterPassiveId(
                "changed_passive");

        Assert.That(
            unit.PassiveIds.Count,
            Is.EqualTo(1));

        Assert.That(
            unit.PassiveIds[0].Value,
            Is.EqualTo(
                "shield_mastery"));

        Object.DestroyImmediate(unitObject);
    }

    [Test]
    public void ApplyBuild_PreservesEquipment()
    {
        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        unit.Initialize(
            newUnitName: "Guardian",
            newTeam: FightTeam.Player,
            newMaxHealth: 10,
            newAttackPower: 4,
            newInitiative: 5);

        EquipmentLoadoutSnapshot loadout =
            new EquipmentLoadoutSnapshot(
                new[]
                {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.MainHand,
                        new CharacterItemId(
                            "iron_sword"))
                });

        ResolvedCharacterBuild build =
            new ResolvedCharacterBuild(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                null,
                loadout,
                null);

        Assert.That(
            unit.ApplyBuild(build),
            Is.True);

        Assert.That(
            unit.EquipmentLoadout,
            Is.EqualTo(loadout));

        Object.DestroyImmediate(unitObject);
    }

    [Test]
    public void ApplyBuild_NullBuildReturnsFalse()
    {
        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        unit.Initialize(
            newUnitName: "Unit",
            newTeam: FightTeam.Player,
            newMaxHealth: 10,
            newAttackPower: 4,
            newInitiative: 5);

        Assert.That(
            unit.ApplyBuild(null),
            Is.False);

        Object.DestroyImmediate(unitObject);
    }

    [Test]
    public void ApplyBuild_DoesNotModifyResolvedBuild()
    {
        SkillDefinition skillDefinition =
            CreateSkillDefinition(
                "shield_bash",
                3);

        ResolvedCharacterBuild build =
            new ResolvedCharacterBuild(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                    new UnitStartingSkill(
                        skillDefinition,
                        2)
                },
                new[]
                {
                    new FightStatModifier(
                        FightStatType.AttackPower,
                        FightStatModifierType.Flat,
                        3)
                },
                null,
                null);

        GameObject unitObject =
            new GameObject("Unit");

        FightUnit unit =
            unitObject.AddComponent<FightUnit>();

        unitObject.AddComponent<FightUnitSkills>();

        unit.Initialize(
            newUnitName: "Unit",
            newTeam: FightTeam.Player,
            newMaxHealth: 10,
            newAttackPower: 4,
            newInitiative: 5);

        unit.ApplyBuild(build);

        Assert.That(
            build.Skills.Count,
            Is.EqualTo(1));

        Assert.That(
            build.Skills[0].Definition,
            Is.SameAs(skillDefinition));

        Assert.That(
            build.Skills[0].Level,
            Is.EqualTo(2));

        Assert.That(
            build.StatModifiers.Count,
            Is.EqualTo(1));

        Object.DestroyImmediate(unitObject);
        Object.DestroyImmediate(skillDefinition);
    }

    private static SkillDefinition CreateSkillDefinition(
        string skillId,
        int maxLevel)
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

    [Test]
    public void UnitsUsingSameResolvedBuild_HaveIndependentRuntimeState()
    {
        SkillDefinition skillDefinition =
            CreateSkillDefinition(
                "shield_bash",
                3);

        ResolvedCharacterBuild build =
            new ResolvedCharacterBuild(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                new UnitStartingSkill(
                    skillDefinition,
                    2)
                },
                new[]
                {
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    3)
                },
                null,
                null);

        GameObject firstObject =
            new GameObject("First Unit");

        GameObject secondObject =
            new GameObject("Second Unit");

        FightUnit firstUnit =
            firstObject.AddComponent<FightUnit>();

        FightUnit secondUnit =
            secondObject.AddComponent<FightUnit>();

        FightUnitSkills firstSkills =
            firstObject.AddComponent<FightUnitSkills>();

        FightUnitSkills secondSkills =
            secondObject.AddComponent<FightUnitSkills>();

        firstUnit.Initialize(
            newUnitName: "First",
            newTeam: FightTeam.Player,
            newMaxHealth: 10,
            newAttackPower: 4,
            newInitiative: 5);

        secondUnit.Initialize(
            newUnitName: "Second",
            newTeam: FightTeam.Player,
            newMaxHealth: 10,
            newAttackPower: 4,
            newInitiative: 5);

        firstUnit.ApplyBuild(build);
        secondUnit.ApplyBuild(build);

        Assert.That(
            firstUnit.AttackPower,
            Is.EqualTo(7));

        Assert.That(
            secondUnit.AttackPower,
            Is.EqualTo(7));

        Assert.That(
            ReferenceEquals(
                firstUnit.Stats,
                secondUnit.Stats),
            Is.False);

        Assert.That(
            ReferenceEquals(
                firstSkills.Skills,
                secondSkills.Skills),
            Is.False);

        Assert.That(
            ReferenceEquals(
                firstSkills.Skills[0],
                secondSkills.Skills[0]),
            Is.False);

        firstUnit.Stats.AddModifier(
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                5));

        Assert.That(
            firstUnit.AttackPower,
            Is.EqualTo(12));

        Assert.That(
            secondUnit.AttackPower,
            Is.EqualTo(7));

        Assert.That(
            firstSkills.Skills[0]
                .TryStartCooldown(),
            Is.True);

        Assert.That(
            firstSkills.Skills[0]
                .CurrentCooldown,
            Is.GreaterThanOrEqualTo(0));

        Assert.That(
            secondSkills.Skills[0]
                .CurrentCooldown,
            Is.EqualTo(0));

        Object.DestroyImmediate(firstObject);
        Object.DestroyImmediate(secondObject);
        Object.DestroyImmediate(skillDefinition);
    }
}