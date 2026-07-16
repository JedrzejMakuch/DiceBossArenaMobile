using System;

namespace DiceBossArena.Game
{
    public readonly struct CharacterItemInstance :
        IEquatable<CharacterItemInstance>
    {
        public CharacterItemInstanceId InstanceId { get; }

        public CharacterItemId ItemId { get; }

        public int Level { get; }

        public int UpgradeLevel { get; }

        public int Quantity { get; }

        public CharacterItemInstance(
            CharacterItemInstanceId instanceId,
            CharacterItemId itemId,
            int level,
            int upgradeLevel,
            int quantity)
        {
            if (!instanceId.IsValid)
            {
                throw new ArgumentException(
                    "Item instance id must be valid.",
                    nameof(instanceId));
            }

            if (!itemId.IsValid)
            {
                throw new ArgumentException(
                    "Item definition id must be valid.",
                    nameof(itemId));
            }

            if (level < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(level),
                    level,
                    "Item level must be at least 1.");
            }

            if (upgradeLevel < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(upgradeLevel),
                    upgradeLevel,
                    "Item upgrade level cannot be negative.");
            }

            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    quantity,
                    "Item quantity must be at least 1.");
            }

            InstanceId = instanceId;
            ItemId = itemId;
            Level = level;
            UpgradeLevel = upgradeLevel;
            Quantity = quantity;
        }

        public bool Equals(
            CharacterItemInstance other)
        {
            return InstanceId.Equals(other.InstanceId) &&
                   ItemId.Equals(other.ItemId) &&
                   Level == other.Level &&
                   UpgradeLevel == other.UpgradeLevel &&
                   Quantity == other.Quantity;
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterItemInstance other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                InstanceId,
                ItemId,
                Level,
                UpgradeLevel,
                Quantity);
        }

        public override string ToString()
        {
            return $"{InstanceId}: {ItemId} " +
                   $"(level {Level}, " +
                   $"+{UpgradeLevel}, " +
                   $"quantity {Quantity})";
        }
    }
}