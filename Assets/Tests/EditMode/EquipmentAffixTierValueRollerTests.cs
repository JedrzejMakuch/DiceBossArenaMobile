using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixTierValueRollerTests
    {
        [Test]
        public void Constructor_NullRandomSource_Throws()
        {
            Assert.That(
                () => new EquipmentAffixTierValueRoller(
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Roll_NullTier_Throws()
        {
            EquipmentAffixTierValueRoller roller =
                new EquipmentAffixTierValueRoller(
                    new StubRandomSource(1));

            Assert.That(
                () => roller.Roll(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Roll_RequestsInclusiveTierValueRange()
        {
            StubRandomSource randomSource =
                new StubRandomSource(3);

            EquipmentAffixTierValueRoller roller =
                new EquipmentAffixTierValueRoller(
                    randomSource);

            EquipmentAffixTierDefinition tier =
                new EquipmentAffixTierDefinition(
                    1,
                    2,
                    5);

            roller.Roll(tier);

            Assert.That(
                randomSource.LastMinimumInclusive,
                Is.EqualTo(2));

            Assert.That(
                randomSource.LastMaximumExclusive,
                Is.EqualTo(6));
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(5)]
        public void Roll_ReturnsRandomSourceValue(
            int randomValue)
        {
            EquipmentAffixTierValueRoller roller =
                new EquipmentAffixTierValueRoller(
                    new StubRandomSource(
                        randomValue));

            EquipmentAffixTierDefinition tier =
                new EquipmentAffixTierDefinition(
                    1,
                    2,
                    5);

            int result =
                roller.Roll(tier);

            Assert.That(
                result,
                Is.EqualTo(randomValue));
        }

        [Test]
        public void Roll_EqualMinimumAndMaximum_RequestsSingleValueRange()
        {
            StubRandomSource randomSource =
                new StubRandomSource(4);

            EquipmentAffixTierValueRoller roller =
                new EquipmentAffixTierValueRoller(
                    randomSource);

            EquipmentAffixTierDefinition tier =
                new EquipmentAffixTierDefinition(
                    1,
                    4,
                    4);

            int result =
                roller.Roll(tier);

            Assert.That(
                randomSource.LastMinimumInclusive,
                Is.EqualTo(4));

            Assert.That(
                randomSource.LastMaximumExclusive,
                Is.EqualTo(5));

            Assert.That(
                result,
                Is.EqualTo(4));
        }

        [Test]
        public void Roll_MaximumValueOverflow_Throws()
        {
            EquipmentAffixTierValueRoller roller =
                new EquipmentAffixTierValueRoller(
                    new StubRandomSource(0));

            EquipmentAffixTierDefinition tier =
                new EquipmentAffixTierDefinition(
                    1,
                    1,
                    int.MaxValue);

            Assert.That(
                () => roller.Roll(tier),
                Throws.TypeOf<
                    OverflowException>());
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