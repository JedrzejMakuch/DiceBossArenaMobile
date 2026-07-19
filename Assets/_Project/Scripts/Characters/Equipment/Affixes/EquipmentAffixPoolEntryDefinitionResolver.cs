using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixPoolEntryDefinitionResolver
    {
        private readonly EquipmentAffixDefinitionCatalog
            catalog;

        public EquipmentAffixPoolEntryDefinitionResolver(
            EquipmentAffixDefinitionCatalog newCatalog)
        {
            catalog =
                newCatalog ??
                throw new ArgumentNullException(
                    nameof(newCatalog));
        }

        public EquipmentAffixDefinition Resolve(
            EquipmentAffixPoolEntryDefinition entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(
                    nameof(entry));
            }

            return catalog.Get(
                entry.AffixId);
        }
    }
}