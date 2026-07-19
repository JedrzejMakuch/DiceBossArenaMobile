using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolAvailabilityFilterTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () =>
                    new EquipmentAffixPoolAvailabilityFilter(
                        null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Filter_NullPool_Throws()
        {
            EquipmentAffixPoolAvailabilityFilter filter =
                new EquipmentAffixPoolAvailabilityFilter(
                    CreateCatalog());

            Assert.That(
                () => filter.Filter(
                    null,
                    1),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Filter_InvalidItemLevel_Throws(
            int itemLevel)
        {
            EquipmentAffixPoolAvailabilityFilter filter =
                new EquipmentAffixPoolAvailabilityFilter(
                    CreateCatalog());

            Assert.That(
                () => filter.Filter(
                    CreatePool(),
                    itemLevel),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Filter_NoAvailableEntries_ReturnsEmptyPool()
        {
            EquipmentAffixPoolAvailabilityFilter filter =
                new EquipmentAffixPoolAvailabilityFilter(
                    CreateCatalog());

            EquipmentAffixPoolDefinition result =
                filter.Filter(
                    CreatePool(),
                    4);

            Assert.That(
                result,
                Is.Not.Null);

            Assert.That(
                result.Entries,
                Is.Empty);
        }

        [Test]
        public void Filter_ReturnsOnlyEntriesAvailableForItemLevel()
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolAvailabilityFilter filter =
                new EquipmentAffixPoolAvailabilityFilter(
                    CreateCatalog());

            EquipmentAffixPoolDefinition result =
                filter.Filter(
                    pool,
                    7);

            Assert.That(
                result.Entries,
                Has.Count.EqualTo(1));

            Assert.That(
                result.Entries[0],
                Is.SameAs(
                    pool.Entries[0]));
        }

        [Test]
        public void Filter_AllEntriesAvailable_ReturnsAllEntries()
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolAvailabilityFilter filter =
                new EquipmentAffixPoolAvailabilityFilter(
                    CreateCatalog());

            EquipmentAffixPoolDefinition result =
                filter.Filter(
                    pool,
                    10);

            Assert.That(
                result.Entries,
                Has.Count.EqualTo(2));

            Assert.That(
                result.Entries[0],
                Is.SameAs(
                    pool.Entries[0]));

            Assert.That(
                result.Entries[1],
                Is.SameAs(
                    pool.Entries[1]));
        }

        [Test]
        public void Filter_DoesNotModifyOriginalPool()
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolAvailabilityFilter filter =
                new EquipmentAffixPoolAvailabilityFilter(
                    CreateCatalog());

            filter.Filter(
                pool,
                7);

            Assert.That(
                pool.Entries,
                Has.Count.EqualTo(2));
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
    }
}