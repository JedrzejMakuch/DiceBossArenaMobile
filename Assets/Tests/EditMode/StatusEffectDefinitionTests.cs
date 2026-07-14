using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class StatusEffectDefinitionTests
    {
        private StatusEffectDefinition definition;

        [SetUp]
        public void SetUp()
        {
            definition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();
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
        public void InitializeForTests_AssignsDefinitionValues()
        {
            definition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newBaseDurationTurns:
                    3,
                newMaxStacks:
                    5,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .AddStacksAndRefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                newNameLocalizationKey:
                    "status_effects.bleed.name",
                newDescriptionLocalizationKey:
                    "status_effects.bleed.description",
                newDescription:
                    "Deals damage at the end of the turn.");

            Assert.That(
                definition.StatusEffectId.Value,
                Is.EqualTo(
                    "bleed"));

            Assert.That(
                definition.DisplayName,
                Is.EqualTo(
                    "Bleed"));

            Assert.That(
                definition.BaseDurationTurns,
                Is.EqualTo(
                    3));

            Assert.That(
                definition.MaxStacks,
                Is.EqualTo(
                    5));

            Assert.That(
                definition.StackingPolicy,
                Is.EqualTo(
                    StatusEffectStackingPolicy
                        .AddStacksAndRefreshDuration));

            Assert.That(
                definition.TickTiming,
                Is.EqualTo(
                    StatusEffectTickTiming
                        .EndOfOwnerTurn));

            Assert.That(
                definition.NameLocalizationKey.Value,
                Is.EqualTo(
                    "status_effects.bleed.name"));

            Assert.That(
                definition
                    .DescriptionLocalizationKey
                    .Value,
                Is.EqualTo(
                    "status_effects.bleed.description"));
        }

        [Test]
        public void InitializeForTests_ClampsDurationAndStacks()
        {
            definition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newBaseDurationTurns:
                    0,
                newMaxStacks:
                    -5);

            Assert.That(
                definition.BaseDurationTurns,
                Is.EqualTo(
                    1));

            Assert.That(
                definition.MaxStacks,
                Is.EqualTo(
                    1));
        }

        [Test]
        public void MissingDisplayName_UsesStableIdAsFallback()
        {
            definition.InitializeForTests(
                newStatusEffectId:
                    "bleed");

            Assert.That(
                definition.DisplayName,
                Is.EqualTo(
                    "bleed"));
        }

        [Test]
        public void ChangingDisplayName_DoesNotChangeStableId()
        {
            StatusEffectDefinition renamedDefinition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            try
            {
                definition.InitializeForTests(
                    newStatusEffectId:
                        "bleed",
                    newDisplayName:
                        "Bleed");

                renamedDefinition.InitializeForTests(
                    newStatusEffectId:
                        "bleed",
                    newDisplayName:
                        "Open Wound");

                Assert.That(
                    definition.DisplayName,
                    Is.Not.EqualTo(
                        renamedDefinition.DisplayName));

                Assert.That(
                    definition.StatusEffectId,
                    Is.EqualTo(
                        renamedDefinition.StatusEffectId));
            }
            finally
            {
                Object.DestroyImmediate(
                    renamedDefinition);
            }
        }

        [Test]
        public void LocalizationKeys_MatchStableId()
        {
            definition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newNameLocalizationKey:
                    "status_effects.bleed.name",
                newDescriptionLocalizationKey:
                    "status_effects.bleed.description");

            Assert.That(
                definition.NameLocalizationKey,
                Is.EqualTo(
                    LocalizationKeys.StatusEffectName(
                        definition.StatusEffectId)));

            Assert.That(
                definition.DescriptionLocalizationKey,
                Is.EqualTo(
                    LocalizationKeys
                        .StatusEffectDescription(
                            definition.StatusEffectId)));
        }

        [Test]
        public void InitializeForTests_AssignsStatModifiers()
        {
            StatusEffectStatModifierDefinition modifier =
                new StatusEffectStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    -2);

            definition.InitializeForTests(
                newStatusEffectId:
                    "weakened",
                newDisplayName:
                    "Weakened",
                newStatModifiers:
                    new[]
                    {
                modifier
                    });

            Assert.That(
                definition.StatModifiers.Count,
                Is.EqualTo(1));

            Assert.That(
                definition.StatModifiers[0],
                Is.SameAs(modifier));
        }

        [Test]
        public void InitializeForTests_CopiesStatModifierList()
        {
            StatusEffectStatModifierDefinition modifier =
                new StatusEffectStatModifierDefinition(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    -2);

            var source =
                new List<
                    StatusEffectStatModifierDefinition>
                {
            modifier
                };

            definition.InitializeForTests(
                newStatusEffectId:
                    "weakened",
                newStatModifiers:
                    source);

            source.Clear();

            Assert.That(
                definition.StatModifiers.Count,
                Is.EqualTo(1));
        }
    }
}