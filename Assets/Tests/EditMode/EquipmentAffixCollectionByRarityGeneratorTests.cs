using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixCollectionByRarityGeneratorTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () =>
                    new EquipmentAffixCollectionByRarityGenerator(
                        null,
                        new SequenceRandomSource()),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Constructor_NullRandomSource_Throws()
        {
            Assert.That(
                () =>
                    new EquipmentAffixCollectionByRarityGenerator(
                        CreateCatalog(),
                        null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Generate_NullPool_Throws()
        {
            EquipmentAffixCollectionByRarityGenerator generator =
                new EquipmentAffixCollectionByRarityGenerator(
                    CreateCatalog(),
                    new SequenceRandomSource());

            Assert.That(
                () => generator.Generate(
                    null,
                    1,
                    EquipmentItemRarity.Common),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Generate_InvalidItemLevel_Throws(
            int itemLevel)
        {
            EquipmentAffixCollectionByRarityGenerator generator =
                new EquipmentAffixCollectionByRarityGenerator(
                    CreateCatalog(),
                    new SequenceRandomSource());

            Assert.That(
                () => generator.Generate(
                    CreatePool(),
                    itemLevel,
                    EquipmentItemRarity.Common),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Generate_MagicWithTooFewAvailableAffixes_ThrowsBeforeRandomRoll()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                CreateDefinition(
                    "strength_flat",
                    FightStatType.Strength),

                new EquipmentAffixDefinition(
                    "delayed_affix",
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            10,
                            1,
                            5)
                    })
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

            EquipmentAffixCollectionByRarityGenerator generator =
                new EquipmentAffixCollectionByRarityGenerator(
                    catalog,
                    randomSource);

            Assert.That(
                () => generator.Generate(
                    pool,
                    1,
                    EquipmentItemRarity.Magic),
                Throws.TypeOf<
                    InvalidOperationException>());

            Assert.That(
                randomSource.CallCount,
                Is.EqualTo(0));
        }

        [Test]
        public void Generate_RareWithTooFewAvailableAffixes_ThrowsBeforeRandomRoll()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                CreateDefinition(
                    "strength_flat",
                    FightStatType.Strength),

                CreateDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth),

                CreateDefinition(
                    "dexterity_flat",
                    FightStatType.Dexterity)
                    });

            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10),

                new EquipmentAffixPoolEntryDefinition(
                    "vitality_flat",
                    10),

                new EquipmentAffixPoolEntryDefinition(
                    "dexterity_flat",
                    10)
                    });

            SequenceRandomSource randomSource =
                new SequenceRandomSource();

            EquipmentAffixCollectionByRarityGenerator generator =
                new EquipmentAffixCollectionByRarityGenerator(
                    catalog,
                    randomSource);

            Assert.That(
                () => generator.Generate(
                    pool,
                    1,
                    EquipmentItemRarity.Rare),
                Throws.TypeOf<
                    InvalidOperationException>());

            Assert.That(
                randomSource.CallCount,
                Is.EqualTo(0));
        }

        [Test]
        public void Generate_Common_ReturnsNoAffixes()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource();

            EquipmentAffixCollectionByRarityGenerator generator =
                new EquipmentAffixCollectionByRarityGenerator(
                    CreateCatalog(),
                    randomSource);

            IReadOnlyList<RolledEquipmentAffix> result =
                generator.Generate(
                    CreatePool(),
                    1,
                    EquipmentItemRarity.Common);

            Assert.That(
                result,
                Is.Empty);

            Assert.That(
                randomSource.CallCount,
                Is.EqualTo(0));
        }

        [Test]
        public void Generate_Magic_ReturnsTwoAffixes()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource(
                    0,
                    2,
                    0,
                    4);

            EquipmentAffixCollectionByRarityGenerator generator =
                new EquipmentAffixCollectionByRarityGenerator(
                    CreateCatalog(),
                    randomSource);

            IReadOnlyList<RolledEquipmentAffix> result =
                generator.Generate(
                    CreatePool(),
                    1,
                    EquipmentItemRarity.Magic);

            Assert.That(
                result,
                Has.Count.EqualTo(2));
        }

        [Test]
        public void Generate_Rare_ReturnsFourAffixes()
        {
            SequenceRandomSource randomSource =
                new SequenceRandomSource(
                    0,
                    1,
                    0,
                    2,
                    0,
                    3,
                    0,
                    4);

            EquipmentAffixCollectionByRarityGenerator generator =
                new EquipmentAffixCollectionByRarityGenerator(
                    CreateCatalog(),
                    randomSource);

            IReadOnlyList<RolledEquipmentAffix> result =
                generator.Generate(
                    CreatePool(),
                    1,
                    EquipmentItemRarity.Rare);

            Assert.That(
                result,
                Has.Count.EqualTo(4));
        }

        [Test]
        public void Generate_UnsupportedRarity_Throws()
        {
            EquipmentAffixCollectionByRarityGenerator generator =
                new EquipmentAffixCollectionByRarityGenerator(
                    CreateCatalog(),
                    new SequenceRandomSource());

            Assert.That(
                () => generator.Generate(
                    CreatePool(),
                    1,
                    (EquipmentItemRarity)999),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
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
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "dexterity_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "intelligence_flat",
                        10)
                });
        }

        private static EquipmentAffixDefinitionCatalog
            CreateCatalog()
        {
            return new EquipmentAffixDefinitionCatalog(
                new[]
                {
                    CreateDefinition(
                        "strength_flat",
                        FightStatType.Strength),

                    CreateDefinition(
                        "vitality_flat",
                        FightStatType.MaxHealth),

                    CreateDefinition(
                        "dexterity_flat",
                        FightStatType.Dexterity),

                    CreateDefinition(
                        "intelligence_flat",
                        FightStatType.Intelligence)
                });
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

            public int CallCount
            {
                get;
                private set;
            }

            public int Next(
                int minimumInclusive,
                int maximumExclusive)
            {
                CallCount++;

                if (values.Count == 0)
                {
                    throw new InvalidOperationException(
                        "No configured random value remains.");
                }

                return values.Dequeue();
            }
        }
    }
}