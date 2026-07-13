using DiceBossArena.Tests.Fixtures;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightTestBuilderTests
    {
        [Test]
        public void Build_CreatesMinimalFight()
        {
            using FightTestContext fight =
                new FightTestBuilder().Build();

            Assert.That(fight.Player, Is.Not.Null);
            Assert.That(fight.Enemy, Is.Not.Null);
            Assert.That(fight.PlayerSkill, Is.Not.Null);

            Assert.That(
                fight.Player.Team,
                Is.EqualTo(FightTeam.Player));

            Assert.That(
                fight.Enemy.Team,
                Is.EqualTo(FightTeam.Enemy));

            Assert.That(fight.Player.IsAlive, Is.True);
            Assert.That(fight.Enemy.IsAlive, Is.True);

            Assert.That(
                fight.PlayerSkill.TargetType,
                Is.EqualTo(SkillTargetType.SingleEnemy));
        }
    }
}