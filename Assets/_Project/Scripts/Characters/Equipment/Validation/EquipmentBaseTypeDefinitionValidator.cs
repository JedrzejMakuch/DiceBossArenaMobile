using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentBaseTypeDefinitionValidator
    {
        public void Validate(
            EquipmentBaseTypeDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (!definition.BaseTypeId.IsValid)
            {
                throw new InvalidOperationException(
                    "Equipment base type must have a valid ID.");
            }

            if (definition.SlotType ==
                EquipmentSlotType.None)
            {
                throw new InvalidOperationException(
                    "Equipment base type must define a slot.");
            }

            if (definition.Category ==
                EquipmentBaseTypeCategory.None)
            {
                throw new InvalidOperationException(
                    "Equipment base type must define a category.");
            }

            if (!IsCategoryAllowedForSlot(
                    definition.SlotType,
                    definition.Category))
            {
                throw new InvalidOperationException(
                    $"Equipment base type category " +
                    $"{definition.Category} is not valid " +
                    $"for slot {definition.SlotType}.");
            }

            ValidateStatModifiers(
    definition.StatModifiers);
        }

        private static bool IsCategoryAllowedForSlot(
            EquipmentSlotType slotType,
            EquipmentBaseTypeCategory category)
        {
            switch (slotType)
            {
                case EquipmentSlotType.MainHand:
                    return category ==
                           EquipmentBaseTypeCategory.Sword ||
                           category ==
                           EquipmentBaseTypeCategory.Axe ||
                           category ==
                           EquipmentBaseTypeCategory.Hammer ||
                           category ==
                           EquipmentBaseTypeCategory.Dagger ||
                           category ==
                           EquipmentBaseTypeCategory.Bow ||
                           category ==
                           EquipmentBaseTypeCategory.Staff ||
                           category ==
                           EquipmentBaseTypeCategory.Wand;

                case EquipmentSlotType.OffHand:
                    return category ==
                           EquipmentBaseTypeCategory.Shield;

                case EquipmentSlotType.Head:
                    return category ==
                           EquipmentBaseTypeCategory.Helmet;

                case EquipmentSlotType.Armor:
                    return category ==
                           EquipmentBaseTypeCategory.ChestArmor;

                case EquipmentSlotType.Hands:
                    return category ==
                           EquipmentBaseTypeCategory.Gloves;

                case EquipmentSlotType.Feet:
                    return category ==
                           EquipmentBaseTypeCategory.Boots;

                case EquipmentSlotType.Accessory:
                case EquipmentSlotType.AccessoryTwo:
                    return category ==
                           EquipmentBaseTypeCategory.Ring ||
                           category ==
                           EquipmentBaseTypeCategory.Amulet;

                default:
                    return false;
            }
        }

        private static void ValidateStatModifiers(
    System.Collections.Generic.IReadOnlyList<
        CharacterStatModifierDefinition>
        statModifiers)
        {
            if (statModifiers == null)
            {
                throw new InvalidOperationException(
                    "Equipment base type stat modifiers " +
                    "collection cannot be null.");
            }

            for (int i = 0;
                 i < statModifiers.Count;
                 i++)
            {
                if (statModifiers[i] == null)
                {
                    throw new InvalidOperationException(
                        $"Equipment base type stat modifier " +
                        $"at index {i} cannot be null.");
                }
            }
        }
    }
}