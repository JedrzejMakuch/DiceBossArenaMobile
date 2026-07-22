using System;
using System.Collections.Generic;
using DiceBossArena.Game;

public sealed class WeaponAttackEffectsProfileResolver
{
    private readonly WeaponAttackEffectLineResolver
        lineResolver;

    public WeaponAttackEffectsProfileResolver(
        WeaponAttackEffectLineResolver newLineResolver)
    {
        lineResolver =
            newLineResolver ??
            throw new ArgumentNullException(
                nameof(newLineResolver));
    }

    public IReadOnlyList<WeaponAttackEffectLineResult> Resolve(
        RolledWeaponProfile weaponProfile,
        IReadOnlyList<WeaponAttackDamageLineResult>
            damageLines)
    {
        if (weaponProfile == null)
        {
            throw new ArgumentNullException(
                nameof(weaponProfile));
        }

        if (damageLines == null)
        {
            throw new ArgumentNullException(
                nameof(damageLines));
        }

        if (weaponProfile.Lines.Count !=
            damageLines.Count)
        {
            throw new InvalidOperationException(
                "Weapon profile line count must match " +
                "damage line count.");
        }

        Dictionary<WeaponAttackLineId,
            WeaponAttackDamageLineResult>
            damageLinesById =
                CreateDamageLinesById(
                    damageLines);

        List<WeaponAttackEffectLineResult> results =
            new List<WeaponAttackEffectLineResult>(
                weaponProfile.Lines.Count);

        for (int index = 0;
             index < weaponProfile.Lines.Count;
             index++)
        {
            RolledWeaponAttackLine attackLine =
                weaponProfile.Lines[index];

            if (!damageLinesById.TryGetValue(
                    attackLine.LineId,
                    out WeaponAttackDamageLineResult
                        damageLine))
            {
                throw new InvalidOperationException(
                    "No matching damage line was found " +
                    "for weapon attack line.");
            }

            results.Add(
                lineResolver.Resolve(
                    attackLine,
                    damageLine));
        }

        return results;
    }

    private static Dictionary<
        WeaponAttackLineId,
        WeaponAttackDamageLineResult>
        CreateDamageLinesById(
            IReadOnlyList<WeaponAttackDamageLineResult>
                damageLines)
    {
        Dictionary<WeaponAttackLineId,
            WeaponAttackDamageLineResult>
            damageLinesById =
                new Dictionary<
                    WeaponAttackLineId,
                    WeaponAttackDamageLineResult>();

        for (int index = 0;
             index < damageLines.Count;
             index++)
        {
            WeaponAttackDamageLineResult damageLine =
                damageLines[index];

            if (damageLine == null)
            {
                throw new InvalidOperationException(
                    "Damage lines cannot contain null.");
            }

            if (!damageLinesById.TryAdd(
                    damageLine.LineId,
                    damageLine))
            {
                throw new InvalidOperationException(
                    "Damage lines cannot contain " +
                    "duplicate line IDs.");
            }
        }

        return damageLinesById;
    }
}