using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentRarityAffixCountResolverTests
    {
        [TestCase(
            EquipmentItemRarity.Common,
            0)]
        [TestCase(
            EquipmentItemRarity.Magic,
            2)]
        [TestCase(
            EquipmentItemRarity.Rare,
            4)]
        public void Resolve_ValidRarity_ReturnsAffixCount(
            EquipmentItemRarity rarity,
            int expectedCount)
        {
            EquipmentRarityAffixCountResolver resolver =
                new EquipmentRarityAffixCountResolver();

            int result =
                resolver.Resolve(rarity);

            Assert.That(
                result,
                Is.EqualTo(expectedCount));
        }

        [Test]
        public void Resolve_UnsupportedRarity_Throws()
        {
            EquipmentRarityAffixCountResolver resolver =
                new EquipmentRarityAffixCountResolver();

            EquipmentItemRarity unsupportedRarity =
                (EquipmentItemRarity)999;

            Assert.That(
                () => resolver.Resolve(
                    unsupportedRarity),
                Throws.TypeOf<
                    ArgumentOutOfRangeException>());
        }
    }
}