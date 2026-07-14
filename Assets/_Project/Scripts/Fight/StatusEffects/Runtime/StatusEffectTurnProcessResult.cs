public readonly struct StatusEffectTurnProcessResult
{
    public int EffectsExecuted { get; }

    public int EffectsExpired { get; }

    public StatusEffectTurnProcessResult(
        int effectsExecuted,
        int effectsExpired)
    {
        EffectsExecuted =
            effectsExecuted;

        EffectsExpired =
            effectsExpired;
    }
}