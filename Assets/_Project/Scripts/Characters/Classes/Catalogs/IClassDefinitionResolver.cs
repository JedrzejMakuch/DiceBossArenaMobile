namespace DiceBossArena.Game
{
    public interface IClassDefinitionResolver
    {
        bool TryResolve(
            CharacterClassId classId,
            out ClassDefinition definition);
    }
}