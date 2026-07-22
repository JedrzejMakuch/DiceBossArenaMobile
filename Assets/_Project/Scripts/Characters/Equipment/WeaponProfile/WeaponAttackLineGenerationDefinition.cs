using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class WeaponAttackLineGenerationDefinition
    {
        [SerializeField]
        private string lineId;

        [SerializeField]
        private int minDamage;

        [SerializeField]
        private int maxDamage;

        [SerializeField]
        private List<WeaponAttackElement> allowedElements =
            new List<WeaponAttackElement>();

        [SerializeField]
        private List<WeaponAttackEffectDefinition> effects =
            new List<WeaponAttackEffectDefinition>();

        public WeaponAttackLineId LineId =>
            new WeaponAttackLineId(lineId);

        public int MinDamage =>
            minDamage;

        public int MaxDamage =>
            maxDamage;

        public IReadOnlyList<WeaponAttackElement>
            AllowedElements =>
                allowedElements;

        public IReadOnlyList<WeaponAttackEffectDefinition>
            Effects =>
                effects;

#if UNITY_EDITOR
        public WeaponAttackLineGenerationDefinition(
            string newLineId,
            int newMinDamage,
            int newMaxDamage,
            IEnumerable<WeaponAttackElement>
                newAllowedElements,
            IEnumerable<WeaponAttackEffectDefinition>
                newEffects = null)
        {
            lineId =
                newLineId;

            minDamage =
                newMinDamage;

            maxDamage =
                newMaxDamage;

            allowedElements =
                newAllowedElements == null
                    ? new List<WeaponAttackElement>()
                    : new List<WeaponAttackElement>(
                        newAllowedElements);

            effects =
                newEffects == null
                    ? new List<WeaponAttackEffectDefinition>()
                    : new List<WeaponAttackEffectDefinition>(
                        newEffects);
        }
#endif
    }
}