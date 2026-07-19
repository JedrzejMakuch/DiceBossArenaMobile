using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        SystemEquipmentAffixRandomSourceTests
    {
        [Test]
        public void Next_ReturnsValueWithinRequestedRange()
        {
            SystemEquipmentAffixRandomSource randomSource =
                new SystemEquipmentAffixRandomSource(
                    12345);

            for (int index = 0;
                 index < 100;
                 index++)
            {
                int result =
                    randomSource.Next(
                        5,
                        10);

                Assert.That(
                    result,
                    Is.GreaterThanOrEqualTo(5));

                Assert.That(
                    result,
                    Is.LessThan(10));
            }
        }

        [Test]
        public void SameSeed_ProducesSameSequence()
        {
            SystemEquipmentAffixRandomSource first =
                new SystemEquipmentAffixRandomSource(
                    12345);

            SystemEquipmentAffixRandomSource second =
                new SystemEquipmentAffixRandomSource(
                    12345);

            for (int index = 0;
                 index < 20;
                 index++)
            {
                Assert.That(
                    first.Next(
                        0,
                        1000),
                    Is.EqualTo(
                        second.Next(
                            0,
                            1000)));
            }
        }

        [Test]
        public void Next_EqualRangeBounds_ReturnsMinimum()
        {
            SystemEquipmentAffixRandomSource randomSource =
                new SystemEquipmentAffixRandomSource(
                    12345);

            int result =
                randomSource.Next(
                    7,
                    7);

            Assert.That(
                result,
                Is.EqualTo(7));
        }
    }
}