using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    FightWeaponAttackProfileResolverTests
{
    [Test]
    public void TryResolve_ReturnsWeaponAttackProfile()
    {
        RolledWeaponProfile expectedProfile =
            new RolledWeaponProfile(
                new[]
                {
                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId("test_line"),
                        WeaponAttackElement.Neutral,
                        3,
                        7)
                });

        CharacterActionSet actionSet =
            CreateActionSet(expectedProfile);

        bool result =
            FightWeaponAttackProfileResolver.TryResolve(
                actionSet,
                out RolledWeaponProfile actualProfile);

        Assert.That(result, Is.True);
        Assert.That(
            actualProfile,
            Is.SameAs(expectedProfile));
    }

    [Test]
    public void TryResolve_NullActionSetReturnsFalse()
    {
        bool result =
            FightWeaponAttackProfileResolver.TryResolve(
                null,
                out RolledWeaponProfile weaponProfile);

        Assert.That(result, Is.False);
        Assert.That(weaponProfile, Is.Null);
    }

    [Test]
    public void TryResolve_MissingWeaponProfileReturnsFalse()
    {
        CharacterActionSet actionSet =
            CreateActionSet(null);

        bool result =
            FightWeaponAttackProfileResolver.TryResolve(
                actionSet,
                out RolledWeaponProfile weaponProfile);

        Assert.That(result, Is.False);
        Assert.That(weaponProfile, Is.Null);
    }

    private static CharacterActionSet CreateActionSet(
        RolledWeaponProfile weaponProfile)
    {
        return new CharacterActionSet(
            new List<CharacterActionContent>
            {
                CharacterActionContent
                    .CreateWeaponAttack(
                        weaponProfile),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.BasicAttack,
                    CreateSkill("basic_attack")),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillOne,
                    CreateSkill("skill_one")),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillTwo,
                    CreateSkill("skill_two")),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillThree,
                    CreateSkill("skill_three")),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillFour,
                    CreateSkill("skill_four"))
            });
    }

    private static CharacterBuildSkill CreateSkill(
        string skillId)
    {
        return new CharacterBuildSkill(
            skillId,
            1);
    }
}