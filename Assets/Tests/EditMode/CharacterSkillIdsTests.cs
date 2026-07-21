using DiceBossArena.Game;
using NUnit.Framework;

public sealed class CharacterSkillIdsTests
{
    [Test]
    public void BasicAttack_HasExpectedId()
    {
        Assert.That(
            CharacterSkillIds.BasicAttack,
            Is.EqualTo("basic_attack"));
    }
}