using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolEntryAvailabilityResolverTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () =>
                    new EquipmentAffixPoolEntryAvailabilityResolver(
                        null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void IsAvailable_NullEntry_Throws()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    Array.Empty<
                        EquipmentAffixDefinition>());

            EquipmentAffixPoolEntryAvailabilityResolver resolver =
                new EquipmentAffixPoolEntryAvailabilityResolver(
                    catalog);

            Assert.That(
                () => resolver.IsAvailable(
                    null,
                    1),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void IsAvailable_InvalidItemLevel_Throws(
            int itemLevel)
        {
            EquipmentAffixDefinitionCatalog catalog =
                CreateCatalog();

            EquipmentAffixPoolEntryAvailabilityResolver resolver =
                new EquipmentAffixPoolEntryAvailabilityResolver(
                    catalog);

            EquipmentAffixPoolEntryDefinition entry =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10);

            Assert.That(
                () => resolver.IsAvailable(
                    entry,
                    itemLevel),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }

        [Test]
        public void IsAvailable_LevelBelowFirstTier_ReturnsFalse()
        {
            EquipmentAffixPoolEntryAvailabilityResolver resolver =
                new EquipmentAffixPoolEntryAvailabilityResolver(
                    CreateCatalog());

            EquipmentAffixPoolEntryDefinition entry =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10);

            bool result =
                resolver.IsAvailable(
                    entry,
                    4);

            Assert.That(
                result,
                Is.False);
        }

        [Test]
        public void IsAvailable_LevelEqualToFirstTier_ReturnsTrue()
        {
            EquipmentAffixPoolEntryAvailabilityResolver resolver =
                new EquipmentAffixPoolEntryAvailabilityResolver(
                    CreateCatalog());

            EquipmentAffixPoolEntryDefinition entry =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10);

            bool result =
                resolver.IsAvailable(
                    entry,
                    5);

            Assert.That(
                result,
                Is.True);
        }

        [Test]
        public void IsAvailable_LevelAboveFirstTier_ReturnsTrue()
        {
            EquipmentAffixPoolEntryAvailabilityResolver resolver =
                new EquipmentAffixPoolEntryAvailabilityResolver(
                    CreateCatalog());

            EquipmentAffixPoolEntryDefinition entry =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10);

            bool result =
                resolver.IsAvailable(
                    entry,
                    20);

            Assert.That(
                result,
                Is.True);
        }

        [Test]
        public void IsAvailable_UnknownAffixId_Throws()
        {
            EquipmentAffixPoolEntryAvailabilityResolver resolver =
                new EquipmentAffixPoolEntryAvailabilityResolver(
                    CreateCatalog());

            EquipmentAffixPoolEntryDefinition entry =
                new EquipmentAffixPoolEntryDefinition(
                    "unknown_affix",
                    10);

            Assert.That(
                () => resolver.IsAvailable(
                    entry,
                    10),
                Throws.TypeOf<
                    KeyNotFoundException>());
        }

        private static EquipmentAffixDefinitionCatalog
            CreateCatalog()
        {
            EquipmentAffixDefinition definition =
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

            return new EquipmentAffixDefinitionCatalog(
                new[]
                {
                    definition
                });
        }
    }
}