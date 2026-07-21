using System;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class CharacterItemInstanceSaveData
    {
        public string InstanceId;
        public string ItemId;
        public string BaseTypeId;
        public int Level;
        public int UpgradeLevel;
        public int Quantity;
        public EquipmentItemRarity Rarity;

        public RolledEquipmentAffixSaveData[] Affixes =
            Array.Empty<RolledEquipmentAffixSaveData>();

        public CharacterItemInstanceSaveData()
        {
        }

        public CharacterItemInstanceSaveData(
            string newInstanceId,
            string newItemId,
            string newBaseTypeId,
            int newLevel,
            int newUpgradeLevel,
            int newQuantity,
            EquipmentItemRarity newRarity,
            RolledEquipmentAffixSaveData[] newAffixes)
        {
            InstanceId = newInstanceId;
            ItemId = newItemId;
            BaseTypeId = newBaseTypeId;
            Level = newLevel;
            UpgradeLevel = newUpgradeLevel;
            Quantity = newQuantity;
            Rarity = newRarity;

            Affixes =
                newAffixes ??
                Array.Empty<RolledEquipmentAffixSaveData>();
        }
    }
}