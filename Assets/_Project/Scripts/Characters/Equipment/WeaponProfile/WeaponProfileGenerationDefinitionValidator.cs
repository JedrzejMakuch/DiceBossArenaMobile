using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        WeaponProfileGenerationDefinitionValidator
    {
        private readonly
            WeaponAttackLineGenerationDefinitionValidator
            lineValidator;

        public WeaponProfileGenerationDefinitionValidator(
            WeaponAttackLineGenerationDefinitionValidator
                newLineValidator)
        {
            lineValidator =
                newLineValidator ??
                throw new ArgumentNullException(
                    nameof(newLineValidator));
        }

        public void Validate(
            WeaponProfileGenerationDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (definition.Lines == null)
            {
                throw new InvalidOperationException(
                    "Weapon profile lines cannot be null.");
            }

            if (definition.Lines.Count == 0)
            {
                throw new InvalidOperationException(
                    "Weapon profile must contain " +
                    "at least one line.");
            }

            HashSet<WeaponAttackLineId> uniqueLineIds =
                new HashSet<WeaponAttackLineId>();

            for (int index = 0;
                 index < definition.Lines.Count;
                 index++)
            {
                WeaponAttackLineGenerationDefinition line =
                    definition.Lines[index];

                if (line == null)
                {
                    throw new InvalidOperationException(
                        "Weapon profile cannot contain " +
                        "a null line.");
                }

                lineValidator.Validate(line);

                if (!uniqueLineIds.Add(line.LineId))
                {
                    throw new InvalidOperationException(
                        "Weapon profile cannot contain " +
                        "duplicate line IDs.");
                }
            }
        }
    }
}