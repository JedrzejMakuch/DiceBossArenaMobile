using DiceBossArena.Game;
using NUnit.Framework;

public sealed class CharacterActionSlotsTests
{
    [Test]
    public void All_ContainsSixSlotsInFixedOrder()
    {
        Assert.That(
            CharacterActionSlots.Count,
            Is.EqualTo(6));

        Assert.That(
            CharacterActionSlots.All,
            Is.EqualTo(
                new[]
                {
                    CharacterActionSlot.WeaponAttack,
                    CharacterActionSlot.BasicAttack,
                    CharacterActionSlot.SkillOne,
                    CharacterActionSlot.SkillTwo,
                    CharacterActionSlot.SkillThree,
                    CharacterActionSlot.SkillFour
                }));
    }

    [Test]
    public void All_ReturnsSameReadOnlyCollection()
    {
        Assert.That(
            CharacterActionSlots.All,
            Is.SameAs(
                CharacterActionSlots.All));
    }
}