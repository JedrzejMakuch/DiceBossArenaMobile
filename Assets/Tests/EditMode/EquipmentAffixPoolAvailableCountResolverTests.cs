using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolAvailableCountResolverTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () =>
                    new EquipmentAffixPoolAvailableCountResolver(
                        null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Resolve_NullPool_Throws()
        {
            EquipmentAffixPoolAvailableCountResolver resolver =
                new EquipmentAffixPoolAvailableCountResolver(
                    CreateCatalog());

            Assert.That(
                () => resolver.Resolve(
                    null,
                    1),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void Resolve_InvalidItemLevel_Throws(
            int itemLevel)
        {
            EquipmentAffixPoolAvailableCountResolver resolver =
                new EquipmentAffixPoolAvailableCountResolver(
                    CreateCatalog());

            Assert.That(
                () => resolver.Resolve(
                    CreatePool(),
                    itemLevel),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void Resolve_NoAvailableEntries_ReturnsZero()
        {
            EquipmentAffixPoolAvailableCountResolver resolver =
                new EquipmentAffixPoolAvailableCountResolver(
                    CreateCatalog());

            int result =
                resolver.Resolve(
                    CreatePool(),
                    4);

            Assert.That(
                result,
                Is.EqualTo(0));
        }

        [Test]
        public void Resolve_OneAvailableEntry_ReturnsOne()
        {
            EquipmentAffixPoolAvailableCountResolver resolver =
                new EquipmentAffixPoolAvailableCountResolver(
                    CreateCatalog());

            int result =
                resolver.Resolve(
                    CreatePool(),
                    7);

            Assert.That(
                result,
                Is.EqualTo(1));
        }

        [Test]
        public void Resolve_AllAvailableEntries_ReturnsPoolCount()
        {
            EquipmentAffixPoolAvailableCountResolver resolver =
                new EquipmentAffixPoolAvailableCountResolver(
                    CreateCatalog());

            int result =
                resolver.Resolve(
                    CreatePool(),
                    20);

            Assert.That(
                result,
                Is.EqualTo(3));
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

        private static EquipmentAffixDefinitionCatalog
            CreateCatalog()
        {
            return new EquipmentAffixDefinitionCatalog(
                new[]
                {
                    CreateDefinition(
                        "strength_flat",
                        FightStatType.Strength,
                        5),

                    CreateDefinition(
                        "vitality_flat",
                        FightStatType.MaxHealth,
                        10),

                    CreateDefinition(
                        "initiative_flat",
                        FightStatType.Initiative,
                        15)
                });
        }

        private static EquipmentAffixDefinition
            CreateDefinition(
                string id,
                FightStatType statType,
                int minimumItemLevel)
        {
            return new EquipmentAffixDefinition(
                id,
                statType,
                FightStatModifierType.Flat,
                new[]
                {
                    new EquipmentAffixTierDefinition(
                        minimumItemLevel,
                        1,
                        5)
                });
        }
    }
}