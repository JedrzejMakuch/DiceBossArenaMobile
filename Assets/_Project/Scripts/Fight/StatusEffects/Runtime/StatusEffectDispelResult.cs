public readonly struct StatusEffectDispelResult
{
    public int RemovedCount { get; }

    public bool RemovedAny =>
        RemovedCount > 0;

    public StatusEffectDispelResult(
        int removedCount)
    {
        RemovedCount =
            removedCount;
    }
}