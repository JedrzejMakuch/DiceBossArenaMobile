using System;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class WeaponAttackEffectDefinition
    {
        [SerializeField]
        private WeaponAttackEffectType effectType;

        [SerializeField]
        [Range(0, 100)]
        private int triggerChancePercent;

        [SerializeField]
        private StatusEffectDefinition statusEffect;

        [SerializeField]
        [Min(0)]
        private int lifeStealPercent;

        public WeaponAttackEffectType EffectType =>
            effectType;

        public int TriggerChancePercent =>
            triggerChancePercent;

        public StatusEffectDefinition StatusEffect =>
            statusEffect;

        public int LifeStealPercent =>
            lifeStealPercent;

#if UNITY_EDITOR
        public WeaponAttackEffectDefinition(
            WeaponAttackEffectType newEffectType,
            int newTriggerChancePercent,
            StatusEffectDefinition newStatusEffect,
            int newLifeStealPercent)
        {
            effectType =
                newEffectType;

            triggerChancePercent =
                newTriggerChancePercent;

            statusEffect =
                newStatusEffect;

            lifeStealPercent =
                newLifeStealPercent;
        }
#endif
    }
}