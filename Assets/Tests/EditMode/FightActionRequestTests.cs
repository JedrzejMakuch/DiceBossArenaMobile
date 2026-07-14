using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightActionRequestTests
    {
        [Test]
        public void MoveRequest_StoresActorAndTargetTile()
        {
            GameObject actorObject =
                new GameObject("Actor");

            GameObject tileObject =
                new GameObject("Tile");

            try
            {
                FightUnit actor =
                    actorObject.AddComponent<FightUnit>();

                FightGridTile tile =
                    tileObject.AddComponent<FightGridTile>();

                FightMoveActionRequest request =
                    new FightMoveActionRequest(
                        actor,
                        tile);

                Assert.That(
                    request.ActionType,
                    Is.EqualTo(FightActionType.Move));

                Assert.That(
                    request.Actor,
                    Is.SameAs(actor));

                Assert.That(
                    request.TargetTile,
                    Is.SameAs(tile));
            }
            finally
            {
                Object.DestroyImmediate(actorObject);
                Object.DestroyImmediate(tileObject);
            }
        }

        [Test]
        public void SkillRequest_StoresCompleteExecutionData()
        {
            GameObject actorObject =
                new GameObject("Actor");

            GameObject targetObject =
                new GameObject("Target");

            GameObject tileObject =
                new GameObject("Tile");

            try
            {
                FightUnit actor =
                    actorObject.AddComponent<FightUnit>();

                FightUnit target =
                    targetObject.AddComponent<FightUnit>();

                FightGridTile tile =
                    tileObject.AddComponent<FightGridTile>();

                UnitSkillState skillState =
                    new UnitSkillState();

                FightUnit[] affectedUnits =
                {
                    target
                };

                FightSkillActionRequest request =
                    new FightSkillActionRequest(
                        actor,
                        skillState,
                        target,
                        tile,
                        affectedUnits);

                Assert.That(
                    request.ActionType,
                    Is.EqualTo(FightActionType.Skill));

                Assert.That(
                    request.Actor,
                    Is.SameAs(actor));

                Assert.That(
                    request.SkillState,
                    Is.SameAs(skillState));

                Assert.That(
                    request.PrimaryTarget,
                    Is.SameAs(target));

                Assert.That(
                    request.TargetTile,
                    Is.SameAs(tile));

                Assert.That(
                    request.AffectedUnits,
                    Is.SameAs(affectedUnits));
            }
            finally
            {
                Object.DestroyImmediate(actorObject);
                Object.DestroyImmediate(targetObject);
                Object.DestroyImmediate(tileObject);
            }
        }

        [Test]
        public void EndTurnRequest_StoresActor()
        {
            GameObject actorObject =
                new GameObject("Actor");

            try
            {
                FightUnit actor =
                    actorObject.AddComponent<FightUnit>();

                FightEndTurnActionRequest request =
                    new FightEndTurnActionRequest(
                        actor);

                Assert.That(
                    request.ActionType,
                    Is.EqualTo(FightActionType.EndTurn));

                Assert.That(
                    request.Actor,
                    Is.SameAs(actor));
            }
            finally
            {
                Object.DestroyImmediate(actorObject);
            }
        }
    }
}