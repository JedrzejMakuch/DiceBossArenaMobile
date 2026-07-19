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
        private EquipmentBaseTypeDefinition baseType;
        [SerializeField]
        private EquipmentSlotType slotType;

        public bool IsEquippable =>
    slotType != EquipmentSlotType.None;

        [SerializeField]
        private EquipmentItemCategory category;

        [SerializeField]
        private WeaponHandedness handedness;

        [SerializeField, Min(1)]
        private int maxStackSize = 1;

        [Header("Requirements")]
        [SerializeField]
        private List<string> requiredClassIds = new();

        [SerializeField]
        private List<string>
            requiredSpecializationIds = new();

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

        public EquipmentBaseTypeDefinition BaseType =>
    baseType;

        public EquipmentSlotType SlotType =>
            slotType;

        public EquipmentItemCategory Category =>
    category;

        public WeaponHandedness Handedness =>
            handedness;

        public int MaxStackSize =>
            maxStackSize;

        public IReadOnlyList<string>
    RequiredClassIds =>
        requiredClassIds;

        public IReadOnlyList<string>
            RequiredSpecializationIds =>
                requiredSpecializationIds;

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

            requiredClassIds =
    NormalizeIds(requiredClassIds);

            requiredSpecializationIds =
                NormalizeIds(
                    requiredSpecializationIds);

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

        private static List<string> NormalizeIds(
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
                    Normalize(source[i]));
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
            string newItemId,
            EquipmentSlotType newSlotType,
            int newMaxStackSize = 1,
            EquipmentItemCategory newCategory =
                EquipmentItemCategory.Weapon,
            WeaponHandedness newHandedness =
                WeaponHandedness.OneHanded,
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
                newGrantedPassives = null,
            IReadOnlyList<string>
                newRequiredClassIds = null,
            IReadOnlyList<string>
    newRequiredSpecializationIds = null,
EquipmentBaseTypeDefinition newBaseType = null)
        {
            itemId =
                Normalize(newItemId);

            slotType =
                newSlotType;

            category =
    newCategory;

            handedness =
                newHandedness;

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

            requiredClassIds =
    NormalizeIds(
        newRequiredClassIds);

            requiredSpecializationIds =
                NormalizeIds(
                    newRequiredSpecializationIds);

            baseType =
    newBaseType;
        }
#endif
    }
}