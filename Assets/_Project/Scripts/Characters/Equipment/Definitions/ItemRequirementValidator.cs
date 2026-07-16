using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class ItemRequirementValidator
    {
        public bool MeetsRequirements(
            ItemDefinition definition,
            CharacterClassId classId,
            CharacterSpecializationId specializationId)
        {
            if (definition == null)
            {
                return false;
            }

            if (!MatchesRequirement(
                    definition.RequiredClassIds,
                    classId.Value))
            {
                return false;
            }

            if (!MatchesRequirement(
                    definition.RequiredSpecializationIds,
                    specializationId.Value))
            {
                return false;
            }

            return true;
        }

        private static bool MatchesRequirement(
            IReadOnlyList<string> requiredIds,
            string currentId)
        {
            if (requiredIds == null ||
                requiredIds.Count == 0)
            {
                return true;
            }

            if (string.IsNullOrWhiteSpace(currentId))
            {
                return false;
            }

            for (int i = 0;
                 i < requiredIds.Count;
                 i++)
            {
                if (string.Equals(
                        requiredIds[i],
                        currentId,
                        StringComparison.Ordinal))
                {
                    return true;
                }
            }

            return false;
        }
    }
}