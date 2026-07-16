namespace DiceBossArena.Game
{
    public interface IItemDefinitionResolver
    {
        bool TryResolve(
            CharacterItemId itemId,
            out ItemDefinition definition);
    }
}