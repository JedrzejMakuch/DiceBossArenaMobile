using System;
using System.Collections.Generic;
using DiceBossArena.Game;

public sealed class WeaponAttackProfileDamageRoller
{
    private readonly WeaponAttackDamageRoller
        damageRoller;

    public WeaponAttackProfileDamageRoller(
        WeaponAttackDamageRoller damageRoller)
    {
        this.damageRoller =
            damageRoller ??
            throw new ArgumentNullException(
                nameof(damageRoller));
    }

    public IReadOnlyList<WeaponAttackDamageLineResult>
        Roll(RolledWeaponProfile profile)
    {
        if (profile == null)
        {
            throw new ArgumentNullException(
                nameof(profile));
        }

        List<WeaponAttackDamageLineResult> results =
            new List<WeaponAttackDamageLineResult>(
                profile.Lines.Count);

        foreach (RolledWeaponAttackLine line
                 in profile.Lines)
        {
            int damage =
                damageRoller.Roll(line);

            results.Add(
                new WeaponAttackDamageLineResult(
                    line.LineId,
                    line.Element,
                    damage));
        }

        return results;
    }
}