using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixPoolTotalWeightResolver
    {
        public int Resolve(
            EquipmentAffixPoolDefinition pool)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(
                    nameof(pool));
            }

            int totalWeight = 0;

            for (int index = 0;
                 index < pool.Entries.Count;
                 index++)
            {
                EquipmentAffixPoolEntryDefinition entry =
                    pool.Entries[index];

                checked
                {
                    totalWeight += entry.Weight;
                }
            }

            return totalWeight;
        }
    }
}