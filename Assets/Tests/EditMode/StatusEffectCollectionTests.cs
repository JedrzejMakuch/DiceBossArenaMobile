using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class StatusEffectCollectionTests
    {
        private StatusEffectDefinition firstDefinition;
        private StatusEffectDefinition secondDefinition;

        [TearDown]
        public void TearDown()
        {
            if (firstDefinition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    firstDefinition);
            }

            if (secondDefinition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    secondDefinition);
            }
        }

        [Test]
        public void Apply_NewEffectAddsRuntimeState()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed");

            StatusEffectCollection collection =
                new StatusEffectCollection();

            StatusEffectApplyResult result =
                collection.Apply(
                    firstDefinition);

            Assert.That(
                result,
                Is.EqualTo(
                    StatusEffectApplyResult.Added));

            Assert.That(
                collection.Count,
                Is.EqualTo(1));

            Assert.That(
                collection.TryGet(
                    new StatusEffectId("bleed"),
                    out StatusEffectRuntimeState state),
                Is.True);

            Assert.That(
                state.Definition,
                Is.SameAs(firstDefinition));
        }

        [Test]
        public void GetStatesSnapshot_ReturnsCurrentStates()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed");

            secondDefinition =
                CreateDefinition(
                    "fortified");

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.Apply(
                secondDefinition);

            var states =
                collection.GetStatesSnapshot();

            Assert.That(
                states.Count,
                Is.EqualTo(2));
        }

        [Test]
        public void Dispel_RemovesEffectsMatchingCategory()
        {
            firstDefinition =
                CreateDefinition(
                    "weakened",
                    category:
                        StatusEffectCategory.Debuff);

            secondDefinition =
                CreateDefinition(
                    "fortified",
                    category:
                        StatusEffectCategory.Buff);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.Apply(
                secondDefinition);

            StatusEffectDispelResult result =
                collection.Dispel(
                    StatusEffectCategory.Debuff);

            Assert.That(
                result.RemovedCount,
                Is.EqualTo(1));

            Assert.That(
                result.RemovedAny,
                Is.True);

            Assert.That(
                collection.Contains(
                    new StatusEffectId(
                        "weakened")),
                Is.False);

            Assert.That(
                collection.Contains(
                    new StatusEffectId(
                        "fortified")),
                Is.True);
        }

        [Test]
        public void Dispel_RespectsMaximumEffects()
        {
            firstDefinition =
                CreateDefinition(
                    "weakened",
                    category:
                        StatusEffectCategory.Debuff);

            secondDefinition =
                CreateDefinition(
                    "bleed",
                    category:
                        StatusEffectCategory.Debuff);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.Apply(
                secondDefinition);

            StatusEffectDispelResult result =
                collection.Dispel(
                    StatusEffectCategory.Debuff,
                    maximumEffects:
                        1);

            Assert.That(
                result.RemovedCount,
                Is.EqualTo(1));

            Assert.That(
                collection.Count,
                Is.EqualTo(1));
        }

        [Test]
        public void Dispel_NoMatchingEffectsReturnsZero()
        {
            firstDefinition =
                CreateDefinition(
                    "fortified",
                    category:
                        StatusEffectCategory.Buff);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            StatusEffectDispelResult result =
                collection.Dispel(
                    StatusEffectCategory.Debuff);

            Assert.That(
                result.RemovedCount,
                Is.EqualTo(0));

            Assert.That(
                result.RemovedAny,
                Is.False);
        }

        [Test]
        public void Apply_NullDefinitionThrows()
        {
            StatusEffectCollection collection =
                new StatusEffectCollection();

            Assert.Throws<ArgumentNullException>(
                () =>
                    collection.Apply(
                        null));
        }

        [Test]
        public void Apply_InvalidDefinitionIdThrows()
        {
            firstDefinition =
                CreateDefinition(
                    string.Empty);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            Assert.Throws<ArgumentException>(
                () =>
                    collection.Apply(
                        firstDefinition));
        }

        [Test]
        public void Apply_RefreshPolicyRefreshesDuration()
        {
            firstDefinition =
                CreateDefinition(
                    "stun",
                    duration:
                        2,
                    policy:
                        StatusEffectStackingPolicy
                            .RefreshDuration);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.TryGet(
                new StatusEffectId("stun"),
                out StatusEffectRuntimeState state);

            state.AdvanceDuration();

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(1));

            StatusEffectApplyResult result =
                collection.Apply(
                    firstDefinition);

            Assert.That(
                result,
                Is.EqualTo(
                    StatusEffectApplyResult
                        .DurationRefreshed));

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(2));

            Assert.That(
                state.Stacks,
                Is.EqualTo(1));
        }

        [Test]
        public void Apply_AddStacksPolicyAddsStackAndRefreshesDuration()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed",
                    duration:
                        3,
                    maxStacks:
                        5,
                    policy:
                        StatusEffectStackingPolicy
                            .AddStacksAndRefreshDuration);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.TryGet(
                new StatusEffectId("bleed"),
                out StatusEffectRuntimeState state);

            state.AdvanceDuration();

            StatusEffectApplyResult result =
                collection.Apply(
                    firstDefinition);

            Assert.That(
                result,
                Is.EqualTo(
                    StatusEffectApplyResult
                        .StackAddedAndDurationRefreshed));

            Assert.That(
                state.Stacks,
                Is.EqualTo(2));

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(3));
        }

        [Test]
        public void Apply_AtMaxStacksOnlyRefreshesDuration()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed",
                    duration:
                        3,
                    maxStacks:
                        2,
                    policy:
                        StatusEffectStackingPolicy
                            .AddStacksAndRefreshDuration);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.Apply(
                firstDefinition);

            collection.TryGet(
                new StatusEffectId("bleed"),
                out StatusEffectRuntimeState state);

            state.AdvanceDuration();

            StatusEffectApplyResult result =
                collection.Apply(
                    firstDefinition);

            Assert.That(
                result,
                Is.EqualTo(
                    StatusEffectApplyResult
                        .DurationRefreshed));

            Assert.That(
                state.Stacks,
                Is.EqualTo(2));

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(3));
        }

        [Test]
        public void Apply_RejectPolicyDoesNotChangeState()
        {
            firstDefinition =
                CreateDefinition(
                    "stun",
                    duration:
                        2,
                    policy:
                        StatusEffectStackingPolicy
                            .RejectNewApplication);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.TryGet(
                new StatusEffectId("stun"),
                out StatusEffectRuntimeState state);

            state.AdvanceDuration();

            StatusEffectApplyResult result =
                collection.Apply(
                    firstDefinition);

            Assert.That(
                result,
                Is.EqualTo(
                    StatusEffectApplyResult.Rejected));

            Assert.That(
                state.RemainingDurationTurns,
                Is.EqualTo(1));

            Assert.That(
                state.Stacks,
                Is.EqualTo(1));
        }

        [Test]
        public void Remove_KnownEffectRemovesState()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed");

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            bool removed =
                collection.Remove(
                    new StatusEffectId(
                        "bleed"));

            Assert.That(
                removed,
                Is.True);

            Assert.That(
                collection.Count,
                Is.EqualTo(0));
        }

        [Test]
        public void Remove_UnknownEffectReturnsFalse()
        {
            StatusEffectCollection collection =
                new StatusEffectCollection();

            bool removed =
                collection.Remove(
                    new StatusEffectId(
                        "bleed"));

            Assert.That(
                removed,
                Is.False);
        }

        [Test]
        public void RemoveExpired_RemovesOnlyExpiredEffects()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed",
                    duration:
                        1);

            secondDefinition =
                CreateDefinition(
                    "fortified",
                    duration:
                        3);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                firstDefinition);

            collection.Apply(
                secondDefinition);

            collection.TryGet(
                new StatusEffectId("bleed"),
                out StatusEffectRuntimeState bleed);

            bleed.AdvanceDuration();

            int removed =
                collection.RemoveExpired();

            Assert.That(
                removed,
                Is.EqualTo(1));

            Assert.That(
                collection.Contains(
                    new StatusEffectId("bleed")),
                Is.False);

            Assert.That(
                collection.Contains(
                    new StatusEffectId("fortified")),
                Is.True);
        }

        [Test]
        public void Events_AreRaisedForAddChangeAndRemove()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed",
                    maxStacks:
                        3,
                    policy:
                        StatusEffectStackingPolicy
                            .AddStacksAndRefreshDuration);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            int addedCount = 0;
            int changedCount = 0;
            int removedCount = 0;

            collection.EffectAdded +=
                _ => addedCount++;

            collection.EffectChanged +=
                _ => changedCount++;

            collection.EffectRemoved +=
                _ => removedCount++;

            collection.Apply(
                firstDefinition);

            collection.Apply(
                firstDefinition);

            collection.Remove(
                new StatusEffectId(
                    "bleed"));

            Assert.That(
                addedCount,
                Is.EqualTo(1));

            Assert.That(
                changedCount,
                Is.EqualTo(1));

            Assert.That(
                removedCount,
                Is.EqualTo(1));
        }

        [Test]
        public void TwoCollections_AreIndependent()
        {
            firstDefinition =
                CreateDefinition(
                    "bleed",
                    maxStacks:
                        3,
                    policy:
                        StatusEffectStackingPolicy
                            .AddStacksAndRefreshDuration);

            StatusEffectCollection first =
                new StatusEffectCollection();

            StatusEffectCollection second =
                new StatusEffectCollection();

            first.Apply(
                firstDefinition);

            first.Apply(
                firstDefinition);

            second.Apply(
                firstDefinition);

            first.TryGet(
                new StatusEffectId("bleed"),
                out StatusEffectRuntimeState firstState);

            second.TryGet(
                new StatusEffectId("bleed"),
                out StatusEffectRuntimeState secondState);

            Assert.That(
                firstState.Stacks,
                Is.EqualTo(2));

            Assert.That(
                secondState.Stacks,
                Is.EqualTo(1));
        }

        private StatusEffectDefinition CreateDefinition(
            string id,
            int duration = 3,
            int maxStacks = 1,
            StatusEffectStackingPolicy policy =
                StatusEffectStackingPolicy.RefreshDuration,
            StatusEffectCategory category =
                StatusEffectCategory.Neutral)
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
                    maxStacks,
                newStackingPolicy:
                    policy,
                newCategory: category);

            return result;
        }
    }
}