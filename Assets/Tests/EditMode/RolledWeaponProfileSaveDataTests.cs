using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class RolledWeaponProfileSaveDataTests
    {
        [Test]
        public void Constructor_StoresLines()
        {
            RolledWeaponAttackLineSaveData[] lines =
                CreateLines();

            RolledWeaponProfileSaveData data =
                new RolledWeaponProfileSaveData(lines);

            Assert.That(
                data.Lines,
                Is.SameAs(lines));

            Assert.That(
                data.Lines.Length,
                Is.EqualTo(2));
        }

        [Test]
        public void JsonUtility_RoundTrip_PreservesLines()
        {
            RolledWeaponProfileSaveData original =
                new RolledWeaponProfileSaveData(
                    CreateLines());

            string json =
                JsonUtility.ToJson(original);

            RolledWeaponProfileSaveData restored =
                JsonUtility.FromJson<
                    RolledWeaponProfileSaveData>(
                    json);

            Assert.That(
                restored,
                Is.Not.Null);

            Assert.That(
                restored.Lines,
                Is.Not.Null);

            Assert.That(
                restored.Lines.Length,
                Is.EqualTo(2));

            AssertLine(
                restored.Lines[0],
                "primary_damage",
                WeaponAttackElement.Neutral,
                4,
                8);

            AssertLine(
                restored.Lines[1],
                "secondary_damage",
                WeaponAttackElement.Fire,
                2,
                5);
        }

        private static RolledWeaponAttackLineSaveData[]
            CreateLines()
        {
            return new[]
            {
                new RolledWeaponAttackLineSaveData(
                    "primary_damage",
                    WeaponAttackElement.Neutral,
                    4,
                    8),

                new RolledWeaponAttackLineSaveData(
                    "secondary_damage",
                    WeaponAttackElement.Fire,
                    2,
                    5)
            };
        }

        private static void AssertLine(
            RolledWeaponAttackLineSaveData line,
            string expectedLineId,
            WeaponAttackElement expectedElement,
            int expectedMinDamage,
            int expectedMaxDamage)
        {
            Assert.That(
                line.LineId,
                Is.EqualTo(expectedLineId));

            Assert.That(
                line.Element,
                Is.EqualTo(expectedElement));

            Assert.That(
                line.MinDamage,
                Is.EqualTo(expectedMinDamage));

            Assert.That(
                line.MaxDamage,
                Is.EqualTo(expectedMaxDamage));
        }
    }
}