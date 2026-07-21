using System;

namespace DiceBossArena.Game
{
    public sealed class RolledEquipmentAffixSaveDataMapper
    {
        private readonly EquipmentAffixDefinitionCatalog
            affixCatalog;

        public RolledEquipmentAffixSaveDataMapper(
            EquipmentAffixDefinitionCatalog
                newAffixCatalog)
        {
            affixCatalog =
                newAffixCatalog ??
                throw new ArgumentNullException(
                    nameof(newAffixCatalog));
        }

        public RolledEquipmentAffixSaveData ToSaveData(
            RolledEquipmentAffix affix)
        {
            if (affix == null)
            {
                throw new ArgumentNullException(
                    nameof(affix));
            }

            return new RolledEquipmentAffixSaveData(
                affix.AffixId.Value,
                affix.Value);
        }

        public RolledEquipmentAffix FromSaveData(
            RolledEquipmentAffixSaveData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(
                    nameof(data));
            }

            EquipmentAffixId affixId =
                new EquipmentAffixId(
                    data.AffixId);

            EquipmentAffixDefinition definition =
                affixCatalog.Get(
                    affixId);

            return new RolledEquipmentAffix(
                affixId,
                definition.StatType,
                definition.ModifierType,
                data.Value);
        }
    }
}