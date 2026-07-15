using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        SpecializationDefinitionTests
    {
        private SpecializationDefinition definition;

        [SetUp]
        public void SetUp()
        {
            definition =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();
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
        public void InitializeForTests_NormalizesIds()
        {
            definition.InitializeForTests(
                " berserker ",
                " companion ");

            Assert.That(
                definition.SpecializationId.Value,
                Is.EqualTo(
                    "berserker"));

            Assert.That(
                definition.RequiredClassId.Value,
                Is.EqualTo(
                    "companion"));
        }

        [Test]
        public void InitializeForTests_AssignsPresentationData()
        {
            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newDisplayName:
                    "Berserker",
                newNameLocalizationKey:
                    "specializations.berserker.name",
                newDescriptionLocalizationKey:
                    "specializations.berserker.description",
                newDescription:
                    "Ofensywna specjalizacja Drużynnika.");

            Assert.That(
                definition.DisplayName,
                Is.EqualTo(
                    "Berserker"));

            Assert.That(
                definition.NameLocalizationKey.Value,
                Is.EqualTo(
                    "specializations.berserker.name"));

            Assert.That(
                definition.DescriptionLocalizationKey.Value,
                Is.EqualTo(
                    "specializations.berserker.description"));

            Assert.That(
                definition.Description,
                Is.EqualTo(
                    "Ofensywna specjalizacja Drużynnika."));
        }

        [Test]
        public void IsCompatibleWith_RequiredClassReturnsTrue()
        {
            definition.InitializeForTests(
                "berserker",
                "companion");

            bool result =
                definition.IsCompatibleWith(
                    new CharacterClassId(
                        "companion"));

            Assert.That(
                result,
                Is.True);
        }

        [Test]
        public void IsCompatibleWith_DifferentClassReturnsFalse()
        {
            definition.InitializeForTests(
                "berserker",
                "companion");

            bool result =
                definition.IsCompatibleWith(
                    new CharacterClassId(
                        "mage"));

            Assert.That(
                result,
                Is.False);
        }

        [Test]
        public void IsCompatibleWith_InvalidClassReturnsFalse()
        {
            definition.InitializeForTests(
                "berserker",
                "companion");

            bool result =
                definition.IsCompatibleWith(
                    new CharacterClassId(
                        string.Empty));

            Assert.That(
                result,
                Is.False);
        }

        [Test]
        public void IsCompatibleWith_MissingRequiredClassReturnsFalse()
        {
            definition.InitializeForTests(
                "berserker",
                string.Empty);

            bool result =
                definition.IsCompatibleWith(
                    new CharacterClassId(
                        "companion"));

            Assert.That(
                result,
                Is.False);
        }

        [Test]
        public void InitializeForTests_AssignsStatModifiers()
        {
            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newStatModifiers:
                    new[]
                    {
                        new CharacterStatModifierDefinition(
                            FightStatType.AttackPower,
                            FightStatModifierType.Percent,
                            15)
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
                        FightStatType.AttackPower,
                        FightStatModifierType.Percent,
                        15)));
        }

        [Test]
        public void InitializeForTests_CopiesStatModifierCollection()
        {
            List<CharacterStatModifierDefinition>
                source =
                    new()
                    {
                        new CharacterStatModifierDefinition(
                            FightStatType.AttackPower,
                            FightStatModifierType.Percent,
                            15)
                    };

            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newStatModifiers:
                    source);

            source.Add(
                new CharacterStatModifierDefinition(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    -5));

            Assert.That(
                definition.StatModifiers.Count,
                Is.EqualTo(1));
        }

        [Test]
        public void InitializeForTests_AssignsStartingSkills()
        {
            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "rage",
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
                    "rage"));

            Assert.That(
                skill.Level,
                Is.EqualTo(2));
        }

        [Test]
        public void InitializeForTests_NormalizesSkillPoolIds()
        {
            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newSkillPoolIds:
                    new[]
                    {
                        " blood_rage ",
                        " reckless_strike "
                    });

            Assert.That(
                definition.SkillPoolIds,
                Is.EqualTo(
                    new[]
                    {
                        "blood_rage",
                        "reckless_strike"
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
                            "rage",
                            1)
                    };

            List<string> skillPoolIds =
                new()
                {
                    "blood_rage"
                };

            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newStartingSkills:
                    startingSkills,
                newSkillPoolIds:
                    skillPoolIds);

            startingSkills.Add(
                new CharacterSkillDefinitionEntry(
                    "reckless_strike",
                    1));

            skillPoolIds.Add(
                "executioner");

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
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newPassives:
                    new[]
                    {
                        new CharacterPassiveDefinitionEntry(
                            "blood_frenzy")
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
                    "blood_frenzy"));
        }

        [Test]
        public void InitializeForTests_CopiesPassiveCollection()
        {
            List<CharacterPassiveDefinitionEntry>
                source =
                    new()
                    {
                        new CharacterPassiveDefinitionEntry(
                            "blood_frenzy")
                    };

            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newPassives:
                    source);

            source.Add(
                new CharacterPassiveDefinitionEntry(
                    "executioner"));

            Assert.That(
                definition.Passives.Count,
                Is.EqualTo(1));
        }

        [Test]
        public void InitializeForTests_AssignsSkillReplacements()
        {
            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newSkillReplacements:
                    new[]
                    {
                        new SkillReplacementDefinition(
                            "basic_attack",
                            "berserker_basic_attack")
                    });

            Assert.That(
                definition.SkillReplacements.Count,
                Is.EqualTo(1));

            Assert.That(
                definition.SkillReplacements[0]
                    .SourceSkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                definition.SkillReplacements[0]
                    .ReplacementSkillId,
                Is.EqualTo(
                    "berserker_basic_attack"));

            Assert.That(
                definition.SkillReplacements[0]
                    .IsValid,
                Is.True);
        }

        [Test]
        public void InitializeForTests_CopiesSkillReplacementCollection()
        {
            List<SkillReplacementDefinition>
                source =
                    new()
                    {
                        new SkillReplacementDefinition(
                            "basic_attack",
                            "berserker_basic_attack")
                    };

            definition.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newSkillReplacements:
                    source);

            source.Add(
                new SkillReplacementDefinition(
                    "rage",
                    "greater_rage"));

            Assert.That(
                definition.SkillReplacements.Count,
                Is.EqualTo(1));
        }
    }
}