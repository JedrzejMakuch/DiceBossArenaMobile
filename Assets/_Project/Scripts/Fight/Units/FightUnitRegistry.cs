using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FightUnitRegistry : MonoBehaviour
{
    private readonly List<FightUnit> units = new();

    public IReadOnlyList<FightUnit> Units => units;

    public event Action<FightUnit> UnitRegistered;
    public event Action<FightUnit> UnitUnregistered;

    private void OnDestroy()
    {
        DetachAllUnits();
    }

    public bool Register(FightUnit unit)
    {
        if (unit == null ||
            units.Contains(unit))
        {
            return false;
        }

        units.Add(unit);
        unit.Died += HandleUnitDied;

        UnitRegistered?.Invoke(unit);

        return true;
    }

    public bool Unregister(FightUnit unit)
    {
        if (unit == null ||
            !units.Remove(unit))
        {
            return false;
        }

        unit.Died -= HandleUnitDied;

        UnitUnregistered?.Invoke(unit);

        return true;
    }

    private void HandleUnitDied(FightUnit unit)
    {
        Unregister(unit);
    }

    public void DetachAllUnits()
    {
        foreach (FightUnit unit in units)
        {
            if (unit != null)
            {
                unit.Died -= HandleUnitDied;
            }
        }

        units.Clear();
    }
}