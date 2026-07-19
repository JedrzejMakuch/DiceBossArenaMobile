using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EquipmentBaseTypeDefinitionTests
    {
        [Test]
        public void InitializeForTests_AssignsDefinitionData()
        {
            EquipmentBaseTypeDefinition definition =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            try
            {
                definition.InitializeForTests(
                    "  iron_sword  ",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

                Assert.That(
                    definition.BaseTypeId.Value,
                    Is.EqualTo("iron_sword"));

                Assert.That(
                    definition.SlotType,
                    Is.EqualTo(
                        EquipmentSlotType.MainHand));

                Assert.That(
                    definition.Category,
                    Is.EqualTo(
                        EquipmentBaseTypeCategory.Sword));
            }
            finally
            {
                Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void InitializeForTests_AssignsStatModifiers()
        {
            CharacterStatModifierDefinition modifier =
                new CharacterStatModifierDefinition(
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    8);

            EquipmentBaseTypeDefinition definition =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            try
            {
                definition.InitializeForTests(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword,
                    new[]
                    {
                modifier
                    });

                Assert.That(
                    definition.StatModifiers,
                    Has.Count.EqualTo(1));

                Assert.That(
                    definition.StatModifiers[0],
                    Is.SameAs(modifier));
            }
            finally
            {
                Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void InitializeForTests_WithoutStatModifiers_UsesEmptyList()
        {
            EquipmentBaseTypeDefinition definition =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            try
            {
                definition.InitializeForTests(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

                Assert.That(
                    definition.StatModifiers,
                    Is.Not.Null);

                Assert.That(
                    definition.StatModifiers,
                    Is.Empty);
            }
            finally
            {
                Object.DestroyImmediate(definition);
            }
        }
    }
}