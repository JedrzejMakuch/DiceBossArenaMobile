using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentSlotCompatibilityValidator
    {
        private readonly ItemDefinitionContentValidator
            contentValidator;

        public EquipmentSlotCompatibilityValidator(
            ItemDefinitionContentValidator
                contentValidator)
        {
            this.contentValidator =
                contentValidator ??
                throw new ArgumentNullException(
                    nameof(contentValidator));
        }

        public bool CanEquip(
            ItemDefinition definition,
            EquipmentSlotType targetSlot)
        {
            if (definition == null)
            {
                return false;
            }

            try
            {
                contentValidator.Validate(
                    definition);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            if (!definition.IsEquippable)
            {
                return false;
            }

            if (targetSlot ==
                EquipmentSlotType.None)
            {
                return false;
            }

            return definition.SlotType ==
                   targetSlot;
        }
    }
}