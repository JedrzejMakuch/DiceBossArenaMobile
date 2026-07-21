using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class RolledEquipmentAffixSaveDataMapperTests
    {
        [Test]
        public void Constructor_NullCatalog_Throws()
        {
            Assert.That(
                () => new RolledEquipmentAffixSaveDataMapper(
                    null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ToSaveData_NullAffix_Throws()
        {
            RolledEquipmentAffixSaveDataMapper mapper =
                CreateMapper();

            Assert.That(
                () => mapper.ToSaveData(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void ToSaveData_ValidAffix_StoresIdAndValue()
        {
            RolledEquipmentAffix affix =
                new RolledEquipmentAffix(
                    new EquipmentAffixId(
                        "strength_flat"),
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    7);

            RolledEquipmentAffixSaveData data =
                CreateMapper().ToSaveData(
                    affix);

            Assert.That(
                data.AffixId,
                Is.EqualTo("strength_flat"));

            Assert.That(
                data.Value,
                Is.EqualTo(7));
        }

        [Test]
        public void FromSaveData_NullData_Throws()
        {
            RolledEquipmentAffixSaveDataMapper mapper =
                CreateMapper();

            Assert.That(
                () => mapper.FromSaveData(null),
                Throws.TypeOf<ArgumentNullException>());
        }

        [Test]
        public void FromSaveData_ValidData_RestoresDefinitionData()
        {
            RolledEquipmentAffixSaveData data =
                new RolledEquipmentAffixSaveData(
                    "strength_flat",
                    7);

            RolledEquipmentAffix affix =
                CreateMapper().FromSaveData(
                    data);

            Assert.That(
                affix.AffixId,
                Is.EqualTo(
                    new EquipmentAffixId(
                        "strength_flat")));

            Assert.That(
                affix.StatType,
                Is.EqualTo(
                    FightStatType.Strength));

            Assert.That(
                affix.ModifierType,
                Is.EqualTo(
                    FightStatModifierType.Flat));

            Assert.That(
                affix.Value,
                Is.EqualTo(7));
        }

        [Test]
        public void FromSaveData_UnknownAffixId_Throws()
        {
            RolledEquipmentAffixSaveData data =
                new RolledEquipmentAffixSaveData(
                    "unknown_affix",
                    7);

            RolledEquipmentAffixSaveDataMapper mapper =
                CreateMapper();

            Assert.That(
                () => mapper.FromSaveData(data),
                Throws.TypeOf<KeyNotFoundException>());
        }

        private static RolledEquipmentAffixSaveDataMapper
            CreateMapper()
        {
            EquipmentAffixDefinition definition =
                new EquipmentAffixDefinition(
                    "strength_flat",
                    FightStatType.Strength,
                    FightStatModifierType.Flat);

            EquipmentAffixDefinitionCatalog catalog =
                new EquipmentAffixDefinitionCatalog(
                    new[]
                    {
                        definition
                    });

            return new RolledEquipmentAffixSaveDataMapper(
                catalog);
        }
    }
}