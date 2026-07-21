using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class CharacterActionSetFactoryTests
{
    [Test]
    public void Create_MapsBuildSkillsToFixedSlots()
    {
        CharacterActionSet actionSet =
            CharacterActionSetFactory.Create(
                CreateSnapshot());

        Assert.That(
            actionSet.Count,
            Is.EqualTo(6));

        Assert.That(
            actionSet[
                CharacterActionSlot.WeaponAttack]
                .ContentType,
            Is.EqualTo(
                CharacterActionContentType
                    .WeaponProfile));

        Assert.That(
            actionSet[
                CharacterActionSlot.WeaponAttack]
                .WeaponProfile,
            Is.SameAs(
                CharacterWeaponProfiles.Unarmed));

        Assert.That(
            actionSet[
                CharacterActionSlot.WeaponAttack]
                .HasWeaponProfile,
            Is.True);

        Assert.That(
            actionSet[
                CharacterActionSlot.BasicAttack]
                .Skill.SkillId,
            Is.EqualTo(
                CharacterSkillIds.BasicAttack));

        Assert.That(
            actionSet[
                CharacterActionSlot.SkillOne]
                .Skill.SkillId,
            Is.EqualTo("skill_one"));

        Assert.That(
            actionSet[
                CharacterActionSlot.SkillFour]
                .Skill.SkillId,
            Is.EqualTo("skill_four"));
    }

    [Test]
    public void Create_NullSnapshotThrowsException()
    {
        Assert.Throws<ArgumentNullException>(
            () =>
                CharacterActionSetFactory.Create(
                    null));
    }

    [Test]
    public void Create_IncompleteBuildThrowsException()
    {
        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                    CreateSkill(
                        CharacterSkillIds.BasicAttack),
                    CreateSkill("skill_one")
                },
                null);

        Assert.Throws<ArgumentException>(
            () =>
                CharacterActionSetFactory.Create(
                    snapshot));
    }

    [Test]
    public void Create_MissingBasicAttackThrowsException()
    {
        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                    CreateSkill("wrong_first_skill"),
                    CreateSkill("skill_one"),
                    CreateSkill("skill_two"),
                    CreateSkill("skill_three"),
                    CreateSkill("skill_four")
                },
                null);

        Assert.Throws<ArgumentException>(
            () =>
                CharacterActionSetFactory.Create(
                    snapshot));
    }

    [Test]
    public void Create_PreservesMainHandWeaponProfile()
    {
        RolledWeaponProfile profile =
            new RolledWeaponProfile(
                new[]
                {
                new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Fire,
                    4,
                    8)
                });

        EquipmentLoadoutSnapshot loadout =
            new EquipmentLoadoutSnapshot(
                new[]
                {
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "iron_sword"),
                    profile)
                });

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                CreateSkill(
                    CharacterSkillIds.BasicAttack),
                CreateSkill("skill_one"),
                CreateSkill("skill_two"),
                CreateSkill("skill_three"),
                CreateSkill("skill_four")
                },
                null,
                loadout);

        CharacterActionSet actionSet =
            CharacterActionSetFactory.Create(
                snapshot);

        Assert.That(
            actionSet[
                CharacterActionSlot.WeaponAttack]
                .WeaponProfile,
            Is.SameAs(profile));
    }

    private static CharacterBuildSnapshot
        CreateSnapshot()
    {
        return new CharacterBuildSnapshot(
            new CharacterClassId("warrior"),
            new CharacterSpecializationId(
                "guardian"),
            new[]
            {
                CreateSkill(
                    CharacterSkillIds.BasicAttack),
                CreateSkill("skill_one"),
                CreateSkill("skill_two"),
                CreateSkill("skill_three"),
                CreateSkill("skill_four")
            },
            null);
    }

    private static CharacterBuildSkill CreateSkill(
        string skillId)
    {
        return new CharacterBuildSkill(
            skillId,
            1);
    }
}