using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixTierDefinitionValidator
    {
        public void Validate(
            EquipmentAffixTierDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (definition.MinimumItemLevel < 1)
            {
                throw new InvalidOperationException(
                    "Minimum item level must be at least 1.");
            }

            if (definition.MinimumValue >
                definition.MaximumValue)
            {
                throw new InvalidOperationException(
                    "Minimum affix value cannot be greater than maximum value.");
            }
        }
    }
}