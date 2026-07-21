using DiceBossArena.Game;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class CharacterItemInstanceGeneratorTests
    {
        [Test]
        public void Generate_Magic_StoresTwoGeneratedAffixes()
        {
            EquipmentBaseTypeDefinition baseType =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            ItemDefinition itemDefinition =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();

            try
            {
                baseType.InitializeForTests(
                    "sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

                itemDefinition.InitializeForTests(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    newBaseType: baseType);

                EquipmentAffixPoolDefinition affixPool =
                    new EquipmentAffixPoolDefinition(
                        new[]
                        {
                    new EquipmentAffixPoolEntryDefinition(
                        "strength_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "vitality_flat",
                        5)
                        });

                CharacterItemInstanceGenerationRequest request =
                    new CharacterItemInstanceGenerationRequest(
                        itemDefinition,
                        affixPool,
                        10,
                        2,
                        1);

                CharacterItemInstanceGenerator generator =
                    new CharacterItemInstanceGenerator(
                        new StubInstanceIdGenerator(),
                        CreateMagicRarityRoller(),
                        CreateMagicAffixGenerator());

                CharacterItemInstance result =
                    generator.Generate(request);

                Assert.That(
                    result.Rarity,
                    Is.EqualTo(EquipmentItemRarity.Magic));

                Assert.That(
                    result.Affixes.Count,
                    Is.EqualTo(2));

                Assert.That(
                    result.Affixes[0].AffixId,
                    Is.EqualTo(
                        new EquipmentAffixId(
                            "strength_flat")));

                Assert.That(
                    result.Affixes[0].Value,
                    Is.EqualTo(2));

                Assert.That(
                    result.Affixes[1].AffixId,
                    Is.EqualTo(
                        new EquipmentAffixId(
                            "vitality_flat")));

                Assert.That(
                    result.Affixes[1].Value,
                    Is.EqualTo(4));

                Assert.That(
                    result.Affixes[0].AffixId,
                    Is.Not.EqualTo(
                        result.Affixes[1].AffixId));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(
                    itemDefinition);

                UnityEngine.Object.DestroyImmediate(
                    baseType);
            }
        }

        [Test]
        public void Generate_Rare_StoresFourGeneratedAffixes()
        {
            EquipmentBaseTypeDefinition baseType =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            ItemDefinition itemDefinition =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();

            try
            {
                baseType.InitializeForTests(
                    "sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

                itemDefinition.InitializeForTests(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    newBaseType: baseType);

                EquipmentAffixPoolDefinition affixPool =
                    new EquipmentAffixPoolDefinition(
                        new[]
                        {
                    new EquipmentAffixPoolEntryDefinition(
                        "strength_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "vitality_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "dexterity_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "intelligence_flat",
                        10)
                        });

                CharacterItemInstanceGenerationRequest request =
                    new CharacterItemInstanceGenerationRequest(
                        itemDefinition,
                        affixPool,
                        10,
                        2,
                        1);

                CharacterItemInstanceGenerator generator =
                    new CharacterItemInstanceGenerator(
                        new StubInstanceIdGenerator(),
                        CreateRareRarityRoller(),
                        CreateRareAffixGenerator());

                CharacterItemInstance result =
                    generator.Generate(request);

                Assert.That(
                    result.Rarity,
                    Is.EqualTo(EquipmentItemRarity.Rare));

                Assert.That(
                    result.Affixes.Count,
                    Is.EqualTo(4));

                Assert.That(
                    result.Affixes[0].AffixId,
                    Is.EqualTo(
                        new EquipmentAffixId(
                            "strength_flat")));

                Assert.That(
                    result.Affixes[0].Value,
                    Is.EqualTo(1));

                Assert.That(
                    result.Affixes[1].AffixId,
                    Is.EqualTo(
                        new EquipmentAffixId(
                            "vitality_flat")));

                Assert.That(
                    result.Affixes[1].Value,
                    Is.EqualTo(2));

                Assert.That(
                    result.Affixes[2].AffixId,
                    Is.EqualTo(
                        new EquipmentAffixId(
                            "dexterity_flat")));

                Assert.That(
                    result.Affixes[2].Value,
                    Is.EqualTo(3));

                Assert.That(
                    result.Affixes[3].AffixId,
                    Is.EqualTo(
                        new EquipmentAffixId(
                            "intelligence_flat")));

                Assert.That(
                    result.Affixes[3].Value,
                    Is.EqualTo(4));

                HashSet<EquipmentAffixId> uniqueAffixIds =
                    new HashSet<EquipmentAffixId>();

                foreach (RolledEquipmentAffix affix
                         in result.Affixes)
                {
                    uniqueAffixIds.Add(
                        affix.AffixId);
                }

                Assert.That(
                    uniqueAffixIds.Count,
                    Is.EqualTo(4));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(
                    itemDefinition);

                UnityEngine.Object.DestroyImmediate(
                    baseType);
            }
        }

        [Test]
        public void Constructor_ValidDependencies_DoesNotThrow()
        {
            Assert.That(
                () => new CharacterItemInstanceGenerator(
                    new StubInstanceIdGenerator(),
                    CreateRarityRoller(),
                    CreateAffixGenerator()),
                Throws.Nothing);
        }

        [Test]
        public void Constructor_NullInstanceIdGenerator_Throws()
        {
            Assert.That(
                () => new CharacterItemInstanceGenerator(
                    null,
                    CreateRarityRoller(),
                    CreateAffixGenerator()),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_NullRarityRoller_Throws()
        {
            Assert.That(
                () => new CharacterItemInstanceGenerator(
                    new StubInstanceIdGenerator(),
                    null,
                    CreateAffixGenerator()),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_NullAffixGenerator_Throws()
        {
            Assert.That(
                () => new CharacterItemInstanceGenerator(
                    new StubInstanceIdGenerator(),
                    CreateRarityRoller(),
                    null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Generate_NullRequest_Throws()
        {
            CharacterItemInstanceGenerator generator =
                new CharacterItemInstanceGenerator(
                    new StubInstanceIdGenerator(),
                    CreateRarityRoller(),
                    CreateAffixGenerator());

            Assert.That(
                () => generator.Generate(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Generate_Common_CreatesCompleteInstance()
        {
            EquipmentBaseTypeDefinition baseType =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            ItemDefinition itemDefinition =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();

            try
            {
                baseType.InitializeForTests(
                    "sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

                itemDefinition.InitializeForTests(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    newBaseType: baseType);

                CharacterItemInstanceGenerationRequest request =
                    new CharacterItemInstanceGenerationRequest(
                        itemDefinition,
                        new EquipmentAffixPoolDefinition(),
                        10,
                        2,
                        1);

                CharacterItemInstanceGenerator generator =
                    new CharacterItemInstanceGenerator(
                        new StubInstanceIdGenerator(),
                        CreateRarityRoller(),
                        CreateAffixGenerator());

                CharacterItemInstance result =
                    generator.Generate(request);

                Assert.That(
                    result.InstanceId,
                    Is.EqualTo(
                        new CharacterItemInstanceId(
                            "instance_001")));

                Assert.That(
                    result.ItemId,
                    Is.EqualTo(
                        new CharacterItemId(
                            "iron_sword")));

                Assert.That(
                    result.BaseTypeId,
                    Is.EqualTo(
                        new EquipmentBaseTypeId(
                            "sword")));

                Assert.That(
                    result.Level,
                    Is.EqualTo(10));

                Assert.That(
                    result.UpgradeLevel,
                    Is.EqualTo(2));

                Assert.That(
                    result.Quantity,
                    Is.EqualTo(1));

                Assert.That(
                    result.Rarity,
                    Is.EqualTo(
                        EquipmentItemRarity.Common));

                Assert.That(
                    result.Affixes,
                    Is.Empty);
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(
                    itemDefinition);

                UnityEngine.Object.DestroyImmediate(
                    baseType);
            }
        }

        [Test]
        public void Generate_IdenticalRandomSequences_CreatesIdenticalData()
        {
            EquipmentBaseTypeDefinition baseType =
                ScriptableObject.CreateInstance<
                    EquipmentBaseTypeDefinition>();

            ItemDefinition itemDefinition =
                ScriptableObject.CreateInstance<
                    ItemDefinition>();

            try
            {
                baseType.InitializeForTests(
                    "sword",
                    EquipmentSlotType.MainHand,
                    EquipmentBaseTypeCategory.Sword);

                itemDefinition.InitializeForTests(
                    "iron_sword",
                    EquipmentSlotType.MainHand,
                    newBaseType: baseType);

                EquipmentAffixPoolDefinition affixPool =
                    new EquipmentAffixPoolDefinition(
                        new[]
                        {
                    new EquipmentAffixPoolEntryDefinition(
                        "strength_flat",
                        10),

                    new EquipmentAffixPoolEntryDefinition(
                        "vitality_flat",
                        5)
                        });

                CharacterItemInstanceGenerationRequest request =
                    new CharacterItemInstanceGenerationRequest(
                        itemDefinition,
                        affixPool,
                        10,
                        2,
                        1);

                CharacterItemInstance first =
                    CreateDeterministicMagicGenerator()
                        .Generate(request);

                CharacterItemInstance second =
                    CreateDeterministicMagicGenerator()
                        .Generate(request);

                Assert.That(
                    second.InstanceId,
                    Is.EqualTo(first.InstanceId));

                Assert.That(
                    second.ItemId,
                    Is.EqualTo(first.ItemId));

                Assert.That(
                    second.BaseTypeId,
                    Is.EqualTo(first.BaseTypeId));

                Assert.That(
                    second.Level,
                    Is.EqualTo(first.Level));

                Assert.That(
                    second.UpgradeLevel,
                    Is.EqualTo(first.UpgradeLevel));

                Assert.That(
                    second.Quantity,
                    Is.EqualTo(first.Quantity));

                Assert.That(
                    second.Rarity,
                    Is.EqualTo(first.Rarity));

                Assert.That(
                    second.Affixes.Count,
                    Is.EqualTo(first.Affixes.Count));

                for (int index = 0;
                     index < first.Affixes.Count;
                     index++)
                {
                    Assert.That(
                        second.Affixes[index].AffixId,
                        Is.EqualTo(
                            first.Affixes[index].AffixId));

                    Assert.That(
                        second.Affixes[index].Value,
                        Is.EqualTo(
                            first.Affixes[index].Value));
                }
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(
                    itemDefinition);

                UnityEngine.Object.DestroyImmediate(
                    baseType);
            }
        }

        private static CharacterItemInstanceGenerator
    CreateDeterministicMagicGenerator()
        {
            EquipmentItemRarityRoller rarityRoller =
                new EquipmentItemRarityRoller(
                    new SequenceRandomSource(0),
                    0,
                    1,
                    0);

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            1,
                            1,
                            3)
                    }),

                new EquipmentAffixDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            1,
                            2,
                            5)
                    })
                    });

            EquipmentAffixCollectionByRarityGenerator
                affixGenerator =
                    new EquipmentAffixCollectionByRarityGenerator(
                        catalog,
                        new SequenceRandomSource(
                            0,
                            2,
                            0,
                            4));

            return new CharacterItemInstanceGenerator(
                new StubInstanceIdGenerator(),
                rarityRoller,
                affixGenerator);
        }

        private static EquipmentItemRarityRoller
    CreateRareRarityRoller()
        {
            return new EquipmentItemRarityRoller(
                new StubRandomSource(),
                0,
                0,
                1);
        }

        private static EquipmentAffixCollectionByRarityGenerator
    CreateRareAffixGenerator()
        {
            EquipmentAffixDefinition strength =
                CreateRareAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength);

            EquipmentAffixDefinition vitality =
                CreateRareAffixDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth);

            EquipmentAffixDefinition dexterity =
                CreateRareAffixDefinition(
                    "dexterity_flat",
                    FightStatType.Dexterity);

            EquipmentAffixDefinition intelligence =
                CreateRareAffixDefinition(
                    "intelligence_flat",
                    FightStatType.Intelligence);

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                strength,
                vitality,
                dexterity,
                intelligence
                    });

            return new EquipmentAffixCollectionByRarityGenerator(
                catalog,
                new SequenceRandomSource(
                    0,
                    1,
                    0,
                    2,
                    0,
                    3,
                    0,
                    4));
        }

        private static EquipmentAffixDefinition
            CreateRareAffixDefinition(
                string id,
                FightStatType statType)
        {
            return new EquipmentAffixDefinition(
                id,
                statType,
                FightStatModifierType.Flat,
                new[]
                {
            new EquipmentAffixTierDefinition(
                1,
                1,
                5)
                });
        }

        private static EquipmentItemRarityRoller
            CreateRarityRoller()
        {
            return new EquipmentItemRarityRoller(
                new StubRandomSource(),
                1,
                0,
                0);
        }

        private static EquipmentAffixCollectionByRarityGenerator
            CreateAffixGenerator()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                        new EquipmentAffixTierDefinition(
                            1,
                            1,
                            5)
                    });

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        definition
                    });

            return new EquipmentAffixCollectionByRarityGenerator(
                catalog,
                new StubRandomSource());
        }

        private sealed class StubInstanceIdGenerator :
            ICharacterItemInstanceIdGenerator
        {
            public CharacterItemInstanceId Generate()
            {
                return new CharacterItemInstanceId(
                    "instance_001");
            }
        }

        private sealed class StubRandomSource :
            IEquipmentAffixRandomSource
        {
            public int Next(
                int minimumInclusive,
                int maximumExclusive)
            {
                return minimumInclusive;
            }
        }

        private static EquipmentItemRarityRoller
    CreateMagicRarityRoller()
        {
            return new EquipmentItemRarityRoller(
                new StubRandomSource(),
                0,
                1,
                0);
        }

        private static EquipmentAffixCollectionByRarityGenerator
            CreateMagicAffixGenerator()
        {
            EquipmentAffixDefinition strength =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    new[]
                    {
                new EquipmentAffixTierDefinition(
                    1,
                    1,
                    3)
                    });

            EquipmentAffixDefinition vitality =
                new EquipmentAffixDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    new[]
                    {
                new EquipmentAffixTierDefinition(
                    1,
                    2,
                    5)
                    });

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                strength,
                vitality
                    });

            return new EquipmentAffixCollectionByRarityGenerator(
                catalog,
                new SequenceRandomSource(
                    0,
                    2,
                    0,
                    4));
        }

        private sealed class SequenceRandomSource :
    IEquipmentAffixRandomSource
        {
            private readonly Queue<int> values;

            public SequenceRandomSource(
                params int[] newValues)
            {
                values =
                    new Queue<int>(
                        newValues);
            }

            public int Next(
                int minimumInclusive,
                int maximumExclusive)
            {
                if (values.Count == 0)
                {
                    throw new InvalidOperationException(
                        "No configured random value remains.");
                }

                return values.Dequeue();
            }
        }
    }
}