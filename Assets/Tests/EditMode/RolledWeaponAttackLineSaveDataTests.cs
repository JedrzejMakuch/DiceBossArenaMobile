using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class RolledWeaponAttackLineSaveDataTests
    {
        [Test]
        public void Constructor_StoresValues()
        {
            RolledWeaponAttackLineSaveData data =
                CreateSaveData();

            Assert.That(
                data.LineId,
                Is.EqualTo("primary_damage"));

            Assert.That(
                data.Element,
                Is.EqualTo(WeaponAttackElement.Fire));

            Assert.That(
                data.MinDamage,
                Is.EqualTo(4));

            Assert.That(
                data.MaxDamage,
                Is.EqualTo(8));
        }

        [Test]
        public void JsonUtility_RoundTrip_PreservesValues()
        {
            RolledWeaponAttackLineSaveData original =
                CreateSaveData();

            string json =
                JsonUtility.ToJson(original);

            RolledWeaponAttackLineSaveData restored =
                JsonUtility.FromJson<
                    RolledWeaponAttackLineSaveData>(
                    json);

            Assert.That(
                restored,
                Is.Not.Null);

            Assert.That(
                restored.LineId,
                Is.EqualTo(original.LineId));

            Assert.That(
                restored.Element,
                Is.EqualTo(original.Element));

            Assert.That(
                restored.MinDamage,
                Is.EqualTo(original.MinDamage));

            Assert.That(
                restored.MaxDamage,
                Is.EqualTo(original.MaxDamage));
        }

        private static RolledWeaponAttackLineSaveData
            CreateSaveData()
        {
            return new RolledWeaponAttackLineSaveData(
                "primary_damage",
                WeaponAttackElement.Fire,
                4,
                8);
        }
    }
}