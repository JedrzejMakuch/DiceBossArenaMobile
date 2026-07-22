using System;
using DiceBossArena.Game;

public sealed class WeaponAttackLifeStealApplier
{
    private readonly WeaponAttackLifeStealCalculator
        calculator;

    public WeaponAttackLifeStealApplier(
        WeaponAttackLifeStealCalculator calculator)
    {
        this.calculator =
            calculator ??
            throw new ArgumentNullException(
                nameof(calculator));
    }

    public int Apply(
        WeaponAttackRollResult attackResult)
    {
        if (attackResult == null)
        {
            throw new ArgumentNullException(
                nameof(attackResult));
        }

        FightUnit attacker =
            attackResult.Attacker;

        if (attacker == null ||
            !attacker.IsAlive)
        {
            return 0;
        }

        int totalHealing = 0;

        foreach (WeaponAttackEffectLineResult effectLine
                 in attackResult.EffectLines)
        {
            foreach (WeaponAttackEffectTriggerResult effectResult
                     in effectLine.EffectResults)
            {
                if (effectResult.Definition.EffectType !=
                    WeaponAttackEffectType.LifeSteal)
                {
                    continue;
                }

                totalHealing +=
                    calculator.Calculate(
                        effectResult,
                        effectLine.DamageLine);
            }
        }

        if (totalHealing <= 0)
        {
            return 0;
        }

        int before =
            attacker.CurrentHealth;

        attacker.Heal(
            totalHealing);

        return attacker.CurrentHealth - before;
    }
}