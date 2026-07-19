using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixDefinitionCatalogValidator
    {
        private readonly EquipmentAffixDefinitionValidator
            definitionValidator =
                new EquipmentAffixDefinitionValidator();

        public void Validate(
            EquipmentAffixDefinitionCatalog catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(
                    nameof(catalog));
            }

            foreach (EquipmentAffixDefinition definition
                     in catalog.Definitions)
            {
                definitionValidator.Validate(
                    definition);
            }
        }
    }
}