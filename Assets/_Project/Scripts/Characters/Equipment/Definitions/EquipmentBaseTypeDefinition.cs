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

        [SerializeField]
        private EquipmentBaseTypeCategory category;

        public EquipmentBaseTypeId BaseTypeId =>
            new EquipmentBaseTypeId(baseTypeId);

        public EquipmentSlotType SlotType =>
            slotType;

        public EquipmentBaseTypeCategory Category =>
            category;

        private void OnValidate()
        {
            baseTypeId =
                baseTypeId?.Trim() ??
                string.Empty;
        }

#if UNITY_EDITOR
        public void InitializeForTests(
            string newBaseTypeId,
            EquipmentSlotType newSlotType,
            EquipmentBaseTypeCategory newCategory)
        {
            baseTypeId =
                newBaseTypeId?.Trim() ??
                string.Empty;

            slotType =
                newSlotType;

            category =
                newCategory;
        }
#endif
    }
}