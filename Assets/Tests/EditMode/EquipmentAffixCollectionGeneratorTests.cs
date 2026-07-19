using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixCollectionGeneratorTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () => new EquipmentAffixCollectionGenerator(
                    null,
                    new SequenceRandomSource()),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Constructor_NullRandomSource_Throws()
        {
            Assert.That(
                () => new EquipmentAffixCollectionGenerator(
                    CreateCatalog(),
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Generate_NullPool_Throws()
        {
            EquipmentAffixCollectionGenerator generator =
                new EquipmentAffixCollectionGenerator(
                    CreateCatalog(),
                    new SequenceRandomSource());

            Assert.That(
                () => generator.Generate(
                    null,
                    1,
                    0),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Generate_InvalidItemLevel_Throws(
            int itemLevel)
        {
            EquipmentAffixCollectionGenerator generator =
                new EquipmentAffixCollectionGenerator(
                    CreateCatalog(),
                    new SequenceRandomSource());

            Assert.That(
                () => generator.Generate(
                    CreatePool(),
                    itemLevel,
                    0),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Generate_NegativeAffixCount_Throws()
        {
            EquipmentAffixCollectionGenerator generator =
                new EquipmentAffixCollectionGenerator(
                    CreateCatalog(),
                    new SequenceRandomSource());

            Assert.That(
                () => generator.Generate(
                    CreatePool(),
                    1,
                    -1),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Generate_ZeroAffixes_ReturnsEmptyCollection()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource();

            EquipmentAffixCollectionGenerator generator =
                new EquipmentAffixCollectionGenerator(
                    CreateCatalog(),
                    randomSource);

            IReadOnlyList<RolledEquipmentAffix> result =
                generator.Generate(
                    CreatePool(),
                    1,
                    0);

            Assert.That(
                result,
                Is.Empty);

            Assert.That(
                randomSource.CallCount,
                Is.EqualTo(0));
        }

        [Test]
        public void Generate_MultipleAffixes_ReturnsUniqueAffixIds()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource(
                    0,
                    2,
                    0,
                    4);

            EquipmentAffixCollectionGenerator generator =
                new EquipmentAffixCollectionGenerator(
                    CreateCatalog(),
                    randomSource);

            IReadOnlyList<RolledEquipmentAffix> result =
                generator.Generate(
                    CreatePool(),
                    1,
                    2);

            Assert.That(
                result,
                Has.Count.EqualTo(2));

            Assert.That(
                result[0].AffixId,
                Is.EqualTo(
                    new EquipmentAffixId(
                        "strength_flat")));

            Assert.That(
                result[0].Value,
                Is.EqualTo(2));

            Assert.That(
                result[1].AffixId,
                Is.EqualTo(
                    new EquipmentAffixId(
                        "vitality_flat")));

            Assert.That(
                result[1].Value,
                Is.EqualTo(4));

            Assert.That(
                result[0].AffixId,
                Is.Not.EqualTo(
                    result[1].AffixId));
        }

        [Test]
        public void Generate_RemovesRolledAffixFromNextPool()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource(
                    0,
                    2,
                    0,
                    4);

            EquipmentAffixCollectionGenerator generator =
                new EquipmentAffixCollectionGenerator(
                    CreateCatalog(),
                    randomSource);

            generator.Generate(
                CreatePool(),
                1,
                2);

            Assert.That(
                randomSource.Requests[0].MinimumInclusive,
                Is.EqualTo(0));

            Assert.That(
                randomSource.Requests[0].MaximumExclusive,
                Is.EqualTo(15));

            Assert.That(
                randomSource.Requests[2].MinimumInclusive,
                Is.EqualTo(0));

            Assert.That(
                randomSource.Requests[2].MaximumExclusive,
                Is.EqualTo(5));
        }

        [Test]
        public void Generate_NotEnoughUniqueAffixes_ThrowsBeforeRandomRoll()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource();

            EquipmentAffixCollectionGenerator generator =
                new EquipmentAffixCollectionGenerator(
                    CreateCatalog(),
                    randomSource);

            Assert.That(
                () => generator.Generate(
                    CreatePool(),
                    1,
                    3),
                Throws.TypeOf<
                    InvalidOperationException>());

            Assert.That(
                randomSource.CallCount,
                Is.EqualTo(0));
        }

        [Test]
        public void Generate_NotEnoughAffixesAvailableForItemLevel_ThrowsBeforeRandomRoll()
        {
            EquipmentAffixDefinition delayedAffix =
                new EquipmentAffixDefinition(
                    "delayed_affix",
                    FightStatType.Intelligence,
                    FightStatModifierType.Flat,
                    new[]
                    {
                new EquipmentAffixTierDefinition(
                    10,
                    1,
                    5)
                    });

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                CreateDefinition(
                    "strength_flat",
                    FightStatType.Strength),

                delayedAffix
                    });

            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10),

                new EquipmentAffixPoolEntryDefinition(
                    "delayed_affix",
                    10)
                    });

            SequenceRandomSource randomSource =
                new SequenceRandomSource();

            EquipmentAffixCollectionGenerator generator =
                new EquipmentAffixCollectionGenerator(
                    catalog,
                    randomSource);

            Assert.That(
                () => generator.Generate(
                    pool,
                    1,
                    2),
                Throws.TypeOf<
                    InvalidOperationException>());

            Assert.That(
                randomSource.CallCount,
                Is.EqualTo(0));
        }

        private static EquipmentAffixDefinition
    CreateDefinition(
        string id,
        FightStatType statType)
        {
            return new EquipmentAffixDefinition(
                id,
                statType,
                FightStatModifierType.Flat,
                new[]
                {
            new EquipmentAffixTierDefinition(
                1,
                1,
                5)
                });
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
                            1,
                            1,
                            3)
                    });

            EquipmentAffixDefinition vitality =
                new EquipmentAffixDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            1,
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