using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixPoolCatalogValidator
    {
        private readonly EquipmentAffixDefinitionCatalog
            catalog;

        public EquipmentAffixPoolCatalogValidator(
            EquipmentAffixDefinitionCatalog newCatalog)
        {
            catalog =
                newCatalog ??
                throw new ArgumentNullException(
                    nameof(newCatalog));
        }

        public void Validate(
            EquipmentAffixPoolDefinition pool)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(
                    nameof(pool));
            }

            for (int index = 0;
                 index < pool.Entries.Count;
                 index++)
            {
                EquipmentAffixPoolEntryDefinition entry =
                    pool.Entries[index];

                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Equipment affix pool entry cannot be null.");
                }

                try
                {
                    catalog.Get(
                        entry.AffixId);
                }
                catch (KeyNotFoundException exception)
                {
                    throw new InvalidOperationException(
                        $"Equipment affix pool references an unknown affix ID '{entry.AffixId}'.",
                        exception);
                }
            }
        }
    }
}