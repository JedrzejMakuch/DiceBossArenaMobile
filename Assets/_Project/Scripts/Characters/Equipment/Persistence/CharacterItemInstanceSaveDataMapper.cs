using System;

namespace DiceBossArena.Game
{
    public sealed class CharacterItemInstanceSaveDataMapper
    {
        private readonly RolledEquipmentAffixSaveDataMapper
            affixMapper;

        public CharacterItemInstanceSaveDataMapper(
            RolledEquipmentAffixSaveDataMapper
                newAffixMapper)
        {
            affixMapper =
                newAffixMapper ??
                throw new ArgumentNullException(
                    nameof(newAffixMapper));
        }

        public CharacterItemInstanceSaveData ToSaveData(
            CharacterItemInstance item)
        {
            if (!item.IsValid)
            {
                throw new ArgumentException(
                    "Item instance must be valid.",
                    nameof(item));
            }

            RolledEquipmentAffixSaveData[] affixes =
                new RolledEquipmentAffixSaveData[
                    item.Affixes.Count];

            for (int index = 0;
                 index < item.Affixes.Count;
                 index++)
            {
                affixes[index] =
                    affixMapper.ToSaveData(
                        item.Affixes[index]);
            }

            return new CharacterItemInstanceSaveData(
                item.InstanceId.Value,
                item.ItemId.Value,
                item.BaseTypeId.Value,
                item.Level,
                item.UpgradeLevel,
                item.Quantity,
                item.Rarity,
                affixes);
        }

        public CharacterItemInstance FromSaveData(
            CharacterItemInstanceSaveData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(
                    nameof(data));
            }

            if (data.Affixes == null)
            {
                throw new ArgumentException(
                    "Saved item affixes cannot be null.",
                    nameof(data));
            }

            RolledEquipmentAffix[] affixes =
                new RolledEquipmentAffix[
                    data.Affixes.Length];

            for (int index = 0;
                 index < data.Affixes.Length;
                 index++)
            {
                affixes[index] =
                    affixMapper.FromSaveData(
                        data.Affixes[index]);
            }

            return new CharacterItemInstance(
                new CharacterItemInstanceId(
                    data.InstanceId),
                new CharacterItemId(
                    data.ItemId),
                new EquipmentBaseTypeId(
                    data.BaseTypeId),
                data.Level,
                data.UpgradeLevel,
                data.Quantity,
                data.Rarity,
                affixes);
        }
    }
}