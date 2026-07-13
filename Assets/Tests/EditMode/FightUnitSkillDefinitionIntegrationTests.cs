using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitSkillDefinitionIntegrationTests
    {
        [Test]
        public void TwoUnits_FromSameDefinition_CreateIndependentSkillStates()
        {
            SkillDefinition skillDefinition =
                ScriptableObject.CreateInstance<SkillDefinition>();

            UnitStartingSkill startingSkill =
                new UnitStartingSkill(
                    skillDefinition,
                    level: 1);

            FightUnitDefinition unitDefinition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            unitDefinition.InitializeForTests(
                newUnitName: "Mage",
                newTeam: FightTeam.Player,
                newMaxHealth: 12,
                newAttackPower: 3,
                newInitiative: 9,
                newStartingSkills:
                    new[] { startingSkill });

            GameObject firstObject =
                new GameObject("First Mage");

            GameObject secondObject =
                new GameObject("Second Mage");

            firstObject.AddComponent<FightUnitSkills>();
            secondObject.AddComponent<FightUnitSkills>();

            FightUnit firstUnit =
                firstObject.AddComponent<FightUnit>();

            FightUnit secondUnit =
                secondObject.AddComponent<FightUnit>();

            Assert.That(
                firstUnit.Initialize(unitDefinition),
                Is.True);

            Assert.That(
                secondUnit.Initialize(unitDefinition),
                Is.True);

            Assert.That(firstUnit.Skills.Skills.Count, Is.EqualTo(1));
            Assert.That(secondUnit.Skills.Skills.Count, Is.EqualTo(1));

            UnitSkillState firstState =
                firstUnit.Skills.Skills[0];

            UnitSkillState secondState =
                secondUnit.Skills.Skills[0];

            Assert.That(
                firstState,
                Is.Not.SameAs(secondState));

            Assert.That(
                firstState.Definition,
                Is.SameAs(skillDefinition));

            Assert.That(
                secondState.Definition,
                Is.SameAs(skillDefinition));

            Object.DestroyImmediate(firstObject);
            Object.DestroyImmediate(secondObject);
            Object.DestroyImmediate(unitDefinition);
            Object.DestroyImmediate(skillDefinition);
        }
    }
}