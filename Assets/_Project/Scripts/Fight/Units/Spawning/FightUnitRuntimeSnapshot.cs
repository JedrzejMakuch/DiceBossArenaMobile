using System;

public sealed class FightUnitRuntimeSnapshot :
    IEquatable<FightUnitRuntimeSnapshot>
{
    public static FightUnitRuntimeSnapshot Fresh =>
        new FightUnitRuntimeSnapshot();

    public bool HasCurrentHealth { get; }

    public int CurrentHealth { get; }

    private FightUnitRuntimeSnapshot()
    {
        HasCurrentHealth = false;
        CurrentHealth = 0;
    }

    public FightUnitRuntimeSnapshot(
        int currentHealth)
    {
        HasCurrentHealth = true;
        CurrentHealth =
            Math.Max(0, currentHealth);
    }

    public bool Equals(
        FightUnitRuntimeSnapshot other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return HasCurrentHealth ==
                   other.HasCurrentHealth &&
               CurrentHealth ==
                   other.CurrentHealth;
    }

    public override bool Equals(
        object obj)
    {
        return obj is FightUnitRuntimeSnapshot other &&
               Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            HasCurrentHealth,
            CurrentHealth);
    }
}