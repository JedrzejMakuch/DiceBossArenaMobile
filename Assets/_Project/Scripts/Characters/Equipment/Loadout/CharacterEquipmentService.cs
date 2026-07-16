using System;

namespace DiceBossArena.Game
{
    public sealed class CharacterEquipmentService
    {
        private readonly IItemDefinitionResolver
            definitionResolver;

        private readonly
            EquipmentSlotCompatibilityValidator
            slotValidator;

        private readonly ItemRequirementValidator
            requirementValidator;

        public CharacterEquipmentService(
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

        public EquipmentOperationResult TryEquip(
            CharacterInventory inventory,
            CharacterEquipmentLoadout loadout,
            CharacterItemInstanceId instanceId,
            EquipmentSlotType targetSlot,
            CharacterClassId classId,
            CharacterSpecializationId specializationId)
        {
            if (inventory == null)
            {
                throw new ArgumentNullException(
                    nameof(inventory));
            }

            if (loadout == null)
            {
                throw new ArgumentNullException(
                    nameof(loadout));
            }

            if (!inventory.TryGet(
                    instanceId,
                    out CharacterItemInstance item))
            {
                return EquipmentOperationResult
                    .ItemNotFound;
            }

            if (!definitionResolver.TryResolve(
                    item.ItemId,
                    out ItemDefinition definition))
            {
                return EquipmentOperationResult
                    .UnknownDefinition;
            }

            if (!slotValidator.CanEquip(
                    definition,
                    targetSlot))
            {
                return EquipmentOperationResult
                    .InvalidSlot;
            }

            if (!requirementValidator
                    .MeetsRequirements(
                        definition,
                        classId,
                        specializationId))
            {
                return EquipmentOperationResult
                    .RequirementsNotMet;
            }

            if (loadout.IsSlotOccupied(
                    targetSlot))
            {
                return EquipmentOperationResult
                    .SlotOccupied;
            }

            if (CreatesHandConflict(
                    inventory,
                    loadout,
                    definition,
                    targetSlot))
            {
                return EquipmentOperationResult
                    .LoadoutConflict;
            }

            loadout.Set(
                new EquippedItemInstance(
                    targetSlot,
                    instanceId),
                out _);

            return EquipmentOperationResult
                .Equipped;
        }

        public EquipmentOperationResult TryUnequip(
    CharacterEquipmentLoadout loadout,
    EquipmentSlotType targetSlot)
        {
            if (loadout == null)
            {
                throw new ArgumentNullException(
                    nameof(loadout));
            }

            if (targetSlot ==
                EquipmentSlotType.None)
            {
                return EquipmentOperationResult
                    .InvalidSlot;
            }

            if (!loadout.TryRemove(
                    targetSlot,
                    out _))
            {
                return EquipmentOperationResult
                    .SlotEmpty;
            }

            return EquipmentOperationResult
                .Unequipped;
        }

        public EquipmentOperationResult TrySwap(
    CharacterInventory inventory,
    CharacterEquipmentLoadout loadout,
    CharacterItemInstanceId newInstanceId,
    EquipmentSlotType targetSlot,
    CharacterClassId classId,
    CharacterSpecializationId specializationId)
        {
            if (inventory == null)
            {
                throw new ArgumentNullException(
                    nameof(inventory));
            }

            if (loadout == null)
            {
                throw new ArgumentNullException(
                    nameof(loadout));
            }

            if (!loadout.TryGet(
                    targetSlot,
                    out EquippedItemInstance currentItem))
            {
                return EquipmentOperationResult
                    .SlotEmpty;
            }

            if (currentItem.InstanceId.Equals(
                    newInstanceId))
            {
                return EquipmentOperationResult
                    .AlreadyEquipped;
            }

            if (loadout.IsInstanceEquipped(
                    newInstanceId))
            {
                return EquipmentOperationResult
                    .AlreadyEquipped;
            }

            if (!inventory.TryGet(
                    newInstanceId,
                    out CharacterItemInstance newItem))
            {
                return EquipmentOperationResult
                    .ItemNotFound;
            }

            if (!definitionResolver.TryResolve(
                    newItem.ItemId,
                    out ItemDefinition newDefinition))
            {
                return EquipmentOperationResult
                    .UnknownDefinition;
            }

            if (!slotValidator.CanEquip(
                    newDefinition,
                    targetSlot))
            {
                return EquipmentOperationResult
                    .InvalidSlot;
            }

            if (!requirementValidator
                    .MeetsRequirements(
                        newDefinition,
                        classId,
                        specializationId))
            {
                return EquipmentOperationResult
                    .RequirementsNotMet;
            }

            if (CreatesHandConflict(
                    inventory,
                    loadout,
                    newDefinition,
                    targetSlot))
            {
                return EquipmentOperationResult
                    .LoadoutConflict;
            }

            bool replaced =
                loadout.Set(
                    new EquippedItemInstance(
                        targetSlot,
                        newInstanceId),
                    out EquippedItemInstance replacedItem);

            if (!replaced ||
                !replacedItem.InstanceId.Equals(
                    currentItem.InstanceId))
            {
                throw new InvalidOperationException(
                    "Loadout changed unexpectedly " +
                    "during swap.");
            }

            return EquipmentOperationResult
                .Swapped;
        }

        private bool CreatesHandConflict(
            CharacterInventory inventory,
            CharacterEquipmentLoadout loadout,
            ItemDefinition newDefinition,
            EquipmentSlotType targetSlot)
        {
            if (targetSlot ==
                    EquipmentSlotType.MainHand &&
                newDefinition.Handedness ==
                    WeaponHandedness.TwoHanded &&
                loadout.IsSlotOccupied(
                    EquipmentSlotType.OffHand))
            {
                return true;
            }

            if (targetSlot !=
                EquipmentSlotType.OffHand)
            {
                return false;
            }

            if (!loadout.TryGet(
                    EquipmentSlotType.MainHand,
                    out EquippedItemInstance mainHand))
            {
                return false;
            }

            if (!inventory.TryGet(
                    mainHand.InstanceId,
                    out CharacterItemInstance
                        mainHandItem))
            {
                return true;
            }

            if (!definitionResolver.TryResolve(
                    mainHandItem.ItemId,
                    out ItemDefinition
                        mainHandDefinition))
            {
                return true;
            }

            return mainHandDefinition.Handedness ==
                   WeaponHandedness.TwoHanded;
        }
    }
}