using System;
using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public sealed class WeaponAttackEffectDefinitionValidatorTests
{
    private WeaponAttackEffectDefinitionValidator validator;

    [SetUp]
    public void SetUp()
    {
        validator =
            new WeaponAttackEffectDefinitionValidator();
    }

    [Test]
    public void Validate_NullDefinition_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => validator.Validate(null));
    }

    [Test]
    public void Validate_ValidNone_DoesNotThrow()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.None,
                0,
                null,
                0);

        Assert.DoesNotThrow(
            () => validator.Validate(definition));
    }

    [Test]
    public void Validate_ValidApplyStatusEffect_DoesNotThrow()
    {
        StatusEffectDefinition statusEffect =
            ScriptableObject.CreateInstance<
                StatusEffectDefinition>();

        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.ApplyStatusEffect,
                50,
                statusEffect,
                0);

        Assert.DoesNotThrow(
            () => validator.Validate(definition));

        UnityEngine.Object.DestroyImmediate(statusEffect);
    }

    [Test]
    public void Validate_ValidLifeSteal_DoesNotThrow()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.LifeSteal,
                100,
                null,
                25);

        Assert.DoesNotThrow(
            () => validator.Validate(definition));
    }

    [TestCase(-1)]
    [TestCase(101)]
    public void Validate_TriggerChanceOutsideRange_Throws(
        int triggerChance)
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.LifeSteal,
                triggerChance,
                null,
                25);

        Assert.Throws<InvalidOperationException>(
            () => validator.Validate(definition));
    }

    [Test]
    public void Validate_UnsupportedEffectType_Throws()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                (WeaponAttackEffectType)999,
                50,
                null,
                0);

        Assert.Throws<InvalidOperationException>(
            () => validator.Validate(definition));
    }

    [Test]
    public void Validate_NoneWithTriggerChance_Throws()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.None,
                25,
                null,
                0);

        Assert.Throws<InvalidOperationException>(
            () => validator.Validate(definition));
    }

    [Test]
    public void Validate_NoneWithLifeSteal_Throws()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.None,
                0,
                null,
                10);

        Assert.Throws<InvalidOperationException>(
            () => validator.Validate(definition));
    }

    [Test]
    public void Validate_ApplyStatusEffectWithoutStatus_Throws()
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.ApplyStatusEffect,
                50,
                null,
                0);

        Assert.Throws<InvalidOperationException>(
            () => validator.Validate(definition));
    }

    [Test]
    public void Validate_ApplyStatusEffectWithLifeSteal_Throws()
    {
        StatusEffectDefinition statusEffect =
            ScriptableObject.CreateInstance<
                StatusEffectDefinition>();

        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.ApplyStatusEffect,
                50,
                statusEffect,
                10);

        Assert.Throws<InvalidOperationException>(
            () => validator.Validate(definition));

        UnityEngine.Object.DestroyImmediate(statusEffect);
    }

    [TestCase(0)]
    [TestCase(-10)]
    public void Validate_NonPositiveLifeSteal_Throws(
        int lifeStealPercent)
    {
        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.LifeSteal,
                50,
                null,
                lifeStealPercent);

        Assert.Throws<InvalidOperationException>(
            () => validator.Validate(definition));
    }

    [Test]
    public void Validate_LifeStealWithStatusEffect_Throws()
    {
        StatusEffectDefinition statusEffect =
            ScriptableObject.CreateInstance<
                StatusEffectDefinition>();

        WeaponAttackEffectDefinition definition =
            CreateDefinition(
                WeaponAttackEffectType.LifeSteal,
                50,
                statusEffect,
                25);

        Assert.Throws<InvalidOperationException>(
            () => validator.Validate(definition));

        UnityEngine.Object.DestroyImmediate(statusEffect);
    }

    private static WeaponAttackEffectDefinition CreateDefinition(
        WeaponAttackEffectType effectType,
        int triggerChancePercent,
        StatusEffectDefinition statusEffect,
        int lifeStealPercent)
    {
        return new WeaponAttackEffectDefinition(
            effectType,
            triggerChancePercent,
            statusEffect,
            lifeStealPercent);
    }
}