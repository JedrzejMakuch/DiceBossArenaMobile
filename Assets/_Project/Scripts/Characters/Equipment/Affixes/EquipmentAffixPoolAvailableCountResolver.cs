using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixPoolAvailableCountResolver
    {
        private readonly EquipmentAffixPoolAvailabilityFilter
            availabilityFilter;

        public EquipmentAffixPoolAvailableCountResolver(
            EquipmentAffixDefinitionCatalog catalog)
        {
            availabilityFilter =
                new EquipmentAffixPoolAvailabilityFilter(
                    catalog ??
                    throw new ArgumentNullException(
                        nameof(catalog)));
        }

        public int Resolve(
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

            EquipmentAffixPoolDefinition availablePool =
                availabilityFilter.Filter(
                    pool,
                    itemLevel);

            return availablePool.Entries.Count;
        }
    }
}