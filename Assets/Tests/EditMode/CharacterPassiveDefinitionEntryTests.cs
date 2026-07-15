using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        CharacterPassiveDefinitionEntryTests
    {
        [Test]
        public void Constructor_NormalizesPassiveId()
        {
            CharacterPassiveDefinitionEntry entry =
                new CharacterPassiveDefinitionEntry(
                    " shield_mastery ");

            Assert.That(
                entry.PassiveId,
                Is.EqualTo(
                    "shield_mastery"));
        }

        [Test]
        public void IsValid_ValidPassiveIdReturnsTrue()
        {
            CharacterPassiveDefinitionEntry entry =
                new CharacterPassiveDefinitionEntry(
                    "shield_mastery");

            Assert.That(
                entry.IsValid,
                Is.True);
        }

        [Test]
        public void IsValid_MissingPassiveIdReturnsFalse()
        {
            CharacterPassiveDefinitionEntry entry =
                new CharacterPassiveDefinitionEntry(
                    " ");

            Assert.That(
                entry.IsValid,
                Is.False);
        }

        [Test]
        public void CreatePassiveId_CopiesId()
        {
            CharacterPassiveDefinitionEntry entry =
                new CharacterPassiveDefinitionEntry(
                    "shield_mastery");

            CharacterPassiveId passiveId =
                entry.CreatePassiveId();

            Assert.That(
                passiveId.Value,
                Is.EqualTo(
                    "shield_mastery"));
        }
    }
}