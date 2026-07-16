using System;

namespace DiceBossArena.Game
{
    public readonly struct EquippedItemInstance :
        IEquatable<EquippedItemInstance>
    {
        public EquipmentSlotType SlotType { get; }

        public CharacterItemInstanceId
            InstanceId
        { get; }

        public bool IsValid =>
            SlotType != EquipmentSlotType.None &&
            InstanceId.IsValid;

        public EquippedItemInstance(
            EquipmentSlotType slotType,
            CharacterItemInstanceId instanceId)
        {
            if (slotType ==
                EquipmentSlotType.None)
            {
                throw new ArgumentException(
                    "Equipped item must use a valid slot.",
                    nameof(slotType));
            }

            if (!instanceId.IsValid)
            {
                throw new ArgumentException(
                    "Equipped item instance id " +
                    "must be valid.",
                    nameof(instanceId));
            }

            SlotType = slotType;
            InstanceId = instanceId;
        }

        public bool Equals(
            EquippedItemInstance other)
        {
            return SlotType == other.SlotType &&
                   InstanceId.Equals(
                       other.InstanceId);
        }

        public override bool Equals(
            object obj)
        {
            return obj is EquippedItemInstance other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                SlotType,
                InstanceId);
        }

        public override string ToString()
        {
            return $"{SlotType}: {InstanceId}";
        }
    }
}