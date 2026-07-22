using System;

namespace DiceBossArena.Game
{
    public sealed class WeaponAttackEffectDefinitionValidator
    {
        public void Validate(
            WeaponAttackEffectDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            ValidateEffectType(definition);
            ValidateTriggerChance(definition);
            ValidateEffectConfiguration(definition);
        }

        private static void ValidateEffectType(
            WeaponAttackEffectDefinition definition)
        {
            if (!Enum.IsDefined(
                    typeof(WeaponAttackEffectType),
                    definition.EffectType))
            {
                throw new InvalidOperationException(
                    "Weapon attack effect type is unsupported.");
            }
        }

        private static void ValidateTriggerChance(
            WeaponAttackEffectDefinition definition)
        {
            if (definition.TriggerChancePercent < 0 ||
                definition.TriggerChancePercent > 100)
            {
                throw new InvalidOperationException(
                    "Weapon attack effect trigger chance " +
                    "must be between 0 and 100.");
            }
        }

        private static void ValidateEffectConfiguration(
            WeaponAttackEffectDefinition definition)
        {
            switch (definition.EffectType)
            {
                case WeaponAttackEffectType.None:
                    ValidateNone(definition);
                    break;

                case WeaponAttackEffectType.ApplyStatusEffect:
                    ValidateApplyStatusEffect(definition);
                    break;

                case WeaponAttackEffectType.LifeSteal:
                    ValidateLifeSteal(definition);
                    break;

                default:
                    throw new InvalidOperationException(
                        "Weapon attack effect type is unsupported.");
            }
        }

        private static void ValidateNone(
            WeaponAttackEffectDefinition definition)
        {
            if (definition.TriggerChancePercent != 0)
            {
                throw new InvalidOperationException(
                    "None weapon attack effect must have " +
                    "zero trigger chance.");
            }

            if (definition.StatusEffect != null)
            {
                throw new InvalidOperationException(
                    "None weapon attack effect cannot have " +
                    "a status effect.");
            }

            if (definition.LifeStealPercent != 0)
            {
                throw new InvalidOperationException(
                    "None weapon attack effect must have " +
                    "zero life steal.");
            }
        }

        private static void ValidateApplyStatusEffect(
            WeaponAttackEffectDefinition definition)
        {
            if (definition.StatusEffect == null)
            {
                throw new InvalidOperationException(
                    "Apply status effect weapon attack effect " +
                    "requires a status effect definition.");
            }

            if (definition.LifeStealPercent != 0)
            {
                throw new InvalidOperationException(
                    "Apply status effect weapon attack effect " +
                    "must have zero life steal.");
            }
        }

        private static void ValidateLifeSteal(
            WeaponAttackEffectDefinition definition)
        {
            if (definition.LifeStealPercent <= 0)
            {
                throw new InvalidOperationException(
                    "Life steal weapon attack effect requires " +
                    "a positive life steal percentage.");
            }

            if (definition.StatusEffect != null)
            {
                throw new InvalidOperationException(
                    "Life steal weapon attack effect cannot have " +
                    "a status effect.");
            }
        }
    }
}