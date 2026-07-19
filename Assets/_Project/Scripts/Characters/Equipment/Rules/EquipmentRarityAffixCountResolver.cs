using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentRarityAffixCountResolver
    {
        public int Resolve(
            EquipmentItemRarity rarity)
        {
            switch (rarity)
            {
                case EquipmentItemRarity.Common:
                    return 0;

                case EquipmentItemRarity.Magic:
                    return 2;

                case EquipmentItemRarity.Rare:
                    return 4;

                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(rarity),
                        rarity,
                        "Unsupported equipment item rarity.");
            }
        }
    }
}