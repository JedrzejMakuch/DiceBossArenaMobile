using DiceBossArena.Game;
using NUnit.Framework;

public sealed class CharacterActionSlotTests
{
    [Test]
    public void Slots_HaveExpectedFixedOrder()
    {
        Assert.That(
            (int)CharacterActionSlot.WeaponAttack,
            Is.EqualTo(0));

        Assert.That(
            (int)CharacterActionSlot.BasicAttack,
            Is.EqualTo(1));

        Assert.That(
            (int)CharacterActionSlot.SkillOne,
            Is.EqualTo(2));

        Assert.That(
            (int)CharacterActionSlot.SkillTwo,
            Is.EqualTo(3));

        Assert.That(
            (int)CharacterActionSlot.SkillThree,
            Is.EqualTo(4));

        Assert.That(
            (int)CharacterActionSlot.SkillFour,
            Is.EqualTo(5));
    }
}