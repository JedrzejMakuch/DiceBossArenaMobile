using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class CharacterBuildResolver
    {
        private readonly ISkillDefinitionResolver
            skillResolver;

        private readonly CharacterBuildComposer
            buildComposer;

        public CharacterBuildResolver(
            ISkillDefinitionResolver skillResolver,
            CharacterBuildComposer buildComposer = null)
        {
            this.skillResolver =
                skillResolver ??
                throw new ArgumentNullException(
                    nameof(skillResolver));

            this.buildComposer =
                buildComposer ??
                new CharacterBuildComposer();
        }

        public ResolvedCharacterBuild Resolve(
            CharacterBuildCompositionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(
                    nameof(request));
            }

            CharacterBuildSnapshot snapshot =
                buildComposer.Compose(
                    request);

            return Resolve(
                snapshot);
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

                if (buildSkill.Level >
                    definition.MaxLevel)
                {
                    throw new InvalidOperationException(
                        $"Skill {buildSkill.SkillId} " +
                        $"has level {buildSkill.Level}, " +
                        $"but its maximum level is " +
                        $"{definition.MaxLevel}.");
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