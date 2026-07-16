using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceBossArena.Game
{
    public sealed class CharacterInventory
    {
        private readonly List<
            CharacterItemInstance> items;

        private readonly ReadOnlyCollection<
            CharacterItemInstance> readOnlyItems;

        private readonly IItemDefinitionResolver
    definitionResolver;

        public int Capacity { get; }

        public IReadOnlyList<
            CharacterItemInstance> Items =>
                readOnlyItems;

        public int Count =>
            items.Count;

        public bool IsFull =>
            items.Count >= Capacity;

        public CharacterInventory(
    int capacity,
    IItemDefinitionResolver definitionResolver,
    IReadOnlyList<
        CharacterItemInstance>
        initialItems = null)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(capacity),
                    capacity,
                    "Inventory capacity must be at least 1.");
            }

            this.definitionResolver =
                definitionResolver ??
                throw new ArgumentNullException(
                    nameof(definitionResolver));

            Capacity = capacity;

            items =
                CopyAndValidateItems(
                    initialItems,
                    capacity);

            readOnlyItems =
                items.AsReadOnly();
        }

        public InventoryAddResult TryAdd(
    CharacterItemInstance item)
        {
            if (!item.IsValid)
            {
                return InventoryAddResult.InvalidItem;
            }

            if (ContainsInstanceId(
                    item.InstanceId))
            {
                return InventoryAddResult
                    .DuplicateInstanceId;
            }

            if (!definitionResolver.TryResolve(
                    item.ItemId,
                    out ItemDefinition definition))
            {
                return InventoryAddResult
                    .UnknownDefinition;
            }

            CharacterItemInstanceValidator validator =
                new CharacterItemInstanceValidator(
                    definitionResolver);

            try
            {
                validator.ValidateAndResolve(item);
            }
            catch (InvalidOperationException)
            {
                return InventoryAddResult.InvalidItem;
            }

            for (int i = 0;
                 i < items.Count;
                 i++)
            {
                CharacterItemInstance existing =
                    items[i];

                if (!existing.CanStackWith(item))
                {
                    continue;
                }

                int combinedQuantity =
                    existing.Quantity +
                    item.Quantity;

                if (combinedQuantity >
                    definition.MaxStackSize)
                {
                    continue;
                }

                items[i] =
                    existing.WithQuantity(
                        combinedQuantity);

                return InventoryAddResult.Stacked;
            }

            if (IsFull)
            {
                return InventoryAddResult.InventoryFull;
            }

            items.Add(item);

            return InventoryAddResult.Added;
        }

        public InventoryRemoveResult TryRemove(
    CharacterItemInstanceId instanceId,
    int quantity)
        {
            if (!instanceId.IsValid)
            {
                return InventoryRemoveResult
                    .InvalidInstanceId;
            }

            if (quantity < 1)
            {
                return InventoryRemoveResult
                    .InvalidQuantity;
            }

            for (int i = 0;
                 i < items.Count;
                 i++)
            {
                CharacterItemInstance existing =
                    items[i];

                if (!existing.InstanceId.Equals(
                        instanceId))
                {
                    continue;
                }

                if (quantity >
                    existing.Quantity)
                {
                    return InventoryRemoveResult
                        .InvalidQuantity;
                }

                if (quantity ==
                    existing.Quantity)
                {
                    items.RemoveAt(i);

                    return InventoryRemoveResult
                        .Removed;
                }

                items[i] =
                    existing.WithQuantity(
                        existing.Quantity -
                        quantity);

                return InventoryRemoveResult
                    .QuantityReduced;
            }

            return InventoryRemoveResult
                .ItemNotFound;
        }

        private bool ContainsInstanceId(
            CharacterItemInstanceId instanceId)
        {
            for (int i = 0;
                 i < items.Count;
                 i++)
            {
                if (items[i].InstanceId.Equals(
                        instanceId))
                {
                    return true;
                }
            }

            return false;
        }

        private static List<
            CharacterItemInstance>
            CopyAndValidateItems(
                IReadOnlyList<
                    CharacterItemInstance> source,
                int capacity)
        {
            List<CharacterItemInstance> result =
                new();

            if (source == null)
            {
                return result;
            }

            if (source.Count > capacity)
            {
                throw new ArgumentException(
                    "Initial inventory exceeds capacity.",
                    nameof(source));
            }

            HashSet<CharacterItemInstanceId>
                instanceIds = new();

            for (int i = 0;
                 i < source.Count;
                 i++)
            {
                CharacterItemInstance item =
                    source[i];

                if (!item.IsValid)
                {
                    throw new ArgumentException(
                        "Initial inventory contains " +
                        "an invalid item instance.",
                        nameof(source));
                }

                if (!instanceIds.Add(
                        item.InstanceId))
                {
                    throw new ArgumentException(
                        $"Initial inventory contains " +
                        $"duplicate instance id: " +
                        $"{item.InstanceId}.",
                        nameof(source));
                }

                result.Add(item);
            }

            return result;
        }
    }
}