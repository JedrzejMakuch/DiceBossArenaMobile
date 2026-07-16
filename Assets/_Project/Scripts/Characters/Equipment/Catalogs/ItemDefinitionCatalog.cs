using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class ItemDefinitionCatalog :
        IItemDefinitionResolver
    {
        private readonly Dictionary<
            CharacterItemId,
            ItemDefinition> definitionsById;

        public ItemDefinitionCatalog(
            IReadOnlyList<ItemDefinition> definitions)
        {
            definitionsById =
                new Dictionary<
                    CharacterItemId,
                    ItemDefinition>();

            if (definitions == null)
            {
                return;
            }

            for (int i = 0;
                 i < definitions.Count;
                 i++)
            {
                ItemDefinition definition =
                    definitions[i];

                if (definition == null)
                {
                    throw new ArgumentException(
                        "Catalog contains a null item definition.",
                        nameof(definitions));
                }

                CharacterItemId itemId =
                    definition.ItemId;

                if (!itemId.IsValid)
                {
                    throw new ArgumentException(
                        "Catalog contains an item with an invalid id.",
                        nameof(definitions));
                }

                if (!definitionsById.TryAdd(
                        itemId,
                        definition))
                {
                    throw new ArgumentException(
                        $"Catalog contains duplicate item id: " +
                        $"{itemId}.",
                        nameof(definitions));
                }
            }
        }

        public bool TryResolve(
            CharacterItemId itemId,
            out ItemDefinition definition)
        {
            definition = null;

            if (!itemId.IsValid)
            {
                return false;
            }

            return definitionsById.TryGetValue(
                itemId,
                out definition);
        }
    }
}