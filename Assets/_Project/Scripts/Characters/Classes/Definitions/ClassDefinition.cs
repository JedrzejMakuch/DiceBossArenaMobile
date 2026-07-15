using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Game
{
    [CreateAssetMenu(
        fileName = "Class_",
        menuName =
            "Dice Boss Arena/Characters/Class Definition")]
    public sealed class ClassDefinition :
        ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        private string classId;

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

        public CharacterClassId ClassId =>
            new CharacterClassId(
                classId);

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

        private void OnValidate()
        {
            classId =
                Normalize(classId);

            nameLocalizationKey =
                Normalize(nameLocalizationKey);

            descriptionLocalizationKey =
                Normalize(
                    descriptionLocalizationKey);

            displayName =
                string.IsNullOrWhiteSpace(
                    displayName)
                    ? classId
                    : displayName.Trim();

            description =
                Normalize(description);

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
        }

        private static string Normalize(
            string value)
        {
            return value?.Trim() ??
                   string.Empty;
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

#if UNITY_EDITOR
        public void InitializeForTests(
            string newClassId,
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
                newPassives = null)
        {
            classId =
                Normalize(newClassId);

            nameLocalizationKey =
                Normalize(
                    newNameLocalizationKey);

            descriptionLocalizationKey =
                Normalize(
                    newDescriptionLocalizationKey);

            displayName =
                string.IsNullOrWhiteSpace(
                    newDisplayName)
                    ? classId
                    : newDisplayName.Trim();

            description =
                Normalize(newDescription);

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
                newSkillPoolIds != null
                    ? CopyAndNormalizeSkillIds(
                        newSkillPoolIds)
                    : new List<string>();

            passives =
                newPassives != null
                    ? new List<
                        CharacterPassiveDefinitionEntry>(
                            newPassives)
                    : new List<
                        CharacterPassiveDefinitionEntry>();
        }
#endif
    }
}