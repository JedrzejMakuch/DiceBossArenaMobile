using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitRelationshipTests
    {
        [Test]
        public void UnitsOnSameTeam_AreAlliesDespiteDifferentControllers()
        {
            FightUnitDefinition definition =
                CreateDefinition();

            FightUnit localUnit =
                CreateUnit(
                    "Local Unit",
                    definition,
                    new FightUnitOwnership(
                        FightTeamId.TeamA,
                        new FightParticipantId("player-one"),
                        FightControllerType.LocalPlayer));

            FightUnit aiUnit =
                CreateUnit(
                    "AI Companion",
                    definition,
                    new FightUnitOwnership(
                        FightTeamId.TeamA,
                        new FightParticipantId("companion"),
                        FightControllerType.AI));

            Assert.That(
                localUnit.IsAlliedWith(aiUnit),
                Is.True);

            Assert.That(
                localUnit.IsHostileTo(aiUnit),
                Is.False);

            Assert.That(
                localUnit.ControllerType,
                Is.Not.EqualTo(aiUnit.ControllerType));

            DestroyUnit(localUnit);
            DestroyUnit(aiUnit);
            Object.DestroyImmediate(definition);
        }

        [Test]
        public void Targeting_AcceptsOnlyUnitFromDifferentTeam()
        {
            FightUnitDefinition definition =
                CreateDefinition();

            FightUnit attacker =
                CreateUnit(
                    "Attacker",
                    definition,
                    new FightUnitOwnership(
                        FightTeamId.TeamA,
                        new FightParticipantId("player-one"),
                        FightControllerType.LocalPlayer));

            FightUnit ally =
                CreateUnit(
                    "Ally",
                    definition,
                    new FightUnitOwnership(
                        FightTeamId.TeamA,
                        new FightParticipantId("player-two"),
                        FightControllerType.RemotePlayer));

            FightUnit enemy =
                CreateUnit(
                    "Enemy",
                    definition,
                    new FightUnitOwnership(
                        FightTeamId.TeamB,
                        new FightParticipantId("enemy-one"),
                        FightControllerType.AI));

            GameObject managerObject =
                new GameObject("Targeting Manager");

            FightTargetingManager targetingManager =
                managerObject.AddComponent<FightTargetingManager>();

            Assert.That(
                targetingManager.IsValidTarget(
                    attacker,
                    ally),
                Is.False);

            Assert.That(
                targetingManager.IsValidTarget(
                    attacker,
                    enemy),
                Is.True);

            DestroyUnit(attacker);
            DestroyUnit(ally);
            DestroyUnit(enemy);
            Object.DestroyImmediate(managerObject);
            Object.DestroyImmediate(definition);
        }

        private static FightUnitDefinition CreateDefinition()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: "Test Unit",
                newTeam: FightTeam.Enemy,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            return definition;
        }

        private static FightUnit CreateUnit(
            string objectName,
            FightUnitDefinition definition,
            FightUnitOwnership ownership)
        {
            GameObject unitObject =
                new GameObject(objectName);

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            bool initialized =
                unit.Initialize(
                    definition,
                    ownership);

            Assert.That(initialized, Is.True);

            return unit;
        }

        private static void DestroyUnit(
            FightUnit unit)
        {
            if (unit != null)
            {
                Object.DestroyImmediate(
                    unit.gameObject);
            }
        }

        [Test]
        public void SameLegacyTeam_DoesNotDetermineCombatRelation()
        {
            FightUnitDefinition definition =
                CreateDefinition();

            FightUnit teamAUnit =
                CreateUnit(
                    "Team A",
                    definition,
                    new FightUnitOwnership(
                        FightTeamId.TeamA,
                        new FightParticipantId("participant-a"),
                        FightControllerType.LocalPlayer));

            FightUnit teamBUnit =
                CreateUnit(
                    "Team B",
                    definition,
                    new FightUnitOwnership(
                        FightTeamId.TeamB,
                        new FightParticipantId("participant-b"),
                        FightControllerType.AI));

            Assert.That(
                teamAUnit.Team,
                Is.EqualTo(teamBUnit.Team));

            Assert.That(
                teamAUnit.IsHostileTo(teamBUnit),
                Is.True);

            Assert.That(
                teamAUnit.IsAlliedWith(teamBUnit),
                Is.False);

            DestroyUnit(teamAUnit);
            DestroyUnit(teamBUnit);
            Object.DestroyImmediate(definition);
        }
    }
}