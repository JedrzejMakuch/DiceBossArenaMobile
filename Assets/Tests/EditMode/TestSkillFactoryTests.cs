using DiceBossArena.Tests.Fixtures;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class TestSkillFactoryTests
    {
        [Test]
        public void Create_ReturnsConfiguredSkillDefinition()
        {
            SkillDefinition skill = TestSkillFactory.Create(
                skillId: "power_strike",
                displayName: "Power Strike",
                description: "Heavy test attack.",
                targetType: SkillTargetType.SingleEnemy,
                rangeShape: SkillRangeShape.StraightLine,
                minRange: 1,
                maxRange: 3,
                actionPointCost: 2,
                movementPointCost: 1,
                maxLevel: 2);

            try
            {
                Assert.That(skill, Is.Not.Null);
                Assert.That(skill.name, Is.EqualTo("Power Strike"));
                Assert.That(skill.SkillId, Is.EqualTo("power_strike"));
                Assert.That(skill.DisplayName, Is.EqualTo("Power Strike"));
                Assert.That(skill.Description, Is.EqualTo("Heavy test attack."));
                Assert.That(
                    skill.TargetType,
                    Is.EqualTo(SkillTargetType.SingleEnemy));
                Assert.That(
                    skill.RangeShape,
                    Is.EqualTo(SkillRangeShape.StraightLine));
                Assert.That(skill.MinRange, Is.EqualTo(1));
                Assert.That(skill.MaxRange, Is.EqualTo(3));
                Assert.That(skill.ActionPointCost, Is.EqualTo(2));
                Assert.That(skill.MovementPointCost, Is.EqualTo(1));
                Assert.That(skill.MaxLevel, Is.EqualTo(2));
            }
            finally
            {
                Object.DestroyImmediate(skill);
            }
        }
    }
}