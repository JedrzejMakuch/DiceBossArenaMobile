using DiceBossArena.Game;

public sealed class WeaponAttackResistanceStatResolver
{
    public bool TryResolve(
        WeaponAttackElement element,
        out FightStatType resistanceStatType)
    {
        switch (element)
        {
            case WeaponAttackElement.Fire:
                resistanceStatType =
                    FightStatType.FireResistance;

                return true;

            case WeaponAttackElement.Water:
                resistanceStatType =
                    FightStatType.WaterResistance;

                return true;

            case WeaponAttackElement.Earth:
                resistanceStatType =
                    FightStatType.EarthResistance;

                return true;

            case WeaponAttackElement.Air:
                resistanceStatType =
                    FightStatType.AirResistance;

                return true;

            case WeaponAttackElement.Neutral:
                resistanceStatType = default;
                return false;

            default:
                resistanceStatType = default;
                return false;
        }
    }
}