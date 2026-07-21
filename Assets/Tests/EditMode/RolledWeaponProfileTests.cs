using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class RolledWeaponProfileTests
    {
        [Test]
        public void Constructor_ValidLines_StoresLinesInOrder()
        {
            RolledWeaponAttackLine first =
                CreateLine(
                    "primary_damage",
                    WeaponAttackElement.Neutral,
                    4,
                    8);

            RolledWeaponAttackLine second =
                CreateLine(
                    "secondary_damage",
                    WeaponAttackElement.Fire,
                    2,
                    5);

            RolledWeaponProfile profile =
                new RolledWeaponProfile(
                    new[]
                    {
                        first,
                        second
                    });

            Assert.That(
                profile.Lines.Count,
                Is.EqualTo(2));

            Assert.That(
                profile.Lines[0],
                Is.SameAs(first));

            Assert.That(
                profile.Lines[1],
                Is.SameAs(second));
        }

        [Test]
        public void Constructor_NullCollection_Throws()
        {
            Assert.That(
                () => new RolledWeaponProfile(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_EmptyCollection_Throws()
        {
            Assert.That(
                () => new RolledWeaponProfile(
                    Array.Empty<RolledWeaponAttackLine>()),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Constructor_NullLine_Throws()
        {
            Assert.That(
                () => new RolledWeaponProfile(
                    new RolledWeaponAttackLine[]
                    {
                        CreateLine(
                            "primary_damage",
                            WeaponAttackElement.Neutral,
                            4,
                            8),
                        null
                    }),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Constructor_DuplicateLineId_Throws()
        {
            Assert.That(
                () => new RolledWeaponProfile(
                    new[]
                    {
                        CreateLine(
                            "primary_damage",
                            WeaponAttackElement.Neutral,
                            4,
                            8),

                        CreateLine(
                            "primary_damage",
                            WeaponAttackElement.Fire,
                            2,
                            5)
                    }),
                Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void Constructor_CopiesSourceCollection()
        {
            RolledWeaponAttackLine[] source =
            {
                CreateLine(
                    "primary_damage",
                    WeaponAttackElement.Neutral,
                    4,
                    8)
            };

            RolledWeaponProfile profile =
                new RolledWeaponProfile(source);

            source[0] =
                CreateLine(
                    "replacement",
                    WeaponAttackElement.Fire,
                    1,
                    2);

            Assert.That(
                profile.Lines[0].LineId,
                Is.EqualTo(
                    new WeaponAttackLineId(
                        "primary_damage")));
        }

        [Test]
        public void EqualProfiles_AreEqual()
        {
            RolledWeaponProfile first =
                CreateProfile();

            RolledWeaponProfile second =
                CreateProfile();

            Assert.That(first, Is.EqualTo(second));

            Assert.That(
                first.GetHashCode(),
                Is.EqualTo(second.GetHashCode()));
        }

        [Test]
        public void DifferentLineValues_AreNotEqual()
        {
            RolledWeaponProfile first =
                CreateProfile();

            RolledWeaponProfile second =
                new RolledWeaponProfile(
                    new[]
                    {
                CreateLine(
                    "primary_damage",
                    WeaponAttackElement.Neutral,
                    5,
                    9)
                    });

            Assert.That(first, Is.Not.EqualTo(second));
        }

        [Test]
        public void DifferentLineOrder_IsNotEqual()
        {
            RolledWeaponAttackLine firstLine =
                CreateLine(
                    "primary_damage",
                    WeaponAttackElement.Neutral,
                    4,
                    8);

            RolledWeaponAttackLine secondLine =
                CreateLine(
                    "secondary_damage",
                    WeaponAttackElement.Fire,
                    2,
                    5);

            RolledWeaponProfile first =
                new RolledWeaponProfile(
                    new[]
                    {
                firstLine,
                secondLine
                    });

            RolledWeaponProfile second =
                new RolledWeaponProfile(
                    new[]
                    {
                secondLine,
                firstLine
                    });

            Assert.That(first, Is.Not.EqualTo(second));
        }

        [Test]
        public void Equals_Null_ReturnsFalse()
        {
            Assert.That(
                CreateProfile().Equals(null),
                Is.False);
        }

        private static RolledWeaponProfile CreateProfile()
        {
            return new RolledWeaponProfile(
                new[]
                {
            CreateLine(
                "primary_damage",
                WeaponAttackElement.Neutral,
                4,
                8)
                });
        }

        private static RolledWeaponAttackLine CreateLine(
            string lineId,
            WeaponAttackElement element,
            int minDamage,
            int maxDamage)
        {
            return new RolledWeaponAttackLine(
                new WeaponAttackLineId(lineId),
                element,
                minDamage,
                maxDamage);
        }
    }
}