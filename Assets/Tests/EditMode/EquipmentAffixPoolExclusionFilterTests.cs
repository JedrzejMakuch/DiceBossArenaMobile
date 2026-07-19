using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolExclusionFilterTests
    {
        [Test]
        public void Filter_NullPool_Throws()
        {
            EquipmentAffixPoolExclusionFilter filter =
                new EquipmentAffixPoolExclusionFilter();

            Assert.That(
                () => filter.Filter(
                    null,
                    Array.Empty<
                        EquipmentAffixId>()),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Filter_NullExcludedIds_Throws()
        {
            EquipmentAffixPoolExclusionFilter filter =
                new EquipmentAffixPoolExclusionFilter();

            Assert.That(
                () => filter.Filter(
                    CreatePool(),
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Filter_NoExcludedIds_ReturnsAllEntries()
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolExclusionFilter filter =
                new EquipmentAffixPoolExclusionFilter();

            EquipmentAffixPoolDefinition result =
                filter.Filter(
                    pool,
                    Array.Empty<
                        EquipmentAffixId>());

            Assert.That(
                result.Entries,
                Has.Count.EqualTo(3));

            Assert.That(
                result.Entries[0],
                Is.SameAs(
                    pool.Entries[0]));

            Assert.That(
                result.Entries[1],
                Is.SameAs(
                    pool.Entries[1]));

            Assert.That(
                result.Entries[2],
                Is.SameAs(
                    pool.Entries[2]));
        }

        [Test]
        public void Filter_RemovesExcludedEntries()
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolExclusionFilter filter =
                new EquipmentAffixPoolExclusionFilter();

            EquipmentAffixPoolDefinition result =
                filter.Filter(
                    pool,
                    new[]
                    {
                        new EquipmentAffixId(
                            "strength_flat"),

                        new EquipmentAffixId(
                            "initiative_flat")
                    });

            Assert.That(
                result.Entries,
                Has.Count.EqualTo(1));

            Assert.That(
                result.Entries[0],
                Is.SameAs(
                    pool.Entries[1]));
        }

        [Test]
        public void Filter_UnknownExcludedId_DoesNotRemoveEntries()
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolExclusionFilter filter =
                new EquipmentAffixPoolExclusionFilter();

            EquipmentAffixPoolDefinition result =
                filter.Filter(
                    pool,
                    new[]
                    {
                        new EquipmentAffixId(
                            "unknown_affix")
                    });

            Assert.That(
                result.Entries,
                Has.Count.EqualTo(3));
        }

        [Test]
        public void Filter_AllEntriesExcluded_ReturnsEmptyPool()
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolExclusionFilter filter =
                new EquipmentAffixPoolExclusionFilter();

            EquipmentAffixPoolDefinition result =
                filter.Filter(
                    pool,
                    new[]
                    {
                        new EquipmentAffixId(
                            "strength_flat"),

                        new EquipmentAffixId(
                            "vitality_flat"),

                        new EquipmentAffixId(
                            "initiative_flat")
                    });

            Assert.That(
                result.Entries,
                Is.Empty);
        }

        [Test]
        public void Filter_DoesNotModifyOriginalPool()
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolExclusionFilter filter =
                new EquipmentAffixPoolExclusionFilter();

            filter.Filter(
                pool,
                new[]
                {
                    new EquipmentAffixId(
                        "strength_flat")
                });

            Assert.That(
                pool.Entries,
                Has.Count.EqualTo(3));
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
    }
}