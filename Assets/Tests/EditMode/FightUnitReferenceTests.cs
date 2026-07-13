using DiceBossArena.Tests.Fixtures;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitReferenceTests
    {
        [Test]
        public void TestUnitFactory_CreatesInitializedFightUnit()
        {
            FightUnit unit = TestUnitFactory.Create(
                unitName: "Hero",
                team: FightTeam.Player,
                maxHealth: 20,
                attackPower: 5,
                initiative: 15);

            try
            {
                Assert.That(unit, Is.Not.Null);
                Assert.That(unit.UnitName, Is.EqualTo("Hero"));
                Assert.That(unit.Team, Is.EqualTo(FightTeam.Player));
                Assert.That(unit.MaxHealth, Is.EqualTo(20));
                Assert.That(unit.CurrentHealth, Is.EqualTo(20));
                Assert.That(unit.AttackPower, Is.EqualTo(5));
                Assert.That(unit.Initiative, Is.EqualTo(15));
                Assert.That(unit.IsAlive, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(unit.gameObject);
            }
        }
    }
}