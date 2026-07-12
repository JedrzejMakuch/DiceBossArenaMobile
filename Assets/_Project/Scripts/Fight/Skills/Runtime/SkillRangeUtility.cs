using UnityEngine;

public static class SkillRangeUtility
{
    public static bool IsWithinRange(
        FightGridTile origin,
        FightGridTile target,
        SkillDefinition definition)
    {
        if (origin == null ||
            target == null ||
            definition == null)
        {
            return false;
        }

        int deltaX =
            Mathf.Abs(origin.GridX - target.GridX);

        int deltaY =
            Mathf.Abs(origin.GridY - target.GridY);

        int distance =
            deltaX + deltaY;

        if (distance < definition.MinRange ||
            distance > definition.MaxRange)
        {
            return false;
        }

        return definition.RangeShape switch
        {
            SkillRangeShape.Manhattan =>
                true,

            SkillRangeShape.StraightLine =>
                deltaX == 0 ||
                deltaY == 0,

            _ => false
        };
    }
}