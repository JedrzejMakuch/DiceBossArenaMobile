public sealed class FightEndTurnActionRequest :
    IFightActionRequest
{
    public FightActionType ActionType =>
        FightActionType.EndTurn;

    public FightUnit Actor { get; }

    public FightEndTurnActionRequest(
        FightUnit actor)
    {
        Actor = actor;
    }
}