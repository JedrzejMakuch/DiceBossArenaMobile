using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixPoolEntryAvailabilityResolver
    {
        private readonly
            EquipmentAffixPoolEntryDefinitionResolver
                definitionResolver;

        private readonly EquipmentAffixTierResolver
            tierResolver =
                new EquipmentAffixTierResolver();

        public EquipmentAffixPoolEntryAvailabilityResolver(
            EquipmentAffixDefinitionCatalog catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(
                    nameof(catalog));
            }

            definitionResolver =
                new EquipmentAffixPoolEntryDefinitionResolver(
                    catalog);
        }

        public bool IsAvailable(
            EquipmentAffixPoolEntryDefinition entry,
            int itemLevel)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(
                    nameof(entry));
            }

            if (itemLevel < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(itemLevel),
                    itemLevel,
                    "Item level must be at least 1.");
            }

            EquipmentAffixDefinition definition =
                definitionResolver.Resolve(entry);

            EquipmentAffixTierDefinition tier =
                tierResolver.Resolve(
                    definition,
                    itemLevel);

            return tier != null;
        }
    }
}