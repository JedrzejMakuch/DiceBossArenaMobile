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

        public WeaponAttackLineId LineId =>
            new WeaponAttackLineId(lineId);

        public int MinDamage =>
            minDamage;

        public int MaxDamage =>
            maxDamage;

        public IReadOnlyList<WeaponAttackElement>
            AllowedElements =>
                allowedElements;

#if UNITY_EDITOR
        public WeaponAttackLineGenerationDefinition(
            string newLineId,
            int newMinDamage,
            int newMaxDamage,
            IEnumerable<WeaponAttackElement>
                newAllowedElements)
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
        }
#endif
    }
}