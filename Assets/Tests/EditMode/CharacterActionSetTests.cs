using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class CharacterActionSetTests
{
    [Test]
    public void Constructor_CreatesCompleteActionSet()
    {
        CharacterActionSet actionSet =
            new CharacterActionSet(
                CreateValidContents());

        Assert.That(
            actionSet.Count,
            Is.EqualTo(6));

        Assert.That(
            actionSet.Contents[0].Slot,
            Is.EqualTo(
                CharacterActionSlot.WeaponAttack));

        Assert.That(
            actionSet.Contents[5].Slot,
            Is.EqualTo(
                CharacterActionSlot.SkillFour));
    }

    [Test]
    public void Indexer_ReturnsContentForRequestedSlot()
    {
        CharacterActionSet actionSet =
            new CharacterActionSet(
                CreateValidContents());

        CharacterActionContent content =
            actionSet[
                CharacterActionSlot.SkillTwo];

        Assert.That(
            content.Skill.SkillId,
            Is.EqualTo("skill_two"));
    }

    [Test]
    public void Constructor_CopiesSourceCollection()
    {
        List<CharacterActionContent> source =
            CreateValidContents();

        CharacterActionSet actionSet =
            new CharacterActionSet(source);

        source.Clear();

        Assert.That(
            actionSet.Count,
            Is.EqualTo(6));
    }

    [Test]
    public void Constructor_NullCollectionThrowsException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new CharacterActionSet(null));
    }

    [Test]
    public void Constructor_IncompleteCollectionThrowsException()
    {
        List<CharacterActionContent> source =
            CreateValidContents();

        source.RemoveAt(source.Count - 1);

        Assert.Throws<ArgumentException>(
            () => new CharacterActionSet(source));
    }

    [Test]
    public void Constructor_WrongSlotOrderThrowsException()
    {
        List<CharacterActionContent> source =
            CreateValidContents();

        CharacterActionContent temporary =
            source[2];

        source[2] = source[3];
        source[3] = temporary;

        Assert.Throws<ArgumentException>(
            () => new CharacterActionSet(source));
    }

    [Test]
    public void Constructor_InvalidContentThrowsException()
    {
        List<CharacterActionContent> source =
            CreateValidContents();

        source[2] = default;

        Assert.Throws<ArgumentException>(
            () => new CharacterActionSet(source));
    }

    private static List<CharacterActionContent>
        CreateValidContents()
    {
        return new List<CharacterActionContent>
        {
            CharacterActionContent
                .CreateWeaponAttack(),

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
        };
    }

    private static CharacterBuildSkill CreateSkill(
        string skillId)
    {
        return new CharacterBuildSkill(
            skillId,
            1);
    }
}