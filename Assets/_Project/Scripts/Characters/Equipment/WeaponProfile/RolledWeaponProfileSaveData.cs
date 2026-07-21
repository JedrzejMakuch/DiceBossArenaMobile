using System;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class RolledWeaponProfileSaveData
    {
        public RolledWeaponAttackLineSaveData[] Lines;

        public RolledWeaponProfileSaveData()
        {
        }

        public RolledWeaponProfileSaveData(
            RolledWeaponAttackLineSaveData[] newLines)
        {
            Lines = newLines;
        }
    }
}