using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class EquipmentAffixPoolDefinition
    {
        [SerializeField]
        private List<EquipmentAffixPoolEntryDefinition> entries =
            new List<EquipmentAffixPoolEntryDefinition>();

        public IReadOnlyList<EquipmentAffixPoolEntryDefinition> Entries =>
            entries;

#if UNITY_EDITOR
        public EquipmentAffixPoolDefinition(
            IEnumerable<EquipmentAffixPoolEntryDefinition> newEntries = null)
        {
            entries =
                newEntries == null
                    ? new List<EquipmentAffixPoolEntryDefinition>()
                    : new List<EquipmentAffixPoolEntryDefinition>(
                        newEntries);
        }
#endif
    }
}