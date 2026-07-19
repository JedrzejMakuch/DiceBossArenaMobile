using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Game
{
    [CreateAssetMenu(
        fileName = "EquipmentBaseType_",
        menuName =
            "Dice Boss Arena/Characters/Equipment Base Type")]
    public sealed class EquipmentBaseTypeDefinition :
        ScriptableObject
    {
        [Header("Identity")]
        [SerializeField]
        private string baseTypeId;

        [Header("Equipment")]
        [SerializeField]
        private EquipmentSlotType slotType;

        [Header("Base Stats")]
        [SerializeField]
        private List<CharacterStatModifierDefinition>
    statModifiers = new();

        [SerializeField]
        private EquipmentBaseTypeCategory category;

        public EquipmentBaseTypeId BaseTypeId =>
            new EquipmentBaseTypeId(baseTypeId);

        public EquipmentSlotType SlotType =>
            slotType;

        public EquipmentBaseTypeCategory Category =>
            category;

        public IReadOnlyList<
    CharacterStatModifierDefinition>
    StatModifiers =>
        statModifiers;

        private void OnValidate()
        {
            baseTypeId =
                baseTypeId?.Trim() ??
                string.Empty;

            statModifiers ??=
                new List<
                    CharacterStatModifierDefinition>();
        }

#if UNITY_EDITOR
        public void InitializeForTests(
    string newBaseTypeId,
    EquipmentSlotType newSlotType,
    EquipmentBaseTypeCategory newCategory,
    IReadOnlyList<
        CharacterStatModifierDefinition>
        newStatModifiers = null)
        {
            baseTypeId =
                newBaseTypeId?.Trim() ??
                string.Empty;

            slotType =
                newSlotType;

            category =
                newCategory;

            statModifiers =
    newStatModifiers != null
        ? new List<
            CharacterStatModifierDefinition>(
                newStatModifiers)
        : new List<
            CharacterStatModifierDefinition>();
        }
#endif
    }
}