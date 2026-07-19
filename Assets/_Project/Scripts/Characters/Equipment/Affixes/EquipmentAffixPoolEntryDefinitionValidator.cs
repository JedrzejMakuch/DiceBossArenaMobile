using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixPoolEntryDefinitionValidator
    {
        public void Validate(
            EquipmentAffixPoolEntryDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            ValidateAffixId(definition);
            ValidateWeight(definition.Weight);
        }

        private static void ValidateAffixId(
            EquipmentAffixPoolEntryDefinition definition)
        {
            try
            {
                _ = definition.AffixId;
            }
            catch (ArgumentException exception)
            {
                throw new InvalidOperationException(
                    "Equipment affix pool entry must contain a valid affix ID.",
                    exception);
            }
        }

        private static void ValidateWeight(
            int weight)
        {
            if (weight <= 0)
            {
                throw new InvalidOperationException(
                    "Equipment affix pool entry weight must be greater than zero.");
            }
        }
    }
}