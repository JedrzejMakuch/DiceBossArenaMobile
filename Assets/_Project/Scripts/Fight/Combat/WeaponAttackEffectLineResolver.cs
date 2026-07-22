using System;
using DiceBossArena.Game;

public sealed class WeaponAttackEffectLineResolver
{
    private readonly WeaponAttackEffectsTriggerResolver
        effectsTriggerResolver;

    public WeaponAttackEffectLineResolver(
        WeaponAttackEffectsTriggerResolver
            newEffectsTriggerResolver)
    {
        effectsTriggerResolver =
            newEffectsTriggerResolver ??
            throw new ArgumentNullException(
                nameof(newEffectsTriggerResolver));
    }

    public WeaponAttackEffectLineResult Resolve(
        RolledWeaponAttackLine attackLine,
        WeaponAttackDamageLineResult damageLine)
    {
        if (attackLine == null)
        {
            throw new ArgumentNullException(
                nameof(attackLine));
        }

        if (damageLine == null)
        {
            throw new ArgumentNullException(
                nameof(damageLine));
        }

        if (attackLine.LineId != damageLine.LineId)
        {
            throw new InvalidOperationException(
                "Weapon attack line and damage line " +
                "must have the same line ID.");
        }

        return new WeaponAttackEffectLineResult(
            damageLine,
            effectsTriggerResolver.Resolve(
                attackLine.Effects));
    }
}