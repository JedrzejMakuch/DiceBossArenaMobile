using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentBaseTypeWeaponProfileValidator
    {
        private readonly
            WeaponProfileGenerationDefinitionValidator
            profileValidator;

        public EquipmentBaseTypeWeaponProfileValidator(
            WeaponProfileGenerationDefinitionValidator
                newProfileValidator)
        {
            profileValidator =
                newProfileValidator ??
                throw new ArgumentNullException(
                    nameof(newProfileValidator));
        }

        public void Validate(
            EquipmentBaseTypeDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            bool isWeapon =
                definition.SlotType ==
                EquipmentSlotType.MainHand;

            if (isWeapon &&
                definition.WeaponProfileGeneration == null)
            {
                throw new InvalidOperationException(
                    "Main-hand equipment must define " +
                    "weapon profile generation.");
            }

            if (!isWeapon &&
                definition.WeaponProfileGeneration != null)
            {
                throw new InvalidOperationException(
                    "Only main-hand equipment can define " +
                    "weapon profile generation.");
            }

            if (definition.WeaponProfileGeneration != null)
            {
                profileValidator.Validate(
                    definition.WeaponProfileGeneration);
            }
        }
    }
}