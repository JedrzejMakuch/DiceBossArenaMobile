using System;

namespace DiceBossArena.Game
{
    public static class LocalizationKeys
    {
        public static LocalizationKey UnitName(
            FightUnitDefinitionId unitId)
        {
            return Create(
                "units",
                unitId.Value,
                "name");
        }

        public static LocalizationKey UnitDescription(
            FightUnitDefinitionId unitId)
        {
            return Create(
                "units",
                unitId.Value,
                "description");
        }

        public static LocalizationKey SkillName(
            string skillId)
        {
            return Create(
                "skills",
                skillId,
                "name");
        }

        public static LocalizationKey SkillDescription(
            string skillId)
        {
            return Create(
                "skills",
                skillId,
                "description");
        }

        private static LocalizationKey Create(
            string category,
            string contentId,
            string field)
        {
            string normalizedId =
                contentId?.Trim() ??
                string.Empty;

            if (string.IsNullOrWhiteSpace(normalizedId))
            {
                return new LocalizationKey(
                    string.Empty);
            }

            return new LocalizationKey(
                $"{category}.{normalizedId}.{field}");
        }
    }
}