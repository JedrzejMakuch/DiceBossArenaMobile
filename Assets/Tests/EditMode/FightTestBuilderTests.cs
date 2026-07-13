using DiceBossArena.Tests.Fixtures;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightTestBuilderTests
    {
        [Test]
        public void Build_CreatesConfiguredMinimalFight()
        {
            using FightTestContext fight =
                new FightTestBuilder()
                    .WithPlayer(
                        unitName: "Hero",
                        maxHealth: 30,
                        attackPower: 7,
                        initiative: 12)
                    .WithEnemy(
                        unitName: "Goblin",
                        maxHealth: 18,
                        attackPower: 4,
                        initiative: 8)
                    .WithPlayerSkill(
                        newSkillId: "power_strike",
                        displayName: "Power Strike",
                        minRange: 1,
                        maxRange: 2,
                        actionPointCost: 2)
                    .Build();

            Assert.That(fight.Player.UnitName, Is.EqualTo("Hero"));
            Assert.That(fight.Player.MaxHealth, Is.EqualTo(30));
            Assert.That(fight.Player.AttackPower, Is.EqualTo(7));
            Assert.That(fight.Player.Initiative, Is.EqualTo(12));

            Assert.That(fight.Enemy.UnitName, Is.EqualTo("Goblin"));
            Assert.That(fight.Enemy.MaxHealth, Is.EqualTo(18));
            Assert.That(fight.Enemy.AttackPower, Is.EqualTo(4));
            Assert.That(fight.Enemy.Initiative, Is.EqualTo(8));

            Assert.That(
                fight.PlayerSkill.SkillId,
                Is.EqualTo("power_strike"));

            Assert.That(
                fight.PlayerSkill.DisplayName,
                Is.EqualTo("Power Strike"));

            Assert.That(fight.PlayerSkill.MaxRange, Is.EqualTo(2));
            Assert.That(fight.PlayerSkill.ActionPointCost, Is.EqualTo(2));
        }
    }
}