using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public static class CharacterActionSetFactory
    {
        private const int ExpectedSkillCount = 5;

        public static CharacterActionSet Create(
            CharacterBuildSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            ValidateSkills(snapshot.Skills);

            return new CharacterActionSet(
                new[]
                {
                    CharacterActionContent
                        .CreateWeaponAttack(),

                    CharacterActionContent.CreateSkill(
                        CharacterActionSlot.BasicAttack,
                        snapshot.Skills[0]),

                    CharacterActionContent.CreateSkill(
                        CharacterActionSlot.SkillOne,
                        snapshot.Skills[1]),

                    CharacterActionContent.CreateSkill(
                        CharacterActionSlot.SkillTwo,
                        snapshot.Skills[2]),

                    CharacterActionContent.CreateSkill(
                        CharacterActionSlot.SkillThree,
                        snapshot.Skills[3]),

                    CharacterActionContent.CreateSkill(
                        CharacterActionSlot.SkillFour,
                        snapshot.Skills[4])
                });
        }

        private static void ValidateSkills(
            IReadOnlyList<CharacterBuildSkill> skills)
        {
            if (skills.Count != ExpectedSkillCount)
            {
                throw new ArgumentException(
                    "Character build must contain " +
                    "Basic Attack and exactly four " +
                    "selected skills.",
                    nameof(skills));
            }

            if (!string.Equals(
                    skills[0].SkillId,
                    CharacterSkillIds.BasicAttack,
                    StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    "The first character build skill " +
                    "must be Basic Attack.",
                    nameof(skills));
            }

            HashSet<string> skillIds =
            new HashSet<string>(
                StringComparer.Ordinal);

            for (int i = 0; i < skills.Count; i++)
            {
                if (!skills[i].IsValid)
                {
                    throw new ArgumentException(
                        $"Character build skill at index {i} " +
                        "is invalid.",
                        nameof(skills));
                }

                if (!skillIds.Add(skills[i].SkillId))
                {
                    throw new ArgumentException(
                        $"Character build contains duplicate " +
                        $"skill ID: {skills[i].SkillId}.",
                        nameof(skills));
                }
            }
        }
    }
}