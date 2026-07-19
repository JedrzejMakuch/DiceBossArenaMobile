using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixCollectionGenerator
    {
        private readonly EquipmentAffixFromPoolGenerator
            affixGenerator;

        private readonly EquipmentAffixPoolExclusionFilter
            exclusionFilter =
                new EquipmentAffixPoolExclusionFilter();

        private readonly EquipmentAffixPoolAvailableCountResolver
            availableCountResolver;

        public EquipmentAffixCollectionGenerator(
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

            affixGenerator =
                new EquipmentAffixFromPoolGenerator(
                    catalog,
                    randomSource);

            availableCountResolver =
                new EquipmentAffixPoolAvailableCountResolver(
                    catalog);
        }

        public IReadOnlyList<RolledEquipmentAffix> Generate(
            EquipmentAffixPoolDefinition pool,
            int itemLevel,
            int affixCount)
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

            if (affixCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(affixCount),
                    affixCount,
                    "Affix count cannot be negative.");
            }

            int availableAffixCount =
                availableCountResolver.Resolve(
                    pool,
                    itemLevel);

            if (availableAffixCount < affixCount)
            {
                throw new InvalidOperationException(
                    "The equipment affix pool does not contain enough available unique affixes.");
            }

            List<RolledEquipmentAffix> rolledAffixes =
                new List<RolledEquipmentAffix>(
                    affixCount);

            List<EquipmentAffixId> excludedAffixIds =
                new List<EquipmentAffixId>(
                    affixCount);

            for (int index = 0;
                 index < affixCount;
                 index++)
            {
                EquipmentAffixPoolDefinition remainingPool =
                    exclusionFilter.Filter(
                        pool,
                        excludedAffixIds);

                RolledEquipmentAffix rolledAffix =
                    affixGenerator.Generate(
                        remainingPool,
                        itemLevel);

                if (rolledAffix == null)
                {
                    throw new InvalidOperationException(
                        "The equipment affix pool does not contain enough available unique affixes.");
                }

                rolledAffixes.Add(
                    rolledAffix);

                excludedAffixIds.Add(
                    rolledAffix.AffixId);
            }

            return rolledAffixes;
        }
    }
}