using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentLoadoutValidator
    {
        private readonly IItemDefinitionResolver
            definitionResolver;

        private readonly
            EquipmentSlotCompatibilityValidator
            slotValidator;

        private readonly ItemRequirementValidator
            requirementValidator;

        public EquipmentLoadoutValidator(
            IItemDefinitionResolver definitionResolver,
            EquipmentSlotCompatibilityValidator
                slotValidator,
            ItemRequirementValidator
                requirementValidator)
        {
            this.definitionResolver =
                definitionResolver ??
                throw new ArgumentNullException(
                    nameof(definitionResolver));

            this.slotValidator =
                slotValidator ??
                throw new ArgumentNullException(
                    nameof(slotValidator));

            this.requirementValidator =
                requirementValidator ??
                throw new ArgumentNullException(
                    nameof(requirementValidator));
        }

        public void Validate(
            EquipmentLoadoutSnapshot loadout,
            CharacterClassId classId,
            CharacterSpecializationId specializationId)
        {
            if (loadout == null)
            {
                throw new ArgumentNullException(
                    nameof(loadout));
            }

            ItemDefinition mainHandDefinition =
                null;

            bool hasOffHandItem =
                false;

            for (int i = 0;
                 i < loadout.Items.Count;
                 i++)
            {
                EquippedItemSnapshot item =
                    loadout.Items[i];

                if (!definitionResolver.TryResolve(
                        item.ItemId,
                        out ItemDefinition definition))
                {
                    throw new InvalidOperationException(
                        $"Unknown item definition: " +
                        $"{item.ItemId}.");
                }

                if (!slotValidator.CanEquip(
                        definition,
                        item.SlotType))
                {
                    throw new InvalidOperationException(
                        $"Item {item.ItemId} cannot be " +
                        $"equipped in slot " +
                        $"{item.SlotType}.");
                }

                if (!requirementValidator
                        .MeetsRequirements(
                            definition,
                            classId,
                            specializationId))
                {
                    throw new InvalidOperationException(
                        $"Character does not meet " +
                        $"requirements for item " +
                        $"{item.ItemId}.");
                }

                if (item.SlotType ==
                    EquipmentSlotType.MainHand)
                {
                    mainHandDefinition =
                        definition;
                }

                if (item.SlotType ==
                    EquipmentSlotType.OffHand)
                {
                    hasOffHandItem =
                        true;
                }
            }

            if (mainHandDefinition != null &&
                mainHandDefinition.Handedness ==
                    WeaponHandedness.TwoHanded &&
                hasOffHandItem)
            {
                throw new InvalidOperationException(
                    $"Two-handed weapon " +
                    $"{mainHandDefinition.ItemId} " +
                    $"blocks the OffHand slot.");
            }
        }
    }
}