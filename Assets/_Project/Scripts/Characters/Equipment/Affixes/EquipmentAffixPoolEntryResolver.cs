using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixPoolEntryResolver
    {
        private readonly EquipmentAffixPoolTotalWeightResolver
            totalWeightResolver =
                new EquipmentAffixPoolTotalWeightResolver();

        public EquipmentAffixPoolEntryDefinition Resolve(
            EquipmentAffixPoolDefinition pool,
            int roll)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(
                    nameof(pool));
            }

            int totalWeight =
                totalWeightResolver.Resolve(pool);

            if (roll < 0 ||
                roll >= totalWeight)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(roll),
                    roll,
                    "Roll must be within the pool weight range.");
            }

            int accumulatedWeight = 0;

            for (int index = 0;
                 index < pool.Entries.Count;
                 index++)
            {
                EquipmentAffixPoolEntryDefinition entry =
                    pool.Entries[index];

                checked
                {
                    accumulatedWeight +=
                        entry.Weight;
                }

                if (roll < accumulatedWeight)
                {
                    return entry;
                }
            }

            throw new InvalidOperationException(
                "Could not resolve an equipment affix pool entry.");
        }
    }
}