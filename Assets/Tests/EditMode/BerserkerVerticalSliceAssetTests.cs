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
    }
}