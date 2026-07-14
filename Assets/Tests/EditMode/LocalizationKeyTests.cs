using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class LocalizationKeyTests
    {
        [Test]
        public void Constructor_TrimsValue()
        {
            LocalizationKey key =
                new LocalizationKey(
                    "  units.enemy_slime.name  ");

            Assert.That(
                key.Value,
                Is.EqualTo(
                    "units.enemy_slime.name"));
        }

        [Test]
        public void EmptyValue_IsInvalid()
        {
            LocalizationKey key =
                new LocalizationKey(
                    null);

            Assert.That(
                key.IsValid,
                Is.False);
        }

        [Test]
        public void EqualValues_AreEqual()
        {
            LocalizationKey first =
                new LocalizationKey(
                    "skills.basic_attack.name");

            LocalizationKey second =
                new LocalizationKey(
                    "skills.basic_attack.name");

            Assert.That(
                first,
                Is.EqualTo(second));
        }
    }
}