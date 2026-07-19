using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixDefinitionCatalog
    {
        private readonly Dictionary<
            EquipmentAffixId,
            EquipmentAffixDefinition> definitions;

        public IReadOnlyCollection<EquipmentAffixDefinition>
    Definitions =>
        definitions.Values;

        public EquipmentAffixDefinitionCatalog(
            IEnumerable<EquipmentAffixDefinition>
                newDefinitions)
        {
            if (newDefinitions == null)
            {
                throw new ArgumentNullException(
                    nameof(newDefinitions));
            }

            definitions =
                new Dictionary<
                    EquipmentAffixId,
                    EquipmentAffixDefinition>();

            foreach (EquipmentAffixDefinition definition
                     in newDefinitions)
            {
                if (definition == null)
                {
                    throw new ArgumentException(
                        "Equipment affix definition collection cannot contain null.",
                        nameof(newDefinitions));
                }

                if (!definitions.TryAdd(
                        definition.Id,
                        definition))
                {
                    throw new ArgumentException(
                        "Equipment affix definition IDs must be unique.",
                        nameof(newDefinitions));
                }
            }
        }

        public EquipmentAffixDefinition Get(
            EquipmentAffixId affixId)
        {
            if (string.IsNullOrWhiteSpace(
                    affixId.Value))
            {
                throw new ArgumentException(
                    "Equipment affix ID must be valid.",
                    nameof(affixId));
            }

            if (!definitions.TryGetValue(
                    affixId,
                    out EquipmentAffixDefinition definition))
            {
                throw new KeyNotFoundException(
                    $"Equipment affix definition '{affixId}' was not found.");
            }

            return definition;
        }
    }
}