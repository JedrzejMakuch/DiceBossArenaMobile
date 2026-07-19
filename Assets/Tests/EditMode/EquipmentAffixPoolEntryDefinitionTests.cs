using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolEntryDefinitionTests
    {
        [Test]
        public void Constructor_AssignsAffixId()
        {
            EquipmentAffixPoolEntryDefinition definition =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10);

            Assert.That(
                definition.AffixId,
                Is.EqualTo(
                    new EquipmentAffixId(
                        "strength_flat")));
        }

        [Test]
        public void Constructor_AssignsWeight()
        {
            EquipmentAffixPoolEntryDefinition definition =
                new EquipmentAffixPoolEntryDefinition(
                    "strength_flat",
                    10);

            Assert.That(
                definition.Weight,
                Is.EqualTo(10));
        }
    }
}