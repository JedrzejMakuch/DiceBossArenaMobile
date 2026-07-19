using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixPoolDefinitionValidator
    {
        private readonly EquipmentAffixPoolEntryDefinitionValidator
            entryValidator =
                new EquipmentAffixPoolEntryDefinitionValidator();

        public void Validate(
            EquipmentAffixPoolDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (definition.Entries == null)
            {
                throw new InvalidOperationException(
                    "Equipment affix pool entries cannot be null.");
            }

            if (definition.Entries.Count == 0)
            {
                throw new InvalidOperationException(
                    "Equipment affix pool must contain at least one entry.");
            }

            HashSet<EquipmentAffixId> affixIds =
                new HashSet<EquipmentAffixId>();

            for (int index = 0;
                 index < definition.Entries.Count;
                 index++)
            {
                EquipmentAffixPoolEntryDefinition entry =
                    definition.Entries[index];

                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Equipment affix pool entry cannot be null.");
                }

                entryValidator.Validate(entry);

                if (!affixIds.Add(entry.AffixId))
                {
                    throw new InvalidOperationException(
                        "Equipment affix pool cannot contain duplicate affix IDs.");
                }
            }
        }
    }
}