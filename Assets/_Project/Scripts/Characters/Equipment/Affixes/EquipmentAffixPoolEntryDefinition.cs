using System;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class EquipmentAffixPoolEntryDefinition
    {
        [SerializeField]
        private string affixId;

        [SerializeField]
        private int weight;

        public EquipmentAffixId AffixId =>
            new EquipmentAffixId(affixId);

        public int Weight =>
            weight;

#if UNITY_EDITOR
        public EquipmentAffixPoolEntryDefinition(
            string newAffixId,
            int newWeight)
        {
            affixId = newAffixId;
            weight = newWeight;
        }
#endif
    }
}