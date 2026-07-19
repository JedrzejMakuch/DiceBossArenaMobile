using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixConfigurationValidator
    {
        private readonly EquipmentAffixDefinitionCatalog
            catalog;

        private readonly EquipmentAffixDefinitionCatalogValidator
            catalogValidator =
                new EquipmentAffixDefinitionCatalogValidator();

        private readonly EquipmentAffixPoolConfigurationValidator
            poolValidator;

        public EquipmentAffixConfigurationValidator(
            EquipmentAffixDefinitionCatalog newCatalog)
        {
            catalog =
                newCatalog ??
                throw new ArgumentNullException(
                    nameof(newCatalog));

            poolValidator =
                new EquipmentAffixPoolConfigurationValidator(
                    catalog);
        }

        public void Validate(
            EquipmentAffixPoolDefinition pool)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(
                    nameof(pool));
            }

            catalogValidator.Validate(
                catalog);

            poolValidator.Validate(
                pool);
        }
    }
}