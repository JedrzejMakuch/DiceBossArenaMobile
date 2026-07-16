using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Game
{
    [CreateAssetMenu(
        fileName = "Item_",
        menuName =
            "Dice Boss Arena/Characters/Item Definition")]
    public sealed class ItemDefinition :
        ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        private string itemId;

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

        [Header("Equipment")]
        [SerializeField]
        private EquipmentSlotType slotType;

        [SerializeField, Min(1)]
        private int maxStackSize = 1;

        [Header("Build")]
        [SerializeField]
        private List<CharacterStatModifierDefinition>
            statModifiers = new();

        [SerializeField]
        private List<CharacterSkillDefinitionEntry>
            grantedSkills = new();

        [SerializeField]
        private List<CharacterPassiveDefinitionEntry>
            grantedPassives = new();

        public CharacterItemId ItemId =>
            new CharacterItemId(itemId);

        public LocalizationKey NameLocalizationKey =>
            new LocalizationKey(nameLocalizationKey);

        public LocalizationKey DescriptionLocalizationKey =>
            new LocalizationKey(
                descriptionLocalizationKey);

        public string DisplayName =>
            displayName;

        public string Description =>
            description;

        public Sprite Icon =>
            icon;

        public EquipmentSlotType SlotType =>
            slotType;

        public int MaxStackSize =>
            maxStackSize;

        public IReadOnlyList<
            CharacterStatModifierDefinition>
            StatModifiers =>
                statModifiers;

        public IReadOnlyList<
            CharacterSkillDefinitionEntry>
            GrantedSkills =>
                grantedSkills;

        public IReadOnlyList<
            CharacterPassiveDefinitionEntry>
            GrantedPassives =>
                grantedPassives;

        private void OnValidate()
        {
            itemId =
                Normalize(itemId);

            nameLocalizationKey =
                Normalize(nameLocalizationKey);

            descriptionLocalizationKey =
                Normalize(
                    descriptionLocalizationKey);

            displayName =
                string.IsNullOrWhiteSpace(displayName)
                    ? itemId
                    : displayName.Trim();

            description =
                Normalize(description);

            maxStackSize =
                Mathf.Max(1, maxStackSize);

            statModifiers ??=
                new List<
                    CharacterStatModifierDefinition>();

            grantedSkills ??=
                new List<
                    CharacterSkillDefinitionEntry>();

            grantedPassives ??=
                new List<
                    CharacterPassiveDefinitionEntry>();
        }

        private static string Normalize(
            string value)
        {
            return value?.Trim() ??
                   string.Empty;
        }

#if UNITY_EDITOR
        public void InitializeForTests(
            string newItemId,
            EquipmentSlotType newSlotType,
            int newMaxStackSize = 1,
            string newDisplayName = null,
            string newNameLocalizationKey = null,
            string newDescriptionLocalizationKey = null,
            string newDescription = null,
            IReadOnlyList<
                CharacterStatModifierDefinition>
                newStatModifiers = null,
            IReadOnlyList<
                CharacterSkillDefinitionEntry>
                newGrantedSkills = null,
            IReadOnlyList<
                CharacterPassiveDefinitionEntry>
                newGrantedPassives = null)
        {
            itemId =
                Normalize(newItemId);

            slotType =
                newSlotType;

            maxStackSize =
                Mathf.Max(1, newMaxStackSize);

            displayName =
                string.IsNullOrWhiteSpace(
                    newDisplayName)
                    ? itemId
                    : newDisplayName.Trim();

            nameLocalizationKey =
                Normalize(
                    newNameLocalizationKey);

            descriptionLocalizationKey =
                Normalize(
                    newDescriptionLocalizationKey);

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

            grantedSkills =
                newGrantedSkills != null
                    ? new List<
                        CharacterSkillDefinitionEntry>(
                            newGrantedSkills)
                    : new List<
                        CharacterSkillDefinitionEntry>();

            grantedPassives =
                newGrantedPassives != null
                    ? new List<
                        CharacterPassiveDefinitionEntry>(
                            newGrantedPassives)
                    : new List<
                        CharacterPassiveDefinitionEntry>();
        }
#endif
    }
}