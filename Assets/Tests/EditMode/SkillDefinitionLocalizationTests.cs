using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class SkillDefinitionLocalizationTests
    {
        private SkillDefinition definition;

        [SetUp]
        public void SetUp()
        {
            definition =
                ScriptableObject.CreateInstance<
                    SkillDefinition>();
        }

        [TearDown]
        public void TearDown()
        {
            if (definition != null)
            {
                Object.DestroyImmediate(
                    definition);
            }
        }

        [Test]
        public void InitializeForTests_AssignsStableIdAndLocalizationKeys()
        {
            definition.InitializeForTests(
                newSkillId:
                    "shield_bash",
                newDisplayName:
                    "Shield Bash",
                newMaxLevel:
                    3,
                newNameLocalizationKey:
                    "skills.shield_bash.name",
                newDescriptionLocalizationKey:
                    "skills.shield_bash.description",
                newDescription:
                    "Strike an enemy with a shield.");

            Assert.That(
                definition.SkillId,
                Is.EqualTo(
                    "shield_bash"));

            Assert.That(
                definition.NameLocalizationKey.Value,
                Is.EqualTo(
                    "skills.shield_bash.name"));

            Assert.That(
                definition
                    .DescriptionLocalizationKey
                    .Value,
                Is.EqualTo(
                    "skills.shield_bash.description"));

            Assert.That(
                definition.DisplayName,
                Is.EqualTo(
                    "Shield Bash"));

            Assert.That(
                definition.Description,
                Is.EqualTo(
                    "Strike an enemy with a shield."));
        }

        [Test]
        public void ChangingDisplayName_DoesNotChangeStableId()
        {
            SkillDefinition renamedDefinition =
                ScriptableObject.CreateInstance<
                    SkillDefinition>();

            try
            {
                definition.InitializeForTests(
                    newSkillId:
                        "shield_bash",
                    newDisplayName:
                        "Shield Bash");

                renamedDefinition.InitializeForTests(
                    newSkillId:
                        "shield_bash",
                    newDisplayName:
                        "Defensive Strike");

                Assert.That(
                    definition.DisplayName,
                    Is.Not.EqualTo(
                        renamedDefinition.DisplayName));

                Assert.That(
                    definition.SkillId,
                    Is.EqualTo(
                        renamedDefinition.SkillId));
            }
            finally
            {
                Object.DestroyImmediate(
                    renamedDefinition);
            }
        }

        [Test]
        public void ChangingLocalizationKey_DoesNotChangeStableId()
        {
            definition.InitializeForTests(
                newSkillId:
                    "basic_attack",
                newDisplayName:
                    "Basic Attack",
                newNameLocalizationKey:
                    "skills.basic_attack.name");

            Assert.That(
                definition.SkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                definition.NameLocalizationKey.Value,
                Is.EqualTo(
                    "skills.basic_attack.name"));
        }

        [Test]
        public void MissingLocalizationKeys_AreInvalidButFallbackRemainsAvailable()
        {
            definition.InitializeForTests(
                newSkillId:
                    "basic_attack",
                newDisplayName:
                    "Basic Attack");

            Assert.That(
                definition.NameLocalizationKey.IsValid,
                Is.False);

            Assert.That(
                definition
                    .DescriptionLocalizationKey
                    .IsValid,
                Is.False);

            Assert.That(
                definition.DisplayName,
                Is.EqualTo(
                    "Basic Attack"));
        }

        [Test]
        public void InitializeForTests_TrimsIdentityAndLocalizationValues()
        {
            definition.InitializeForTests(
                newSkillId:
                    "  shield_bash  ",
                newDisplayName:
                    "  Shield Bash  ",
                newNameLocalizationKey:
                    "  skills.shield_bash.name  ",
                newDescriptionLocalizationKey:
                    "  skills.shield_bash.description  ");

            Assert.That(
                definition.SkillId,
                Is.EqualTo(
                    "shield_bash"));

            Assert.That(
                definition.DisplayName,
                Is.EqualTo(
                    "Shield Bash"));

            Assert.That(
                definition.NameLocalizationKey.Value,
                Is.EqualTo(
                    "skills.shield_bash.name"));

            Assert.That(
                definition
                    .DescriptionLocalizationKey
                    .Value,
                Is.EqualTo(
                    "skills.shield_bash.description"));
        }

        [Test]
        public void LocalizationKeys_MatchStableSkillId()
        {
            definition.InitializeForTests(
                newSkillId:
                    "basic_attack",
                newDisplayName:
                    "Basic Attack",
                newNameLocalizationKey:
                    "skills.basic_attack.name",
                newDescriptionLocalizationKey:
                    "skills.basic_attack.description");

            Assert.That(
                definition.NameLocalizationKey,
                Is.EqualTo(
                    LocalizationKeys.SkillName(
                        definition.SkillId)));

            Assert.That(
                definition.DescriptionLocalizationKey,
                Is.EqualTo(
                    LocalizationKeys.SkillDescription(
                        definition.SkillId)));
        }
    }
}