using DiceBossArena.Game;

public static class FightWeaponAttackProfileResolver
{
    public static bool TryResolve(
        CharacterActionSet actionSet,
        out RolledWeaponProfile weaponProfile)
    {
        weaponProfile = null;

        if (actionSet == null)
        {
            return false;
        }

        CharacterActionContent content =
            actionSet[
                CharacterActionSlot.WeaponAttack];

        if (!content.HasWeaponProfile)
        {
            return false;
        }

        weaponProfile = content.WeaponProfile;

        return true;
    }
}