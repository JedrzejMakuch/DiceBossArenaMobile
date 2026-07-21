using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class WeaponProfileRollerTests
    {
        [Test]
        public void Constructor_NullLineRoller_Throws()
        {
            Assert.That(
                () => new WeaponProfileRoller(
                    null,
                    CreateProfileValidator()),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Constructor_NullProfileValidator_Throws()
        {
            Assert.That(
                () => new WeaponProfileRoller(
                    CreateLineRoller(
                        new StubRandomSource(0)),
                    null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Roll_NullDefinition_Throws()
        {
            Assert.That(
                () => CreateRoller(
                    new StubRandomSource(0))
                    .Roll(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void Roll_InvalidDefinition_Throws()
        {
            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    Array.Empty<
                        WeaponAttackLineGenerationDefinition>());

            Assert.That(
                () => CreateRoller(
                    new StubRandomSource(0))
                    .Roll(definition),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void Roll_AllLines_PreservesOrder()
        {
            WeaponProfileGenerationDefinition definition =
                new WeaponProfileGenerationDefinition(
                    new[]
                    {
                        CreateLine(
                            "primary_damage"),
                        CreateLine(
                            "secondary_damage")
                    });

            RolledWeaponProfile result =
                CreateRoller(
                    new StubRandomSource(
                        0,
                        1))
                    .Roll(definition);

            Assert.That(
                result.Lines,
                Has.Length.EqualTo(2));

            Assert.That(
                result.Lines[0].LineId,
                Is.EqualTo(
                    new WeaponAttackLineId(
                        "primary_damage")));

            Assert.That(
                result.Lines[1].LineId,
                Is.EqualTo(
                    new WeaponAttackLineId(
                        "secondary_damage")));
        }

        [Test]
        public void Roll_AllLines_UsesSelectedElements()
        {
            RolledWeaponProfile result =
                CreateRoller(
                    new StubRandomSource(
                        0,
                        1))
                    .Roll(
                        new WeaponProfileGenerationDefinition(
                            new[]
                            {
                                CreateLine(
                                    "primary_damage"),
                                CreateLine(
                                    "secondary_damage")
                            }));

            Assert.That(
                result.Lines[0].Element,
                Is.EqualTo(
                    WeaponAttackElement.Fire));

            Assert.That(
                result.Lines[1].Element,
                Is.EqualTo(
                    WeaponAttackElement.Water));

            Assert.That(
                result.Lines[0].MinDamage,
                Is.EqualTo(4));

            Assert.That(
                result.Lines[0].MaxDamage,
                Is.EqualTo(8));
        }

        private static
            WeaponAttackLineGenerationDefinition
            CreateLine(string lineId)
        {
            return new WeaponAttackLineGenerationDefinition(
                lineId,
                4,
                8,
                new[]
                {
                    WeaponAttackElement.Fire,
                    WeaponAttackElement.Water
                });
        }

        private static WeaponProfileRoller CreateRoller(
            StubRandomSource randomSource)
        {
            return new WeaponProfileRoller(
                CreateLineRoller(randomSource),
                CreateProfileValidator());
        }

        private static WeaponAttackLineRoller
            CreateLineRoller(
                StubRandomSource randomSource)
        {
            return new WeaponAttackLineRoller(
                randomSource,
                new
                    WeaponAttackLineGenerationDefinitionValidator());
        }

        private static
            WeaponProfileGenerationDefinitionValidator
            CreateProfileValidator()
        {
            return new
                WeaponProfileGenerationDefinitionValidator(
                    new
                        WeaponAttackLineGenerationDefinitionValidator());
        }

        private sealed class StubRandomSource :
            IEquipmentAffixRandomSource
        {
            private readonly Queue<int> values;

            public StubRandomSource(
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
                return values.Dequeue();
            }
        }
    }
}