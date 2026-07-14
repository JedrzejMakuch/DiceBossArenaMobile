using System;
using System.Collections.Generic;

public sealed class SkillDefinitionCatalog :
    ISkillDefinitionResolver
{
    private readonly Dictionary<string, SkillDefinition>
        definitionsById;

    public SkillDefinitionCatalog(
        IReadOnlyList<SkillDefinition> definitions)
    {
        definitionsById =
            new Dictionary<string, SkillDefinition>(
                StringComparer.Ordinal);

        if (definitions == null)
        {
            return;
        }

        for (int i = 0; i < definitions.Count; i++)
        {
            SkillDefinition definition =
                definitions[i];

            if (definition == null)
            {
                throw new ArgumentException(
                    "Catalog contains a null skill definition.",
                    nameof(definitions));
            }

            string skillId =
                definition.SkillId?.Trim() ??
                string.Empty;

            if (string.IsNullOrWhiteSpace(skillId))
            {
                throw new ArgumentException(
                    "Catalog contains a skill with an invalid id.",
                    nameof(definitions));
            }

            if (!definitionsById.TryAdd(
                    skillId,
                    definition))
            {
                throw new ArgumentException(
                    $"Catalog contains duplicate skill id: " +
                    $"{skillId}.",
                    nameof(definitions));
            }
        }
    }

    public bool TryResolve(
        string skillId,
        out SkillDefinition definition)
    {
        definition = null;

        if (string.IsNullOrWhiteSpace(skillId))
        {
            return false;
        }

        return definitionsById.TryGetValue(
            skillId.Trim(),
            out definition);
    }
}