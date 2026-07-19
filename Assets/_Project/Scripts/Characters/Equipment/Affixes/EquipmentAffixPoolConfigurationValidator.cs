using System;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentAffixPoolConfigurationValidator
    {
        private readonly EquipmentAffixPoolDefinitionValidator
            poolValidator =
                new EquipmentAffixPoolDefinitionValidator();

        private readonly EquipmentAffixPoolCatalogValidator
            catalogValidator;

        public EquipmentAffixPoolConfigurationValidator(
            EquipmentAffixDefinitionCatalog catalog)
        {
            catalogValidator =
                new EquipmentAffixPoolCatalogValidator(
                    catalog ??
                    throw new ArgumentNullException(
                        nameof(catalog)));
        }

        public void Validate(
            EquipmentAffixPoolDefinition pool)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(
                    nameof(pool));
            }

            poolValidator.Validate(pool);
            catalogValidator.Validate(pool);
        }
    }
}