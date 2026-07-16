using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceBossArena.Game
{
    public sealed class CharacterEquipmentLoadout
    {
        private readonly List<
            EquippedItemInstance> items;

        private readonly ReadOnlyCollection<
            EquippedItemInstance> readOnlyItems;

        public IReadOnlyList<
            EquippedItemInstance> Items =>
                readOnlyItems;

        public int Count =>
            items.Count;

        public CharacterEquipmentLoadout(
            IReadOnlyList<
                EquippedItemInstance>
                initialItems = null)
        {
            items =
                CopyAndValidateItems(
                    initialItems);

            readOnlyItems =
                items.AsReadOnly();
        }

        public bool IsSlotOccupied(
            EquipmentSlotType slotType)
        {
            return TryGet(
                slotType,
                out _);
        }

        public bool IsInstanceEquipped(
    CharacterItemInstanceId instanceId)
        {
            if (!instanceId.IsValid)
            {
                return false;
            }

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

        public bool TryGet(
            EquipmentSlotType slotType,
            out EquippedItemInstance item)
        {
            for (int i = 0;
                 i < items.Count;
                 i++)
            {
                if (items[i].SlotType ==
                    slotType)
                {
                    item = items[i];
                    return true;
                }
            }

            item = default;
            return false;
        }

        public bool Set(
    EquippedItemInstance item,
    out EquippedItemInstance replacedItem)
        {
            if (!item.IsValid)
            {
                throw new ArgumentException(
                    "Equipped item must be valid.",
                    nameof(item));
            }

            for (int i = 0;
                 i < items.Count;
                 i++)
            {
                if (items[i].InstanceId.Equals(
                        item.InstanceId) &&
                    items[i].SlotType !=
                        item.SlotType)
                {
                    throw new InvalidOperationException(
                        $"Item instance {item.InstanceId} " +
                        $"is already equipped in slot " +
                        $"{items[i].SlotType}.");
                }
            }

            for (int i = 0;
                 i < items.Count;
                 i++)
            {
                if (items[i].SlotType ==
                    item.SlotType)
                {
                    replacedItem =
                        items[i];

                    items[i] =
                        item;

                    return true;
                }
            }

            items.Add(item);

            replacedItem =
                default;

            return false;
        }

        public bool TryRemove(
    EquipmentSlotType slotType,
    out EquippedItemInstance removedItem)
        {
            if (slotType ==
                EquipmentSlotType.None)
            {
                removedItem = default;
                return false;
            }

            for (int i = 0;
                 i < items.Count;
                 i++)
            {
                if (items[i].SlotType !=
                    slotType)
                {
                    continue;
                }

                removedItem =
                    items[i];

                items.RemoveAt(i);

                return true;
            }

            removedItem =
                default;

            return false;
        }

        private static List<
            EquippedItemInstance>
            CopyAndValidateItems(
                IReadOnlyList<
                    EquippedItemInstance> source)
        {
            List<EquippedItemInstance> result =
                new();

            if (source == null)
            {
                return result;
            }

            HashSet<EquipmentSlotType>
                occupiedSlots = new();

            HashSet<CharacterItemInstanceId>
                equippedInstanceIds = new();

            for (int i = 0;
                 i < source.Count;
                 i++)
            {
                EquippedItemInstance item =
                    source[i];

                if (!item.IsValid)
                {
                    throw new ArgumentException(
                        "Loadout contains an invalid " +
                        "equipped item.",
                        nameof(source));
                }

                if (!occupiedSlots.Add(
                        item.SlotType))
                {
                    throw new ArgumentException(
                        $"Loadout contains duplicate slot: " +
                        $"{item.SlotType}.",
                        nameof(source));
                }

                if (!equippedInstanceIds.Add(
                        item.InstanceId))
                {
                    throw new ArgumentException(
                        $"Loadout contains duplicate " +
                        $"instance id: {item.InstanceId}.",
                        nameof(source));
                }

                result.Add(item);
            }

            return result;
        }
    }
}