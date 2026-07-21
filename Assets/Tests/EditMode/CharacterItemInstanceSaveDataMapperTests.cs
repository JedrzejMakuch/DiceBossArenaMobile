using DiceBossArena.Game;
using NUnit.Framework;
using System;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class CharacterItemInstanceSaveDataMapperTests
    {

        private static RolledEquipmentAffixSaveDataMapper
    CreateAffixMapper()
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

            return new RolledEquipmentAffixSaveDataMapper(
                catalog);
        }


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
        public void Constructor_NullWeaponProfileMapper_Throws()
        {
            Assert.That(
                () => new CharacterItemInstanceSaveDataMapper(
                    CreateAffixMapper(),
                    null),
                Throws.TypeOf<ArgumentNullException>());
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

        [Test]
        public void ToSaveData_ItemWithWeaponProfile_StoresProfile()
        {
            CharacterItemInstanceSaveData data =
                CreateMapper().ToSaveData(
                    CreateItemWithWeaponProfile());

            Assert.That(
                data.WeaponProfile,
                Is.Not.Null);

            Assert.That(
                data.WeaponProfile.Lines,
                Has.Length.EqualTo(1));

            Assert.That(
                data.WeaponProfile.Lines[0].LineId,
                Is.EqualTo("primary_damage"));

            Assert.That(
                data.WeaponProfile.Lines[0].Element,
                Is.EqualTo(WeaponAttackElement.Fire));

            Assert.That(
                data.WeaponProfile.Lines[0].MinDamage,
                Is.EqualTo(4));

            Assert.That(
                data.WeaponProfile.Lines[0].MaxDamage,
                Is.EqualTo(8));
        }

        [Test]
        public void JsonRoundTrip_ItemWithWeaponProfile_RestoresIdenticalItem()
        {
            CharacterItemInstanceSaveDataMapper mapper =
                CreateMapper();

            CharacterItemInstance original =
                CreateItemWithWeaponProfile();

            string json =
                JsonUtility.ToJson(
                    mapper.ToSaveData(original));

            CharacterItemInstanceSaveData restoredData =
                JsonUtility.FromJson<
                    CharacterItemInstanceSaveData>(
                    json);

            CharacterItemInstance restored =
                mapper.FromSaveData(restoredData);

            Assert.That(
                restored,
                Is.EqualTo(original));

            Assert.That(
                restored.WeaponProfile,
                Is.EqualTo(original.WeaponProfile));
        }

        private static CharacterItemInstance
    CreateItemWithWeaponProfile()
        {
            return new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_weapon_001"),
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
                },
                new RolledWeaponProfile(
                    new[]
                    {
                new RolledWeaponAttackLine(
                    new WeaponAttackLineId(
                        "primary_damage"),
                    WeaponAttackElement.Fire,
                    4,
                    8)
                    }));
        }

        private static CharacterItemInstanceSaveDataMapper
    CreateMapper()
        {
            return new CharacterItemInstanceSaveDataMapper(
                CreateAffixMapper(),
                new RolledWeaponProfileSaveDataMapper());
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