using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class LocalizationKeysTests
    {
        [Test]
        public void UnitName_CreatesExpectedKey()
        {
            LocalizationKey key =
                LocalizationKeys.UnitName(
                    new FightUnitDefinitionId(
                        "enemy_slime"));

            Assert.That(
                key.Value,
                Is.EqualTo(
                    "units.enemy_slime.name"));
        }

        [Test]
        public void UnitDescription_CreatesExpectedKey()
        {
            LocalizationKey key =
                LocalizationKeys.UnitDescription(
                    new FightUnitDefinitionId(
                        "enemy_slime"));

            Assert.That(
                key.Value,
                Is.EqualTo(
                    "units.enemy_slime.description"));
        }

        [Test]
        public void SkillName_CreatesExpectedKey()
        {
            LocalizationKey key =
                LocalizationKeys.SkillName(
                    "basic_attack");

            Assert.That(
                key.Value,
                Is.EqualTo(
                    "skills.basic_attack.name"));
        }

        [Test]
        public void SkillDescription_CreatesExpectedKey()
        {
            LocalizationKey key =
                LocalizationKeys.SkillDescription(
                    "basic_attack");

            Assert.That(
                key.Value,
                Is.EqualTo(
                    "skills.basic_attack.description"));
        }

        [Test]
        public void SkillKey_TrimsId()
        {
            LocalizationKey key =
                LocalizationKeys.SkillName(
                    "  basic_attack  ");

            Assert.That(
                key.Value,
                Is.EqualTo(
                    "skills.basic_attack.name"));
        }

        [Test]
        public void EmptySkillId_ReturnsInvalidKey()
        {
            LocalizationKey key =
                LocalizationKeys.SkillName(
                    "   ");

            Assert.That(
                key.IsValid,
                Is.False);
        }

        [Test]
        public void InvalidUnitId_ReturnsInvalidKey()
        {
            LocalizationKey key =
                LocalizationKeys.UnitName(
                    new FightUnitDefinitionId(
                        null));

            Assert.That(
                key.IsValid,
                Is.False);
        }

        [Test]
        public void StatusEffectName_CreatesExpectedKey()
        {
            LocalizationKey key =
                LocalizationKeys.StatusEffectName(
                    new StatusEffectId(
                        "bleed"));

            Assert.That(
                key.Value,
                Is.EqualTo(
                    "status_effects.bleed.name"));
        }

        [Test]
        public void StatusEffectDescription_CreatesExpectedKey()
        {
            LocalizationKey key =
                LocalizationKeys.StatusEffectDescription(
                    new StatusEffectId(
                        "bleed"));

            Assert.That(
                key.Value,
                Is.EqualTo(
                    "status_effects.bleed.description"));
        }

        [Test]
        public void InvalidStatusEffectId_ReturnsInvalidKey()
        {
            LocalizationKey key =
                LocalizationKeys.StatusEffectName(
                    new StatusEffectId(
                        null));

            Assert.That(
                key.IsValid,
                Is.False);
        }
    }
}