using System;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterItemInstanceGenerationRequest
    {
        public ItemDefinition ItemDefinition
        {
            get;
        }

        public EquipmentAffixPoolDefinition AffixPool
        {
            get;
        }

        public int Level
        {
            get;
        }

        public int UpgradeLevel
        {
            get;
        }

        public int Quantity
        {
            get;
        }

        public CharacterItemInstanceGenerationRequest(
            ItemDefinition newItemDefinition,
            EquipmentAffixPoolDefinition newAffixPool,
            int newLevel,
            int newUpgradeLevel,
            int newQuantity)
        {
            ItemDefinition =
                newItemDefinition ??
                throw new ArgumentNullException(
                    nameof(newItemDefinition));

            if (newItemDefinition.BaseType == null)
            {
                throw new ArgumentException(
                    "Generated equipment item must have " +
                    "a base type definition.",
                    nameof(newItemDefinition));
            }

            AffixPool =
                newAffixPool ??
                throw new ArgumentNullException(
                    nameof(newAffixPool));

            if (newLevel < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(newLevel),
                    newLevel,
                    "Item level must be at least 1.");
            }

            if (newUpgradeLevel < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(newUpgradeLevel),
                    newUpgradeLevel,
                    "Item upgrade level cannot be negative.");
            }

            if (newQuantity < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(newQuantity),
                    newQuantity,
                    "Item quantity must be at least 1.");
            }

            Level = newLevel;
            UpgradeLevel = newUpgradeLevel;
            Quantity = newQuantity;
        }
    }
}