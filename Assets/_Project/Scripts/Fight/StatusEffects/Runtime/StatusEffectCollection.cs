using System;
using System.Collections.Generic;
using DiceBossArena.Game;

public sealed class StatusEffectCollection
{
    private readonly Dictionary<
        StatusEffectId,
        StatusEffectRuntimeState> statesById =
            new();

    public int Count =>
        statesById.Count;

    public event Action<StatusEffectRuntimeState>
        EffectAdded;

    public event Action<StatusEffectRuntimeState>
        EffectChanged;

    public event Action<StatusEffectId>
        EffectRemoved;

    public StatusEffectApplyResult Apply(
        StatusEffectDefinition definition)
    {
        if (definition == null)
        {
            throw new ArgumentNullException(
                nameof(definition));
        }

        StatusEffectId statusEffectId =
            definition.StatusEffectId;

        if (!statusEffectId.IsValid)
        {
            throw new ArgumentException(
                "Status effect definition must have a valid id.",
                nameof(definition));
        }

        if (!statesById.TryGetValue(
                statusEffectId,
                out StatusEffectRuntimeState existingState))
        {
            StatusEffectRuntimeState newState =
                new StatusEffectRuntimeState(
                    definition);

            statesById.Add(
                statusEffectId,
                newState);

            EffectAdded?.Invoke(
                newState);

            return StatusEffectApplyResult.Added;
        }

        switch (definition.StackingPolicy)
        {
            case StatusEffectStackingPolicy.RefreshDuration:
                existingState.RefreshDuration();

                EffectChanged?.Invoke(
                    existingState);

                return StatusEffectApplyResult
                    .DurationRefreshed;

            case StatusEffectStackingPolicy
                .AddStacksAndRefreshDuration:
                {
                    bool stackAdded =
                        existingState.TryAddStack();

                    existingState.RefreshDuration();

                    EffectChanged?.Invoke(
                        existingState);

                    return stackAdded
                        ? StatusEffectApplyResult
                            .StackAddedAndDurationRefreshed
                        : StatusEffectApplyResult
                            .DurationRefreshed;
                }

            case StatusEffectStackingPolicy
                .RejectNewApplication:
                return StatusEffectApplyResult.Rejected;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(definition),
                    definition.StackingPolicy,
                    "Unsupported status effect stacking policy.");
        }
    }

    public bool TryGet(
        StatusEffectId statusEffectId,
        out StatusEffectRuntimeState state)
    {
        state = null;

        if (!statusEffectId.IsValid)
        {
            return false;
        }

        return statesById.TryGetValue(
            statusEffectId,
            out state);
    }

    public bool Contains(
        StatusEffectId statusEffectId)
    {
        return statusEffectId.IsValid &&
               statesById.ContainsKey(
                   statusEffectId);
    }

    public bool Remove(
        StatusEffectId statusEffectId)
    {
        if (!statusEffectId.IsValid)
        {
            return false;
        }

        bool removed =
            statesById.Remove(
                statusEffectId);

        if (removed)
        {
            EffectRemoved?.Invoke(
                statusEffectId);
        }

        return removed;
    }

    public IReadOnlyList<StatusEffectRuntimeState> GetStatesSnapshot()
    {
        return new List<StatusEffectRuntimeState>(
            statesById.Values);
    }

    public int RemoveExpired()
    {
        if (statesById.Count == 0)
        {
            return 0;
        }

        List<StatusEffectId> expiredIds =
            new();

        foreach (
            KeyValuePair<
                StatusEffectId,
                StatusEffectRuntimeState> entry
            in statesById)
        {
            if (entry.Value.IsExpired)
            {
                expiredIds.Add(
                    entry.Key);
            }
        }

        for (int i = 0; i < expiredIds.Count; i++)
        {
            Remove(
                expiredIds[i]);
        }

        return expiredIds.Count;
    }

    public void Clear()
    {
        if (statesById.Count == 0)
        {
            return;
        }

        List<StatusEffectId> ids =
            new(
                statesById.Keys);

        statesById.Clear();

        for (int i = 0; i < ids.Count; i++)
        {
            EffectRemoved?.Invoke(
                ids[i]);
        }
    }

    public StatusEffectDispelResult Dispel(
    StatusEffectCategory category,
    int maximumEffects = int.MaxValue)
    {
        if (maximumEffects <= 0 ||
            statesById.Count == 0)
        {
            return new StatusEffectDispelResult(
                0);
        }

        List<StatusEffectId> idsToRemove =
            new();

        foreach (
            KeyValuePair<
                StatusEffectId,
                StatusEffectRuntimeState> entry
            in statesById)
        {
            if (entry.Value == null ||
                entry.Value.Definition.Category != category)
            {
                continue;
            }

            idsToRemove.Add(
                entry.Key);

            if (idsToRemove.Count >= maximumEffects)
            {
                break;
            }
        }

        for (int i = 0; i < idsToRemove.Count; i++)
        {
            Remove(
                idsToRemove[i]);
        }

        return new StatusEffectDispelResult(
            idsToRemove.Count);
    }
}