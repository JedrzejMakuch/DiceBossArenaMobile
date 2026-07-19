using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixPoolRoller
    {
        private readonly IEquipmentAffixRandomSource
            randomSource;

        private readonly EquipmentAffixPoolTotalWeightResolver
            totalWeightResolver =
                new EquipmentAffixPoolTotalWeightResolver();

        private readonly EquipmentAffixPoolEntryResolver
            entryResolver =
                new EquipmentAffixPoolEntryResolver();

        public EquipmentAffixPoolRoller(
            IEquipmentAffixRandomSource newRandomSource)
        {
            randomSource =
                newRandomSource ??
                throw new ArgumentNullException(
                    nameof(newRandomSource));
        }

        public EquipmentAffixPoolEntryDefinition Roll(
            EquipmentAffixPoolDefinition pool)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(
                    nameof(pool));
            }

            int totalWeight =
                totalWeightResolver.Resolve(pool);

            int roll =
                randomSource.Next(
                    0,
                    totalWeight);

            return entryResolver.Resolve(
                pool,
                roll);
        }
    }
}