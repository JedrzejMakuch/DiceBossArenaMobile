public interface ISkillDefinitionResolver
{
    bool TryResolve(
        string skillId,
        out SkillDefinition definition);
}