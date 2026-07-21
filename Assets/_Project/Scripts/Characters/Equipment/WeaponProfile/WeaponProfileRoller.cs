using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class WeaponProfileRoller
    {
        private readonly WeaponAttackLineRoller
            lineRoller;

        private readonly
            WeaponProfileGenerationDefinitionValidator
            definitionValidator;

        public WeaponProfileRoller(
            WeaponAttackLineRoller newLineRoller,
            WeaponProfileGenerationDefinitionValidator
                newDefinitionValidator)
        {
            lineRoller =
                newLineRoller ??
                throw new ArgumentNullException(
                    nameof(newLineRoller));

            definitionValidator =
                newDefinitionValidator ??
                throw new ArgumentNullException(
                    nameof(newDefinitionValidator));
        }

        public RolledWeaponProfile Roll(
            WeaponProfileGenerationDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            definitionValidator.Validate(definition);

            List<RolledWeaponAttackLine> rolledLines =
                new List<RolledWeaponAttackLine>(
                    definition.Lines.Count);

            for (int index = 0;
                 index < definition.Lines.Count;
                 index++)
            {
                rolledLines.Add(
                    lineRoller.Roll(
                        definition.Lines[index]));
            }

            return new RolledWeaponProfile(
                rolledLines);
        }
    }
}