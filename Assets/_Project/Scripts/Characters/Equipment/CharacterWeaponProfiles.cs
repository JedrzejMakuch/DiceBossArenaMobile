namespace DiceBossArena.Game
{
    public static class CharacterWeaponProfiles
    {
        public static RolledWeaponProfile Unarmed
        {
            get;
        } =
            new RolledWeaponProfile(
                new[]
                {
                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "unarmed_damage"),
                        WeaponAttackElement.Neutral,
                        1,
                        1)
                });
    }
}