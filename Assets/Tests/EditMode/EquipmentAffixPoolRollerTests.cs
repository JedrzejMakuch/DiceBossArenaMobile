using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EquipmentAffixPoolRollerTests
    {
        [Test]
        public void Constructor_NullRandomSource_Throws()
        {
            Assert.That(
                () => new EquipmentAffixPoolRoller(
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Roll_NullPool_Throws()
        {
            EquipmentAffixPoolRoller roller =
                new EquipmentAffixPoolRoller(
                    new StubRandomSource(0));

            Assert.That(
                () => roller.Roll(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Roll_RequestsFullPoolWeightRange()
        {
            StubRandomSource randomSource =
                new StubRandomSource(0);

            EquipmentAffixPoolRoller roller =
                new EquipmentAffixPoolRoller(
                    randomSource);

            roller.Roll(CreatePool());

            Assert.That(
                randomSource.LastMinimumInclusive,
                Is.EqualTo(0));

            Assert.That(
                randomSource.LastMaximumExclusive,
                Is.EqualTo(18));
        }

        [TestCase(0, 0)]
        [TestCase(9, 0)]
        [TestCase(10, 1)]
        [TestCase(14, 1)]
        [TestCase(15, 2)]
        [TestCase(17, 2)]
        public void Roll_ReturnsEntryResolvedFromRandomValue(
            int randomValue,
            int expectedEntryIndex)
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolRoller roller =
                new EquipmentAffixPoolRoller(
                    new StubRandomSource(
                        randomValue));

            EquipmentAffixPoolEntryDefinition result =
                roller.Roll(pool);

            Assert.That(
                result,
                Is.SameAs(
                    pool.Entries[
                        expectedEntryIndex]));
        }

        private static EquipmentAffixPoolDefinition
            CreatePool()
        {
            return new EquipmentAffixPoolDefinition(
                new[]
                {
                    new EquipmentAffixPoolEntryDefinition(
                        "strength_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "vitality_flat",
                        5),

                    new EquipmentAffixPoolEntryDefinition(
                        "initiative_flat",
                        3)
                });
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