using System;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class EquipmentAffixTierDefinition
    {
        [SerializeField]
        private int minimumItemLevel;

        [SerializeField]
        private int minimumValue;

        [SerializeField]
        private int maximumValue;

        public int MinimumItemLevel =>
            minimumItemLevel;

        public int MinimumValue =>
            minimumValue;

        public int MaximumValue =>
            maximumValue;

#if UNITY_EDITOR
        public EquipmentAffixTierDefinition(
            int newMinimumItemLevel,
            int newMinimumValue,
            int newMaximumValue)
        {
            minimumItemLevel =
                newMinimumItemLevel;

            minimumValue =
                newMinimumValue;

            maximumValue =
                newMaximumValue;
        }
#endif
    }
}