using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixPoolAvailabilityFilter
    {
        private readonly
            EquipmentAffixPoolEntryAvailabilityResolver
                availabilityResolver;

        public EquipmentAffixPoolAvailabilityFilter(
            EquipmentAffixDefinitionCatalog catalog)
        {
            availabilityResolver =
                new EquipmentAffixPoolEntryAvailabilityResolver(
                    catalog ??
                    throw new ArgumentNullException(
                        nameof(catalog)));
        }

        public EquipmentAffixPoolDefinition Filter(
            EquipmentAffixPoolDefinition pool,
            int itemLevel)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(
                    nameof(pool));
            }

            if (itemLevel < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(itemLevel),
                    itemLevel,
                    "Item level must be at least 1.");
            }

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

                if (availabilityResolver.IsAvailable(
                        entry,
                        itemLevel))
                {
                    availableEntries.Add(entry);
                }
            }

            return new EquipmentAffixPoolDefinition(
                availableEntries);
        }
    }
}