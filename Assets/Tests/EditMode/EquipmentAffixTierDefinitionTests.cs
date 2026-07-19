using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixTierDefinitionTests
    {
        [Test]
        public void Constructor_AssignsTierData()
        {
            EquipmentAffixTierDefinition definition =
                new EquipmentAffixTierDefinition(
                    10,
                    3,
                    7);

            Assert.That(
                definition.MinimumItemLevel,
                Is.EqualTo(10));

            Assert.That(
                definition.MinimumValue,
                Is.EqualTo(3));

            Assert.That(
                definition.MaximumValue,
                Is.EqualTo(7));
        }
    }
}