using System;

namespace DiceBossArena.Game
{
    public readonly struct CharacterBuildSkill :
        IEquatable<CharacterBuildSkill>
    {
        public string SkillId { get; }
        public int Level { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(SkillId);

        public CharacterBuildSkill(
            string skillId,
            int level)
        {
            SkillId =
                skillId?.Trim() ?? string.Empty;

            Level =
                Math.Max(1, level);
        }

        public bool Equals(
            CharacterBuildSkill other)
        {
            return string.Equals(
                       SkillId,
                       other.SkillId,
                       StringComparison.Ordinal) &&
                   Level == other.Level;
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterBuildSkill other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                StringComparer.Ordinal.GetHashCode(
                    SkillId ?? string.Empty),
                Level);
        }
    }
}