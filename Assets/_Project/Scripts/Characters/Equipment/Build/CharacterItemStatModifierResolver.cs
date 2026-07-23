using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class CharacterItemStatModifierResolver
    {
        public IReadOnlyList<FightStatModifier> Resolve(
            CharacterItemInstance item)
        {
            if (!item.IsValid)
            {
                throw new ArgumentException(
                    "Character item instance must be valid.",
                    nameof(item));
            }

            List<FightStatModifier> result =
                new List<FightStatModifier>(
                    item.Affixes.Count);

            for (int index = 0;
                 index < item.Affixes.Count;
                 index++)
            {
                RolledEquipmentAffix affix =
                    item.Affixes[index];

                result.Add(
                    new FightStatModifier(
                        affix.StatType,
                        affix.ModifierType,
                        affix.Value));
            }

            return result;
        }
    }
}