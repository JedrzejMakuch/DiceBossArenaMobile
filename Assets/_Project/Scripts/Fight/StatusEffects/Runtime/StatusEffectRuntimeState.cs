using System;
using DiceBossArena.Game;

public sealed class StatusEffectRuntimeState
{
    public StatusEffectDefinition Definition { get; }

    public StatusEffectId StatusEffectId =>
        Definition.StatusEffectId;

    public int RemainingDurationTurns { get; private set; }

    public int Stacks { get; private set; }

    public bool IsExpired =>
        RemainingDurationTurns <= 0;

    public StatusEffectRuntimeState(
        StatusEffectDefinition definition)
    {
        Definition =
            definition ??
            throw new ArgumentNullException(
                nameof(definition));

        if (!definition.StatusEffectId.IsValid)
        {
            throw new ArgumentException(
                "Status effect definition must have a valid id.",
                nameof(definition));
        }

        RemainingDurationTurns =
            definition.BaseDurationTurns;

        Stacks = 1;
    }

    public void RefreshDuration()
    {
        RemainingDurationTurns =
            Definition.BaseDurationTurns;
    }

    public bool TryAddStack()
    {
        if (Stacks >= Definition.MaxStacks)
        {
            return false;
        }

        Stacks++;

        return true;
    }

    public void AdvanceDuration()
    {
        if (IsExpired)
        {
            return;
        }

        RemainingDurationTurns--;
    }
}