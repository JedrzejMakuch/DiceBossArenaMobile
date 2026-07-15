using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Game
{
    [CreateAssetMenu(
        fileName = "Specialization_",
        menuName =
            "Dice Boss Arena/Characters/" +
            "Specialization Definition")]
    public sealed class SpecializationDefinition :
        ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        private string specializationId;

        [SerializeField]
        private string requiredClassId;

        [SerializeField]
        private string nameLocalizationKey;

        [SerializeField]
        private string descriptionLocalizationKey;

        [Header("Presentation")]
        [SerializeField]
        private string displayName;

        [SerializeField, TextArea]
        private string description;

        [SerializeField]
        private Sprite icon;

        [Header("Build")]
        [SerializeField]
        private List<CharacterStatModifierDefinition>
            statModifiers = new();

        [SerializeField]
        private List<CharacterSkillDefinitionEntry>
           startingSkills = new();

        [SerializeField]
        private List<string>
            skillPoolIds = new();

        [SerializeField]
        private List<CharacterPassiveDefinitionEntry>
           passives = new();

        [SerializeField]
        private List<SkillReplacementDefinition>
            skillReplacements = new();

        public CharacterSpecializationId
            SpecializationId =>
                new CharacterSpecializationId(
                    specializationId);

        public CharacterClassId RequiredClassId =>
            new CharacterClassId(
                requiredClassId);

        public LocalizationKey NameLocalizationKey =>
            new LocalizationKey(
                nameLocalizationKey);

        public LocalizationKey
            DescriptionLocalizationKey =>
                new LocalizationKey(
                    descriptionLocalizationKey);

        public string DisplayName =>
            displayName;

        public string Description =>
            description;

        public Sprite Icon =>
            icon;

        public IReadOnlyList<
        CharacterStatModifierDefinition>
        StatModifiers =>
            statModifiers;

        public IReadOnlyList<
           CharacterSkillDefinitionEntry>
           StartingSkills =>
               startingSkills;

        public IReadOnlyList<string>
            SkillPoolIds =>
                skillPoolIds;

        public IReadOnlyList<
    CharacterPassiveDefinitionEntry>
    Passives =>
        passives;

        public IReadOnlyList<
           SkillReplacementDefinition>
           SkillReplacements =>
               skillReplacements;

        public bool IsCompatibleWith(
            CharacterClassId classId)
        {
            return RequiredClassId.IsValid &&
                   classId.IsValid &&
                   RequiredClassId.Equals(
                       classId);
        }

        private void OnValidate()
        {
            specializationId =
                Normalize(
                    specializationId);

            requiredClassId =
                Normalize(
                    requiredClassId);

            nameLocalizationKey =
                Normalize(
                    nameLocalizationKey);

            descriptionLocalizationKey =
                Normalize(
                    descriptionLocalizationKey);

            displayName =
                string.IsNullOrWhiteSpace(
                    displayName)
                    ? specializationId
                    : displayName.Trim();

            description =
                Normalize(
                    description);

            if (statModifiers == null)
            {
                statModifiers =
                    new List<
                        CharacterStatModifierDefinition>();
            }

            if (startingSkills == null)
            {
                startingSkills =
                    new List<
                        CharacterSkillDefinitionEntry>();
            }

            if (skillPoolIds == null)
            {
                skillPoolIds =
                    new List<string>();
            }

            for (int i = 0;
                 i < skillPoolIds.Count;
                 i++)
            {
                skillPoolIds[i] =
                    Normalize(
                        skillPoolIds[i]);
            }

            if (passives == null)
            {
                passives =
                    new List<
                        CharacterPassiveDefinitionEntry>();
            }

            if (skillReplacements == null)
            {
                skillReplacements =
                    new List<
                        SkillReplacementDefinition>();
            }
        }

        private static List<string>
            CopyAndNormalizeSkillIds(
                IReadOnlyList<string> source)
        {
            List<string> result =
                new();

            if (source == null)
            {
                return result;
            }

            for (int i = 0;
                 i < source.Count;
                 i++)
            {
                result.Add(
                    Normalize(
                        source[i]));
            }

            return result;
        }

        private static string Normalize(
            string value)
        {
            return value?.Trim() ??
                   string.Empty;
        }

#if UNITY_EDITOR
        public void InitializeForTests(
            string newSpecializationId,
            string newRequiredClassId,
            string newDisplayName = null,
            string newNameLocalizationKey = null,
            string newDescriptionLocalizationKey = null,
            string newDescription = null,
            IReadOnlyList<
                CharacterStatModifierDefinition>
                newStatModifiers = null,
            IReadOnlyList<
                CharacterSkillDefinitionEntry>
                newStartingSkills = null,
            IReadOnlyList<string>
                newSkillPoolIds = null,
            IReadOnlyList<
                CharacterPassiveDefinitionEntry>
                newPassives = null,
                        IReadOnlyList<
                SkillReplacementDefinition>
                newSkillReplacements = null)
        {
            specializationId =
                Normalize(
                    newSpecializationId);

            requiredClassId =
                Normalize(
                    newRequiredClassId);

            nameLocalizationKey =
                Normalize(
                    newNameLocalizationKey);

            descriptionLocalizationKey =
                Normalize(
                    newDescriptionLocalizationKey);

            displayName =
                string.IsNullOrWhiteSpace(
                    newDisplayName)
                    ? specializationId
                    : newDisplayName.Trim();

            description =
                Normalize(
                    newDescription);

            icon = null;

            statModifiers =
                newStatModifiers != null
                    ? new List<
                        CharacterStatModifierDefinition>(
                            newStatModifiers)
                    : new List<
                        CharacterStatModifierDefinition>();

            startingSkills =
                newStartingSkills != null
                    ? new List<
                        CharacterSkillDefinitionEntry>(
                            newStartingSkills)
                    : new List<
                        CharacterSkillDefinitionEntry>();

            skillPoolIds =
                CopyAndNormalizeSkillIds(
                    newSkillPoolIds);

            passives =
                newPassives != null
                    ? new List<
                        CharacterPassiveDefinitionEntry>(
                            newPassives)
                    : new List<
                        CharacterPassiveDefinitionEntry>();

            skillReplacements =
                newSkillReplacements != null
                    ? new List<
                        SkillReplacementDefinition>(
                            newSkillReplacements)
                    : new List<
                        SkillReplacementDefinition>();
#endif
        }
    }
}