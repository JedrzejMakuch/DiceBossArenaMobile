using System.Collections.Generic;
using DiceBossArena.Game;

public sealed class WeaponAttackDamageLineResolver
{
    private readonly IReadOnlyList<IWeaponAttackDamageModifier>
        modifiers;

    public WeaponAttackDamageLineResolver()
    {
        modifiers =
            new IWeaponAttackDamageModifier[]
            {
                new WeaponAttackResistanceResolver(),
                new WeaponAttackArmorResolver()
            };
    }

    public int Resolve(
        WeaponAttackDamageLineResult damageLine,
        FightUnitStats targetStats)
    {
        if (damageLine == null)
        {
            return 0;
        }

        if (damageLine.Damage <= 0)
        {
            return 0;
        }

        if (damageLine.IsTrueDamage)
        {
            return damageLine.Damage;
        }

        int damage =
            damageLine.Damage;

        for (int i = 0;
             i < modifiers.Count;
             i++)
        {
            damage =
                modifiers[i].Resolve(
                    damageLine,
                    targetStats,
                    damage);

            if (damage <= 0)
            {
                return 0;
            }
        }

        return damage;
    }
}