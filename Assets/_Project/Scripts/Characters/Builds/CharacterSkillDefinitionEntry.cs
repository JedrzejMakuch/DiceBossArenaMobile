using System;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class
        CharacterSkillDefinitionEntry
    {
        [SerializeField]
        private string skillId;

        [SerializeField, Min(1)]
        private int level = 1;

        public string SkillId =>
            skillId;

        public int Level =>
            level;

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(
                skillId) &&
            level >= 1;

        public CharacterBuildSkill CreateBuildSkill()
        {
            return new CharacterBuildSkill(
                skillId,
                level);
        }

#if UNITY_EDITOR
        public CharacterSkillDefinitionEntry(
            string newSkillId,
            int newLevel = 1)
        {
            skillId =
                Normalize(newSkillId);

            level =
                Mathf.Max(
                    1,
                    newLevel);
        }
#endif

        private static string Normalize(
            string value)
        {
            return value?.Trim() ??
                   string.Empty;
        }
    }
}