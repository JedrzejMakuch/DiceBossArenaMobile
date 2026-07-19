using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixPoolExclusionFilter
    {
        public EquipmentAffixPoolDefinition Filter(
            EquipmentAffixPoolDefinition pool,
            IEnumerable<EquipmentAffixId> excludedAffixIds)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(
                    nameof(pool));
            }

            if (excludedAffixIds == null)
            {
                throw new ArgumentNullException(
                    nameof(excludedAffixIds));
            }

            HashSet<EquipmentAffixId> excludedIds =
                new HashSet<EquipmentAffixId>(
                    excludedAffixIds);

            List<EquipmentAffixPoolEntryDefinition>
                availableEntries =
                    new List<
                        EquipmentAffixPoolEntryDefinition>();

            for (int index = 0;
                 index < pool.Entries.Count;
                 index++)
            {
                EquipmentAffixPoolEntryDefinition entry =
                    pool.Entries[index];

                if (!excludedIds.Contains(
                        entry.AffixId))
                {
                    availableEntries.Add(entry);
                }
            }

            return new EquipmentAffixPoolDefinition(
                availableEntries);
        }
    }
}