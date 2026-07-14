using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Tests.Fixtures
{
    public static class TestSkillFactory
    {
        public static SkillDefinition Create(
            string skillId = "test_skill",
            string displayName = "Test Skill",
            string description = "Test skill description.",
            SkillTargetType targetType = SkillTargetType.SingleEnemy,
            SkillRangeShape rangeShape = SkillRangeShape.Manhattan,
            int minRange = 0,
            int maxRange = 1,
            int actionPointCost = 1,
            int movementPointCost = 0,
            int maxLevel = 1,
            float powerMultiplier = 1f,
            int flatValueBonus = 0,
            int cooldown = 0)
        {
            SkillDefinition skill =
                ScriptableObject.CreateInstance<SkillDefinition>();

            skill.name = displayName;

            var serializedData = new SerializedSkillData
            {
                levels = new List<SerializedSkillLevelData>(),
                skillId = skillId,
                displayName = displayName,
                description = description,
                targetType = targetType,
                rangeShape = rangeShape,
                minRange = Mathf.Max(0, minRange),
                maxRange = Mathf.Max(minRange, maxRange),
                actionPointCost = Mathf.Max(0, actionPointCost),
                movementPointCost = Mathf.Max(0, movementPointCost),
                maxLevel = Mathf.Max(1, maxLevel)
            };

            for (int i = 0; i < serializedData.maxLevel; i++)
            {
                serializedData.levels.Add(
                    new SerializedSkillLevelData
                    {
                        powerMultiplier = Mathf.Max(0f, powerMultiplier),
                        flatValueBonus = flatValueBonus,
                        cooldown = Mathf.Max(0, cooldown)
                    });
            }

            string json = JsonUtility.ToJson(serializedData);
            JsonUtility.FromJsonOverwrite(json, skill);

            return skill;
        }

        [Serializable]
        private sealed class SerializedSkillData
        {
            public string skillId;
            public string displayName;
            public string description;
            public SkillTargetType targetType;
            public SkillRangeShape rangeShape;
            public int minRange;
            public int maxRange;
            public int actionPointCost;
            public int movementPointCost;
            public int maxLevel;
            public List<SerializedSkillLevelData> levels;
        }

        [Serializable]
        private sealed class SerializedSkillLevelData
        {
            public float powerMultiplier;
            public int flatValueBonus;
            public int cooldown;
        }
    }
}