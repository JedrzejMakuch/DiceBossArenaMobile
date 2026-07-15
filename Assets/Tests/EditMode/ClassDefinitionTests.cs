using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ClassDefinitionTests
    {
        private ClassDefinition definition;

        [SetUp]
        public void SetUp()
        {
            definition =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();
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
        public void InitializeForTests_NormalizesClassId()
        {
            definition.InitializeForTests(
                " companion ");

            Assert.That(
                definition.ClassId.Value,
                Is.EqualTo(
                    "companion"));
        }

        [Test]
        public void InitializeForTests_AssignsPresentationData()
        {
            definition.InitializeForTests(
                newClassId:
                    "companion",
                newDisplayName:
                    "Drużynnik",
                newNameLocalizationKey:
                    "classes.companion.name",
                newDescriptionLocalizationKey:
                    "classes.companion.description",
                newDescription:
                    "Wszechstronna klasa walcząca w zwarciu.");

            Assert.That(
                definition.DisplayName,
                Is.EqualTo(
                    "Drużynnik"));

            Assert.That(
                definition.NameLocalizationKey.Value,
                Is.EqualTo(
                    "classes.companion.name"));

            Assert.That(
                definition.DescriptionLocalizationKey.Value,
                Is.EqualTo(
                    "classes.companion.description"));

            Assert.That(
                definition.Description,
                Is.EqualTo(
                    "Wszechstronna klasa walcząca w zwarciu."));
        }

        [Test]
        public void InitializeForTests_MissingDisplayNameUsesClassId()
        {
            definition.InitializeForTests(
                newClassId:
                    "companion");

            Assert.That(
                definition.DisplayName,
                Is.EqualTo(
                    "companion"));
        }

        [Test]
        public void NewDefinition_HasNoIcon()
        {
            definition.InitializeForTests(
                "companion");

            Assert.That(
                definition.Icon,
                Is.Null);
        }

        [Test]
        public void InitializeForTests_AssignsStatModifiers()
        {
            CharacterStatModifierDefinition modifier =
                new CharacterStatModifierDefinition(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10);

            definition.InitializeForTests(
                newClassId:
                    "companion",
                newStatModifiers:
                    new[]
                    {
                        modifier
                    });

            Assert.That(
                definition.StatModifiers.Count,
                Is.EqualTo(1));

            FightStatModifier runtimeModifier =
                definition.StatModifiers[0]
                    .CreateModifier();

            Assert.That(
                runtimeModifier,
                Is.EqualTo(
                    new FightStatModifier(
                        FightStatType.MaxHealth,
                        FightStatModifierType.Flat,
                        10)));
        }

        [Test]
        public void InitializeForTests_CopiesStatModifierCollection()
        {
            List<CharacterStatModifierDefinition>
                source =
                    new()
                    {
                        new CharacterStatModifierDefinition(
                            FightStatType.MaxHealth,
                            FightStatModifierType.Flat,
                            10)
                    };

            definition.InitializeForTests(
                newClassId:
                    "companion",
                newStatModifiers:
                    source);

            source.Add(
                new CharacterStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    2));

            Assert.That(
                definition.StatModifiers.Count,
                Is.EqualTo(1));
        }

        [Test]
        public void InitializeForTests_AssignsStartingSkills()
        {
            definition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "basic_attack",
                            2)
                    });

            Assert.That(
                definition.StartingSkills.Count,
                Is.EqualTo(1));

            CharacterBuildSkill skill =
                definition.StartingSkills[0]
                    .CreateBuildSkill();

            Assert.That(
                skill.SkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                skill.Level,
                Is.EqualTo(2));
        }

        [Test]
        public void InitializeForTests_NormalizesSkillPoolIds()
        {
            definition.InitializeForTests(
                newClassId:
                    "companion",
                newSkillPoolIds:
                    new[]
                    {
                        " shield_bash ",
                        " battle_shout "
                    });

            Assert.That(
                definition.SkillPoolIds,
                Is.EqualTo(
                    new[]
                    {
                        "shield_bash",
                        "battle_shout"
                    }));
        }

        [Test]
        public void InitializeForTests_CopiesSkillCollections()
        {
            List<CharacterSkillDefinitionEntry>
                startingSkills =
                    new()
                    {
                        new CharacterSkillDefinitionEntry(
                            "basic_attack",
                            1)
                    };

            List<string> skillPoolIds =
                new()
                {
                    "shield_bash"
                };

            definition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    startingSkills,
                newSkillPoolIds:
                    skillPoolIds);

            startingSkills.Add(
                new CharacterSkillDefinitionEntry(
                    "battle_shout",
                    1));

            skillPoolIds.Add(
                "defensive_stance");

            Assert.That(
                definition.StartingSkills.Count,
                Is.EqualTo(1));

            Assert.That(
                definition.SkillPoolIds.Count,
                Is.EqualTo(1));
        }

        [Test]
        public void InitializeForTests_AssignsPassives()
        {
            definition.InitializeForTests(
                newClassId:
                    "companion",
                newPassives:
                    new[]
                    {
                        new CharacterPassiveDefinitionEntry(
                            "shield_mastery")
                    });

            Assert.That(
                definition.Passives.Count,
                Is.EqualTo(1));

            CharacterPassiveId passiveId =
                definition.Passives[0]
                    .CreatePassiveId();

            Assert.That(
                passiveId.Value,
                Is.EqualTo(
                    "shield_mastery"));
        }

        [Test]
        public void InitializeForTests_CopiesPassiveCollection()
        {
            List<CharacterPassiveDefinitionEntry>
                source =
                    new()
                    {
                        new CharacterPassiveDefinitionEntry(
                            "shield_mastery")
                    };

            definition.InitializeForTests(
                newClassId:
                    "companion",
                newPassives:
                    source);

            source.Add(
                new CharacterPassiveDefinitionEntry(
                    "battle_hardened"));

            Assert.That(
                definition.Passives.Count,
                Is.EqualTo(1));
        }
    }
}