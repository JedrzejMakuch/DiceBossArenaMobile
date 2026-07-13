using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitSkillsInitializationTests
    {
        [Test]
        public void TwoComponents_FromSameStartingSkill_CreateIndependentStates()
        {
            SkillDefinition skillDefinition =
                ScriptableObject.CreateInstance<SkillDefinition>();

            UnitStartingSkill startingSkill =
                new UnitStartingSkill(
                    skillDefinition,
                    level: 1);

            GameObject firstObject =
                new GameObject("First Unit");

            GameObject secondObject =
                new GameObject("Second Unit");

            FightUnitSkills firstSkills =
                firstObject.AddComponent<FightUnitSkills>();

            FightUnitSkills secondSkills =
                secondObject.AddComponent<FightUnitSkills>();

            firstSkills.InitializeFromDefinition(
                new[] { startingSkill });

            secondSkills.InitializeFromDefinition(
                new[] { startingSkill });

            UnitSkillState firstState =
                firstSkills.Skills[0];

            UnitSkillState secondState =
                secondSkills.Skills[0];

            Assert.That(firstSkills.Skills.Count, Is.EqualTo(1));
            Assert.That(secondSkills.Skills.Count, Is.EqualTo(1));

            Assert.That(
                firstState,
                Is.Not.SameAs(secondState));

            Assert.That(
                firstState.Definition,
                Is.SameAs(skillDefinition));

            Assert.That(
                secondState.Definition,
                Is.SameAs(skillDefinition));

            firstState.SetLevel(2);

            Assert.That(
                secondState.Level,
                Is.EqualTo(1));

            Object.DestroyImmediate(firstObject);
            Object.DestroyImmediate(secondObject);
            Object.DestroyImmediate(skillDefinition);
        }
    }
}