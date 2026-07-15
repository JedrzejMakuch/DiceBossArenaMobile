using System;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class SkillReplacementDefinition
    {
        [SerializeField]
        private string sourceSkillId;

        [SerializeField]
        private string replacementSkillId;

        public string SourceSkillId =>
            sourceSkillId;

        public string ReplacementSkillId =>
            replacementSkillId;

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(
                sourceSkillId) &&
            !string.IsNullOrWhiteSpace(
                replacementSkillId) &&
            !string.Equals(
                sourceSkillId,
                replacementSkillId,
                StringComparison.Ordinal);

        public SkillReplacementDefinition(
            string sourceSkillId,
            string replacementSkillId)
        {
            this.sourceSkillId =
                Normalize(sourceSkillId);

            this.replacementSkillId =
                Normalize(replacementSkillId);
        }

        private static string Normalize(
            string value)
        {
            return value?.Trim() ??
                   string.Empty;
        }
    }
}