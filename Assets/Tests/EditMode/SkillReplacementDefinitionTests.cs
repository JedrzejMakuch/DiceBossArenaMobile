using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        SkillReplacementDefinitionTests
    {
        [Test]
        public void Constructor_NormalizesSkillIds()
        {
            SkillReplacementDefinition replacement =
                new SkillReplacementDefinition(
                    " basic_attack ",
                    " berserker_basic_attack ");

            Assert.That(
                replacement.SourceSkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                replacement.ReplacementSkillId,
                Is.EqualTo(
                    "berserker_basic_attack"));
        }

        [Test]
        public void IsValid_DifferentNonEmptyIdsReturnsTrue()
        {
            SkillReplacementDefinition replacement =
                new SkillReplacementDefinition(
                    "basic_attack",
                    "berserker_basic_attack");

            Assert.That(
                replacement.IsValid,
                Is.True);
        }

        [Test]
        public void IsValid_MissingSourceSkillReturnsFalse()
        {
            SkillReplacementDefinition replacement =
                new SkillReplacementDefinition(
                    string.Empty,
                    "berserker_basic_attack");

            Assert.That(
                replacement.IsValid,
                Is.False);
        }

        [Test]
        public void IsValid_MissingReplacementSkillReturnsFalse()
        {
            SkillReplacementDefinition replacement =
                new SkillReplacementDefinition(
                    "basic_attack",
                    string.Empty);

            Assert.That(
                replacement.IsValid,
                Is.False);
        }

        [Test]
        public void IsValid_IdenticalSkillIdsReturnsFalse()
        {
            SkillReplacementDefinition replacement =
                new SkillReplacementDefinition(
                    "basic_attack",
                    "basic_attack");

            Assert.That(
                replacement.IsValid,
                Is.False);
        }
    }
}