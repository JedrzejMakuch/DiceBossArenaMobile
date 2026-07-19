using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixTierValueRoller
    {
        private readonly IEquipmentAffixRandomSource
            randomSource;

        public EquipmentAffixTierValueRoller(
            IEquipmentAffixRandomSource newRandomSource)
        {
            randomSource =
                newRandomSource ??
                throw new ArgumentNullException(
                    nameof(newRandomSource));
        }

        public int Roll(
            EquipmentAffixTierDefinition tier)
        {
            if (tier == null)
            {
                throw new ArgumentNullException(
                    nameof(tier));
            }

            int maximumExclusive;

            checked
            {
                maximumExclusive =
                    tier.MaximumValue + 1;
            }

            return randomSource.Next(
                tier.MinimumValue,
                maximumExclusive);
        }
    }
}