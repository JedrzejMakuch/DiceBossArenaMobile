using System;
using System.Collections.Generic;
using DiceBossArena.Game;

public sealed class WeaponAttackEffectLineResult
{
    private readonly List<
        WeaponAttackEffectTriggerResult> effectResults;

    public WeaponAttackDamageLineResult DamageLine { get; }

    public IReadOnlyList<WeaponAttackEffectTriggerResult>
        EffectResults =>
            effectResults;

    public WeaponAttackEffectLineResult(
        WeaponAttackDamageLineResult damageLine,
        IEnumerable<WeaponAttackEffectTriggerResult>
            newEffectResults)
    {
        DamageLine =
            damageLine ??
            throw new ArgumentNullException(
                nameof(damageLine));

        effectResults =
            newEffectResults == null
                ? new List<
                    WeaponAttackEffectTriggerResult>()
                : new List<
                    WeaponAttackEffectTriggerResult>(
                    newEffectResults);
    }
}