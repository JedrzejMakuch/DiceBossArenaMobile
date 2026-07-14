using System;
using DiceBossArena.Game;
using UnityEngine;

[Serializable]
public sealed class StatusEffectStatModifierDefinition
{
    [SerializeField]
    private FightStatType statType;

    [SerializeField]
    private FightStatModifierType modifierType;

    [SerializeField]
    private int valuePerStack;

    public FightStatType StatType =>
        statType;

    public FightStatModifierType ModifierType =>
        modifierType;

    public int ValuePerStack =>
        valuePerStack;

    public FightStatModifier CreateModifier(
        int stacks)
    {
        int normalizedStacks =
            Mathf.Max(
                1,
                stacks);

        return new FightStatModifier(
            statType,
            modifierType,
            valuePerStack * normalizedStacks);
    }

#if UNITY_EDITOR
    public StatusEffectStatModifierDefinition(
        FightStatType newStatType,
        FightStatModifierType newModifierType,
        int newValuePerStack)
    {
        statType =
            newStatType;

        modifierType =
            newModifierType;

        valuePerStack =
            newValuePerStack;
    }
#endif
}