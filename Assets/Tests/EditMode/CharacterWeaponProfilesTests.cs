using DiceBossArena.Game;
using NUnit.Framework;

public sealed class CharacterWeaponProfilesTests
{
    [Test]
    public void Unarmed_HasSingleNeutralDamageLine()
    {
        RolledWeaponProfile profile =
            CharacterWeaponProfiles.Unarmed;

        Assert.That(
            profile.Lines,
            Has.Length.EqualTo(1));

        Assert.That(
            profile.Lines[0].LineId.Value,
            Is.EqualTo("unarmed_damage"));

        Assert.That(
            profile.Lines[0].Element,
            Is.EqualTo(
                WeaponAttackElement.Neutral));

        Assert.That(
            profile.Lines[0].MinDamage,
            Is.EqualTo(1));

        Assert.That(
            profile.Lines[0].MaxDamage,
            Is.EqualTo(1));
    }
}