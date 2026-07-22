public sealed class FightWeaponAttackActionRequest :
    IFightActionRequest
{
    public FightActionType ActionType =>
        FightActionType.WeaponAttack;

    public FightUnit Actor { get; }

    public FightUnit PrimaryTarget { get; }

    public FightWeaponAttackActionRequest(
        FightUnit actor,
        FightUnit primaryTarget)
    {
        Actor = actor;
        PrimaryTarget = primaryTarget;
    }
}