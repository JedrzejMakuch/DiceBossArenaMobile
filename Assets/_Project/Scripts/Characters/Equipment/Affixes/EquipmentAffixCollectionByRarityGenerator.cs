using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixCollectionByRarityGenerator
    {
        private readonly EquipmentRarityAffixCountResolver
            affixCountResolver =
                new EquipmentRarityAffixCountResolver();

        private readonly EquipmentAffixCollectionGenerator
            collectionGenerator;

        private readonly EquipmentAffixPoolRarityCapacityValidator
            capacityValidator;

        public EquipmentAffixCollectionByRarityGenerator(
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

            collectionGenerator =
                new EquipmentAffixCollectionGenerator(
                    catalog,
                    randomSource);

            capacityValidator =
                new EquipmentAffixPoolRarityCapacityValidator(
                    catalog);
        }

        public IReadOnlyList<RolledEquipmentAffix> Generate(
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

            capacityValidator.Validate(
                pool,
                itemLevel,
                rarity);

            int affixCount =
                affixCountResolver.Resolve(
                    rarity);

            return collectionGenerator.Generate(
                pool,
                itemLevel,
                affixCount);
        }
    }
}