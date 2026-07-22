using System;
using DiceBossArena.Game;

public sealed class WeaponAttackDamageRoller
{
    private readonly IWeaponAttackRandomSource
        randomSource;

    public WeaponAttackDamageRoller(
        IWeaponAttackRandomSource randomSource)
    {
        this.randomSource =
            randomSource ??
            throw new ArgumentNullException(
                nameof(randomSource));
    }

    public int Roll(
        RolledWeaponAttackLine attackLine)
    {
        if (attackLine == null)
        {
            throw new ArgumentNullException(
                nameof(attackLine));
        }

        return randomSource.Next(
            attackLine.MinDamage,
            attackLine.MaxDamage + 1);
    }
}