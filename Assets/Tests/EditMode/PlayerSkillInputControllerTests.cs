using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class PlayerSkillInputControllerTests
    {
        [Test]
        public void LocalController_CanUseSkillInputDespiteLegacyEnemyTeam()
        {
            FightUnit unit =
                CreateUnit(
                    FightTeam.Enemy,
                    FightTeamId.TeamA,
                    FightControllerType.LocalPlayer);

            Assert.That(
                PlayerSkillSelectionManager
                    .CanUseLocalSkillSelection(unit),
                Is.True);

            Object.DestroyImmediate(
                unit.gameObject);
        }

        [Test]
        public void UnitsWithSameLegacyTeam_CanStillBeHostile()
        {
            FightUnit caster =
                CreateUnit(
                    FightTeam.Enemy,
                    FightTeamId.TeamA,
                    FightControllerType.LocalPlayer);

            FightUnit target =
                CreateUnit(
                    FightTeam.Enemy,
                    FightTeamId.TeamB,
                    FightControllerType.AI);

            Assert.That(
                caster.Team,
                Is.EqualTo(target.Team));

            Assert.That(
                caster.IsHostileTo(target),
                Is.True);

            Object.DestroyImmediate(
                caster.gameObject);

            Object.DestroyImmediate(
                target.gameObject);
        }

        private static FightUnit CreateUnit(
            FightTeam legacyTeam,
            FightTeamId teamId,
            FightControllerType controllerType)
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: "Test Unit",
                newTeam: legacyTeam,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            FightUnitOwnership ownership =
                new FightUnitOwnership(
                    teamId,
                    new FightParticipantId(
                        "test-participant"),
                    controllerType);

            GameObject unitObject =
                new GameObject("Test Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            bool initialized =
                unit.Initialize(
                    definition,
                    ownership);

            Object.DestroyImmediate(definition);

            Assert.That(initialized, Is.True);

            return unit;
        }
    }
}