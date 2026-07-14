using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitDefinitionTests
    {
        [Test]
        public void TwoUnits_WithSameDefinition_HaveIndependentHealth()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: "Goblin",
                newTeam: FightTeam.Enemy,
                newMaxHealth: 20,
                newAttackPower: 4,
                newInitiative: 8);

            GameObject firstObject =
                new GameObject("First Goblin");

            GameObject secondObject =
                new GameObject("Second Goblin");

            FightUnit first =
                firstObject.AddComponent<FightUnit>();

            FightUnit second =
                secondObject.AddComponent<FightUnit>();

            Assert.That(
                first.Initialize(definition),
                Is.True);

            Assert.That(
                second.Initialize(definition),
                Is.True);

            first.TakeDamage(7);

            Assert.That(first.Definition, Is.SameAs(definition));
            Assert.That(second.Definition, Is.SameAs(definition));

            Assert.That(first.UnitName, Is.EqualTo("Goblin"));
            Assert.That(second.UnitName, Is.EqualTo("Goblin"));

            Assert.That(first.CurrentHealth, Is.EqualTo(13));
            Assert.That(second.CurrentHealth, Is.EqualTo(20));

            Assert.That(definition.MaxHealth, Is.EqualTo(20));

            Object.DestroyImmediate(firstObject);
            Object.DestroyImmediate(secondObject);
            Object.DestroyImmediate(definition);
        }

        [Test]
        public void InitializeForTests_AssignsStableIdAndLocalizationKey()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName:
                    "Slime",
                newTeam:
                    FightTeam.Enemy,
                newMaxHealth:
                    10,
                newAttackPower:
                    2,
                newInitiative:
                    5,
                newStartingSkills:
                    null,
                newUnitId:
                    "enemy_slime",
                newNameLocalizationKey:
                    "units.enemy_slime.name");

            Assert.That(
                definition.UnitId.Value,
                Is.EqualTo("enemy_slime"));

            Assert.That(
                definition.NameLocalizationKey.Value,
                Is.EqualTo(
                    "units.enemy_slime.name"));

            Assert.That(
                definition.UnitName,
                Is.EqualTo("Slime"));

            Object.DestroyImmediate(definition);
        }

        [Test]
        public void ChangingDisplayName_DoesNotChangeStableId()
        {
            FightUnitDefinition first =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            FightUnitDefinition second =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            first.InitializeForTests(
                newUnitName:
                    "Slime",
                newTeam:
                    FightTeam.Enemy,
                newMaxHealth:
                    10,
                newAttackPower:
                    2,
                newInitiative:
                    5,
                newUnitId:
                    "enemy_slime");

            second.InitializeForTests(
                newUnitName:
                    "Green Slime",
                newTeam:
                    FightTeam.Enemy,
                newMaxHealth:
                    10,
                newAttackPower:
                    2,
                newInitiative:
                    5,
                newUnitId:
                    "enemy_slime");

            Assert.That(
                first.UnitName,
                Is.Not.EqualTo(
                    second.UnitName));

            Assert.That(
                first.UnitId,
                Is.EqualTo(
                    second.UnitId));

            Object.DestroyImmediate(first);
            Object.DestroyImmediate(second);
        }

        [Test]
        public void InitializeForTests_AssignsLocalizedTextContract()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            try
            {
                definition.InitializeForTests(
                    newUnitName:
                        "Green Slime",
                    newTeam:
                        FightTeam.Enemy,
                    newMaxHealth:
                        10,
                    newAttackPower:
                        2,
                    newInitiative:
                        5,
                    newStartingSkills:
                        null,
                    newUnitId:
                        "enemy_green_slime",
                    newNameLocalizationKey:
                        "units.enemy_green_slime.name",
                    newDescriptionLocalizationKey:
                        "units.enemy_green_slime.description",
                    newDescription:
                        "A weak creature formed from slime.");

                Assert.That(
                    definition.UnitId.Value,
                    Is.EqualTo(
                        "enemy_green_slime"));

                Assert.That(
                    definition.NameLocalizationKey.Value,
                    Is.EqualTo(
                        "units.enemy_green_slime.name"));

                Assert.That(
                    definition
                        .DescriptionLocalizationKey
                        .Value,
                    Is.EqualTo(
                        "units.enemy_green_slime.description"));

                Assert.That(
                    definition.UnitName,
                    Is.EqualTo(
                        "Green Slime"));

                Assert.That(
                    definition.Description,
                    Is.EqualTo(
                        "A weak creature formed from slime."));
            }
            finally
            {
                Object.DestroyImmediate(
                    definition);
            }
        }

        [Test]
        public void MissingLocalizationKeys_KeepEnglishFallbacks()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            try
            {
                definition.InitializeForTests(
                    newUnitName:
                        "Player",
                    newTeam:
                        FightTeam.Player,
                    newMaxHealth:
                        20,
                    newAttackPower:
                        5,
                    newInitiative:
                        10,
                    newDescription:
                        "The player's current character.");

                Assert.That(
                    definition.NameLocalizationKey.IsValid,
                    Is.False);

                Assert.That(
                    definition
                        .DescriptionLocalizationKey
                        .IsValid,
                    Is.False);

                Assert.That(
                    definition.UnitName,
                    Is.EqualTo("Player"));

                Assert.That(
                    definition.Description,
                    Is.EqualTo(
                        "The player's current character."));
            }
            finally
            {
                Object.DestroyImmediate(
                    definition);
            }
        }

        [Test]
        public void LocalizationKeys_MatchStableUnitId()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            try
            {
                definition.InitializeForTests(
                    newUnitName:
                        "Slime",
                    newTeam:
                        FightTeam.Enemy,
                    newMaxHealth:
                        10,
                    newAttackPower:
                        2,
                    newInitiative:
                        5,
                    newUnitId:
                        "enemy_slime",
                    newNameLocalizationKey:
                        "units.enemy_slime.name",
                    newDescriptionLocalizationKey:
                        "units.enemy_slime.description");

                Assert.That(
                    definition.NameLocalizationKey,
                    Is.EqualTo(
                        LocalizationKeys.UnitName(
                            definition.UnitId)));

                Assert.That(
                    definition.DescriptionLocalizationKey,
                    Is.EqualTo(
                        LocalizationKeys.UnitDescription(
                            definition.UnitId)));
            }
            finally
            {
                Object.DestroyImmediate(
                    definition);
            }
        }
    }
}