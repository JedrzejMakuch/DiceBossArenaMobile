using System;
using System.Collections.Generic;
using DiceBossArena.Game;

public sealed class StatusEffectStatModifierBinder :
    IDisposable
{
    private readonly StatusEffectCollection collection;
    private readonly FightUnitStats stats;

    private readonly Dictionary<
        StatusEffectId,
        List<FightStatModifier>> appliedModifiersByEffectId =
            new();

    private bool isDisposed;

    public StatusEffectStatModifierBinder(
        StatusEffectCollection collection,
        FightUnitStats stats)
    {
        this.collection =
            collection ??
            throw new ArgumentNullException(
                nameof(collection));

        this.stats =
            stats ??
            throw new ArgumentNullException(
                nameof(stats));

        collection.EffectAdded +=
            HandleEffectAdded;

        collection.EffectChanged +=
            HandleEffectChanged;

        collection.EffectRemoved +=
            HandleEffectRemoved;

        BindExistingEffects();
    }

    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        isDisposed = true;

        collection.EffectAdded -=
            HandleEffectAdded;

        collection.EffectChanged -=
            HandleEffectChanged;

        collection.EffectRemoved -=
            HandleEffectRemoved;

        RemoveAllAppliedModifiers();
    }

    private void BindExistingEffects()
    {
        IReadOnlyList<StatusEffectRuntimeState> states =
            collection.GetStatesSnapshot();

        for (int i = 0; i < states.Count; i++)
        {
            StatusEffectRuntimeState state =
                states[i];

            if (state == null ||
                state.IsExpired)
            {
                continue;
            }

            ApplyModifiers(
                state);
        }
    }

    private void HandleEffectAdded(
        StatusEffectRuntimeState state)
    {
        ApplyModifiers(
            state);
    }

    private void HandleEffectChanged(
        StatusEffectRuntimeState state)
    {
        if (state == null)
        {
            return;
        }

        RemoveAppliedModifiers(
            state.StatusEffectId);

        ApplyModifiers(
            state);
    }

    private void HandleEffectRemoved(
        StatusEffectId statusEffectId)
    {
        RemoveAppliedModifiers(
            statusEffectId);
    }

    private void ApplyModifiers(
        StatusEffectRuntimeState state)
    {
        if (state == null ||
            state.IsExpired)
        {
            return;
        }

        IReadOnlyList<
            StatusEffectStatModifierDefinition> definitions =
                state.Definition.StatModifiers;

        if (definitions == null ||
            definitions.Count == 0)
        {
            return;
        }

        List<FightStatModifier> appliedModifiers =
            new();

        for (int i = 0; i < definitions.Count; i++)
        {
            StatusEffectStatModifierDefinition definition =
                definitions[i];

            if (definition == null)
            {
                continue;
            }

            FightStatModifier modifier =
                definition.CreateModifier(
                    state.Stacks);

            stats.AddModifier(
                modifier);

            appliedModifiers.Add(
                modifier);
        }

        if (appliedModifiers.Count > 0)
        {
            appliedModifiersByEffectId[
                state.StatusEffectId] =
                    appliedModifiers;
        }
    }

    private void RemoveAppliedModifiers(
        StatusEffectId statusEffectId)
    {
        if (!appliedModifiersByEffectId.TryGetValue(
                statusEffectId,
                out List<FightStatModifier> modifiers))
        {
            return;
        }

        for (int i = 0; i < modifiers.Count; i++)
        {
            stats.RemoveModifier(
                modifiers[i]);
        }

        appliedModifiersByEffectId.Remove(
            statusEffectId);
    }

    private void RemoveAllAppliedModifiers()
    {
        if (appliedModifiersByEffectId.Count == 0)
        {
            return;
        }

        List<StatusEffectId> ids =
            new(
                appliedModifiersByEffectId.Keys);

        for (int i = 0; i < ids.Count; i++)
        {
            RemoveAppliedModifiers(
                ids[i]);
        }
    }
}