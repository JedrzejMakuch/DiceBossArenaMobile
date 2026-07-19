using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixDefinitionCatalogTests
    {
        [Test]
        public void Constructor_NullDefinitions_Throws()
        {
            Assert.That(
                () => new EquipmentAffixDefinitionCatalog(
                    null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Constructor_NullDefinition_Throws()
        {
            Assert.That(
                () => new EquipmentAffixDefinitionCatalog(
                    new EquipmentAffixDefinition[]
                    {
                        null
                    }),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Definitions_ReturnsCatalogDefinitions()
        {
            EquipmentAffixDefinition strength =
                CreateDefinition(
                    "strength_flat",
                    FightStatType.Strength);

            EquipmentAffixDefinition vitality =
                CreateDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth);

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                strength,
                vitality
                    });

            Assert.That(
                catalog.Definitions,
                Has.Count.EqualTo(2));

            Assert.That(
                catalog.Definitions,
                Does.Contain(strength));

            Assert.That(
                catalog.Definitions,
                Does.Contain(vitality));
        }

        [Test]
        public void Constructor_DuplicateIds_Throws()
        {
            EquipmentAffixDefinition first =
                CreateDefinition(
                    "strength_flat",
                    FightStatType.Strength);

            EquipmentAffixDefinition second =
                CreateDefinition(
                    "strength_flat",
                    FightStatType.MaxHealth);

            Assert.That(
                () => new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        first,
                        second
                    }),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Get_KnownId_ReturnsDefinition()
        {
            EquipmentAffixDefinition strength =
                CreateDefinition(
                    "strength_flat",
                    FightStatType.Strength);

            EquipmentAffixDefinition vitality =
                CreateDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth);

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        strength,
                        vitality
                    });

            EquipmentAffixDefinition result =
                catalog.Get(
                    new EquipmentAffixId(
                        "vitality_flat"));

            Assert.That(
                result,
                Is.SameAs(vitality));
        }

        [Test]
        public void Get_InvalidId_Throws()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    Array.Empty<
                        EquipmentAffixDefinition>());

            Assert.That(
                () => catalog.Get(default),
                Throws.TypeOf<
                    ArgumentException>());
        }

        [Test]
        public void Get_UnknownId_Throws()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        CreateDefinition(
                            "strength_flat",
                            FightStatType.Strength)
                    });

            Assert.That(
                () => catalog.Get(
                    new EquipmentAffixId(
                        "vitality_flat")),
                Throws.TypeOf<
                    KeyNotFoundException>());
        }

        private static EquipmentAffixDefinition
            CreateDefinition(
                string id,
                FightStatType statType)
        {
            return new EquipmentAffixDefinition(
                id,
                statType,
                FightStatModifierType.Flat,
                new[]
                {
                    new EquipmentAffixTierDefinition(
                        1,
                        1,
                        3)
                });
        }
    }
}