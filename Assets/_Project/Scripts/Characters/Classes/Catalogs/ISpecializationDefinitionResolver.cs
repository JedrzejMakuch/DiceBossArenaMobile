namespace DiceBossArena.Game
{
    public interface
        ISpecializationDefinitionResolver
    {
        bool TryResolve(
            CharacterSpecializationId
                specializationId,
            out SpecializationDefinition definition);
    }
}