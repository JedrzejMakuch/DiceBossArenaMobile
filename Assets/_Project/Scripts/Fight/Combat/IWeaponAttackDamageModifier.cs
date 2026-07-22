using DiceBossArena.Game;

public interface IWeaponAttackDamageModifier
{
    int Resolve(
        WeaponAttackDamageLineResult damageLine,
        FightUnitStats targetStats,
        int damage);
}