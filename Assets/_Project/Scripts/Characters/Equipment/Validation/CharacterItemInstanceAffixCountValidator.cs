using System;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterItemInstanceAffixCountValidator
    {
        private readonly EquipmentRarityAffixCountResolver
            affixCountResolver =
                new EquipmentRarityAffixCountResolver();

        public void Validate(
            CharacterItemInstance item)
        {
            if (!item.IsValid)
            {
                throw new ArgumentException(
                    "Character item instance must be valid.",
                    nameof(item));
            }

            int requiredAffixCount =
                affixCountResolver.Resolve(
                    item.Rarity);

            if (item.Affixes.Count !=
                requiredAffixCount)
            {
                throw new InvalidOperationException(
                    $"Item rarity '{item.Rarity}' requires " +
                    $"{requiredAffixCount} affixes, but the " +
                    $"instance contains {item.Affixes.Count}.");
            }
        }
    }
}