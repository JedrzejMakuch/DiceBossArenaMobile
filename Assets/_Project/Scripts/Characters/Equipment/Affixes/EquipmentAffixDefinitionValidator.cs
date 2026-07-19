using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixDefinitionValidator
    {
        private readonly EquipmentAffixTierDefinitionValidator
            tierValidator =
                new EquipmentAffixTierDefinitionValidator();

        public void Validate(
            EquipmentAffixDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            ValidateId(definition);
            ValidateStatType(definition.StatType);
            ValidateModifierType(
                definition.ModifierType);
            ValidateTiers(definition);
        }

        private static void ValidateId(
            EquipmentAffixDefinition definition)
        {
            EquipmentAffixId id =
                definition.Id;

            if (string.IsNullOrWhiteSpace(id.Value))
            {
                throw new InvalidOperationException(
                    "Equipment affix ID cannot be empty.");
            }
        }

        private static void ValidateStatType(
            FightStatType statType)
        {
            if (!Enum.IsDefined(
                    typeof(FightStatType),
                    statType))
            {
                throw new InvalidOperationException(
                    "Equipment affix has an unsupported stat type.");
            }

            if (statType ==
                    FightStatType.MaxActionPoints ||
                statType ==
                    FightStatType.MaxMovementPoints)
            {
                throw new InvalidOperationException(
                    "Action Points and Movement Points cannot be random affixes.");
            }
        }

        private static void ValidateModifierType(
            FightStatModifierType modifierType)
        {
            if (!Enum.IsDefined(
                    typeof(FightStatModifierType),
                    modifierType))
            {
                throw new InvalidOperationException(
                    "Equipment affix has an unsupported modifier type.");
            }
        }

        private void ValidateTiers(
            EquipmentAffixDefinition definition)
        {
            if (definition.Tiers == null)
            {
                throw new InvalidOperationException(
                    "Equipment affix tiers cannot be null.");
            }

            if (definition.Tiers.Count == 0)
            {
                throw new InvalidOperationException(
                    "Equipment affix must contain at least one tier.");
            }

            for (int index = 0;
             index < definition.Tiers.Count;
             index++)
                {
                    EquipmentAffixTierDefinition tier =
                        definition.Tiers[index];

                    if (tier == null)
                    {
                        throw new InvalidOperationException(
                            "Equipment affix tier cannot be null.");
                    }

                    tierValidator.Validate(tier);
                }

            ValidateTierOrder(definition);
        }

        private static void ValidateTierOrder(
    EquipmentAffixDefinition definition)
        {
            for (int index = 1;
                 index < definition.Tiers.Count;
                 index++)
            {
                EquipmentAffixTierDefinition previousTier =
                    definition.Tiers[index - 1];

                EquipmentAffixTierDefinition currentTier =
                    definition.Tiers[index];

                if (currentTier.MinimumItemLevel <=
                    previousTier.MinimumItemLevel)
                {
                    throw new InvalidOperationException(
                        "Equipment affix tiers must be ordered by unique, increasing minimum item levels.");
                }
            }
        }
    }
}