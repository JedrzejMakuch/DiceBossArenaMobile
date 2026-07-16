using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class ItemRequirementValidatorTests
{
    private ItemDefinition definition;
    private ItemRequirementValidator validator;

    [SetUp]
    public void SetUp()
    {
        validator =
            new ItemRequirementValidator();
    }

    [TearDown]
    public void TearDown()
    {
        if (definition != null)
        {
            Object.DestroyImmediate(definition);
        }
    }

    [Test]
    public void MeetsRequirements_NoRequirementsReturnsTrue()
    {
        definition =
            CreateDefinition();

        Assert.That(
            validator.MeetsRequirements(
                definition,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker")),
            Is.True);
    }

    [Test]
    public void MeetsRequirements_MatchingClassReturnsTrue()
    {
        definition =
            CreateDefinition(
                requiredClassIds:
                    new[]
                    {
                        "companion"
                    });

        Assert.That(
            validator.MeetsRequirements(
                definition,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    string.Empty)),
            Is.True);
    }

    [Test]
    public void MeetsRequirements_WrongClassReturnsFalse()
    {
        definition =
            CreateDefinition(
                requiredClassIds:
                    new[]
                    {
                        "companion"
                    });

        Assert.That(
            validator.MeetsRequirements(
                definition,
                new CharacterClassId("mage"),
                new CharacterSpecializationId(
                    string.Empty)),
            Is.False);
    }

    [Test]
    public void MeetsRequirements_MatchingSpecializationReturnsTrue()
    {
        definition =
            CreateDefinition(
                requiredSpecializationIds:
                    new[]
                    {
                        "berserker"
                    });

        Assert.That(
            validator.MeetsRequirements(
                definition,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker")),
            Is.True);
    }

    [Test]
    public void MeetsRequirements_WrongSpecializationReturnsFalse()
    {
        definition =
            CreateDefinition(
                requiredSpecializationIds:
                    new[]
                    {
                        "berserker"
                    });

        Assert.That(
            validator.MeetsRequirements(
                definition,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "guardian")),
            Is.False);
    }

    [Test]
    public void MeetsRequirements_ClassAndSpecializationMustMatch()
    {
        definition =
            CreateDefinition(
                new[]
                {
                    "companion"
                },
                new[]
                {
                    "berserker"
                });

        bool matchingBuild =
            validator.MeetsRequirements(
                definition,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker"));

        bool wrongClass =
            validator.MeetsRequirements(
                definition,
                new CharacterClassId("mage"),
                new CharacterSpecializationId(
                    "berserker"));

        bool wrongSpecialization =
            validator.MeetsRequirements(
                definition,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "guardian"));

        Assert.That(
            matchingBuild,
            Is.True);

        Assert.That(
            wrongClass,
            Is.False);

        Assert.That(
            wrongSpecialization,
            Is.False);
    }

    [Test]
    public void MeetsRequirements_NullDefinitionReturnsFalse()
    {
        Assert.That(
            validator.MeetsRequirements(
                null,
                new CharacterClassId("companion"),
                new CharacterSpecializationId(
                    "berserker")),
            Is.False);
    }

    private static ItemDefinition CreateDefinition(
        string[] requiredClassIds = null,
        string[] requiredSpecializationIds = null)
    {
        ItemDefinition result =
            ScriptableObject.CreateInstance<
                ItemDefinition>();

        result.InitializeForTests(
            "test_item",
            EquipmentSlotType.Accessory,
            1,
            EquipmentItemCategory.Accessory,
            WeaponHandedness.NotApplicable,
            newRequiredClassIds:
                requiredClassIds,
            newRequiredSpecializationIds:
                requiredSpecializationIds);

        return result;
    }
}