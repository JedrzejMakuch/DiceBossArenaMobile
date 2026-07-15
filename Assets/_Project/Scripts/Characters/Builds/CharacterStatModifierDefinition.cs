using System;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class
        CharacterStatModifierDefinition
    {
        [SerializeField]
        private FightStatType statType;

        [SerializeField]
        private FightStatModifierType modifierType;

        [SerializeField]
        private int value;

        public FightStatType StatType =>
            statType;

        public FightStatModifierType ModifierType =>
            modifierType;

        public int Value =>
            value;

        public FightStatModifier CreateModifier()
        {
            return new FightStatModifier(
                statType,
                modifierType,
                value);
        }

#if UNITY_EDITOR
        public CharacterStatModifierDefinition(
            FightStatType newStatType,
            FightStatModifierType newModifierType,
            int newValue)
        {
            statType =
                newStatType;

            modifierType =
                newModifierType;

            value =
                newValue;
        }
#endif
    }
}