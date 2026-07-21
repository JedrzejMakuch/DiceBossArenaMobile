using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceBossArena.Game
{
    public static class CharacterActionSlots
    {
        private static readonly
            ReadOnlyCollection<CharacterActionSlot>
                all =
                    new List<CharacterActionSlot>
                    {
                        CharacterActionSlot.WeaponAttack,
                        CharacterActionSlot.BasicAttack,
                        CharacterActionSlot.SkillOne,
                        CharacterActionSlot.SkillTwo,
                        CharacterActionSlot.SkillThree,
                        CharacterActionSlot.SkillFour
                    }.AsReadOnly();

        public static IReadOnlyList<CharacterActionSlot> All =>
            all;

        public static int Count =>
            all.Count;
    }
}