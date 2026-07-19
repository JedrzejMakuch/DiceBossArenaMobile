using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class CharacterBuildComposerTests
    {
        private ClassDefinition classDefinition;

        private SpecializationDefinition
            specializationDefinition;

        private CharacterBuildComposer composer;

        [SetUp]
        public void SetUp()
        {
            classDefinition =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();

            classDefinition.InitializeForTests(
                "companion");

            specializationDefinition =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();

            specializationDefinition
                .InitializeForTests(
                    "berserker",
                    "companion");

            composer =
                new CharacterBuildComposer();
        }

        [TearDown]
        public void TearDown()
        {
            if (specializationDefinition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    specializationDefinition);
            }

            if (classDefinition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    classDefinition);
            }
        }

        [Test]
        public void Compose_PreservesEquipmentLoadout()
        {
            EquipmentLoadoutSnapshot loadout =
                new EquipmentLoadoutSnapshot(
                    new[]
                    {
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "starter_sword"))
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    equipmentLoadout:
                        loadout);

            CharacterBuildSnapshot snapshot =
                composer.Compose(request);

            Assert.That(
                snapshot.EquipmentLoadout,
                Is.EqualTo(loadout));

            Assert.That(
                snapshot.EquipmentLoadout.Items.Count,
                Is.EqualTo(1));

            Assert.That(
                snapshot.EquipmentLoadout.Items[0].ItemId,
                Is.EqualTo(
                    new CharacterItemId(
                        "starter_sword")));
        }

        [Test]
        public void Compose_AddsEquipmentModifiersAfterClassModifiers()
        {
            ItemDefinition item =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();

            try
            {
                classDefinition.InitializeForTests(
                    newClassId:
                        "companion",
                    newStatModifiers:
                        new[]
                        {
                    new CharacterStatModifierDefinition(
                        FightStatType.MaxHealth,
                        FightStatModifierType.Flat,
                        10)
                        });

                item.InitializeForTests(
                    "starter_sword",
                    EquipmentSlotType.MainHand,
                    newStatModifiers:
                        new[]
                        {
                    new CharacterStatModifierDefinition(
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        5)
                        });

                ItemDefinitionCatalog catalog =
                    new ItemDefinitionCatalog(
                        new[]
                        {
                    item
                        });

                EquipmentStatModifierResolver
                    equipmentResolver =
                        new EquipmentStatModifierResolver(
                            catalog);

                CharacterBuildComposer equipmentComposer =
                    new CharacterBuildComposer(
                        equipmentResolver);

                EquipmentLoadoutSnapshot loadout =
                    new EquipmentLoadoutSnapshot(
                        new[]
                        {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.MainHand,
                        new CharacterItemId(
                            "starter_sword"))
                        });

                CharacterBuildCompositionRequest request =
                    new CharacterBuildCompositionRequest(
                        classDefinition,
                        equipmentLoadout:
                            loadout);

                CharacterBuildSnapshot result =
                    equipmentComposer.Compose(
                        request);

                Assert.That(
                    result.StatModifiers.Count,
                    Is.EqualTo(2));

                Assert.That(
                    result.StatModifiers[0],
                    Is.EqualTo(
                        new FightStatModifier(
                            FightStatType.MaxHealth,
                            FightStatModifierType.Flat,
                            10)));

                Assert.That(
                    result.StatModifiers[1],
                    Is.EqualTo(
                        new FightStatModifier(
                            FightStatType.Strength,
                            FightStatModifierType.Flat,
                            5)));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(item);
            }
        }

        [Test]
        public void Compose_NullRequestThrows()
        {
            Assert.That(
                () =>
                    composer.Compose(
                        null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Compose_ClassOnlyCopiesClassId()
        {
            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.ClassId.Value,
                Is.EqualTo(
                    "companion"));

            Assert.That(
                result.SpecializationId.IsValid,
                Is.False);
        }

        [Test]
        public void Compose_CompatibleSpecializationCopiesIds()
        {
            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.ClassId.Value,
                Is.EqualTo(
                    "companion"));

            Assert.That(
                result.SpecializationId.Value,
                Is.EqualTo(
                    "berserker"));
        }

        [Test]
        public void Compose_IncompatibleSpecializationThrows()
        {
            specializationDefinition
                .InitializeForTests(
                    "berserker",
                    "mage");

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_InvalidClassIdThrows()
        {
            classDefinition.InitializeForTests(
                string.Empty);

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_InvalidSpecializationIdThrows()
        {
            specializationDefinition
                .InitializeForTests(
                    string.Empty,
                    "companion");

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_AddsClassStatModifiers()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStatModifiers:
                    new[]
                    {
                        new CharacterStatModifierDefinition(
                            FightStatType.MaxHealth,
                            FightStatModifierType.Flat,
                            10),
                        new CharacterStatModifierDefinition(
                            FightStatType.AttackPower,
                            FightStatModifierType.Percent,
                            15)
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.StatModifiers.Count,
                Is.EqualTo(2));

            Assert.That(
                result.StatModifiers[0],
                Is.EqualTo(
                    new FightStatModifier(
                        FightStatType.MaxHealth,
                        FightStatModifierType.Flat,
                        10)));

            Assert.That(
                result.StatModifiers[1],
                Is.EqualTo(
                    new FightStatModifier(
                        FightStatType.AttackPower,
                        FightStatModifierType.Percent,
                        15)));
        }

        [Test]
        public void Compose_NullClassStatModifierThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStatModifiers:
                    new CharacterStatModifierDefinition[]
                    {
                        null
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_AddsSpecializationModifiersAfterClassModifiers()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStatModifiers:
                    new[]
                    {
                        new CharacterStatModifierDefinition(
                            FightStatType.MaxHealth,
                            FightStatModifierType.Flat,
                            10)
                    });

            specializationDefinition
                .InitializeForTests(
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

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.StatModifiers.Count,
                Is.EqualTo(2));

            Assert.That(
                result.StatModifiers[0],
                Is.EqualTo(
                    new FightStatModifier(
                        FightStatType.MaxHealth,
                        FightStatModifierType.Flat,
                        10)));

            Assert.That(
                result.StatModifiers[1],
                Is.EqualTo(
                    new FightStatModifier(
                        FightStatType.AttackPower,
                        FightStatModifierType.Percent,
                        15)));
        }

        [Test]
        public void Compose_NullSpecializationStatModifierThrows()
        {
            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newStatModifiers:
                        new CharacterStatModifierDefinition[]
                        {
                            null
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_AddsClassStartingSkills()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "basic_attack",
                            2),
                        new CharacterSkillDefinitionEntry(
                            "shield_bash",
                            1)
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.Skills.Count,
                Is.EqualTo(2));

            Assert.That(
                result.Skills[0],
                Is.EqualTo(
                    new CharacterBuildSkill(
                        "basic_attack",
                        2)));

            Assert.That(
                result.Skills[1],
                Is.EqualTo(
                    new CharacterBuildSkill(
                        "shield_bash",
                        1)));
        }

        [Test]
        public void Compose_NullClassStartingSkillThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new CharacterSkillDefinitionEntry[]
                    {
                        null
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_InvalidClassStartingSkillThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            string.Empty,
                            1)
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_AddsSpecializationSkillsAfterClassSkills()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "basic_attack",
                            1)
                    });

            specializationDefinition
                .InitializeForTests(
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

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.Skills.Count,
                Is.EqualTo(2));

            Assert.That(
                result.Skills[0].SkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                result.Skills[1],
                Is.EqualTo(
                    new CharacterBuildSkill(
                        "rage",
                        2)));
        }

        [Test]
        public void Compose_NullSpecializationStartingSkillThrows()
        {
            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newStartingSkills:
                        new CharacterSkillDefinitionEntry[]
                        {
                            null
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_DuplicateStartingSkillThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "basic_attack",
                            1)
                    });

            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newStartingSkills:
                        new[]
                        {
                            new CharacterSkillDefinitionEntry(
                                "basic_attack",
                                2)
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Compose_ReplacesSkillAndPreservesLevel()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "basic_attack",
                            3),
                        new CharacterSkillDefinitionEntry(
                            "shield_bash",
                            1)
                    });

            specializationDefinition
                .InitializeForTests(
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

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.Skills.Count,
                Is.EqualTo(2));

            Assert.That(
                result.Skills[0],
                Is.EqualTo(
                    new CharacterBuildSkill(
                        "berserker_basic_attack",
                        3)));

            Assert.That(
                result.Skills[1],
                Is.EqualTo(
                    new CharacterBuildSkill(
                        "shield_bash",
                        1)));
        }

        [Test]
        public void Compose_AppliesReplacementsSimultaneously()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "skill_a",
                            1)
                    });

            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newSkillReplacements:
                        new[]
                        {
                            new SkillReplacementDefinition(
                                "skill_a",
                                "skill_b"),
                            new SkillReplacementDefinition(
                                "skill_b",
                                "skill_c")
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_NullSkillReplacementThrows()
        {
            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newSkillReplacements:
                        new SkillReplacementDefinition[]
                        {
                            null
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_InvalidSkillReplacementThrows()
        {
            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newSkillReplacements:
                        new[]
                        {
                            new SkillReplacementDefinition(
                                "basic_attack",
                                "basic_attack")
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_DuplicateReplacementSourceThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "basic_attack",
                            1)
                    });

            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newSkillReplacements:
                        new[]
                        {
                            new SkillReplacementDefinition(
                                "basic_attack",
                                "berserker_basic_attack"),
                            new SkillReplacementDefinition(
                                "basic_attack",
                                "heavy_attack")
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_ReplacementWithoutSourceSkillThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "shield_bash",
                            1)
                    });

            specializationDefinition
                .InitializeForTests(
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

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_AddsClassAndSpecializationPassives()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newPassives:
                    new[]
                    {
                        new CharacterPassiveDefinitionEntry(
                            "battle_training")
                    });

            specializationDefinition
                .InitializeForTests(
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

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.PassiveIds.Count,
                Is.EqualTo(2));

            Assert.That(
                result.PassiveIds[0].Value,
                Is.EqualTo(
                    "battle_training"));

            Assert.That(
                result.PassiveIds[1].Value,
                Is.EqualTo(
                    "blood_frenzy"));
        }

        [Test]
        public void Compose_NullPassiveEntryThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newPassives:
                    new CharacterPassiveDefinitionEntry[]
                    {
                        null
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_InvalidPassiveEntryThrows()
        {
            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newPassives:
                        new[]
                        {
                            new CharacterPassiveDefinitionEntry(
                                string.Empty)
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_DuplicatePassiveIdThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newPassives:
                    new[]
                    {
                        new CharacterPassiveDefinitionEntry(
                            "battle_training")
                    });

            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newPassives:
                        new[]
                        {
                            new CharacterPassiveDefinitionEntry(
                                "battle_training")
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Compose_AddsSelectedSkillsAfterStartingSkills()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            "basic_attack",
                            1)
                    },
                newSkillPoolIds:
                    new[]
                    {
                        "shield_bash",
                        "battle_shout"
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    selectedSkills:
                        new[]
                        {
                            new CharacterBuildSkill(
                                "shield_bash",
                                2),
                            new CharacterBuildSkill(
                                "battle_shout",
                                1)
                        });

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.Skills.Count,
                Is.EqualTo(3));

            Assert.That(
                result.Skills[0].SkillId,
                Is.EqualTo(
                    "basic_attack"));

            Assert.That(
                result.Skills[1],
                Is.EqualTo(
                    new CharacterBuildSkill(
                        "shield_bash",
                        2)));

            Assert.That(
                result.Skills[2].SkillId,
                Is.EqualTo(
                    "battle_shout"));
        }

        [Test]
        public void Compose_AppliesReplacementToSelectedSkill()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newSkillPoolIds:
                    new[]
                    {
                        "basic_attack"
                    });

            specializationDefinition
                .InitializeForTests(
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

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition,
                    new[]
                    {
                        new CharacterBuildSkill(
                            "basic_attack",
                            3)
                    });

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.Skills,
                Is.EqualTo(
                    new[]
                    {
                        new CharacterBuildSkill(
                            "berserker_basic_attack",
                            3)
                    }));
        }

        [Test]
        public void Compose_InvalidSelectedSkillThrows()
        {
            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    selectedSkills:
                        new[]
                        {
                            new CharacterBuildSkill(
                                string.Empty,
                                1)
                        });

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_AllowsSkillFromClassPool()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newSkillPoolIds:
                    new[]
                    {
                        "shield_bash"
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    selectedSkills:
                        new[]
                        {
                            new CharacterBuildSkill(
                                "shield_bash",
                                2)
                        });

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.Skills,
                Is.EqualTo(
                    new[]
                    {
                        new CharacterBuildSkill(
                            "shield_bash",
                            2)
                    }));
        }

        [Test]
        public void Compose_AllowsSkillFromSpecializationPool()
        {
            specializationDefinition
                .InitializeForTests(
                    newSpecializationId:
                        "berserker",
                    newRequiredClassId:
                        "companion",
                    newSkillPoolIds:
                        new[]
                        {
                            "blood_rage"
                        });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition,
                    new[]
                    {
                        new CharacterBuildSkill(
                            "blood_rage",
                            1)
                    });

            CharacterBuildSnapshot result =
                composer.Compose(
                    request);

            Assert.That(
                result.Skills[0].SkillId,
                Is.EqualTo(
                    "blood_rage"));
        }

        [Test]
        public void Compose_SelectedSkillOutsidePoolThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newSkillPoolIds:
                    new[]
                    {
                        "shield_bash"
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    selectedSkills:
                        new[]
                        {
                            new CharacterBuildSkill(
                                "meteor",
                                1)
                        });

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Compose_InvalidSkillPoolIdThrows()
        {
            classDefinition.InitializeForTests(
                newClassId:
                    "companion",
                newSkillPoolIds:
                    new[]
                    {
                        " "
                    });

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition);

            Assert.That(
                () =>
                    composer.Compose(
                        request),
                Throws.TypeOf<
                    InvalidOperationException>());
        }
    }
}