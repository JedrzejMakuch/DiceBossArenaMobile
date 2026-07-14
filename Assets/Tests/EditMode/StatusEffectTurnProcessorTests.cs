using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class StatusEffectTurnProcessorTests
    {
        private StatusEffectDefinition firstDefinition;
        private StatusEffectDefinition secondDefinition;

        [TearDown]
        public void TearDown()
        {
            if (firstDefinition != null)
            {
                Object.DestroyImmediate(
                    firstDefinition);
            }

            if (secondDefinition != null)
            {
                Object.DestroyImmediate(
                    secondDefinition);
            }
        }

        [Test]
        public void ProcessStartOfTurn_ExecutesOnlyStartEffects()
        {
            firstDefinition =
                CreateDefinition(
                    "regeneration",
                    StatusEffectTickTiming
                        .StartOfOwnerTurn);

            secondDefinition =
                CreateDefinition(
                    "bleed",
                    StatusEffectTickTiming
                        .EndOfOwnerTurn);

            StatusEffectCollection collection =
                CreateCollectionWithBothEffects();

            StatusEffectTurnProcessor processor =
                new StatusEffectTurnProcessor();

            int regenerationExecutions = 0;
            int bleedExecutions = 0;

            StatusEffectTurnProcessResult result =
                processor.ProcessStartOfTurn(
                    collection,
                    state =>
                    {
                        if (state.StatusEffectId ==
                            new StatusEffectId(
                                "regeneration"))
                        {
                            regenerationExecutions++;
                        }

                        if (state.StatusEffectId ==
                            new StatusEffectId(
                                "bleed"))
                        {
                            bleedExecutions++;
                        }
                    });

            Assert.That(
                regenerationExecutions,
                Is.EqualTo(1));

            Assert.That(
                bleedExecutions,
                Is.EqualTo(0));

            Assert.That(
                result.EffectsExecuted,
                Is.EqualTo(1));

            Assert.That(
                result.EffectsExpired,
                Is.EqualTo(0));
        }

        [Test]
        public void ProcessStartOfTurn_DoesNotAdvanceDuration()
        {
            firstDefinition =
                CreateDefinition(
                    "regeneration",
                    StatusEffectTickTiming
                        .StartOfOwnerTurn,
                    duration:
                        3);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.TryGet(
                new StatusEffectId(
                    "regeneration"),
                out StatusEffectRuntimeState state);

            StatusEffectTurnProcessor processor =
                new StatusEffectTurnProcessor();

            processor.ProcessStartOfTurn(
                collection,
                null);

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(3));
        }

        [Test]
        public void ProcessEndOfTurn_ExecutesOnlyEndEffects()
        {
            firstDefinition =
                CreateDefinition(
                    "regeneration",
                    StatusEffectTickTiming
                        .StartOfOwnerTurn);

            secondDefinition =
                CreateDefinition(
                    "bleed",
                    StatusEffectTickTiming
                        .EndOfOwnerTurn);

            StatusEffectCollection collection =
                CreateCollectionWithBothEffects();

            StatusEffectTurnProcessor processor =
                new StatusEffectTurnProcessor();

            int regenerationExecutions = 0;
            int bleedExecutions = 0;

            StatusEffectTurnProcessResult result =
                processor.ProcessEndOfTurn(
                    collection,
                    state =>
                    {
                        if (state.StatusEffectId ==
                            new StatusEffectId(
                                "regeneration"))
                        {
                            regenerationExecutions++;
                        }

                        if (state.StatusEffectId ==
                            new StatusEffectId(
                                "bleed"))
                        {
                            bleedExecutions++;
                        }
                    });

            Assert.That(
                regenerationExecutions,
                Is.EqualTo(0));

            Assert.That(
                bleedExecutions,
                Is.EqualTo(1));

            Assert.That(
                result.EffectsExecuted,
                Is.EqualTo(1));
        }

        [Test]
        public void ProcessEndOfTurn_AdvancesAllEffectDurations()
        {
            firstDefinition =
                CreateDefinition(
                    "fortified",
                    StatusEffectTickTiming.None,
                    duration:
                        3);

            secondDefinition =
                CreateDefinition(
                    "bleed",
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                    duration:
                        2);

            StatusEffectCollection collection =
                CreateCollectionWithBothEffects();

            collection.TryGet(
                new StatusEffectId(
                    "fortified"),
                out StatusEffectRuntimeState fortified);

            collection.TryGet(
                new StatusEffectId(
                    "bleed"),
                out StatusEffectRuntimeState bleed);

            StatusEffectTurnProcessor processor =
                new StatusEffectTurnProcessor();

            processor.ProcessEndOfTurn(
                collection,
                null);

            Assert.That(
                fortified.RemainingDurationTurns,
                Is.EqualTo(2));

            Assert.That(
                bleed.RemainingDurationTurns,
                Is.EqualTo(1));
        }

        [Test]
        public void ProcessEndOfTurn_RemovesExpiredEffects()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed",
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                    duration:
                        1);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            StatusEffectTurnProcessor processor =
                new StatusEffectTurnProcessor();

            StatusEffectTurnProcessResult result =
                processor.ProcessEndOfTurn(
                    collection,
                    null);

            Assert.That(
                collection.Contains(
                    new StatusEffectId(
                        "bleed")),
                Is.False);

            Assert.That(
                result.EffectsExpired,
                Is.EqualTo(1));
        }

        [Test]
        public void ProcessEndOfTurn_LastTickExecutesBeforeExpiration()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed",
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                    duration:
                        1);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            StatusEffectTurnProcessor processor =
                new StatusEffectTurnProcessor();

            int executions = 0;

            StatusEffectTurnProcessResult result =
                processor.ProcessEndOfTurn(
                    collection,
                    _ => executions++);

            Assert.That(
                executions,
                Is.EqualTo(1));

            Assert.That(
                result.EffectsExecuted,
                Is.EqualTo(1));

            Assert.That(
                result.EffectsExpired,
                Is.EqualTo(1));
        }

        [Test]
        public void ProcessEndOfTurn_NoneEffectDoesNotExecuteButExpires()
        {
            firstDefinition =
                CreateDefinition(
                    "fortified",
                    StatusEffectTickTiming.None,
                    duration:
                        1);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            StatusEffectTurnProcessor processor =
                new StatusEffectTurnProcessor();

            int executions = 0;

            StatusEffectTurnProcessResult result =
                processor.ProcessEndOfTurn(
                    collection,
                    _ => executions++);

            Assert.That(
                executions,
                Is.EqualTo(0));

            Assert.That(
                result.EffectsExecuted,
                Is.EqualTo(0));

            Assert.That(
                result.EffectsExpired,
                Is.EqualTo(1));
        }

        private StatusEffectCollection
            CreateCollectionWithBothEffects()
        {
            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.Apply(
                secondDefinition);

            return collection;
        }

        private StatusEffectDefinition CreateDefinition(
            string id,
            StatusEffectTickTiming tickTiming,
            int duration = 3)
        {
            StatusEffectDefinition result =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            result.InitializeForTests(
                newStatusEffectId:
                    id,
                newDisplayName:
                    id,
                newBaseDurationTurns:
                    duration,
                newMaxStacks:
                    1,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .RefreshDuration,
                newTickTiming:
                    tickTiming);

            return result;
        }
    }
}