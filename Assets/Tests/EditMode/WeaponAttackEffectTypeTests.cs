using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackEffectTypeTests
{
    [Test]
    public void None_HasStableSerializedValue()
    {
        Assert.That(
            (int)WeaponAttackEffectType.None,
            Is.EqualTo(0));
    }

    [Test]
    public void ApplyStatusEffect_HasStableSerializedValue()
    {
        Assert.That(
            (int)WeaponAttackEffectType.ApplyStatusEffect,
            Is.EqualTo(1));
    }

    [Test]
    public void LifeSteal_HasStableSerializedValue()
    {
        Assert.That(
            (int)WeaponAttackEffectType.LifeSteal,
            Is.EqualTo(2));
    }
}