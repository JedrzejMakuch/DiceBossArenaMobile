using System;

public sealed class StatusEffectExecutionContext
{
    public FightUnit Owner { get; }

    public StatusEffectRuntimeState State { get; }

    public StatusEffectDefinition Definition =>
        State.Definition;

    public int Stacks =>
        State.Stacks;

    public StatusEffectExecutionContext(
        FightUnit owner,
        StatusEffectRuntimeState state)
    {
        Owner =
            owner ??
            throw new ArgumentNullException(
                nameof(owner));

        State =
            state ??
            throw new ArgumentNullException(
                nameof(state));
    }
}