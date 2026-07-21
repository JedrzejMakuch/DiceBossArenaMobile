using System;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterItemInstanceAffixValidator
    {
        private readonly
            CharacterItemInstanceAffixCountValidator
            countValidator =
                new CharacterItemInstanceAffixCountValidator();

        private readonly
            CharacterItemInstanceAffixUniquenessValidator
            uniquenessValidator =
                new CharacterItemInstanceAffixUniquenessValidator();

        private readonly
            CharacterItemInstanceAffixDefinitionValidator
            definitionValidator;

        private readonly
            CharacterItemInstanceAffixValueValidator
            valueValidator;

        public CharacterItemInstanceAffixValidator(
            EquipmentAffixDefinitionCatalog catalog)
        {
            if (catalog == null)
            {
                throw new ArgumentNullException(
                    nameof(catalog));
            }

            definitionValidator =
                new CharacterItemInstanceAffixDefinitionValidator(
                    catalog);

            valueValidator =
                new CharacterItemInstanceAffixValueValidator(
                    catalog);
        }

        public void Validate(
            CharacterItemInstance item)
        {
            countValidator.Validate(
                item);

            uniquenessValidator.Validate(
                item);

            definitionValidator.Validate(
                item);

            valueValidator.Validate(
                item);
        }
    }
}