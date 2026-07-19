using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentBaseTypeStatModifierResolverTests
    {
        [Test]
        public void Resolve_NullDefinition_Throws()
        {
            EquipmentBaseTypeStatModifierResolver resolver =
                new EquipmentBaseTypeStatModifierResolver();

            Assert.That(
                () => resolver.Resolve(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Resolve_EmptyModifiers_ReturnsEmptyList()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition();

            try
            {
                EquipmentBaseTypeStatModifierResolver resolver =
                    new EquipmentBaseTypeStatModifierResolver();

                var result =
                    resolver.Resolve(definition);

                Assert.That(
                    result,
                    Is.Empty);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Resolve_StatModifier_CreatesRuntimeModifier()
        {
            CharacterStatModifierDefinition modifier =
                new CharacterStatModifierDefinition(
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    8);

            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    modifier);

            try
            {
                EquipmentBaseTypeStatModifierResolver resolver =
                    new EquipmentBaseTypeStatModifierResolver();

                var result =
                    resolver.Resolve(definition);

                Assert.That(
                    result,
                    Has.Count.EqualTo(1));

                Assert.That(
                    result[0].StatType,
                    Is.EqualTo(FightStatType.Strength));

                Assert.That(
                    result[0].ModifierType,
                    Is.EqualTo(
                        FightStatModifierType.Flat));

                Assert.That(
                    result[0].Value,
                    Is.EqualTo(8));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        [Test]
        public void Resolve_NullModifier_Throws()
        {
            EquipmentBaseTypeDefinition definition =
                CreateDefinition(
                    new CharacterStatModifierDefinition[]
                    {
                null
                    });

            try
            {
                EquipmentBaseTypeStatModifierResolver resolver =
                    new EquipmentBaseTypeStatModifierResolver();

                Assert.That(
                    () => resolver.Resolve(definition),
                    Throws.TypeOf<InvalidOperationException>());
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(definition);
            }
        }

        private static EquipmentBaseTypeDefinition
            CreateDefinition(
                params CharacterStatModifierDefinition[]
                    statModifiers)
        {
            EquipmentBaseTypeDefinition definition =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            definition.InitializeForTests(
                "iron_sword",
                EquipmentSlotType.MainHand,
                EquipmentBaseTypeCategory.Sword,
                statModifiers);

            return definition;
        }
    }
}