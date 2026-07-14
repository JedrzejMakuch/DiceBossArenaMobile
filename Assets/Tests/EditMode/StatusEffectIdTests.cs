using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class StatusEffectIdTests
    {
        [Test]
        public void Constructor_TrimsValue()
        {
            StatusEffectId id =
                new StatusEffectId(
                    "  bleed  ");

            Assert.That(
                id.Value,
                Is.EqualTo(
                    "bleed"));
        }

        [Test]
        public void EmptyValue_IsInvalid()
        {
            StatusEffectId id =
                new StatusEffectId(
                    "   ");

            Assert.That(
                id.IsValid,
                Is.False);
        }

        [Test]
        public void EqualValues_AreEqual()
        {
            StatusEffectId first =
                new StatusEffectId(
                    "bleed");

            StatusEffectId second =
                new StatusEffectId(
                    "bleed");

            Assert.That(
                first,
                Is.EqualTo(second));

            Assert.That(
                first == second,
                Is.True);
        }

        [Test]
        public void Comparison_IsCaseSensitive()
        {
            StatusEffectId first =
                new StatusEffectId(
                    "bleed");

            StatusEffectId second =
                new StatusEffectId(
                    "Bleed");

            Assert.That(
                first,
                Is.Not.EqualTo(second));
        }
    }
}