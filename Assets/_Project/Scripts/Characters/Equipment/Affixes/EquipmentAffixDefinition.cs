using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class EquipmentAffixDefinition
    {
        [SerializeField]
        private string id;

        [SerializeField]
        private FightStatType statType;

        [SerializeField]
        private FightStatModifierType modifierType;

        [SerializeField]
        private List<EquipmentAffixTierDefinition> tiers =
            new List<EquipmentAffixTierDefinition>();

        public EquipmentAffixId Id =>
            new EquipmentAffixId(id);

        public FightStatType StatType =>
            statType;

        public FightStatModifierType ModifierType =>
            modifierType;

        public IReadOnlyList<EquipmentAffixTierDefinition> Tiers =>
            tiers;

#if UNITY_EDITOR
        public EquipmentAffixDefinition(
            string newId,
            FightStatType newStatType,
            FightStatModifierType newModifierType,
            IEnumerable<EquipmentAffixTierDefinition> newTiers = null)
        {
            id = newId;
            statType = newStatType;
            modifierType = newModifierType;

            tiers =
                newTiers == null
                    ? new List<EquipmentAffixTierDefinition>()
                    : new List<EquipmentAffixTierDefinition>(
                        newTiers);
        }
#endif
    }
}