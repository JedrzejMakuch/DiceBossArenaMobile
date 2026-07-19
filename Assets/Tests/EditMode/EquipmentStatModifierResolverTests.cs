using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentStatModifierResolverTests
    {
        private ItemDefinition item;

        [SetUp]
        public void SetUp()
        {
            item =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(item);
        }

        [Test]
        public void Resolve_EquippedItem_ReturnsItsStatModifiers()
        {
            CharacterStatModifierDefinition modifier =
                new CharacterStatModifierDefinition(
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    5);

            item.InitializeForTests(
                "starter_sword",
                EquipmentSlotType.MainHand,
                newStatModifiers:
                    new[]
                    {
                        modifier
                    });

            ItemDefinitionCatalog catalog =
                new ItemDefinitionCatalog(
                    new[]
                    {
                        item
                    });

            EquipmentLoadoutSnapshot loadout =
                new EquipmentLoadoutSnapshot(
                    new[]
                    {
                        new EquippedItemSnapshot(
                            EquipmentSlotType.MainHand,
                            new CharacterItemId(
                                "starter_sword"))
                    });

            EquipmentStatModifierResolver resolver =
                new EquipmentStatModifierResolver(
                    catalog);

            IReadOnlyList<FightStatModifier> result =
                resolver.Resolve(loadout);

            Assert.That(
                result.Count,
                Is.EqualTo(1));

            Assert.That(
                result[0].StatType,
                Is.EqualTo(
                    FightStatType.Strength));

            Assert.That(
                result[0].ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));

            Assert.That(
                result[0].Value,
                Is.EqualTo(5));
        }

        [Test]
        public void Resolve_ItemWithBaseType_ReturnsBaseAndItemModifiers()
        {
            EquipmentBaseTypeDefinition baseType =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            try
            {
                CharacterStatModifierDefinition
                    baseModifier =
                        new CharacterStatModifierDefinition(
                            FightStatType.Strength,
                            FightStatModifierType.Flat,
                            3);

                CharacterStatModifierDefinition
                    itemModifier =
                        new CharacterStatModifierDefinition(
                            FightStatType.Strength,
                            FightStatModifierType.Flat,
                            5);

                baseType.InitializeForTests(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword,
                    new[]
                    {
                baseModifier
                    });

                item.InitializeForTests(
                    "starter_sword",
                    EquipmentSlotType.MainHand,
                    newStatModifiers:
                        new[]
                        {
                    itemModifier
                        },
                    newBaseType:
                        baseType);

                ItemDefinitionCatalog catalog =
                    new ItemDefinitionCatalog(
                        new[]
                        {
                    item
                        });

                EquipmentLoadoutSnapshot loadout =
                    new EquipmentLoadoutSnapshot(
                        new[]
                        {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.MainHand,
                        new CharacterItemId(
                            "starter_sword"))
                        });

                EquipmentStatModifierResolver resolver =
                    new EquipmentStatModifierResolver(
                        catalog);

                IReadOnlyList<FightStatModifier> result =
                    resolver.Resolve(loadout);

                Assert.That(
                    result,
                    Has.Count.EqualTo(2));

                Assert.That(
                    result[0].Value,
                    Is.EqualTo(3));

                Assert.That(
                    result[1].Value,
                    Is.EqualTo(5));
            }
            finally
            {
                Object.DestroyImmediate(baseType);
            }
        }

        [Test]
        public void Resolve_TwoEquippedItems_ReturnsModifiersFromBothItems()
        {
            ItemDefinition sword =
                ScriptableObject.CreateInstance<ItemDefinition>();

            ItemDefinition helmet =
                ScriptableObject.CreateInstance<ItemDefinition>();

            try
            {
                sword.InitializeForTests(
                    "sword",
                    EquipmentSlotType.MainHand,
                    newStatModifiers:
                        new[]
                        {
                    new CharacterStatModifierDefinition(
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        10)
                        });

                helmet.InitializeForTests(
                    "helmet",
                    EquipmentSlotType.Head,
                    newStatModifiers:
                        new[]
                        {
                    new CharacterStatModifierDefinition(
                        FightStatType.FireResistance,
                        FightStatModifierType.Flat,
                        15)
                        });

                ItemDefinitionCatalog catalog =
                    new ItemDefinitionCatalog(
                        new[]
                        {
                    sword,
                    helmet
                        });

                EquipmentLoadoutSnapshot loadout =
                    new EquipmentLoadoutSnapshot(
                        new[]
                        {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.MainHand,
                        new CharacterItemId("sword")),

                    new EquippedItemSnapshot(
                        EquipmentSlotType.Head,
                        new CharacterItemId("helmet"))
                        });

                EquipmentStatModifierResolver resolver =
                    new EquipmentStatModifierResolver(catalog);

                IReadOnlyList<FightStatModifier> result =
                    resolver.Resolve(loadout);

                Assert.That(
                    result[0].StatType,
                    Is.EqualTo(FightStatType.Strength));

                Assert.That(
                    result[0].ModifierType,
                    Is.EqualTo(FightStatModifierType.Flat));

                Assert.That(
                    result[0].Value,
                    Is.EqualTo(10));

                Assert.That(
                    result[1].StatType,
                    Is.EqualTo(FightStatType.FireResistance));

                Assert.That(
                    result[1].ModifierType,
                    Is.EqualTo(FightStatModifierType.Flat));

                Assert.That(
                    result[1].Value,
                    Is.EqualTo(15));
            }
                            finally
            {
                Object.DestroyImmediate(sword);
                Object.DestroyImmediate(helmet);
            }
        }
    }
}