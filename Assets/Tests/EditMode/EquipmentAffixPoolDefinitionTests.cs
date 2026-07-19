using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolDefinitionTests
    {
        [Test]
        public void Constructor_AssignsEntries()
        {
            EquipmentAffixPoolEntryDefinition firstEntry =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10);

            EquipmentAffixPoolEntryDefinition secondEntry =
                new EquipmentAffixPoolEntryDefinition(
                    "vitality_flat",
                    5);

            EquipmentAffixPoolDefinition definition =
                new EquipmentAffixPoolDefinition(
                    new[]
                    {
                        firstEntry,
                        secondEntry
                    });

            Assert.That(
                definition.Entries,
                Has.Count.EqualTo(2));

            Assert.That(
                definition.Entries[0],
                Is.SameAs(firstEntry));

            Assert.That(
                definition.Entries[1],
                Is.SameAs(secondEntry));
        }

        [Test]
        public void Constructor_WithoutEntries_CreatesEmptyCollection()
        {
            EquipmentAffixPoolDefinition definition =
                new EquipmentAffixPoolDefinition();

            Assert.That(
                definition.Entries,
                Is.Not.Null);

            Assert.That(
                definition.Entries,
                Is.Empty);
        }
    }
}