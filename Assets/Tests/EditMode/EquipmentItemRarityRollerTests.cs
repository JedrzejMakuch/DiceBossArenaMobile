using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentItemRarityRollerTests
    {
        [Test]
        public void Constructor_NullRandomSource_Throws()
        {
            Assert.That(
                () => new EquipmentItemRarityRoller(
                    null,
                    70,
                    25,
                    5),
                Throws.TypeOf<ArgumentNullException>());
        }

        [TestCase(-1, 0, 0)]
        [TestCase(0, -1, 0)]
        [TestCase(0, 0, -1)]
        public void Constructor_NegativeWeight_Throws(
            int commonWeight,
            int magicWeight,
            int rareWeight)
        {
            Assert.That(
                () => new EquipmentItemRarityRoller(
                    new StubRandomSource(0),
                    commonWeight,
                    magicWeight,
                    rareWeight),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Constructor_AllWeightsZero_Throws()
        {
            Assert.That(
                () => new EquipmentItemRarityRoller(
                    new StubRandomSource(0),
                    0,
                    0,
                    0),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Roll_RequestsFullWeightRange()
        {
            StubRandomSource randomSource =
                new StubRandomSource(0);

            EquipmentItemRarityRoller roller =
                new EquipmentItemRarityRoller(
                    randomSource,
                    70,
                    25,
                    5);

            roller.Roll();

            Assert.That(
                randomSource.LastMinimumInclusive,
                Is.EqualTo(0));

            Assert.That(
                randomSource.LastMaximumExclusive,
                Is.EqualTo(100));
        }

        [TestCase(0, EquipmentItemRarity.Common)]
        [TestCase(69, EquipmentItemRarity.Common)]
        [TestCase(70, EquipmentItemRarity.Magic)]
        [TestCase(94, EquipmentItemRarity.Magic)]
        [TestCase(95, EquipmentItemRarity.Rare)]
        [TestCase(99, EquipmentItemRarity.Rare)]
        public void Roll_ReturnsRarityResolvedFromWeight(
            int randomValue,
            EquipmentItemRarity expectedRarity)
        {
            EquipmentItemRarityRoller roller =
                new EquipmentItemRarityRoller(
                    new StubRandomSource(
                        randomValue),
                    70,
                    25,
                    5);

            EquipmentItemRarity result =
                roller.Roll();

            Assert.That(
                result,
                Is.EqualTo(expectedRarity));
        }

        [Test]
        public void Roll_ZeroWeightRarityIsSkipped()
        {
            EquipmentItemRarityRoller roller =
                new EquipmentItemRarityRoller(
                    new StubRandomSource(0),
                    0,
                    10,
                    0);

            EquipmentItemRarity result =
                roller.Roll();

            Assert.That(
                result,
                Is.EqualTo(
                    EquipmentItemRarity.Magic));
        }

        private sealed class StubRandomSource :
            IEquipmentAffixRandomSource
        {
            private readonly int value;

            public StubRandomSource(
                int newValue)
            {
                value = newValue;
            }

            public int LastMinimumInclusive
            {
                get;
                private set;
            }

            public int LastMaximumExclusive
            {
                get;
                private set;
            }

            public int Next(
                int minimumInclusive,
                int maximumExclusive)
            {
                LastMinimumInclusive =
                    minimumInclusive;

                LastMaximumExclusive =
                    maximumExclusive;

                return value;
            }
        }
    }
}