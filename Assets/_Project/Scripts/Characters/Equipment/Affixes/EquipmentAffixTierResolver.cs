using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixTierResolver
    {
        public EquipmentAffixTierDefinition Resolve(
            EquipmentAffixDefinition affix,
            int itemLevel)
        {
            if (affix == null)
            {
                throw new ArgumentNullException(
                    nameof(affix));
            }

            if (itemLevel < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(itemLevel),
                    itemLevel,
                    "Item level must be at least 1.");
            }

            EquipmentAffixTierDefinition resolvedTier =
                null;

            for (int index = 0;
                 index < affix.Tiers.Count;
                 index++)
            {
                EquipmentAffixTierDefinition tier =
                    affix.Tiers[index];

                if (tier.MinimumItemLevel >
                    itemLevel)
                {
                    break;
                }

                resolvedTier =
                    tier;
            }

            return resolvedTier;
        }
    }
}