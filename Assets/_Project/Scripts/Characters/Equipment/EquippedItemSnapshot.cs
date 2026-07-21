using System;

namespace DiceBossArena.Game
{
    public readonly struct EquippedItemSnapshot :
        IEquatable<EquippedItemSnapshot>
    {
        public EquipmentSlotType SlotType { get; }

        public CharacterItemId ItemId { get; }

        public RolledWeaponProfile WeaponProfile
        {
            get;
        }

        public EquippedItemSnapshot(
            EquipmentSlotType slotType,
            CharacterItemId itemId)
            : this(
                slotType,
                itemId,
                null)
        {
        }

        public EquippedItemSnapshot(
            EquipmentSlotType slotType,
            CharacterItemId itemId,
            RolledWeaponProfile weaponProfile)
        {
            SlotType = slotType;
            ItemId = itemId;
            WeaponProfile = weaponProfile;
        }

        public bool Equals(
            EquippedItemSnapshot other)
        {
            return SlotType == other.SlotType &&
                   ItemId.Equals(other.ItemId) &&
                   Equals(
                       WeaponProfile,
                       other.WeaponProfile);
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
                ItemId,
                WeaponProfile);
        }
    }
}