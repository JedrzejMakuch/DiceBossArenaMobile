using System;

namespace DiceBossArena.Game
{
    public sealed class WeaponAttackLineRoller
    {
        private readonly IEquipmentAffixRandomSource
            randomSource;

        private readonly
            WeaponAttackLineGenerationDefinitionValidator
            definitionValidator;

        public WeaponAttackLineRoller(
            IEquipmentAffixRandomSource newRandomSource,
            WeaponAttackLineGenerationDefinitionValidator
                newDefinitionValidator)
        {
            randomSource =
                newRandomSource ??
                throw new ArgumentNullException(
                    nameof(newRandomSource));

            definitionValidator =
                newDefinitionValidator ??
                throw new ArgumentNullException(
                    nameof(newDefinitionValidator));
        }

        public RolledWeaponAttackLine Roll(
            WeaponAttackLineGenerationDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            definitionValidator.Validate(
                definition);

            int elementIndex =
                randomSource.Next(
                    0,
                    definition.AllowedElements.Count);

            WeaponAttackElement element =
                definition.AllowedElements[elementIndex];

            return new RolledWeaponAttackLine(
                definition.LineId,
                element,
                definition.MinDamage,
                definition.MaxDamage,
                definition.Effects);
        }
    }
}