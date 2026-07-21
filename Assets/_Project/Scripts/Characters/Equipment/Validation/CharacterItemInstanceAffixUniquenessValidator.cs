using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterItemInstanceAffixUniquenessValidator
    {
        public void Validate(
            CharacterItemInstance item)
        {
            if (!item.IsValid)
            {
                throw new ArgumentException(
                    "Character item instance must be valid.",
                    nameof(item));
            }

            HashSet<EquipmentAffixId> usedAffixIds =
                new HashSet<EquipmentAffixId>();

            for (int index = 0;
                 index < item.Affixes.Count;
                 index++)
            {
                EquipmentAffixId affixId =
                    item.Affixes[index].AffixId;

                if (!usedAffixIds.Add(affixId))
                {
                    throw new InvalidOperationException(
                        $"Item instance contains duplicate " +
                        $"affix id '{affixId}'.");
                }
            }
        }
    }
}