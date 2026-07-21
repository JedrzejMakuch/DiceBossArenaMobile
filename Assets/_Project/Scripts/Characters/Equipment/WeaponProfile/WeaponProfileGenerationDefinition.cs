using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class WeaponProfileGenerationDefinition
    {
        [SerializeField]
        private List<WeaponAttackLineGenerationDefinition>
            lines =
                new List<
                    WeaponAttackLineGenerationDefinition>();

        public IReadOnlyList<
            WeaponAttackLineGenerationDefinition> Lines =>
                lines;

#if UNITY_EDITOR
        public WeaponProfileGenerationDefinition(
            IEnumerable<
                WeaponAttackLineGenerationDefinition>
                newLines)
        {
            lines =
                newLines == null
                    ? new List<
                        WeaponAttackLineGenerationDefinition>()
                    : new List<
                        WeaponAttackLineGenerationDefinition>(
                            newLines);
        }
#endif
    }
}