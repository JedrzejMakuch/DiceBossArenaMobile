using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixFromPoolGeneratorTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () => new EquipmentAffixFromPoolGenerator(
                    null,
                    new SequenceRandomSource(0)),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Constructor_NullRandomSource_Throws()
        {
            Assert.That(
                () => new EquipmentAffixFromPoolGenerator(
                    CreateCatalog(),
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Generate_NullPool_Throws()
        {
            EquipmentAffixFromPoolGenerator generator =
                new EquipmentAffixFromPoolGenerator(
                    CreateCatalog(),
                    new SequenceRandomSource(0));

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
            EquipmentAffixFromPoolGenerator generator =
                new EquipmentAffixFromPoolGenerator(
                    CreateCatalog(),
                    new SequenceRandomSource(0));

            Assert.That(
                () => generator.Generate(
                    CreatePool(),
                    itemLevel),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Generate_NoAvailableAffixes_ReturnsNull()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource();

            EquipmentAffixFromPoolGenerator generator =
                new EquipmentAffixFromPoolGenerator(
                    CreateCatalog(),
                    randomSource);

            RolledEquipmentAffix result =
                generator.Generate(
                    CreatePool(),
                    4);

            Assert.That(
                result,
                Is.Null);

            Assert.That(
                randomSource.CallCount,
                Is.EqualTo(0));
        }

        [Test]
        public void Generate_FiltersUnavailableAffixesBeforeRolling()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource(
                    9,
                    3);

            EquipmentAffixFromPoolGenerator generator =
                new EquipmentAffixFromPoolGenerator(
                    CreateCatalog(),
                    randomSource);

            RolledEquipmentAffix result =
                generator.Generate(
                    CreatePool(),
                    7);

            Assert.That(
                result,
                Is.Not.Null);

            Assert.That(
                result.AffixId,
                Is.EqualTo(
                    new EquipmentAffixId(
                        "strength_flat")));

            Assert.That(
                randomSource.Requests[0].MinimumInclusive,
                Is.EqualTo(0));

            Assert.That(
                randomSource.Requests[0].MaximumExclusive,
                Is.EqualTo(10));
        }

        [Test]
        public void Generate_RollsEntryAndAffixValue()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource(
                    12,
                    4);

            EquipmentAffixFromPoolGenerator generator =
                new EquipmentAffixFromPoolGenerator(
                    CreateCatalog(),
                    randomSource);

            RolledEquipmentAffix result =
                generator.Generate(
                    CreatePool(),
                    10);

            Assert.That(
                result,
                Is.Not.Null);

            Assert.That(
                result.AffixId,
                Is.EqualTo(
                    new EquipmentAffixId(
                        "vitality_flat")));

            Assert.That(
                result.StatType,
                Is.EqualTo(
                    FightStatType.MaxHealth));

            Assert.That(
                result.ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));

            Assert.That(
                result.Value,
                Is.EqualTo(4));

            Assert.That(
                randomSource.CallCount,
                Is.EqualTo(2));

            Assert.That(
                randomSource.Requests[0].MinimumInclusive,
                Is.EqualTo(0));

            Assert.That(
                randomSource.Requests[0].MaximumExclusive,
                Is.EqualTo(15));

            Assert.That(
                randomSource.Requests[1].MinimumInclusive,
                Is.EqualTo(2));

            Assert.That(
                randomSource.Requests[1].MaximumExclusive,
                Is.EqualTo(6));
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
                        5)
                });
        }

        private static EquipmentAffixDefinitionCatalog
            CreateCatalog()
        {
            EquipmentAffixDefinition strength =
                new EquipmentAffixDefinition(
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
                            7)
                    });

            EquipmentAffixDefinition vitality =
                new EquipmentAffixDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            10,
                            2,
                            5)
                    });

            return new EquipmentAffixDefinitionCatalog(
                new[]
                {
                    strength,
                    vitality
                });
        }

        private sealed class SequenceRandomSource :
            IEquipmentAffixRandomSource
        {
            private readonly Queue<int>
                values;

            public SequenceRandomSource(
                params int[] newValues)
            {
                values =
                    new Queue<int>(
                        newValues);
            }

            public List<RandomRequest> Requests
            {
                get;
            } = new List<RandomRequest>();

            public int CallCount =>
                Requests.Count;

            public int Next(
                int minimumInclusive,
                int maximumExclusive)
            {
                Requests.Add(
                    new RandomRequest(
                        minimumInclusive,
                        maximumExclusive));

                if (values.Count == 0)
                {
                    throw new InvalidOperationException(
                        "No configured random value remains.");
                }

                return values.Dequeue();
            }
        }

        private readonly struct RandomRequest
        {
            public RandomRequest(
                int minimumInclusive,
                int maximumExclusive)
            {
                MinimumInclusive =
                    minimumInclusive;

                MaximumExclusive =
                    maximumExclusive;
            }

            public int MinimumInclusive
            {
                get;
            }

            public int MaximumExclusive
            {
                get;
            }
        }
    }
}