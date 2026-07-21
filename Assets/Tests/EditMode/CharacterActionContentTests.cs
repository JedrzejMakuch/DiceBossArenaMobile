using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class CharacterActionContentTests
{
    [Test]
    public void CreateWeaponAttack_CreatesWeaponContent()
    {
        CharacterActionContent content =
            CharacterActionContent
                .CreateWeaponAttack();

        Assert.That(
            content.Slot,
            Is.EqualTo(
                CharacterActionSlot.WeaponAttack));

        Assert.That(
            content.ContentType,
            Is.EqualTo(
                CharacterActionContentType
                    .WeaponProfile));

        Assert.That(
            content.HasSkill,
            Is.False);
    }

    [Test]
    public void CreateSkill_CreatesSkillContent()
    {
        CharacterBuildSkill skill =
            new CharacterBuildSkill(
                "basic_attack",
                2);

        CharacterActionContent content =
            CharacterActionContent.CreateSkill(
                CharacterActionSlot.BasicAttack,
                skill);

        Assert.That(
            content.Slot,
            Is.EqualTo(
                CharacterActionSlot.BasicAttack));

        Assert.That(
            content.ContentType,
            Is.EqualTo(
                CharacterActionContentType.Skill));

        Assert.That(
            content.HasSkill,
            Is.True);

        Assert.That(
            content.Skill,
            Is.EqualTo(skill));
    }

    [Test]
    public void CreateSkill_WeaponAttackSlotThrowsException()
    {
        CharacterBuildSkill skill =
            new CharacterBuildSkill(
                "basic_attack",
                1);

        Assert.Throws<ArgumentException>(
            () =>
                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.WeaponAttack,
                    skill));
    }

    [Test]
    public void CreateSkill_InvalidSkillThrowsException()
    {
        Assert.Throws<ArgumentException>(
            () =>
                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillOne,
                    default));
    }

    [Test]
    public void DefaultContent_IsInvalid()
    {
        CharacterActionContent content =
            default;

        Assert.That(
            content.IsValid,
            Is.False);

        Assert.That(
            content.ContentType,
            Is.EqualTo(
                CharacterActionContentType.None));
    }

    [Test]
    public void CreateWeaponAttack_CreatesValidContent()
    {
        CharacterActionContent content =
            CharacterActionContent
                .CreateWeaponAttack();

        Assert.That(
            content.IsValid,
            Is.True);
    }

    [Test]
    public void CreateSkill_CreatesValidContent()
    {
        CharacterActionContent content =
            CharacterActionContent.CreateSkill(
                CharacterActionSlot.SkillOne,
                new CharacterBuildSkill(
                    "test_skill",
                    1));

        Assert.That(
            content.IsValid,
            Is.True);
    }
}