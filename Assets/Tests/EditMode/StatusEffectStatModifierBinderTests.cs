using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        StatusEffectStatModifierBinderTests
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
        public void Constructor_NullCollectionThrows()
        {
            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            Assert.Throws<ArgumentNullException>(
                () =>
                    new StatusEffectStatModifierBinder(
                        null,
                        stats));
        }

        [Test]
        public void Constructor_NullStatsThrows()
        {
            StatusEffectCollection collection =
                new StatusEffectCollection();

            Assert.Throws<ArgumentNullException>(
                () =>
                    new StatusEffectStatModifierBinder(
                        collection,
                        null));
        }

        [Test]
        public void ApplyingEffect_AddsStatModifier()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            using StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(8));
        }

        [Test]
        public void ExistingEffect_IsBoundByConstructor()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            collection.Apply(
                definition);

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            using StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(8));
        }

        [Test]
        public void AddingStack_RecalculatesModifier()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2,
                    maxStacks:
                        3);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            using StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(8));

            collection.Apply(
                definition);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(6));
        }

        [Test]
        public void DurationRefresh_DoesNotDuplicateModifier()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2,
                    maxStacks:
                        1);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            using StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            collection.Apply(
                definition);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(8));
        }

        [Test]
        public void RemovingEffect_RemovesStatModifier()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            using StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            collection.Remove(
                new StatusEffectId(
                    "weakened"));

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));
        }

        [Test]
        public void ExpiredEffect_RemovesStatModifier()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2,
                    duration:
                        1);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            using StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            StatusEffectTurnProcessor processor =
                new StatusEffectTurnProcessor();

            processor.ProcessEndOfTurn(
                collection,
                null);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));
        }

        [Test]
        public void Dispose_RemovesAppliedModifiers()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(8));

            binder.Dispose();

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));
        }

        [Test]
        public void Dispose_IsIdempotent()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            binder.Dispose();
            binder.Dispose();

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));
        }

        [Test]
        public void Dispose_UnsubscribesFromCollection()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            binder.Dispose();

            collection.Apply(
                definition);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));
        }

        [Test]
        public void Binder_DoesNotRemoveUnrelatedModifiers()
        {
            definition =
                CreateAttackModifierEffect(
                    valuePerStack:
                        -2);

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10);

            FightStatModifier unrelatedModifier =
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5);

            stats.AddModifier(
                unrelatedModifier);

            StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(13));

            binder.Dispose();

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(15));
        }

        [Test]
        public void EffectWithMultipleModifiers_AppliesAll()
        {
            StatusEffectStatModifierDefinition attackModifier =
                new StatusEffectStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    2);

            StatusEffectStatModifierDefinition initiativeModifier =
                new StatusEffectStatModifierDefinition(
                    FightStatType.Initiative,
                    FightStatModifierType.Flat,
                    3);

            definition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            definition.InitializeForTests(
                newStatusEffectId:
                    "battle_focus",
                newDisplayName:
                    "Battle Focus",
                newStatModifiers:
                    new[]
                    {
                        attackModifier,
                        initiativeModifier
                    });

            StatusEffectCollection collection =
                new StatusEffectCollection();

            FightUnitStats stats =
                CreateStats(
                    attackPower:
                        10,
                    initiative:
                        5);

            using StatusEffectStatModifierBinder binder =
                new StatusEffectStatModifierBinder(
                    collection,
                    stats);

            collection.Apply(
                definition);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(12));

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.Initiative),
                Is.EqualTo(8));
        }

        private StatusEffectDefinition
            CreateAttackModifierEffect(
                int valuePerStack,
                int maxStacks = 1,
                int duration = 3)
        {
            StatusEffectStatModifierDefinition modifier =
                new StatusEffectStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    valuePerStack);

            StatusEffectDefinition result =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            result.InitializeForTests(
                newStatusEffectId:
                    "weakened",
                newDisplayName:
                    "Weakened",
                newBaseDurationTurns:
                    duration,
                newMaxStacks:
                    maxStacks,
                newStackingPolicy:
                    maxStacks > 1
                        ? StatusEffectStackingPolicy
                            .AddStacksAndRefreshDuration
                        : StatusEffectStackingPolicy
                            .RefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming.None,
                newStatModifiers:
                    new[]
                    {
                        modifier
                    });

            return result;
        }

        private FightUnitStats CreateStats(
            int attackPower,
            int initiative = 5)
        {
            return new FightUnitStats(
                new Dictionary<FightStatType, int>
                {
                    {
                        FightStatType.AttackPower,
                        attackPower
                    },
                    {
                        FightStatType.Initiative,
                        initiative
                    },
                    {
                        FightStatType.MaxHealth,
                        10
                    },
                    {
                        FightStatType.MaxActionPoints,
                        2
                    },
                    {
                        FightStatType.MaxMovementPoints,
                        3
                    }
                });
        }
    }
}