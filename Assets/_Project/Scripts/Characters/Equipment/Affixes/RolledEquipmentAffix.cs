using System;

namespace DiceBossArena.Game
{
    public sealed class RolledEquipmentAffix
    {
        public RolledEquipmentAffix(
            EquipmentAffixId affixId,
            FightStatType statType,
            FightStatModifierType modifierType,
            int value)
        {
            if (string.IsNullOrWhiteSpace(
                    affixId.Value))
            {
                throw new ArgumentException(
                    "Rolled equipment affix must contain a valid affix ID.",
                    nameof(affixId));
            }

            if (!Enum.IsDefined(
                    typeof(FightStatType),
                    statType))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(statType),
                    statType,
                    "Unsupported fight stat type.");
            }

            if (!Enum.IsDefined(
                    typeof(FightStatModifierType),
                    modifierType))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(modifierType),
                    modifierType,
                    "Unsupported fight stat modifier type.");
            }

            AffixId = affixId;
            StatType = statType;
            ModifierType = modifierType;
            Value = value;
        }

        public EquipmentAffixId AffixId
        {
            get;
        }

        public FightStatType StatType
        {
            get;
        }

        public FightStatModifierType ModifierType
        {
            get;
        }

        public int Value
        {
            get;
        }
    }
}