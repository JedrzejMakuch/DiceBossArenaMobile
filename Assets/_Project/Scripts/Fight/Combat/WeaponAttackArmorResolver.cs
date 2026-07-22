using DiceBossArena.Game;

public sealed class WeaponAttackArmorResolver
    : IWeaponAttackDamageModifier
{
    private readonly WeaponAttackArmorCalculator
        armorCalculator =
            new();

    public int Resolve(
        WeaponAttackDamageLineResult damageLine,
        FightUnitStats targetStats,
        int damage)
    {
        if (damageLine == null)
        {
            return 0;
        }

        if (damage <= 0)
        {
            return 0;
        }

        if (targetStats == null)
        {
            return damage;
        }

        int armor =
            targetStats.GetFinalValue(
                FightStatType.Armor);

        return armorCalculator.Calculate(
            damage,
            armor);
    }
}