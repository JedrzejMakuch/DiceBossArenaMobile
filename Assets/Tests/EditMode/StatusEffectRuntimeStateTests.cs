using System;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class StatusEffectRuntimeStateTests
    {
        private StatusEffectDefinition definition;

        [TearDown]
        public void TearDown()
        {
            if (definition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    definition);
            }
        }

        [Test]
        public void Constructor_UsesDefinitionInitialValues()
        {
            definition =
                CreateDefinition(
                    durationTurns:
                        3,
                    maxStacks:
                        5);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            Assert.That(
                state.Definition,
                Is.SameAs(definition));

            Assert.That(
                state.StatusEffectId,
                Is.EqualTo(
                    definition.StatusEffectId));

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(3));

            Assert.That(
                state.Stacks,
                Is.EqualTo(1));

            Assert.That(
                state.IsExpired,
                Is.False);
        }

        [Test]
        public void Constructor_NullDefinitionThrows()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                    new StatusEffectRuntimeState(
                        null));
        }

        [Test]
        public void Constructor_InvalidDefinitionIdThrows()
        {
            definition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            definition.InitializeForTests(
                newStatusEffectId:
                    string.Empty);

            ArgumentException exception =
                Assert.Throws<ArgumentException>(
                    () =>
                        new StatusEffectRuntimeState(
                            definition));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "valid id"));
        }

        [Test]
        public void AdvanceDuration_DecreasesRemainingDuration()
        {
            definition =
                CreateDefinition(
                    durationTurns:
                        3);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            state.AdvanceDuration();

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(2));

            Assert.That(
                state.IsExpired,
                Is.False);
        }

        [Test]
        public void AdvanceDuration_AtZeroMarksEffectExpired()
        {
            definition =
                CreateDefinition(
                    durationTurns:
                        1);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            state.AdvanceDuration();

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(0));

            Assert.That(
                state.IsExpired,
                Is.True);
        }

        [Test]
        public void AdvanceDuration_ExpiredEffectDoesNotGoBelowZero()
        {
            definition =
                CreateDefinition(
                    durationTurns:
                        1);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            state.AdvanceDuration();
            state.AdvanceDuration();

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(0));
        }

        [Test]
        public void RefreshDuration_RestoresBaseDuration()
        {
            definition =
                CreateDefinition(
                    durationTurns:
                        3);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            state.AdvanceDuration();
            state.AdvanceDuration();

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(1));

            state.RefreshDuration();

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(3));
        }

        [Test]
        public void TryAddStack_IncreasesStackCount()
        {
            definition =
                CreateDefinition(
                    maxStacks:
                        3);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            bool added =
                state.TryAddStack();

            Assert.That(
                added,
                Is.True);

            Assert.That(
                state.Stacks,
                Is.EqualTo(2));
        }

        [Test]
        public void TryAddStack_StopsAtMaximum()
        {
            definition =
                CreateDefinition(
                    maxStacks:
                        2);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            bool firstResult =
                state.TryAddStack();

            bool secondResult =
                state.TryAddStack();

            Assert.That(
                firstResult,
                Is.True);

            Assert.That(
                secondResult,
                Is.False);

            Assert.That(
                state.Stacks,
                Is.EqualTo(2));
        }

        [Test]
        public void TwoStates_FromSameDefinitionAreIndependent()
        {
            definition =
                CreateDefinition(
                    durationTurns:
                        3,
                    maxStacks:
                        5);

            StatusEffectRuntimeState first =
                new StatusEffectRuntimeState(
                    definition);

            StatusEffectRuntimeState second =
                new StatusEffectRuntimeState(
                    definition);

            first.AdvanceDuration();
            first.TryAddStack();

            Assert.That(
                first.RemainingDurationTurns,
                Is.EqualTo(2));

            Assert.That(
                first.Stacks,
                Is.EqualTo(2));

            Assert.That(
                second.RemainingDurationTurns,
                Is.EqualTo(3));

            Assert.That(
                second.Stacks,
                Is.EqualTo(1));
        }

        private StatusEffectDefinition CreateDefinition(
            int durationTurns = 3,
            int maxStacks = 1)
        {
            StatusEffectDefinition result =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            result.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newBaseDurationTurns:
                    durationTurns,
                newMaxStacks:
                    maxStacks,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .AddStacksAndRefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn);

            return result;
        }
    }
}