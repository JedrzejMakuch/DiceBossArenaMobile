namespace DiceBossArena.Game
{
    public interface
        IEquipmentBaseTypeDefinitionResolver
    {
        bool TryResolve(
            EquipmentBaseTypeId baseTypeId,
            out EquipmentBaseTypeDefinition definition);
    }
}