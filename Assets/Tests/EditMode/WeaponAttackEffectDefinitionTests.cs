using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackEffectDefinitionTests
{
    [Test]
    public void Constructor_AssignsValues()
    {
        WeaponAttackEffectDefinition definition =
            new WeaponAttackEffectDefinition(
                WeaponAttackEffectType.LifeSteal,
                35,
                null,
                20);

        Assert.That(
            definition.EffectType,
            Is.EqualTo(
                WeaponAttackEffectType.LifeSteal));

        Assert.That(
            definition.TriggerChancePercent,
            Is.EqualTo(35));

        Assert.That(
            definition.StatusEffect,
            Is.Null);

        Assert.That(
            definition.LifeStealPercent,
            Is.EqualTo(20));
    }

    [Test]
    public void Constructor_ApplyStatusEffect_AssignsStatus()
    {
        StatusEffectDefinition statusEffect =
            UnityEngine.ScriptableObject.CreateInstance<
                StatusEffectDefinition>();

        WeaponAttackEffectDefinition definition =
            new WeaponAttackEffectDefinition(
                WeaponAttackEffectType.ApplyStatusEffect,
                50,
                statusEffect,
                0);

        Assert.That(
            definition.StatusEffect,
            Is.SameAs(statusEffect));

        UnityEngine.Object.DestroyImmediate(
            statusEffect);
    }
}