using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentBaseTypeDefinitionCatalog :
        IEquipmentBaseTypeDefinitionResolver
    {
        private readonly Dictionary<
            EquipmentBaseTypeId,
            EquipmentBaseTypeDefinition>
            definitionsById =
                new Dictionary<
                    EquipmentBaseTypeId,
                    EquipmentBaseTypeDefinition>();

        public EquipmentBaseTypeDefinitionCatalog(
            IEnumerable<EquipmentBaseTypeDefinition>
                definitions)
        {
            if (definitions == null)
            {
                return;
            }

            foreach (
                EquipmentBaseTypeDefinition definition
                in definitions)
            {
                if (definition == null)
                {
                    throw new ArgumentException(
                        "Equipment base type catalog " +
                        "cannot contain null definitions.",
                        nameof(definitions));
                }

                EquipmentBaseTypeId baseTypeId =
                    definition.BaseTypeId;

                if (!baseTypeId.IsValid)
                {
                    throw new InvalidOperationException(
                        "Equipment base type definition " +
                        "must contain a valid id.");
                }

                if (!definitionsById.TryAdd(
                        baseTypeId,
                        definition))
                {
                    throw new InvalidOperationException(
                        $"Duplicate equipment base type id: " +
                        $"'{baseTypeId}'.");
                }
            }
        }

        public bool TryResolve(
            EquipmentBaseTypeId baseTypeId,
            out EquipmentBaseTypeDefinition definition)
        {
            if (!baseTypeId.IsValid)
            {
                definition = null;
                return false;
            }

            return definitionsById.TryGetValue(
                baseTypeId,
                out definition);
        }

        public EquipmentBaseTypeDefinition Get(
            EquipmentBaseTypeId baseTypeId)
        {
            if (!baseTypeId.IsValid)
            {
                throw new ArgumentException(
                    "Equipment base type id must be valid.",
                    nameof(baseTypeId));
            }

            if (!definitionsById.TryGetValue(
                    baseTypeId,
                    out EquipmentBaseTypeDefinition
                        definition))
            {
                throw new KeyNotFoundException(
                    $"Unknown equipment base type id: " +
                    $"'{baseTypeId}'.");
            }

            return definition;
        }
    }
}