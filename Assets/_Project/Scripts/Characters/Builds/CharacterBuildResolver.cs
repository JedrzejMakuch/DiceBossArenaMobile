using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class CharacterBuildResolver
    {
        private readonly ISkillDefinitionResolver
            skillResolver;

        public CharacterBuildResolver(
            ISkillDefinitionResolver skillResolver)
        {
            this.skillResolver =
                skillResolver ??
                throw new ArgumentNullException(
                    nameof(skillResolver));
        }

        public ResolvedCharacterBuild Resolve(
            CharacterBuildSnapshot snapshot)
        {
            if (snapshot == null)
            {
                throw new ArgumentNullException(
                    nameof(snapshot));
            }

            List<UnitStartingSkill> resolvedSkills =
                ResolveSkills(snapshot.Skills);

            return new ResolvedCharacterBuild(
                snapshot.ClassId,
                snapshot.SpecializationId,
                resolvedSkills,
                snapshot.StatModifiers,
                snapshot.EquipmentLoadout,
                snapshot.PassiveIds);
        }

        private List<UnitStartingSkill> ResolveSkills(
            IReadOnlyList<CharacterBuildSkill> skills)
        {
            List<UnitStartingSkill> result =
                new();

            for (int i = 0; i < skills.Count; i++)
            {
                CharacterBuildSkill buildSkill =
                    skills[i];

                if (!skillResolver.TryResolve(
                        buildSkill.SkillId,
                        out SkillDefinition definition))
                {
                    throw new InvalidOperationException(
                        $"Could not resolve skill id: " +
                        $"{buildSkill.SkillId}.");
                }

                result.Add(
                    new UnitStartingSkill(
                        definition,
                        buildSkill.Level));
            }

            return result;
        }
    }
}