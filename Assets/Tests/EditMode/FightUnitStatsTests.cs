using System.Collections.Generic;
using NUnit.Framework;
using DiceBossArena.Game;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitStatsTests
    {
        [Test]
        public void Constructor_CopiesInitialBaseValues()
        {
            Dictionary<FightStatType, int> baseValues =
                new()
                {
                    {
                        FightStatType.MaxHealth,
                        20
                    },
                    {
                        FightStatType.AttackPower,
                        5
                    }
                };

            FightUnitStats stats =
                new FightUnitStats(baseValues);

            Assert.That(
                stats.GetBaseValue(
                    FightStatType.MaxHealth),
                Is.EqualTo(20));

            Assert.That(
                stats.GetBaseValue(
                    FightStatType.AttackPower),
                Is.EqualTo(5));
        }

        [Test]
        public void GetBaseValue_ForMissingStat_ReturnsZero()
        {
            Dictionary<FightStatType, int> baseValues =
                new();

            FightUnitStats stats =
                new FightUnitStats(baseValues);

            Assert.That(
                stats.GetBaseValue(
                    FightStatType.Initiative),
                Is.Zero);
        }

        [Test]
        public void Constructor_DoesNotKeepReferenceToSourceDictionary()
        {
            Dictionary<FightStatType, int> baseValues =
                new()
                {
                    {
                        FightStatType.AttackPower,
                        10
                    }
                };

            FightUnitStats stats =
                new FightUnitStats(baseValues);

            baseValues[FightStatType.AttackPower] =
                999;

            Assert.That(
                stats.GetBaseValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));
        }

        [Test]
        public void GetFinalValue_AppliesModifiers()
        {
            Dictionary<FightStatType, int> baseValues =
                new()
                {
            {
                FightStatType.AttackPower,
                10
            }
                };

            FightUnitStats stats =
                new FightUnitStats(baseValues);

            stats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5));

            stats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Percent,
                    20));

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(18));
        }

        [Test]
        public void AddModifier_AffectsOnlyMatchingStat()
        {
            Dictionary<FightStatType, int> baseValues =
                new()
                {
            {
                FightStatType.AttackPower,
                10
            },
            {
                FightStatType.MaxHealth,
                20
            }
                };

            FightUnitStats stats =
                new FightUnitStats(baseValues);

            stats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5));

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(15));

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.MaxHealth),
                Is.EqualTo(20));
        }

        [Test]
        public void SeparateInstances_DoNotShareModifiers()
        {
            Dictionary<FightStatType, int> baseValues =
                new()
                {
            {
                FightStatType.AttackPower,
                10
            }
                };

            FightUnitStats firstStats =
                new FightUnitStats(baseValues);

            FightUnitStats secondStats =
                new FightUnitStats(baseValues);

            firstStats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5));

            Assert.That(
                firstStats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(15));

            Assert.That(
                secondStats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));
        }

        [Test]
        public void RemoveModifier_RemovesMatchingModifier()
        {
            Dictionary<FightStatType, int> baseValues =
                new()
                {
            {
                FightStatType.AttackPower,
                10
            }
                };

            FightUnitStats stats =
                new FightUnitStats(baseValues);

            FightStatModifier modifier =
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5);

            stats.AddModifier(modifier);

            bool removed =
                stats.RemoveModifier(modifier);

            Assert.That(
                removed,
                Is.True);

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));
        }

        [Test]
        public void RemoveModifier_WhenModifierDoesNotExist_ReturnsFalse()
        {
            Dictionary<FightStatType, int> baseValues =
                new()
                {
            {
                FightStatType.AttackPower,
                10
            }
                };

            FightUnitStats stats =
                new FightUnitStats(baseValues);

            FightStatModifier modifier =
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5);

            bool removed =
                stats.RemoveModifier(modifier);

            Assert.That(
                removed,
                Is.False);
        }

        [Test]
        public void ClearModifiers_RemovesAllModifiers()
        {
            Dictionary<FightStatType, int> baseValues =
                new()
                {
            {
                FightStatType.AttackPower,
                10
            },
            {
                FightStatType.MaxHealth,
                20
            }
                };

            FightUnitStats stats =
                new FightUnitStats(baseValues);

            stats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5));

            stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Percent,
                    50));

            stats.ClearModifiers();

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.AttackPower),
                Is.EqualTo(10));

            Assert.That(
                stats.GetFinalValue(
                    FightStatType.MaxHealth),
                Is.EqualTo(20));
        }
    }
}