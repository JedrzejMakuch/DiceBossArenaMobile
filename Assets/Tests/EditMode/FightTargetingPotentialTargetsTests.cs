using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightTargetingPotentialTargetsTests
    {
        [Test]
        public void PotentialTargets_AreNotLimitedByLegacyTeamName()
        {
            GameObject managerObject =
                new GameObject("Targeting Manager");

            FightTargetingManager manager =
                managerObject.AddComponent<
                    FightTargetingManager>();

            FightUnit teamAUnit =
                CreateClickableUnit(
                    "Team A Unit",
                    FightTeamId.TeamA,
                    "participant-a");

            FightUnit teamBUnit =
                CreateClickableUnit(
                    "Team B Unit",
                    FightTeamId.TeamB,
                    "participant-b");

            manager.SetPotentialTargets(
                new[]
                {
                    teamAUnit,
                    teamBUnit,
                    teamAUnit
                });

            Assert.That(
                manager.PotentialTargets.Count,
                Is.EqualTo(2));

            Assert.That(
                manager.PotentialTargets,
                Does.Contain(teamAUnit));

            Assert.That(
                manager.PotentialTargets,
                Does.Contain(teamBUnit));

            Object.DestroyImmediate(
                teamAUnit.gameObject);

            Object.DestroyImmediate(
                teamBUnit.gameObject);

            Object.DestroyImmediate(
                managerObject);
        }

        private static FightUnit CreateClickableUnit(
    string objectName,
    FightTeamId teamId,
    string participantId)
        {
            GameObject unitObject =
                new GameObject(objectName);

            unitObject.AddComponent<
                FightUnitClickHandler>();

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: objectName,
                newTeam: FightTeam.Enemy,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            FightUnitOwnership ownership =
                new FightUnitOwnership(
                    teamId,
                    new FightParticipantId(
                        participantId),
                    FightControllerType.AI);

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