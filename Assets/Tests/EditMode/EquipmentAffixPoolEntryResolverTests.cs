using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolEntryResolverTests
    {
        [Test]
        public void Resolve_NullPool_Throws()
        {
            EquipmentAffixPoolEntryResolver resolver =
                new EquipmentAffixPoolEntryResolver();

            Assert.That(
                () => resolver.Resolve(
                    null,
                    0),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(-1)]
        [TestCase(18)]
        [TestCase(19)]
        public void Resolve_RollOutsidePoolRange_Throws(
            int roll)
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolEntryResolver resolver =
                new EquipmentAffixPoolEntryResolver();

            Assert.That(
                () => resolver.Resolve(
                    pool,
                    roll),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [TestCase(0)]
        [TestCase(5)]
        [TestCase(9)]
        public void Resolve_RollWithinFirstEntryRange_ReturnsFirstEntry(
            int roll)
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolEntryResolver resolver =
                new EquipmentAffixPoolEntryResolver();

            EquipmentAffixPoolEntryDefinition result =
                resolver.Resolve(
                    pool,
                    roll);

            Assert.That(
                result,
                Is.SameAs(
                    pool.Entries[0]));
        }

        [TestCase(10)]
        [TestCase(12)]
        [TestCase(14)]
        public void Resolve_RollWithinSecondEntryRange_ReturnsSecondEntry(
            int roll)
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolEntryResolver resolver =
                new EquipmentAffixPoolEntryResolver();

            EquipmentAffixPoolEntryDefinition result =
                resolver.Resolve(
                    pool,
                    roll);

            Assert.That(
                result,
                Is.SameAs(
                    pool.Entries[1]));
        }

        [TestCase(15)]
        [TestCase(16)]
        [TestCase(17)]
        public void Resolve_RollWithinThirdEntryRange_ReturnsThirdEntry(
            int roll)
        {
            EquipmentAffixPoolDefinition pool =
                CreatePool();

            EquipmentAffixPoolEntryResolver resolver =
                new EquipmentAffixPoolEntryResolver();

            EquipmentAffixPoolEntryDefinition result =
                resolver.Resolve(
                    pool,
                    roll);

            Assert.That(
                result,
                Is.SameAs(
                    pool.Entries[2]));
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