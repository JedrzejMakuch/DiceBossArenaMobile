using System;

namespace DiceBossArena.Game
{
    public sealed class ItemDefinitionContentValidator
    {
        public void Validate(
            ItemDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (!definition.ItemId.IsValid)
            {
                throw new InvalidOperationException(
                    "Item definition has an invalid id.");
            }

            switch (definition.Category)
            {
                case EquipmentItemCategory.Weapon:
                    ValidateWeapon(definition);
                    break;

                case EquipmentItemCategory.Shield:
                    ValidateNonWeaponEquipment(
                        definition,
                        EquipmentSlotType.OffHand);
                    break;

                case EquipmentItemCategory.Armor:
                    ValidateNonWeaponEquipment(
                        definition,
                        EquipmentSlotType.Armor);
                    break;

                case EquipmentItemCategory.Accessory:
                    ValidateNonWeaponEquipment(
                        definition,
                        EquipmentSlotType.Accessory);
                    break;

                case EquipmentItemCategory.Consumable:
                case EquipmentItemCategory.Material:
                    ValidateInventoryItem(definition);
                    break;

                default:
                    throw new InvalidOperationException(
                        $"Item {definition.ItemId} has an " +
                        $"unsupported category: " +
                        $"{definition.Category}.");
            }
        }

        private static void ValidateWeapon(
            ItemDefinition definition)
        {
            if (definition.SlotType !=
                EquipmentSlotType.MainHand)
            {
                throw new InvalidOperationException(
                    $"Weapon {definition.ItemId} must use " +
                    $"{EquipmentSlotType.MainHand}.");
            }

            if (definition.Handedness !=
                    WeaponHandedness.OneHanded &&
                definition.Handedness !=
                    WeaponHandedness.TwoHanded)
            {
                throw new InvalidOperationException(
                    $"Weapon {definition.ItemId} must be " +
                    $"one-handed or two-handed.");
            }

            ValidateSingleEquipmentInstance(
                definition);
        }

        private static void ValidateNonWeaponEquipment(
            ItemDefinition definition,
            EquipmentSlotType requiredSlot)
        {
            if (definition.SlotType != requiredSlot)
            {
                throw new InvalidOperationException(
                    $"Item {definition.ItemId} must use " +
                    $"slot {requiredSlot}.");
            }

            if (definition.Handedness !=
                WeaponHandedness.NotApplicable)
            {
                throw new InvalidOperationException(
                    $"Non-weapon item {definition.ItemId} " +
                    $"cannot define weapon handedness.");
            }

            ValidateSingleEquipmentInstance(
                definition);
        }

        private static void ValidateInventoryItem(
            ItemDefinition definition)
        {
            if (definition.SlotType !=
                EquipmentSlotType.None)
            {
                throw new InvalidOperationException(
                    $"Inventory item {definition.ItemId} " +
                    $"cannot define an equipment slot.");
            }

            if (definition.Handedness !=
                WeaponHandedness.NotApplicable)
            {
                throw new InvalidOperationException(
                    $"Inventory item {definition.ItemId} " +
                    $"cannot define weapon handedness.");
            }
        }

        private static void
            ValidateSingleEquipmentInstance(
                ItemDefinition definition)
        {
            if (definition.MaxStackSize != 1)
            {
                throw new InvalidOperationException(
                    $"Equippable item {definition.ItemId} " +
                    $"must have a maximum stack size of 1.");
            }
        }
    }
}