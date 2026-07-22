using System;
using System.Collections.Generic;
using DiceBossArena.Game;

public sealed class WeaponAttackEffectsTriggerResolver
{
    private readonly WeaponAttackEffectTriggerResolver effectResolver;

    public WeaponAttackEffectsTriggerResolver(
        WeaponAttackEffectTriggerResolver newEffectResolver)
    {
        effectResolver =
            newEffectResolver ??
            throw new ArgumentNullException(
                nameof(newEffectResolver));
    }

    public IReadOnlyList<WeaponAttackEffectTriggerResult> Resolve(
        IReadOnlyList<WeaponAttackEffectDefinition> definitions)
    {
        if (definitions == null)
        {
            return Array.Empty<
                WeaponAttackEffectTriggerResult>();
        }

        List<WeaponAttackEffectTriggerResult> results =
            new List<WeaponAttackEffectTriggerResult>(
                definitions.Count);

        for (int index = 0;
             index < definitions.Count;
             index++)
        {
            WeaponAttackEffectTriggerResult result =
                effectResolver.Resolve(
                    definitions[index]);

            results.Add(result);
        }

        return results;
    }
}