using System;
using System.Collections.Generic;

public sealed class StatusEffectTurnProcessor
{
    public StatusEffectTurnProcessResult ProcessStartOfTurn(
        StatusEffectCollection collection,
        Action<StatusEffectRuntimeState> executeEffect)
    {
        return ProcessEffects(
            collection,
            StatusEffectTickTiming.StartOfOwnerTurn,
            executeEffect,
            advanceDuration:
                false);
    }

    public StatusEffectTurnProcessResult ProcessEndOfTurn(
        StatusEffectCollection collection,
        Action<StatusEffectRuntimeState> executeEffect)
    {
        return ProcessEffects(
            collection,
            StatusEffectTickTiming.EndOfOwnerTurn,
            executeEffect,
            advanceDuration:
                true);
    }

    private StatusEffectTurnProcessResult ProcessEffects(
        StatusEffectCollection collection,
        StatusEffectTickTiming timing,
        Action<StatusEffectRuntimeState> executeEffect,
        bool advanceDuration)
    {
        if (collection == null)
        {
            throw new ArgumentNullException(
                nameof(collection));
        }

        IReadOnlyList<StatusEffectRuntimeState> states =
            collection.GetStatesSnapshot();

        int executedCount = 0;

        for (int i = 0; i < states.Count; i++)
        {
            StatusEffectRuntimeState state =
                states[i];

            if (state == null ||
                state.IsExpired)
            {
                continue;
            }

            if (state.Definition.TickTiming == timing)
            {
                executeEffect?.Invoke(
                    state);

                executedCount++;
            }
        }

        int expiredCount = 0;

        if (advanceDuration)
        {
            IReadOnlyList<StatusEffectRuntimeState>
                durationStates =
                    collection.GetStatesSnapshot();

            for (int i = 0;
                 i < durationStates.Count;
                 i++)
            {
                StatusEffectRuntimeState state =
                    durationStates[i];

                if (state == null ||
                    state.IsExpired)
                {
                    continue;
                }

                state.AdvanceDuration();
            }

            expiredCount =
                collection.RemoveExpired();
        }

        return new StatusEffectTurnProcessResult(
            executedCount,
            expiredCount);
    }
}