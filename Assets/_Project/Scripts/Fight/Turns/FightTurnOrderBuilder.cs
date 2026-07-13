using System.Collections.Generic;
using System.Linq;

public static class FightTurnOrderBuilder
{
    public static List<FightUnit> Build(
        IEnumerable<FightUnit> units)
    {
        if (units == null)
        {
            return new List<FightUnit>();
        }

        return units
            .Where(unit =>
                unit != null &&
                unit.IsAlive)
            .Distinct()
            .OrderByDescending(unit =>
                unit.Initiative)
            .ThenBy(unit =>
                unit.GetInstanceID())
            .ToList();
    }
}