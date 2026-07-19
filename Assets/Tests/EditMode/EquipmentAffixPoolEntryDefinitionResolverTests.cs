using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        EquipmentAffixPoolEntryDefinitionResolverTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () =>
                    new EquipmentAffixPoolEntryDefinitionResolver(
                        null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Resolve_NullEntry_Throws()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    Array.Empty<
                        EquipmentAffixDefinition>());

            EquipmentAffixPoolEntryDefinitionResolver resolver =
                new EquipmentAffixPoolEntryDefinitionResolver(
                    catalog);

            Assert.That(
                () => resolver.Resolve(null),
                Throws.TypeOf<
                    ArgumentNullException>());
        }

        [Test]
        public void Resolve_KnownAffixId_ReturnsDefinition()
        {
            EquipmentAffixDefinition strengthDefinition =
                CreateDefinition(
                    "strength_flat",
                    FightStatType.Strength);

            EquipmentAffixDefinition vitalityDefinition =
                CreateDefinition(
                    "vitality_flat",
                    FightStatType.MaxHealth);

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        strengthDefinition,
                        vitalityDefinition
                    });

            EquipmentAffixPoolEntryDefinitionResolver resolver =
                new EquipmentAffixPoolEntryDefinitionResolver(
                    catalog);

            EquipmentAffixPoolEntryDefinition entry =
                new EquipmentAffixPoolEntryDefinition(
                    "vitality_flat",
                    5);

            EquipmentAffixDefinition result =
                resolver.Resolve(entry);

            Assert.That(
                result,
                Is.SameAs(
                    vitalityDefinition));
        }

        [Test]
        public void Resolve_UnknownAffixId_Throws()
        {
            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        CreateDefinition(
                            "strength_flat",
                            FightStatType.Strength)
                    });

            EquipmentAffixPoolEntryDefinitionResolver resolver =
                new EquipmentAffixPoolEntryDefinitionResolver(
                    catalog);

            EquipmentAffixPoolEntryDefinition entry =
                new EquipmentAffixPoolEntryDefinition(
                    "vitality_flat",
                    5);

            Assert.That(
                () => resolver.Resolve(entry),
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