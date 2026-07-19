using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixPoolRarityCapacityValidator
    {
        private readonly EquipmentAffixPoolAvailableCountResolver
            availableCountResolver;

        private readonly EquipmentRarityAffixCountResolver
            affixCountResolver =
                new EquipmentRarityAffixCountResolver();

        public EquipmentAffixPoolRarityCapacityValidator(
            EquipmentAffixDefinitionCatalog catalog)
        {
            availableCountResolver =
                new EquipmentAffixPoolAvailableCountResolver(
                    catalog ??
                    throw new ArgumentNullException(
                        nameof(catalog)));
        }

        public void Validate(
            EquipmentAffixPoolDefinition pool,
            int itemLevel,
            EquipmentItemRarity rarity)
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

            int requiredAffixCount =
                affixCountResolver.Resolve(
                    rarity);

            int availableAffixCount =
                availableCountResolver.Resolve(
                    pool,
                    itemLevel);

            if (availableAffixCount <
                requiredAffixCount)
            {
                throw new InvalidOperationException(
                    $"Equipment affix pool contains only {availableAffixCount} available affixes, but rarity '{rarity}' requires {requiredAffixCount}.");
            }
        }
    }
}