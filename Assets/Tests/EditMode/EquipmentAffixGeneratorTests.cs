using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EquipmentAffixGeneratorTests
    {
        [Test]
        public void Constructor_NullRandomSource_Throws()
        {
            Assert.That(
                () => new EquipmentAffixGenerator(
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Generate_NullDefinition_Throws()
        {
            EquipmentAffixGenerator generator =
                new EquipmentAffixGenerator(
                    new StubRandomSource(1));

            Assert.That(
                () => generator.Generate(
                    null,
                    1),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Generate_InvalidItemLevel_Throws(
            int itemLevel)
        {
            EquipmentAffixGenerator generator =
                new EquipmentAffixGenerator(
                    new StubRandomSource(1));

            Assert.That(
                () => generator.Generate(
                    CreateDefinition(),
                    itemLevel),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Generate_LevelBelowFirstTier_ReturnsNull()
        {
            EquipmentAffixGenerator generator =
                new EquipmentAffixGenerator(
                    new StubRandomSource(1));

            RolledEquipmentAffix result =
                generator.Generate(
                    CreateDefinition(),
                    4);

            Assert.That(
                result,
                Is.Null);
        }

        [Test]
        public void Generate_UsesHighestAvailableTierRange()
        {
            StubRandomSource randomSource =
                new StubRandomSource(6);

            EquipmentAffixGenerator generator =
                new EquipmentAffixGenerator(
                    randomSource);

            generator.Generate(
                CreateDefinition(),
                15);

            Assert.That(
                randomSource.LastMinimumInclusive,
                Is.EqualTo(4));

            Assert.That(
                randomSource.LastMaximumExclusive,
                Is.EqualTo(8));
        }

        [Test]
        public void Generate_CreatesRolledAffixFromDefinition()
        {
            EquipmentAffixGenerator generator =
                new EquipmentAffixGenerator(
                    new StubRandomSource(6));

            RolledEquipmentAffix result =
                generator.Generate(
                    CreateDefinition(),
                    15);

            Assert.That(
                result,
                Is.Not.Null);

            Assert.That(
                result.AffixId,
                Is.EqualTo(
                    new EquipmentAffixId(
                        "strength_flat")));

            Assert.That(
                result.StatType,
                Is.EqualTo(
                    FightStatType.Strength));

            Assert.That(
                result.ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));

            Assert.That(
                result.Value,
                Is.EqualTo(6));
        }

        private static EquipmentAffixDefinition
            CreateDefinition()
        {
            return new EquipmentAffixDefinition(
                "strength_flat",
                FightStatType.Strength,
                FightStatModifierType.Flat,
                new[]
                {
                    new EquipmentAffixTierDefinition(
                        5,
                        1,
                        3),

                    new EquipmentAffixTierDefinition(
                        10,
                        4,
                        7),

                    new EquipmentAffixTierDefinition(
                        20,
                        8,
                        12)
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