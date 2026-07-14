using System;
using System.Collections.Generic;
using DiceBossArena.Game;

public sealed class FightUnitDefinitionCatalog :
    IFightUnitDefinitionResolver
{
    private readonly Dictionary<
        FightUnitDefinitionId,
        FightUnitDefinition> definitionsById;

    public FightUnitDefinitionCatalog(
        IReadOnlyList<FightUnitDefinition> definitions)
    {
        definitionsById =
            new Dictionary<
                FightUnitDefinitionId,
                FightUnitDefinition>();

        if (definitions == null)
        {
            return;
        }

        for (int i = 0; i < definitions.Count; i++)
        {
            FightUnitDefinition definition =
                definitions[i];

            if (definition == null)
            {
                throw new ArgumentException(
                    "Catalog contains a null unit definition.",
                    nameof(definitions));
            }

            FightUnitDefinitionId unitId =
                definition.UnitId;

            if (!unitId.IsValid)
            {
                throw new ArgumentException(
                    "Catalog contains a unit with an invalid id.",
                    nameof(definitions));
            }

            if (!definitionsById.TryAdd(
                    unitId,
                    definition))
            {
                throw new ArgumentException(
                    $"Catalog contains duplicate unit id: " +
                    $"{unitId.Value}.",
                    nameof(definitions));
            }
        }
    }

    public bool TryResolve(
        FightUnitDefinitionId unitId,
        out FightUnitDefinition definition)
    {
        definition = null;

        if (!unitId.IsValid)
        {
            return false;
        }

        return definitionsById.TryGetValue(
            unitId,
            out definition);
    }
}