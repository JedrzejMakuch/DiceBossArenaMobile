using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixFromPoolGenerator
    {
        private readonly EquipmentAffixPoolAvailabilityFilter
            availabilityFilter;

        private readonly EquipmentAffixPoolRoller
            poolRoller;

        private readonly
            EquipmentAffixPoolEntryDefinitionResolver
                definitionResolver;

        private readonly EquipmentAffixGenerator
            affixGenerator;

        public EquipmentAffixFromPoolGenerator(
            EquipmentAffixDefinitionCatalog catalog,
            IEquipmentAffixRandomSource randomSource)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(
                    nameof(catalog));
            }

            if (randomSource == null)
            {
                throw new ArgumentNullException(
                    nameof(randomSource));
            }

            availabilityFilter =
                new EquipmentAffixPoolAvailabilityFilter(
                    catalog);

            poolRoller =
                new EquipmentAffixPoolRoller(
                    randomSource);

            definitionResolver =
                new EquipmentAffixPoolEntryDefinitionResolver(
                    catalog);

            affixGenerator =
                new EquipmentAffixGenerator(
                    randomSource);
        }

        public RolledEquipmentAffix Generate(
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

            if (availablePool.Entries.Count == 0)
            {
                return null;
            }

            EquipmentAffixPoolEntryDefinition entry =
                poolRoller.Roll(
                    availablePool);

            EquipmentAffixDefinition definition =
                definitionResolver.Resolve(
                    entry);

            return affixGenerator.Generate(
                definition,
                itemLevel);
        }
    }
}