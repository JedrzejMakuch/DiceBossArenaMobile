public sealed class FightMoveActionRequest :
    IFightActionRequest
{
    public FightActionType ActionType =>
        FightActionType.Move;

    public FightUnit Actor { get; }

    public FightGridTile TargetTile { get; }

    public FightMoveActionRequest(
        FightUnit actor,
        FightGridTile targetTile)
    {
        Actor = actor;
        TargetTile = targetTile;
    }
}