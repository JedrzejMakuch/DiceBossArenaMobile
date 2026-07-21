using DiceBossArena.Game;
using NUnit.Framework;
using System;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class CharacterItemInstanceSaveDataMapperTests
    {
        [Test]
        public void Constructor_NullAffixMapper_Throws()
        {
            Assert.That(
                () => new CharacterItemInstanceSaveDataMapper(
                    null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ToSaveData_InvalidItem_Throws()
        {
            CharacterItemInstanceSaveDataMapper mapper =
                CreateMapper();

            Assert.That(
                () => mapper.ToSaveData(default),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void ToSaveData_ValidItem_StoresCompleteData()
        {
            CharacterItemInstance item =
                CreateItem();

            CharacterItemInstanceSaveData data =
                CreateMapper().ToSaveData(
                    item);

            Assert.That(
                data.InstanceId,
                Is.EqualTo("instance_001"));

            Assert.That(
                data.ItemId,
                Is.EqualTo("iron_sword"));

            Assert.That(
                data.BaseTypeId,
                Is.EqualTo("sword"));

            Assert.That(
                data.Level,
                Is.EqualTo(10));

            Assert.That(
                data.UpgradeLevel,
                Is.EqualTo(2));

            Assert.That(
                data.Quantity,
                Is.EqualTo(1));

            Assert.That(
                data.Rarity,
                Is.EqualTo(EquipmentItemRarity.Magic));

            Assert.That(
                data.Affixes.Length,
                Is.EqualTo(1));

            Assert.That(
                data.Affixes[0].AffixId,
                Is.EqualTo("strength_flat"));

            Assert.That(
                data.Affixes[0].Value,
                Is.EqualTo(7));
        }

        [Test]
        public void FromSaveData_NullData_Throws()
        {
            CharacterItemInstanceSaveDataMapper mapper =
                CreateMapper();

            Assert.That(
                () => mapper.FromSaveData(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void FromSaveData_NullAffixes_Throws()
        {
            CharacterItemInstanceSaveData data =
                CreateSaveData();

            data.Affixes = null;

            Assert.That(
                () => CreateMapper().FromSaveData(data),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void JsonRoundTrip_ValidItem_RestoresIdenticalItem()
        {
            CharacterItemInstanceSaveDataMapper mapper =
                CreateMapper();

            CharacterItemInstance original =
                CreateItem();

            CharacterItemInstanceSaveData saveData =
                mapper.ToSaveData(
                    original);

            string json =
                JsonUtility.ToJson(
                    saveData);

            CharacterItemInstanceSaveData restoredSaveData =
                JsonUtility.FromJson<
                    CharacterItemInstanceSaveData>(
                    json);

            CharacterItemInstance restored =
                mapper.FromSaveData(
                    restoredSaveData);

            Assert.That(
                restored,
                Is.EqualTo(original));
        }

        [Test]
        public void JsonRoundTrip_ItemWithoutAffixes_RestoresIdenticalItem()
        {
            CharacterItemInstanceSaveDataMapper mapper =
                CreateMapper();

            CharacterItemInstance original =
                new CharacterItemInstance(
                    new CharacterItemInstanceId(
                        "instance_002"),
                    new CharacterItemId(
                        "iron_sword"),
                    new EquipmentBaseTypeId(
                        "sword"),
                    5,
                    0,
                    1,
                    EquipmentItemRarity.Common,
                    Array.Empty<RolledEquipmentAffix>());

            CharacterItemInstanceSaveData saveData =
                mapper.ToSaveData(
                    original);

            string json =
                JsonUtility.ToJson(
                    saveData);

            CharacterItemInstanceSaveData restoredSaveData =
                JsonUtility.FromJson<
                    CharacterItemInstanceSaveData>(
                    json);

            CharacterItemInstance restored =
                mapper.FromSaveData(
                    restoredSaveData);

            Assert.That(
                restored,
                Is.EqualTo(original));

            Assert.That(
                restored.Affixes,
                Is.Empty);
        }

        [Test]
        public void FromSaveData_ValidData_RestoresCompleteItem()
        {
            CharacterItemInstance restored =
                CreateMapper().FromSaveData(
                    CreateSaveData());

            Assert.That(
                restored,
                Is.EqualTo(CreateItem()));
        }

        private static CharacterItemInstanceSaveDataMapper
            CreateMapper()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat);

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        definition
                    });

            RolledEquipmentAffixSaveDataMapper affixMapper =
                new RolledEquipmentAffixSaveDataMapper(
                    catalog);

            return new CharacterItemInstanceSaveDataMapper(
                affixMapper);
        }

        private static CharacterItemInstance CreateItem()
        {
            return new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                new EquipmentBaseTypeId(
                    "sword"),
                10,
                2,
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "strength_flat"),
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        7)
                });
        }

        private static CharacterItemInstanceSaveData
            CreateSaveData()
        {
            return new CharacterItemInstanceSaveData(
                "instance_001",
                "iron_sword",
                "sword",
                10,
                2,
                1,
                EquipmentItemRarity.Magic,
                new[]
                {
                    new RolledEquipmentAffixSaveData(
                        "strength_flat",
                        7)
                });
        }
    }
}