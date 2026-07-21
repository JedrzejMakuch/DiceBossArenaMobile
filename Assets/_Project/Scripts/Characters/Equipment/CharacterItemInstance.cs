using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public readonly struct CharacterItemInstance :
        IEquatable<CharacterItemInstance>
    {
        private readonly RolledEquipmentAffix[] affixes;

        public CharacterItemInstanceId InstanceId { get; }

        public CharacterItemId ItemId { get; }

        public EquipmentBaseTypeId BaseTypeId { get; }

        public EquipmentItemRarity Rarity { get; }

        public RolledWeaponProfile WeaponProfile { get; }

        public IReadOnlyList<RolledEquipmentAffix> Affixes =>
            affixes ??
            Array.Empty<RolledEquipmentAffix>();

        public int Level { get; }

        public int UpgradeLevel { get; }

        public int Quantity { get; }

        public bool IsValid =>
            InstanceId.IsValid &&
            ItemId.IsValid &&
            Level >= 1 &&
            UpgradeLevel >= 0 &&
            Quantity >= 1 &&
            Enum.IsDefined(
                typeof(EquipmentItemRarity),
                Rarity) &&
            affixes != null;

        public CharacterItemInstance(
            CharacterItemInstanceId instanceId,
            CharacterItemId itemId,
            int level,
            int upgradeLevel,
            int quantity)
            : this(
                instanceId,
                itemId,
                default,
                level,
                upgradeLevel,
                quantity,
                EquipmentItemRarity.Common,
                Array.Empty<RolledEquipmentAffix>(),
                null)
        {
        }

        public CharacterItemInstance(
            CharacterItemInstanceId instanceId,
            CharacterItemId itemId,
            int level,
            int upgradeLevel,
            int quantity,
            EquipmentItemRarity rarity)
            : this(
                instanceId,
                itemId,
                default,
                level,
                upgradeLevel,
                quantity,
                rarity,
                Array.Empty<RolledEquipmentAffix>(),
                null)
        {
        }

        public CharacterItemInstance(
            CharacterItemInstanceId instanceId,
            CharacterItemId itemId,
            int level,
            int upgradeLevel,
            int quantity,
            EquipmentItemRarity rarity,
            IEnumerable<RolledEquipmentAffix> newAffixes)
            : this(
                instanceId,
                itemId,
                default,
                level,
                upgradeLevel,
                quantity,
                rarity,
                newAffixes,
                null)
        {
        }

        public CharacterItemInstance(
            CharacterItemInstanceId instanceId,
            CharacterItemId itemId,
            EquipmentBaseTypeId baseTypeId,
            int level,
            int upgradeLevel,
            int quantity,
            EquipmentItemRarity rarity,
            IEnumerable<RolledEquipmentAffix> newAffixes)
            : this(
                instanceId,
                itemId,
                baseTypeId,
                level,
                upgradeLevel,
                quantity,
                rarity,
                newAffixes,
                null)
        {
        }

        public CharacterItemInstance(
            CharacterItemInstanceId instanceId,
            CharacterItemId itemId,
            EquipmentBaseTypeId baseTypeId,
            int level,
            int upgradeLevel,
            int quantity,
            EquipmentItemRarity rarity,
            IEnumerable<RolledEquipmentAffix> newAffixes,
            RolledWeaponProfile newWeaponProfile)
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

            if (!Enum.IsDefined(
                    typeof(EquipmentItemRarity),
                    rarity))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rarity),
                    rarity,
                    "Unsupported equipment item rarity.");
            }

            if (newAffixes == null)
            {
                throw new ArgumentNullException(
                    nameof(newAffixes));
            }

            List<RolledEquipmentAffix> copiedAffixes =
                new List<RolledEquipmentAffix>();

            foreach (RolledEquipmentAffix affix
                     in newAffixes)
            {
                if (affix == null)
                {
                    throw new ArgumentException(
                        "Item affix collection cannot contain null.",
                        nameof(newAffixes));
                }

                copiedAffixes.Add(affix);
            }

            InstanceId = instanceId;
            ItemId = itemId;
            BaseTypeId = baseTypeId;
            Level = level;
            UpgradeLevel = upgradeLevel;
            Quantity = quantity;
            Rarity = rarity;
            affixes = copiedAffixes.ToArray();
            WeaponProfile = newWeaponProfile;
        }

        public bool CanStackWith(
            CharacterItemInstance other)
        {
            return IsValid &&
                   other.IsValid &&
                   ItemId.Equals(other.ItemId) &&
                   BaseTypeId.Equals(other.BaseTypeId) &&
                   Level == other.Level &&
                   UpgradeLevel == other.UpgradeLevel &&
                   Rarity == other.Rarity &&
                   HaveEqualAffixes(other) &&
                   Equals(
                       WeaponProfile,
                       other.WeaponProfile);
        }

        public CharacterItemInstance WithQuantity(
            int newQuantity)
        {
            return new CharacterItemInstance(
                InstanceId,
                ItemId,
                BaseTypeId,
                Level,
                UpgradeLevel,
                newQuantity,
                Rarity,
                Affixes,
                WeaponProfile);
        }

        public bool Equals(
            CharacterItemInstance other)
        {
            return InstanceId.Equals(other.InstanceId) &&
                   ItemId.Equals(other.ItemId) &&
                   BaseTypeId.Equals(other.BaseTypeId) &&
                   Level == other.Level &&
                   UpgradeLevel == other.UpgradeLevel &&
                   Quantity == other.Quantity &&
                   Rarity == other.Rarity &&
                   HaveEqualAffixes(other) &&
                   Equals(
                       WeaponProfile,
                       other.WeaponProfile);
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterItemInstance other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            HashCode hashCode =
                new HashCode();

            hashCode.Add(InstanceId);
            hashCode.Add(ItemId);
            hashCode.Add(BaseTypeId);
            hashCode.Add(Level);
            hashCode.Add(UpgradeLevel);
            hashCode.Add(Quantity);
            hashCode.Add(Rarity);
            hashCode.Add(WeaponProfile);

            for (int index = 0;
                 index < Affixes.Count;
                 index++)
            {
                RolledEquipmentAffix affix =
                    Affixes[index];

                hashCode.Add(affix.AffixId);
                hashCode.Add(affix.StatType);
                hashCode.Add(affix.ModifierType);
                hashCode.Add(affix.Value);
            }

            return hashCode.ToHashCode();
        }

        public override string ToString()
        {
            return $"{InstanceId}: {ItemId} " +
                   $"[{BaseTypeId}] " +
                   $"({Rarity}, " +
                   $"level {Level}, " +
                   $"+{UpgradeLevel}, " +
                   $"quantity {Quantity}, " +
                   $"affixes {Affixes.Count})";
        }

        private bool HaveEqualAffixes(
            CharacterItemInstance other)
        {
            if (Affixes.Count !=
                other.Affixes.Count)
            {
                return false;
            }

            for (int index = 0;
                 index < Affixes.Count;
                 index++)
            {
                RolledEquipmentAffix first =
                    Affixes[index];

                RolledEquipmentAffix second =
                    other.Affixes[index];

                if (!first.AffixId.Equals(
                        second.AffixId) ||
                    first.StatType !=
                        second.StatType ||
                    first.ModifierType !=
                        second.ModifierType ||
                    first.Value !=
                        second.Value)
                {
                    return false;
                }
            }

            return true;
        }
    }
}