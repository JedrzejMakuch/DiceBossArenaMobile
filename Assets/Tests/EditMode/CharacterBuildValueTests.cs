using DiceBossArena.Game;
using NUnit.Framework;

public class CharacterBuildValueTests
{
    [Test]
    public void ClassId_TrimsValue()
    {
        CharacterClassId id =
            new CharacterClassId(
                "  warrior  ");

        Assert.That(
            id.Value,
            Is.EqualTo("warrior"));

        Assert.That(
            id.IsValid,
            Is.True);
    }

    [Test]
    public void SpecializationId_EmptyValueIsInvalid()
    {
        CharacterSpecializationId id =
            new CharacterSpecializationId(
                "   ");

        Assert.That(
            id.Value,
            Is.Empty);

        Assert.That(
            id.IsValid,
            Is.False);
    }

    [Test]
    public void BuildSkill_ClampsLevelToOne()
    {
        CharacterBuildSkill skill =
            new CharacterBuildSkill(
                "basic_attack",
                -5);

        Assert.That(
            skill.Level,
            Is.EqualTo(1));
    }

    [Test]
    public void BuildSkill_TrimsSkillId()
    {
        CharacterBuildSkill skill =
            new CharacterBuildSkill(
                "  basic_attack  ",
                2);

        Assert.That(
            skill.SkillId,
            Is.EqualTo("basic_attack"));

        Assert.That(
            skill.IsValid,
            Is.True);
    }

    [Test]
    public void EqualClassIds_HaveEqualValuesAndHashCodes()
    {
        CharacterClassId first =
            new CharacterClassId("warrior");

        CharacterClassId second =
            new CharacterClassId("warrior");

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(
                second.GetHashCode()));
    }

    [Test]
    public void EqualBuildSkills_HaveEqualValuesAndHashCodes()
    {
        CharacterBuildSkill first =
            new CharacterBuildSkill(
                "basic_attack",
                2);

        CharacterBuildSkill second =
            new CharacterBuildSkill(
                "basic_attack",
                2);

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(
                second.GetHashCode()));
    }

    [Test]
    public void EqualStatModifiers_HaveEqualValuesAndHashCodes()
    {
        FightStatModifier first =
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Percent,
                20);

        FightStatModifier second =
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Percent,
                20);

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(second.GetHashCode()));

        Assert.That(
            first == second,
            Is.True);
    }

    [Test]
    public void ItemId_TrimsValue()
    {
        CharacterItemId id =
            new CharacterItemId(
                "  iron_sword  ");

        Assert.That(
            id.Value,
            Is.EqualTo("iron_sword"));

        Assert.That(
            id.IsValid,
            Is.True);
    }

    [Test]
    public void PassiveId_EmptyValueIsInvalid()
    {
        CharacterPassiveId id =
            new CharacterPassiveId("   ");

        Assert.That(
            id.IsValid,
            Is.False);
    }
}