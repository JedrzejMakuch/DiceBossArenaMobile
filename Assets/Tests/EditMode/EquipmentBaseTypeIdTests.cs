using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EquipmentBaseTypeIdTests
    {
        [Test]
        public void Constructor_TrimsValue()
        {
            EquipmentBaseTypeId id =
                new EquipmentBaseTypeId(
                    "  iron_sword  ");

            Assert.That(
                id.Value,
                Is.EqualTo("iron_sword"));
        }

        [Test]
        public void EmptyValue_IsInvalid()
        {
            EquipmentBaseTypeId id =
                new EquipmentBaseTypeId("   ");

            Assert.That(
                id.IsValid,
                Is.False);
        }

        [Test]
        public void SameValues_AreEqual()
        {
            EquipmentBaseTypeId first =
                new EquipmentBaseTypeId(
                    "iron_sword");

            EquipmentBaseTypeId second =
                new EquipmentBaseTypeId(
                    "iron_sword");

            Assert.That(
                first,
                Is.EqualTo(second));

            Assert.That(
                first.GetHashCode(),
                Is.EqualTo(second.GetHashCode()));
        }

        [Test]
        public void DifferentValues_AreNotEqual()
        {
            EquipmentBaseTypeId first =
                new EquipmentBaseTypeId(
                    "iron_sword");

            EquipmentBaseTypeId second =
                new EquipmentBaseTypeId(
                    "iron_axe");

            Assert.That(
                first,
                Is.Not.EqualTo(second));
        }

        [Test]
        public void Comparison_IsCaseSensitive()
        {
            EquipmentBaseTypeId first =
                new EquipmentBaseTypeId(
                    "iron_sword");

            EquipmentBaseTypeId second =
                new EquipmentBaseTypeId(
                    "IRON_SWORD");

            Assert.That(
                first,
                Is.Not.EqualTo(second));
        }
    }
}