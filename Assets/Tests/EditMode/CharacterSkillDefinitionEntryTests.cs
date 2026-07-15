using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        CharacterSkillDefinitionEntryTests
    {
        [Test]
        public void Constructor_NormalizesSkillId()
        {
            CharacterSkillDefinitionEntry entry =
                new CharacterSkillDefinitionEntry(
                    " basic_attack ",
                    2);

            Assert.That(
                entry.SkillId,
                Is.EqualTo(
                    "basic_attack"));
        }

        [Test]
        public void Constructor_ClampsLevelToAtLeastOne()
        {
            CharacterSkillDefinitionEntry entry =
                new CharacterSkillDefinitionEntry(
                    "basic_attack",
                    0);

            Assert.That(
                entry.Level,
                Is.EqualTo(1));
        }

        [Test]
        public void IsValid_ValidSkillReturnsTrue()
        {
            CharacterSkillDefinitionEntry entry =
                new CharacterSkillDefinitionEntry(
                    "basic_attack",
                    1);

            Assert.That(
                entry.IsValid,
                Is.True);
        }

        [Test]
        public void IsValid_MissingSkillIdReturnsFalse()
        {
            CharacterSkillDefinitionEntry entry =
                new CharacterSkillDefinitionEntry(
                    string.Empty,
                    1);

            Assert.That(
                entry.IsValid,
                Is.False);
        }

        [Test]
        public void CreateBuildSkill_CopiesIdAndLevel()
        {
            CharacterSkillDefinitionEntry entry =
                new CharacterSkillDefinitionEntry(
                    "basic_attack",
                    3);

            CharacterBuildSkill skill =
                entry.CreateBuildSkill();

            Assert.That(
                skill.SkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                skill.Level,
                Is.EqualTo(3));
        }
    }
}