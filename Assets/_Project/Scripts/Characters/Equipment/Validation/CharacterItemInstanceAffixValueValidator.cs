using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterItemInstanceAffixValueValidator
    {
        private readonly EquipmentAffixDefinitionCatalog
            catalog;

        public CharacterItemInstanceAffixValueValidator(
            EquipmentAffixDefinitionCatalog newCatalog)
        {
            catalog =
                newCatalog ??
                throw new ArgumentNullException(
                    nameof(newCatalog));
        }

        public void Validate(
            CharacterItemInstance item)
        {
            if (!item.IsValid)
            {
                throw new ArgumentException(
                    "Character item instance must be valid.",
                    nameof(item));
            }

            for (int index = 0;
                 index < item.Affixes.Count;
                 index++)
            {
                RolledEquipmentAffix rolledAffix =
                    item.Affixes[index];

                EquipmentAffixDefinition definition =
                    GetDefinition(
                        rolledAffix.AffixId);

                EquipmentAffixTierDefinition tier =
                    ResolveTier(
                        definition,
                        item.Level);

                if (tier == null)
                {
                    throw new InvalidOperationException(
                        $"Affix '{rolledAffix.AffixId}' has no " +
                        $"available tier for item level " +
                        $"{item.Level}.");
                }

                if (rolledAffix.Value <
                        tier.MinimumValue ||
                    rolledAffix.Value >
                        tier.MaximumValue)
                {
                    throw new InvalidOperationException(
                        $"Affix '{rolledAffix.AffixId}' value " +
                        $"{rolledAffix.Value} is outside the " +
                        $"allowed range {tier.MinimumValue} to " +
                        $"{tier.MaximumValue} for item level " +
                        $"{item.Level}.");
                }
            }
        }

        private EquipmentAffixDefinition GetDefinition(
            EquipmentAffixId affixId)
        {
            try
            {
                return catalog.Get(
                    affixId);
            }
            catch (KeyNotFoundException exception)
            {
                throw new InvalidOperationException(
                    $"Item instance contains unknown affix id " +
                    $"'{affixId}'.",
                    exception);
            }
        }

        private static EquipmentAffixTierDefinition
            ResolveTier(
                EquipmentAffixDefinition definition,
                int itemLevel)
        {
            EquipmentAffixTierDefinition resolvedTier =
                null;

            for (int index = 0;
                 index < definition.Tiers.Count;
                 index++)
            {
                EquipmentAffixTierDefinition tier =
                    definition.Tiers[index];

                if (tier.MinimumItemLevel >
                    itemLevel)
                {
                    break;
                }

                resolvedTier = tier;
            }

            return resolvedTier;
        }
    }
}