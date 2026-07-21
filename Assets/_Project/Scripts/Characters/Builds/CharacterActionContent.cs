using System;

namespace DiceBossArena.Game
{
    public readonly struct CharacterActionContent :
        IEquatable<CharacterActionContent>
    {
        public CharacterActionSlot Slot { get; }

        public CharacterActionContentType ContentType
        {
            get;
        }

        public CharacterBuildSkill Skill { get; }

        public bool HasSkill =>
            ContentType ==
            CharacterActionContentType.Skill;

        private CharacterActionContent(
            CharacterActionSlot slot,
            CharacterActionContentType contentType,
            CharacterBuildSkill skill)
        {
            Slot = slot;
            ContentType = contentType;
            Skill = skill;
        }

        public static CharacterActionContent
            CreateWeaponAttack()
        {
            return new CharacterActionContent(
                CharacterActionSlot.WeaponAttack,
                CharacterActionContentType.WeaponProfile,
                default);
        }

        public static CharacterActionContent CreateSkill(
            CharacterActionSlot slot,
            CharacterBuildSkill skill)
        {
            if (slot ==
                CharacterActionSlot.WeaponAttack)
            {
                throw new ArgumentException(
                    "Weapon Attack cannot contain a skill.",
                    nameof(slot));
            }

            if (!skill.IsValid)
            {
                throw new ArgumentException(
                    "Action contains an invalid skill.",
                    nameof(skill));
            }

            return new CharacterActionContent(
                slot,
                CharacterActionContentType.Skill,
                skill);
        }

        public bool Equals(
            CharacterActionContent other)
        {
            return Slot == other.Slot &&
                   ContentType == other.ContentType &&
                   Skill.Equals(other.Skill);
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterActionContent other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                Slot,
                ContentType,
                Skill);
        }

        public bool IsValid
        {
            get
            {
                if (ContentType ==
                    CharacterActionContentType.WeaponProfile)
                {
                    return Slot ==
                           CharacterActionSlot.WeaponAttack;
                }

                if (ContentType ==
                    CharacterActionContentType.Skill)
                {
                    return Slot !=
                           CharacterActionSlot.WeaponAttack &&
                           Skill.IsValid;
                }

                return false;
            }
        }
    }
}