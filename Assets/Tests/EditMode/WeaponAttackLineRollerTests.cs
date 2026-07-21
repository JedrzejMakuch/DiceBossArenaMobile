using System;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class WeaponAttackLineRollerTests
    {
        [Test]
        public void Constructor_NullRandomSource_Throws()
        {
            Assert.That(
                () => new WeaponAttackLineRoller(
                    null,
                    CreateValidator()),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_NullValidator_Throws()
        {
            Assert.That(
                () => new WeaponAttackLineRoller(
                    new StubRandomSource(0),
                    null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Roll_NullDefinition_Throws()
        {
            Assert.That(
                () => CreateRoller(0).Roll(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Roll_InvalidDefinition_Throws()
        {
            WeaponAttackLineGenerationDefinition definition =
                new WeaponAttackLineGenerationDefinition(
                    "primary_damage",
                    4,
                    8,
                    Array.Empty<WeaponAttackElement>());

            Assert.That(
                () => CreateRoller(0).Roll(definition),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Roll_RequestsAllowedElementIndexRange()
        {
            StubRandomSource randomSource =
                new StubRandomSource(0);

            CreateRoller(randomSource).Roll(
                CreateDefinition());

            Assert.That(
                randomSource.LastMinimumInclusive,
                Is.EqualTo(0));

            Assert.That(
                randomSource.LastMaximumExclusive,
                Is.EqualTo(2));
        }

        [Test]
        public void Roll_SelectedElement_CreatesRolledLine()
        {
            RolledWeaponAttackLine result =
                CreateRoller(1).Roll(
                    CreateDefinition());

            Assert.That(
                result.LineId,
                Is.EqualTo(
                    new WeaponAttackLineId(
                        "primary_damage")));

            Assert.That(
                result.Element,
                Is.EqualTo(
                    WeaponAttackElement.Water));

            Assert.That(
                result.MinDamage,
                Is.EqualTo(4));

            Assert.That(
                result.MaxDamage,
                Is.EqualTo(8));
        }

        private static WeaponAttackLineGenerationDefinition
            CreateDefinition()
        {
            return new WeaponAttackLineGenerationDefinition(
                "primary_damage",
                4,
                8,
                new[]
                {
                    WeaponAttackElement.Fire,
                    WeaponAttackElement.Water
                });
        }

        private static WeaponAttackLineRoller
            CreateRoller(int randomValue)
        {
            return CreateRoller(
                new StubRandomSource(
                    randomValue));
        }

        private static WeaponAttackLineRoller
            CreateRoller(
                StubRandomSource randomSource)
        {
            return new WeaponAttackLineRoller(
                randomSource,
                CreateValidator());
        }

        private static
            WeaponAttackLineGenerationDefinitionValidator
            CreateValidator()
        {
            return new
                WeaponAttackLineGenerationDefinitionValidator();
        }

        private sealed class StubRandomSource :
            IEquipmentAffixRandomSource
        {
            private readonly int value;

            public StubRandomSource(int newValue)
            {
                value = newValue;
            }

            public int LastMinimumInclusive
            {
                get;
                private set;
            }

            public int LastMaximumExclusive
            {
                get;
                private set;
            }

            public int Next(
                int minimumInclusive,
                int maximumExclusive)
            {
                LastMinimumInclusive =
                    minimumInclusive;

                LastMaximumExclusive =
                    maximumExclusive;

                return value;
            }
        }
    }
}