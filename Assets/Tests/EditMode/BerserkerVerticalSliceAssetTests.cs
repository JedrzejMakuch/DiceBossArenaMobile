using DiceBossArena.Game;
using NUnit.Framework;
using UnityEditor;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        BerserkerVerticalSliceAssetTests
    {
        private const string ClassPath =
            "Assets/_Project/Data/Characters/" +
            "Classes/Class_Companion.asset";

        private const string SpecializationPath =
            "Assets/_Project/Data/Characters/" +
            "Specializations/" +
            "Specialization_Berserker.asset";

        private const string SkillPath =
            "Assets/_Project/Data/Skills/" +
            "Specializations/Berserker/" +
            "Skill_BerserkerBasicAttack.asset";

        [Test]
        public void CompanionClass_HasExpectedBuildData()
        {
            ClassDefinition definition =
                AssetDatabase.LoadAssetAtPath<
                    ClassDefinition>(
                        ClassPath);

            Assert.That(
                definition,
                Is.Not.Null);

            Assert.That(
                definition.ClassId.Value,
                Is.EqualTo(
                    "companion"));

            Assert.That(
                definition.StartingSkills,
                Has.Count.EqualTo(1));

            Assert.That(
                definition.StartingSkills[0].SkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                definition.SkillPoolIds,
                Is.EquivalentTo(
                    new[]
                    {
                        "power_strike",
                        "dash",
                        "ground_slam",
                        "second_wind"
                    }));

            Assert.That(
                definition.StatModifiers,
                Has.Count.EqualTo(1));

            Assert.That(
                definition.Passives,
                Has.Count.EqualTo(1));

            Assert.That(
                definition.Passives[0].PassiveId,
                Is.EqualTo(
                    "battle_training"));
        }

        [Test]
        public void BerserkerSpecialization_ReplacesBasicAttack()
        {
            SpecializationDefinition definition =
                AssetDatabase.LoadAssetAtPath<
                    SpecializationDefinition>(
                        SpecializationPath);

            Assert.That(
                definition,
                Is.Not.Null);

            Assert.That(
                definition.SpecializationId.Value,
                Is.EqualTo(
                    "berserker"));

            Assert.That(
                definition.RequiredClassId.Value,
                Is.EqualTo(
                    "companion"));

            Assert.That(
                definition.SkillReplacements,
                Has.Count.EqualTo(1));

            SkillReplacementDefinition replacement =
                definition.SkillReplacements[0];

            Assert.That(
                replacement.IsValid,
                Is.True);

            Assert.That(
                replacement.SourceSkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                replacement.ReplacementSkillId,
                Is.EqualTo(
                    "berserker_basic_attack"));

            Assert.That(
                definition.Passives,
                Has.Count.EqualTo(1));

            Assert.That(
                definition.Passives[0].PassiveId,
                Is.EqualTo(
                    "blood_frenzy"));
        }

        [Test]
        public void BerserkerBasicAttack_DealsDamageAndAppliesBleed()
        {
            SkillDefinition definition =
                AssetDatabase.LoadAssetAtPath<
                    SkillDefinition>(
                        SkillPath);

            Assert.That(
                definition,
                Is.Not.Null);

            Assert.That(
                definition.SkillId,
                Is.EqualTo(
                    "berserker_basic_attack"));

            Assert.That(
                definition.Effects,
                Has.Count.EqualTo(2));

            Assert.That(
                definition.Effects[0],
                Is.TypeOf<
                    DamageSkillEffectDefinition>());

            Assert.That(
                definition.Effects[1],
                Is.TypeOf<
                    ApplyStatusEffectSkillEffectDefinition>());

            ApplyStatusEffectSkillEffectDefinition
                statusEffect =
                    (ApplyStatusEffectSkillEffectDefinition)
                    definition.Effects[1];

            Assert.That(
                statusEffect.StatusEffect,
                Is.Not.Null);

            Assert.That(
                statusEffect.StatusEffect
                    .StatusEffectId.Value,
                Is.EqualTo(
                    "bleed"));

            Assert.That(
                statusEffect.TargetRelation,
                Is.EqualTo(
                    StatusEffectTargetRelation.Enemy));
        }

        [Test]
        public void FullContent_BuildsResolvedBerserkerLoadout()
        {
            ClassDefinition classDefinition =
                AssetDatabase.LoadAssetAtPath<
                    ClassDefinition>(
                        ClassPath);

            SpecializationDefinition specialization =
                AssetDatabase.LoadAssetAtPath<
                    SpecializationDefinition>(
                        SpecializationPath);

            SkillDefinition berserkerBasicAttack =
                LoadSkill(
                    "Assets/_Project/Data/Skills/" +
                    "Specializations/Berserker/" +
                    "Skill_BerserkerBasicAttack.asset");

            SkillDefinition powerStrike =
                LoadSkill(
                    "Assets/_Project/Data/Skills/" +
                    "Player/Skill_PowerStrike.asset");

            SkillDefinition dash =
                LoadSkill(
                    "Assets/_Project/Data/Skills/" +
                    "Player/Skill_Dash.asset");

            SkillDefinition groundSlam =
                LoadSkill(
                    "Assets/_Project/Data/Skills/" +
                    "Player/Skill_GroundSlam.asset");

            SkillDefinition secondWind =
                LoadSkill(
                    "Assets/_Project/Data/Skills/" +
                    "Player/Skill_SecondWind.asset");

            CharacterClassContentValidator validator =
                new CharacterClassContentValidator();

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            classDefinition
                        },
                        new[]
                        {
                            specialization
                        }),
                Throws.Nothing);

            CharacterBuildResolver resolver =
                new CharacterBuildResolver(
                    new SkillDefinitionCatalog(
                        new[]
                        {
                            berserkerBasicAttack,
                            powerStrike,
                            dash,
                            groundSlam,
                            secondWind
                        }));

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specialization,
                    new[]
                    {
                        new CharacterBuildSkill(
                            "power_strike",
                            1),
                        new CharacterBuildSkill(
                            "dash",
                            1),
                        new CharacterBuildSkill(
                            "ground_slam",
                            1),
                        new CharacterBuildSkill(
                            "second_wind",
                            1)
                    });

            ResolvedCharacterBuild result =
                resolver.Resolve(
                    request);

            Assert.That(
                result.ClassId.Value,
                Is.EqualTo(
                    "companion"));

            Assert.That(
                result.SpecializationId.Value,
                Is.EqualTo(
                    "berserker"));

            Assert.That(
                result.Skills,
                Has.Count.EqualTo(5));

            Assert.That(
                result.Skills[0].Definition,
                Is.SameAs(
                    berserkerBasicAttack));

            Assert.That(
                result.Skills[1].Definition,
                Is.SameAs(
                    powerStrike));

            Assert.That(
                result.Skills[2].Definition,
                Is.SameAs(
                    dash));

            Assert.That(
                result.Skills[3].Definition,
                Is.SameAs(
                    groundSlam));

            Assert.That(
                result.Skills[4].Definition,
                Is.SameAs(
                    secondWind));

            Assert.That(
                result.StatModifiers,
                Has.Count.EqualTo(2));

            Assert.That(
                result.PassiveIds,
                Has.Count.EqualTo(2));

            Assert.That(
                result.PassiveIds[0].Value,
                Is.EqualTo(
                    "battle_training"));

            Assert.That(
                result.PassiveIds[1].Value,
                Is.EqualTo(
                    "blood_frenzy"));
        }

        private static SkillDefinition LoadSkill(
            string path)
        {
            SkillDefinition definition =
                AssetDatabase.LoadAssetAtPath<
                    SkillDefinition>(
                        path);

            Assert.That(
                definition,
                Is.Not.Null,
                $"Missing skill asset: {path}");

            return definition;
        }
    }
}