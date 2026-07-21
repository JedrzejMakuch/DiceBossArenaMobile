using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class CharacterItemInstanceSaveDataTests
    {
        [Test]
        public void Constructor_StoresCompleteItemData()
        {
            CharacterItemInstanceSaveData data =
                CreateSaveData();

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
                Is.EqualTo(2));

            Assert.That(
                data.Affixes[0].AffixId,
                Is.EqualTo("strength_flat"));

            Assert.That(
                data.Affixes[0].Value,
                Is.EqualTo(7));

            Assert.That(
                data.Affixes[1].AffixId,
                Is.EqualTo("vitality_flat"));

            Assert.That(
                data.Affixes[1].Value,
                Is.EqualTo(4));
        }

        [Test]
        public void JsonUtility_RoundTrip_PreservesCompleteItemData()
        {
            CharacterItemInstanceSaveData original =
                CreateSaveData();

            string json =
                JsonUtility.ToJson(original);

            CharacterItemInstanceSaveData restored =
                JsonUtility.FromJson<
                    CharacterItemInstanceSaveData>(
                    json);

            Assert.That(
                restored,
                Is.Not.Null);

            Assert.That(
                restored.InstanceId,
                Is.EqualTo(original.InstanceId));

            Assert.That(
                restored.ItemId,
                Is.EqualTo(original.ItemId));

            Assert.That(
                restored.BaseTypeId,
                Is.EqualTo(original.BaseTypeId));

            Assert.That(
                restored.Level,
                Is.EqualTo(original.Level));

            Assert.That(
                restored.UpgradeLevel,
                Is.EqualTo(original.UpgradeLevel));

            Assert.That(
                restored.Quantity,
                Is.EqualTo(original.Quantity));

            Assert.That(
                restored.Rarity,
                Is.EqualTo(original.Rarity));

            Assert.That(
                restored.Affixes.Length,
                Is.EqualTo(2));

            Assert.That(
                restored.Affixes[0].AffixId,
                Is.EqualTo("strength_flat"));

            Assert.That(
                restored.Affixes[0].Value,
                Is.EqualTo(7));

            Assert.That(
                restored.Affixes[1].AffixId,
                Is.EqualTo("vitality_flat"));

            Assert.That(
                restored.Affixes[1].Value,
                Is.EqualTo(4));
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
                        7),

                    new RolledEquipmentAffixSaveData(
                        "vitality_flat",
                        4)
                });
        }
    }
}