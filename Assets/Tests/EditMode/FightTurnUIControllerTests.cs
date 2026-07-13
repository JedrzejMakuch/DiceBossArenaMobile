using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightTurnUIControllerTests
    {
        [Test]
        public void LocalPlayerController_CanEndTurnDespiteLegacyEnemyTeam()
        {
            FightUnit unit =
                CreateUnit(
                    FightTeam.Enemy,
                    FightControllerType.LocalPlayer);

            Assert.That(
                FightTurnUIManager
                    .CanLocalPlayerEndTurn(unit),
                Is.True);

            Object.DestroyImmediate(
                unit.gameObject);
        }

        [Test]
        public void AiController_CannotUseLocalEndTurnButton()
        {
            FightUnit unit =
                CreateUnit(
                    FightTeam.Player,
                    FightControllerType.AI);

            Assert.That(
                FightTurnUIManager
                    .CanLocalPlayerEndTurn(unit),
                Is.False);

            Object.DestroyImmediate(
                unit.gameObject);
        }

        private static FightUnit CreateUnit(
            FightTeam legacyTeam,
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
                    FightTeamId.TeamA,
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