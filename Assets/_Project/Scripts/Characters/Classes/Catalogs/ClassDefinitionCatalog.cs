using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class ClassDefinitionCatalog :
        IClassDefinitionResolver
    {
        private readonly Dictionary<
            CharacterClassId,
            ClassDefinition> definitionsById;

        public ClassDefinitionCatalog(
            IReadOnlyList<ClassDefinition> definitions)
        {
            definitionsById =
                new Dictionary<
                    CharacterClassId,
                    ClassDefinition>();

            if (definitions == null)
            {
                return;
            }

            for (int i = 0;
                 i < definitions.Count;
                 i++)
            {
                ClassDefinition definition =
                    definitions[i];

                if (definition == null)
                {
                    throw new ArgumentException(
                        "Catalog contains a null " +
                        "class definition.",
                        nameof(definitions));
                }

                CharacterClassId classId =
                    definition.ClassId;

                if (!classId.IsValid)
                {
                    throw new ArgumentException(
                        "Catalog contains a class " +
                        "with an invalid id.",
                        nameof(definitions));
                }

                if (!definitionsById.TryAdd(
                        classId,
                        definition))
                {
                    throw new ArgumentException(
                        $"Catalog contains duplicate " +
                        $"class id: {classId.Value}.",
                        nameof(definitions));
                }
            }
        }

        public bool TryResolve(
            CharacterClassId classId,
            out ClassDefinition definition)
        {
            definition = null;

            if (!classId.IsValid)
            {
                return false;
            }

            return definitionsById.TryGetValue(
                classId,
                out definition);
        }
    }
}