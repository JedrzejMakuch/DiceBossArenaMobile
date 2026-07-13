using DiceBossArena.Tests.Fixtures;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class TestSkillFactoryTests
    {
        [Test]
        public void Create_ReturnsSkillDefinition()
        {
            SkillDefinition skill = TestSkillFactory.Create();

            try
            {
                Assert.That(skill, Is.Not.Null);
                Assert.That(skill.name, Is.EqualTo("Test Skill"));
                Assert.That(skill.MaxLevel, Is.EqualTo(1));
                Assert.That(skill.MaxRange, Is.EqualTo(1));
                Assert.That(skill.ActionPointCost, Is.EqualTo(1));
            }
            finally
            {
                Object.DestroyImmediate(skill);
            }
        }
    }
}