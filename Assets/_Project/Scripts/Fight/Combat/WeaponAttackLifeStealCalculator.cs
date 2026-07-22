using System;
using DiceBossArena.Game;

public sealed class WeaponAttackLifeStealCalculator
{
    public int Calculate(
        WeaponAttackEffectTriggerResult effectResult,
        WeaponAttackDamageLineResult damageLine)
    {
        if (effectResult == null)
        {
            throw new ArgumentNullException(
                nameof(effectResult));
        }

        if (damageLine == null)
        {
            throw new ArgumentNullException(
                nameof(damageLine));
        }

        if (!effectResult.IsTriggered)
        {
            return 0;
        }

        WeaponAttackEffectDefinition definition =
            effectResult.Definition;

        if (definition == null)
        {
            throw new InvalidOperationException(
                "Triggered weapon attack effect " +
                "must contain a definition.");
        }

        if (definition.EffectType !=
            WeaponAttackEffectType.LifeSteal)
        {
            throw new InvalidOperationException(
                "Weapon attack effect must be " +
                "a Life Steal effect.");
        }

        if (damageLine.Damage <= 0)
        {
            return 0;
        }

        return damageLine.Damage *
               definition.LifeStealPercent /
               100;
    }
}