using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterItemInstanceAffixDefinitionValidator
    {
        private readonly EquipmentAffixDefinitionCatalog
            catalog;

        public CharacterItemInstanceAffixDefinitionValidator(
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

                EquipmentAffixDefinition definition;

                try
                {
                    definition =
                        catalog.Get(
                            rolledAffix.AffixId);
                }
                catch (KeyNotFoundException exception)
                {
                    throw new InvalidOperationException(
                        $"Item instance contains unknown affix id " +
                        $"'{rolledAffix.AffixId}'.",
                        exception);
                }

                if (rolledAffix.StatType !=
                    definition.StatType)
                {
                    throw new InvalidOperationException(
                        $"Rolled affix '{rolledAffix.AffixId}' " +
                        $"uses stat type " +
                        $"'{rolledAffix.StatType}', but its " +
                        $"definition uses " +
                        $"'{definition.StatType}'.");
                }

                if (rolledAffix.ModifierType !=
                    definition.ModifierType)
                {
                    throw new InvalidOperationException(
                        $"Rolled affix '{rolledAffix.AffixId}' " +
                        $"uses modifier type " +
                        $"'{rolledAffix.ModifierType}', but its " +
                        $"definition uses " +
                        $"'{definition.ModifierType}'.");
                }
            }
        }
    }
}