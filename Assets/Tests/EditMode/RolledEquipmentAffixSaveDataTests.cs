using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class RolledEquipmentAffixSaveDataTests
    {
        [Test]
        public void Constructor_StoresValues()
        {
            RolledEquipmentAffixSaveData data =
                new RolledEquipmentAffixSaveData(
                    "strength_flat",
                    7);

            Assert.That(
                data.AffixId,
                Is.EqualTo("strength_flat"));

            Assert.That(
                data.Value,
                Is.EqualTo(7));
        }

        [Test]
        public void JsonUtility_RoundTrip_PreservesValues()
        {
            RolledEquipmentAffixSaveData original =
                new RolledEquipmentAffixSaveData(
                    "strength_flat",
                    7);

            string json =
                JsonUtility.ToJson(original);

            RolledEquipmentAffixSaveData restored =
                JsonUtility.FromJson<
                    RolledEquipmentAffixSaveData>(
                    json);

            Assert.That(
                restored,
                Is.Not.Null);

            Assert.That(
                restored.AffixId,
                Is.EqualTo(original.AffixId));

            Assert.That(
                restored.Value,
                Is.EqualTo(original.Value));
        }
    }
}