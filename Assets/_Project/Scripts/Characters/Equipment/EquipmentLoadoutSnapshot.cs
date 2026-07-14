using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceBossArena.Game
{
    public sealed class EquipmentLoadoutSnapshot :
        IEquatable<EquipmentLoadoutSnapshot>
    {
        private readonly ReadOnlyCollection<
            EquippedItemSnapshot> items;

        public IReadOnlyList<EquippedItemSnapshot> Items =>
            items;

        public EquipmentLoadoutSnapshot(
            IReadOnlyList<EquippedItemSnapshot> items)
        {
            this.items =
                CopyItems(items).AsReadOnly();
        }

        private static List<EquippedItemSnapshot> CopyItems(
            IReadOnlyList<EquippedItemSnapshot> source)
        {
            List<EquippedItemSnapshot> result =
                new();

            if (source == null)
            {
                return result;
            }

            HashSet<EquipmentSlotType> occupiedSlots =
                new();

            for (int i = 0; i < source.Count; i++)
            {
                EquippedItemSnapshot item =
                    source[i];

                if (!item.ItemId.IsValid)
                {
                    throw new ArgumentException(
                        "Loadout contains an invalid item id.",
                        nameof(source));
                }

                if (!occupiedSlots.Add(item.SlotType))
                {
                    throw new ArgumentException(
                        $"Loadout contains duplicate slot: " +
                        $"{item.SlotType}.",
                        nameof(source));
                }

                result.Add(item);
            }

            return result;
        }

        public bool Equals(
            EquipmentLoadoutSnapshot other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (items.Count != other.items.Count)
            {
                return false;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].Equals(other.items[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(
            object obj)
        {
            return obj is EquipmentLoadoutSnapshot other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            HashCode hashCode =
                new();

            for (int i = 0; i < items.Count; i++)
            {
                hashCode.Add(items[i]);
            }

            return hashCode.ToHashCode();
        }
    }
}