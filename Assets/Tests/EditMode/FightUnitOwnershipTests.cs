using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitOwnershipTests
    {
        [Test]
        public void SameTeam_CanContainDifferentControllers()
        {
            FightUnitOwnership localPlayer =
                new FightUnitOwnership(
                    FightTeamId.TeamA,
                    new FightParticipantId("player-one"),
                    FightControllerType.LocalPlayer);

            FightUnitOwnership aiCompanion =
                new FightUnitOwnership(
                    FightTeamId.TeamA,
                    new FightParticipantId("companion"),
                    FightControllerType.AI);

            Assert.That(
                localPlayer.TeamId,
                Is.EqualTo(aiCompanion.TeamId));

            Assert.That(
                localPlayer.ParticipantId,
                Is.Not.EqualTo(aiCompanion.ParticipantId));

            Assert.That(
                localPlayer.ControllerType,
                Is.Not.EqualTo(aiCompanion.ControllerType));
        }

        [Test]
        public void Summon_InheritsOwnersTeamAndParticipant()
        {
            FightUnitOwnership owner =
                new FightUnitOwnership(
                    FightTeamId.TeamB,
                    new FightParticipantId("enemy-boss"),
                    FightControllerType.AI);

            FightUnitOwnership summon =
                owner.CreateForSummon();

            Assert.That(
                summon,
                Is.Not.SameAs(owner));

            Assert.That(
                summon.TeamId,
                Is.EqualTo(owner.TeamId));

            Assert.That(
                summon.ParticipantId,
                Is.EqualTo(owner.ParticipantId));

            Assert.That(
                summon.ControllerType,
                Is.EqualTo(owner.ControllerType));
        }
    }
}