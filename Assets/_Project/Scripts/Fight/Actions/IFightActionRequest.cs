public interface IFightActionRequest
{
    FightActionType ActionType { get; }

    FightUnit Actor { get; }
}