using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        CharacterClassContentValidatorTests
    {
        private ClassDefinition firstClass;
        private ClassDefinition secondClass;

        private SpecializationDefinition
            firstSpecialization;

        private SpecializationDefinition
            secondSpecialization;

        private CharacterClassContentValidator
            validator;

        [SetUp]
        public void SetUp()
        {
            validator =
                new CharacterClassContentValidator();
        }

        [TearDown]
        public void TearDown()
        {
            DestroyDefinition(
                firstSpecialization);

            DestroyDefinition(
                secondSpecialization);

            DestroyDefinition(
                firstClass);

            DestroyDefinition(
                secondClass);
        }

        [Test]
        public void Validate_CompatibleContentDoesNotThrow()
        {
            firstClass =
                CreateClass(
                    "companion");

            firstSpecialization =
                CreateSpecialization(
                    "berserker",
                    "companion");

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            firstClass
                        },
                        new[]
                        {
                            firstSpecialization
                        }),
                Throws.Nothing);
        }

        [Test]
        public void Validate_UnknownRequiredClassThrows()
        {
            firstClass =
                CreateClass(
                    "companion");

            firstSpecialization =
                CreateSpecialization(
                    "necromancer",
                    "mage");

            InvalidOperationException exception =
                Assert.Throws<
                    InvalidOperationException>(
                    () =>
                        validator.Validate(
                            new[]
                            {
                                firstClass
                            },
                            new[]
                            {
                                firstSpecialization
                            }));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "necromancer"));

            Assert.That(
                exception.Message,
                Does.Contain(
                    "unknown class mage"));
        }

        [Test]
        public void Validate_DuplicateClassIdThrows()
        {
            firstClass =
                CreateClass(
                    "companion");

            secondClass =
                CreateClass(
                    "companion");

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            firstClass,
                            secondClass
                        },
                        null),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Validate_DuplicateSpecializationIdThrows()
        {
            firstClass =
                CreateClass(
                    "companion");

            firstSpecialization =
                CreateSpecialization(
                    "berserker",
                    "companion");

            secondSpecialization =
                CreateSpecialization(
                    "berserker",
                    "companion");

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            firstClass
                        },
                        new[]
                        {
                            firstSpecialization,
                            secondSpecialization
                        }),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Validate_InvalidClassStartingSkillThrows()
        {
            firstClass =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();

            firstClass.InitializeForTests(
                newClassId:
                    "companion",
                newStartingSkills:
                    new[]
                    {
                        new CharacterSkillDefinitionEntry(
                            string.Empty,
                            1)
                    });

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            firstClass
                        },
                        null),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_DuplicatePassiveAcrossBuildThrows()
        {
            firstClass =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();

            firstClass.InitializeForTests(
                newClassId:
                    "companion",
                newPassives:
                    new[]
                    {
                        new CharacterPassiveDefinitionEntry(
                            "battle_training")
                    });

            firstSpecialization =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();

            firstSpecialization.InitializeForTests(
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

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            firstClass
                        },
                        new[]
                        {
                            firstSpecialization
                        }),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Validate_ReplacementFromSkillPoolDoesNotThrow()
        {
            firstClass =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();

            firstClass.InitializeForTests(
                newClassId:
                    "companion",
                newSkillPoolIds:
                    new[]
                    {
                        "power_strike"
                    });

            firstSpecialization =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();

            firstSpecialization.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newSkillReplacements:
                    new[]
                    {
                        new SkillReplacementDefinition(
                            "power_strike",
                            "berserker_power_strike")
                    });

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            firstClass
                        },
                        new[]
                        {
                            firstSpecialization
                        }),
                Throws.Nothing);
        }

        [Test]
        public void Validate_ReplacementOutsideBuildThrows()
        {
            firstClass =
                CreateClass(
                    "companion");

            firstSpecialization =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();

            firstSpecialization.InitializeForTests(
                newSpecializationId:
                    "berserker",
                newRequiredClassId:
                    "companion",
                newSkillReplacements:
                    new[]
                    {
                        new SkillReplacementDefinition(
                            "meteor",
                            "blood_meteor")
                    });

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            firstClass
                        },
                        new[]
                        {
                            firstSpecialization
                        }),
                Throws.TypeOf<
                    InvalidOperationException>());
        }

        [Test]
        public void Validate_StackedStatModifiersComposeCorrectly()
        {
            firstClass =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();

            firstClass.InitializeForTests(
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

            firstSpecialization =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();

            firstSpecialization.InitializeForTests(
                newSpecializationId:
                    "guardian",
                newRequiredClassId:
                    "companion",
                newStatModifiers:
                    new[]
                    {
                        new CharacterStatModifierDefinition(
                            FightStatType.MaxHealth,
                            FightStatModifierType.Percent,
                            50)
                    });

            Assert.That(
                () =>
                    validator.Validate(
                        new[]
                        {
                            firstClass
                        },
                        new[]
                        {
                            firstSpecialization
                        }),
                Throws.Nothing);

            CharacterBuildSnapshot snapshot =
                new CharacterBuildComposer()
                    .Compose(
                        new CharacterBuildCompositionRequest(
                            firstClass,
                            firstSpecialization));

            Assert.That(
                snapshot.StatModifiers,
                Has.Count.EqualTo(2));

            int finalMaxHealth =
                FightStatCalculator.Calculate(
                    FightStatType.MaxHealth,
                    baseValue:
                        10,
                    snapshot.StatModifiers);

            Assert.That(
                finalMaxHealth,
                Is.EqualTo(30));
        }

        private static ClassDefinition CreateClass(
            string classId)
        {
            ClassDefinition definition =
                ScriptableObject.CreateInstance<
                    ClassDefinition>();

            definition.InitializeForTests(
                classId);

            return definition;
        }

        private static SpecializationDefinition
            CreateSpecialization(
                string specializationId,
                string requiredClassId)
        {
            SpecializationDefinition definition =
                ScriptableObject.CreateInstance<
                    SpecializationDefinition>();

            definition.InitializeForTests(
                specializationId,
                requiredClassId);

            return definition;
        }

        private static void DestroyDefinition(
            UnityEngine.Object definition)
        {
            if (definition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    definition);
            }
        }
    }
}