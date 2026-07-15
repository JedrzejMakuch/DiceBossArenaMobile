using System;
using UnityEngine;

namespace DiceBossArena.Game
{
    [Serializable]
    public sealed class
        CharacterPassiveDefinitionEntry
    {
        [SerializeField]
        private string passiveId;

        public string PassiveId =>
            passiveId;

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(
                passiveId);

        public CharacterPassiveId CreatePassiveId()
        {
            return new CharacterPassiveId(
                passiveId);
        }

#if UNITY_EDITOR
        public CharacterPassiveDefinitionEntry(
            string newPassiveId)
        {
            passiveId =
                Normalize(newPassiveId);
        }
#endif

        private static string Normalize(
            string value)
        {
            return value?.Trim() ??
                   string.Empty;
        }
    }
}