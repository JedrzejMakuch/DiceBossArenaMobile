using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class RolledWeaponProfileSaveDataMapperTests
    {
        [Test]
        public void ToSaveData_NullProfile_Throws()
        {
            RolledWeaponProfileSaveDataMapper mapper =
                new RolledWeaponProfileSaveDataMapper();

            Assert.That(
                () => mapper.ToSaveData(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ToSaveData_ValidProfile_StoresLinesInOrder()
        {
            RolledWeaponProfileSaveData data =
                new RolledWeaponProfileSaveDataMapper()
                    .ToSaveData(
                        CreateProfile());

            Assert.That(
                data.Lines.Length,
                Is.EqualTo(2));

            AssertLine(
                data.Lines[0],
                "primary_damage",
                WeaponAttackElement.Neutral,
                4,
                8);

            AssertLine(
                data.Lines[1],
                "secondary_damage",
                WeaponAttackElement.Fire,
                2,
                5);
        }

        [Test]
        public void FromSaveData_NullData_Throws()
        {
            RolledWeaponProfileSaveDataMapper mapper =
                new RolledWeaponProfileSaveDataMapper();

            Assert.That(
                () => mapper.FromSaveData(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void FromSaveData_NullLines_Throws()
        {
            RolledWeaponProfileSaveData data =
                new RolledWeaponProfileSaveData();

            data.Lines = null;

            Assert.That(
                () => new RolledWeaponProfileSaveDataMapper()
                    .FromSaveData(data),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void FromSaveData_NullLine_Throws()
        {
            RolledWeaponProfileSaveData data =
                new RolledWeaponProfileSaveData(
                    new RolledWeaponAttackLineSaveData[]
                    {
                        null
                    });

            Assert.That(
                () => new RolledWeaponProfileSaveDataMapper()
                    .FromSaveData(data),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void RoundTrip_ValidProfile_RestoresEqualProfile()
        {
            RolledWeaponProfileSaveDataMapper mapper =
                new RolledWeaponProfileSaveDataMapper();

            RolledWeaponProfile original =
                CreateProfile();

            RolledWeaponProfileSaveData data =
                mapper.ToSaveData(original);

            RolledWeaponProfile restored =
                mapper.FromSaveData(data);

            Assert.That(
                restored,
                Is.EqualTo(original));
        }

        private static RolledWeaponProfile CreateProfile()
        {
            return new RolledWeaponProfile(
                new[]
                {
                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "primary_damage"),
                        WeaponAttackElement.Neutral,
                        4,
                        8),

                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "secondary_damage"),
                        WeaponAttackElement.Fire,
                        2,
                        5)
                });
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