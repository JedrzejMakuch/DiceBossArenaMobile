using DiceBossArena.Game;

public sealed class WeaponAttackDamageLineResult
{
    public WeaponAttackLineId LineId { get; }

    public WeaponAttackElement Element { get; }

    public int Damage { get; }

    public WeaponAttackDamageLineResult(
        WeaponAttackLineId lineId,
        WeaponAttackElement element,
        int damage)
    {
        LineId = lineId;
        Element = element;
        Damage = damage;
    }
}