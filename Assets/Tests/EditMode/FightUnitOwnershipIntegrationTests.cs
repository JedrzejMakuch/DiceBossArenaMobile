using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitOwnershipIntegrationTests
    {
        [Test]
        public void LegacyPlayer_ReceivesTeamAAndLocalController()
        {
            GameObject unitObject =
                new GameObject("Player Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                newUnitName: "Hero",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 10);

            Assert.That(
                unit.TeamId,
                Is.EqualTo(FightTeamId.TeamA));

            Assert.That(
                unit.ParticipantId,
                Is.EqualTo(
                    new FightParticipantId("local-player")));

            Assert.That(
                unit.ControllerType,
                Is.EqualTo(
                    FightControllerType.LocalPlayer));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void ExplicitOwnership_OverridesLegacyDefinitionMapping()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: "Controlled Enemy",
                newTeam: FightTeam.Enemy,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 8);

            FightUnitOwnership ownership =
                new FightUnitOwnership(
                    FightTeamId.TeamA,
                    new FightParticipantId("player-two"),
                    FightControllerType.RemotePlayer);

            GameObject unitObject =
                new GameObject("Controlled Enemy");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            Assert.That(
                unit.Initialize(
                    definition,
                    ownership),
                Is.True);

            Assert.That(
                unit.Team,
                Is.EqualTo(FightTeam.Enemy));

            Assert.That(
                unit.TeamId,
                Is.EqualTo(FightTeamId.TeamA));

            Assert.That(
                unit.ParticipantId,
                Is.EqualTo(
                    new FightParticipantId("player-two")));

            Assert.That(
                unit.ControllerType,
                Is.EqualTo(
                    FightControllerType.RemotePlayer));

            Object.DestroyImmediate(unitObject);
            Object.DestroyImmediate(definition);
        }
    }
}