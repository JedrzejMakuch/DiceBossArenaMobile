using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentItemRarityRoller
    {
        private readonly IEquipmentAffixRandomSource
            randomSource;

        private readonly int commonWeight;
        private readonly int magicWeight;
        private readonly int totalWeight;

        public EquipmentItemRarityRoller(
            IEquipmentAffixRandomSource newRandomSource,
            int newCommonWeight,
            int newMagicWeight,
            int newRareWeight)
        {
            randomSource =
                newRandomSource ??
                throw new ArgumentNullException(
                    nameof(newRandomSource));

            ValidateWeight(
                newCommonWeight,
                nameof(newCommonWeight));

            ValidateWeight(
                newMagicWeight,
                nameof(newMagicWeight));

            ValidateWeight(
                newRareWeight,
                nameof(newRareWeight));

            commonWeight = newCommonWeight;
            magicWeight = newMagicWeight;

            checked
            {
                totalWeight =
                    newCommonWeight +
                    newMagicWeight +
                    newRareWeight;
            }

            if (totalWeight == 0)
            {
                throw new ArgumentException(
                    "At least one equipment rarity weight " +
                    "must be greater than zero.");
            }
        }

        public EquipmentItemRarity Roll()
        {
            int roll =
                randomSource.Next(
                    0,
                    totalWeight);

            if (roll < commonWeight)
            {
                return EquipmentItemRarity.Common;
            }

            if (roll <
                commonWeight + magicWeight)
            {
                return EquipmentItemRarity.Magic;
            }

            return EquipmentItemRarity.Rare;
        }

        private static void ValidateWeight(
            int weight,
            string parameterName)
        {
            if (weight < 0)
            {
                throw new ArgumentOutOfRangeException(
                    parameterName,
                    weight,
                    "Equipment rarity weight cannot be negative.");
            }
        }
    }
}