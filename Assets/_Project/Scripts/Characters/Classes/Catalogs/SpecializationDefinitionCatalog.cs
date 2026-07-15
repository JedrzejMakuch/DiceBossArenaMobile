using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        SpecializationDefinitionCatalog :
            ISpecializationDefinitionResolver
    {
        private readonly Dictionary<
            CharacterSpecializationId,
            SpecializationDefinition>
            definitionsById;

        public SpecializationDefinitionCatalog(
            IReadOnlyList<
                SpecializationDefinition> definitions)
        {
            definitionsById =
                new Dictionary<
                    CharacterSpecializationId,
                    SpecializationDefinition>();

            if (definitions == null)
            {
                return;
            }

            for (int i = 0;
                 i < definitions.Count;
                 i++)
            {
                SpecializationDefinition definition =
                    definitions[i];

                if (definition == null)
                {
                    throw new ArgumentException(
                        "Catalog contains a null " +
                        "specialization definition.",
                        nameof(definitions));
                }

                CharacterSpecializationId
                    specializationId =
                        definition.SpecializationId;

                if (!specializationId.IsValid)
                {
                    throw new ArgumentException(
                        "Catalog contains a specialization " +
                        "with an invalid id.",
                        nameof(definitions));
                }

                if (!definition.RequiredClassId.IsValid)
                {
                    throw new ArgumentException(
                        $"Specialization " +
                        $"{specializationId.Value} has an " +
                        $"invalid required class id.",
                        nameof(definitions));
                }

                if (!definitionsById.TryAdd(
                        specializationId,
                        definition))
                {
                    throw new ArgumentException(
                        $"Catalog contains duplicate " +
                        $"specialization id: " +
                        $"{specializationId.Value}.",
                        nameof(definitions));
                }
            }
        }

        public bool TryResolve(
            CharacterSpecializationId
                specializationId,
            out SpecializationDefinition definition)
        {
            definition = null;

            if (!specializationId.IsValid)
            {
                return false;
            }

            return definitionsById.TryGetValue(
                specializationId,
                out definition);
        }
    }
}