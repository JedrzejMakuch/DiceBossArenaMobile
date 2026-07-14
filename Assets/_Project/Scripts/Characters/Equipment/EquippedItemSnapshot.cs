using System;

namespace DiceBossArena.Game
{
    public readonly struct EquippedItemSnapshot :
        IEquatable<EquippedItemSnapshot>
    {
        public EquipmentSlotType SlotType { get; }

        public CharacterItemId ItemId { get; }

        public EquippedItemSnapshot(
            EquipmentSlotType slotType,
            CharacterItemId itemId)
        {
            SlotType = slotType;
            ItemId = itemId;
        }

        public bool Equals(
            EquippedItemSnapshot other)
        {
            return SlotType == other.SlotType &&
                   ItemId.Equals(other.ItemId);
        }

        public override bool Equals(
            object obj)
        {
            return obj is EquippedItemSnapshot other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                SlotType,
                ItemId);
        }
    }
}