using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolTotalWeightResolverTests
    {
        [Test]
        public void Resolve_NullPool_Throws()
        {
            EquipmentAffixPoolTotalWeightResolver resolver =
                new EquipmentAffixPoolTotalWeightResolver();

            Assert.That(
                () => resolver.Resolve(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Resolve_SingleEntry_ReturnsEntryWeight()
        {
            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            10)
                    });

            EquipmentAffixPoolTotalWeightResolver resolver =
                new EquipmentAffixPoolTotalWeightResolver();

            int result =
                resolver.Resolve(pool);

            Assert.That(
                result,
                Is.EqualTo(10));
        }

        [Test]
        public void Resolve_MultipleEntries_ReturnsWeightSum()
        {
            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
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

            EquipmentAffixPoolTotalWeightResolver resolver =
                new EquipmentAffixPoolTotalWeightResolver();

            int result =
                resolver.Resolve(pool);

            Assert.That(
                result,
                Is.EqualTo(18));
        }

        [Test]
        public void Resolve_WeightSumOverflow_Throws()
        {
            EquipmentAffixPoolDefinition pool =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        new EquipmentAffixPoolEntryDefinition(
                            "strength_flat",
                            int.MaxValue),

                        new EquipmentAffixPoolEntryDefinition(
                            "vitality_flat",
                            1)
                    });

            EquipmentAffixPoolTotalWeightResolver resolver =
                new EquipmentAffixPoolTotalWeightResolver();

            Assert.That(
                () => resolver.Resolve(pool),
                Throws.TypeOf<
                    OverflowException>());
        }
    }
}