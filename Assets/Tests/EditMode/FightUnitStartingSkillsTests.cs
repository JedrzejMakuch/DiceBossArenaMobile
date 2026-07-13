using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitStartingSkillsTests
    {
        [Test]
        public void Definition_StoresStartingSkillWithoutRuntimeCooldown()
        {
            SkillDefinition skill =
                ScriptableObject.CreateInstance<SkillDefinition>();

            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            UnitStartingSkill startingSkill =
                new UnitStartingSkill(
                    skill,
                    level: 2);

            definition.InitializeForTests(
                newUnitName: "Mage",
                newTeam: FightTeam.Player,
                newMaxHealth: 12,
                newAttackPower: 3,
                newInitiative: 9,
                newStartingSkills:
                    new[] { startingSkill });

            Assert.That(
                definition.StartingSkills.Count,
                Is.EqualTo(1));

            Assert.That(
                definition.StartingSkills[0].Definition,
                Is.SameAs(skill));

            Assert.That(
                definition.StartingSkills[0].Level,
                Is.EqualTo(2));

            Object.DestroyImmediate(definition);
            Object.DestroyImmediate(skill);
        }
    }
}