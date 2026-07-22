using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        WeaponAttackLineGenerationDefinitionValidator
    {
        private readonly WeaponAttackEffectDefinitionValidator
            effectValidator;

        public WeaponAttackLineGenerationDefinitionValidator()
            : this(
                new WeaponAttackEffectDefinitionValidator())
        {
        }

        public WeaponAttackLineGenerationDefinitionValidator(
            WeaponAttackEffectDefinitionValidator newEffectValidator)
        {
            effectValidator =
                newEffectValidator ??
                throw new ArgumentNullException(
                    nameof(newEffectValidator));
        }

        public void Validate(
            WeaponAttackLineGenerationDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            ValidateLineId(definition);
            ValidateDamageRange(definition);
            ValidateAllowedElements(definition);
            ValidateEffects(definition);
        }

        private static void ValidateLineId(
            WeaponAttackLineGenerationDefinition definition)
        {
            try
            {
                WeaponAttackLineId lineId =
                    definition.LineId;
            }
            catch (ArgumentException exception)
            {
                throw new InvalidOperationException(
                    "Weapon attack line ID cannot be empty.",
                    exception);
            }
        }

        private static void ValidateDamageRange(
            WeaponAttackLineGenerationDefinition definition)
        {
            if (definition.MinDamage < 0)
            {
                throw new InvalidOperationException(
                    "Weapon attack line minimum damage " +
                    "cannot be negative.");
            }

            if (definition.MaxDamage <
                definition.MinDamage)
            {
                throw new InvalidOperationException(
                    "Weapon attack line maximum damage " +
                    "cannot be lower than minimum damage.");
            }
        }

        private static void ValidateAllowedElements(
            WeaponAttackLineGenerationDefinition definition)
        {
            if (definition.AllowedElements == null)
            {
                throw new InvalidOperationException(
                    "Allowed weapon attack elements " +
                    "cannot be null.");
            }

            if (definition.AllowedElements.Count == 0)
            {
                throw new InvalidOperationException(
                    "Weapon attack line must allow " +
                    "at least one element.");
            }

            HashSet<WeaponAttackElement> uniqueElements =
                new HashSet<WeaponAttackElement>();

            for (int index = 0;
                 index < definition.AllowedElements.Count;
                 index++)
            {
                WeaponAttackElement element =
                    definition.AllowedElements[index];

                if (!Enum.IsDefined(
                        typeof(WeaponAttackElement),
                        element))
                {
                    throw new InvalidOperationException(
                        "Weapon attack line contains " +
                        "an unsupported element.");
                }

                if (!uniqueElements.Add(element))
                {
                    throw new InvalidOperationException(
                        "Weapon attack line cannot contain " +
                        "duplicate allowed elements.");
                }
            }
        }

        private void ValidateEffects(
    WeaponAttackLineGenerationDefinition definition)
        {
            if (definition.Effects == null)
            {
                throw new InvalidOperationException(
                    "Weapon attack line effects cannot be null.");
            }

            for (int index = 0;
                 index < definition.Effects.Count;
                 index++)
            {
                WeaponAttackEffectDefinition effect =
                    definition.Effects[index];

                if (effect == null)
                {
                    throw new InvalidOperationException(
                        "Weapon attack line cannot contain " +
                        "a null effect definition.");
                }

                try
                {
                    effectValidator.Validate(effect);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(
                        "Weapon attack line contains " +
                        "an invalid effect definition.",
                        exception);
                }
            }
        }
    }
}