using System;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class RolledWeaponAttackLineSaveData
    {
        public string LineId;
        public WeaponAttackElement Element;
        public int MinDamage;
        public int MaxDamage;

        public RolledWeaponAttackLineSaveData()
        {
        }

        public RolledWeaponAttackLineSaveData(
            string newLineId,
            WeaponAttackElement newElement,
            int newMinDamage,
            int newMaxDamage)
        {
            LineId = newLineId;
            Element = newElement;
            MinDamage = newMinDamage;
            MaxDamage = newMaxDamage;
        }
    }
}