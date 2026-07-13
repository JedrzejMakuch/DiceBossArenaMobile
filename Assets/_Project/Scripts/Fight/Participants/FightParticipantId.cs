using System;
using UnityEngine;

[Serializable]
public struct FightParticipantId :
    IEquatable<FightParticipantId>
{
    [SerializeField] private string value;

    public string Value => value;
    public bool IsValid =>
        !string.IsNullOrWhiteSpace(value);

    public FightParticipantId(string value)
    {
        this.value =
            string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : value.Trim();
    }

    public bool Equals(
        FightParticipantId other)
    {
        return string.Equals(
            value,
            other.value,
            StringComparison.Ordinal);
    }

    public override bool Equals(object obj)
    {
        return obj is FightParticipantId other &&
               Equals(other);
    }

    public override int GetHashCode()
    {
        return value != null
            ? StringComparer.Ordinal.GetHashCode(value)
            : 0;
    }

    public override string ToString()
    {
        return value ?? string.Empty;
    }

    public static bool operator ==(
        FightParticipantId left,
        FightParticipantId right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(
        FightParticipantId left,
        FightParticipantId right)
    {
        return !left.Equals(right);
    }
}