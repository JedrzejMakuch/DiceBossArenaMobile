using System;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class RolledEquipmentAffixSaveData
    {
        public string AffixId;
        public int Value;

        public RolledEquipmentAffixSaveData()
        {
        }

        public RolledEquipmentAffixSaveData(
            string newAffixId,
            int newValue)
        {
            AffixId = newAffixId;
            Value = newValue;
        }
    }
}