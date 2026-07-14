using DiceBossArena.Game;

public interface IFightUnitDefinitionResolver
{
    bool TryResolve(
        FightUnitDefinitionId unitId,
        out FightUnitDefinition definition);
}