using DiceBossArena.Game;

public sealed class WeaponAttackDamageLineResult
{
    public WeaponAttackLineId LineId { get; }

    public WeaponAttackElement Element { get; }

    public int Damage { get; }
    public bool IsTrueDamage { get; }

    public WeaponAttackDamageLineResult(
        WeaponAttackLineId lineId,
        WeaponAttackElement element,
        int damage,
        bool isTrueDamage = false)
    {
        LineId = lineId;
        Element = element;
        Damage = damage;
        IsTrueDamage = isTrueDamage;
    }
}