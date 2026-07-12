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

    public static SkillDirection GetDirection(
    FightGridTile origin,
    FightGridTile directionTile)
    {
        if (origin == null ||
            directionTile == null)
        {
            return SkillDirection.None;
        }

        int deltaX =
            directionTile.GridX - origin.GridX;

        int deltaY =
            directionTile.GridY - origin.GridY;

        if (deltaX == 0 && deltaY == 1)
        {
            return SkillDirection.Up;
        }

        if (deltaX == 1 && deltaY == 0)
        {
            return SkillDirection.Right;
        }

        if (deltaX == 0 && deltaY == -1)
        {
            return SkillDirection.Down;
        }

        if (deltaX == -1 && deltaY == 0)
        {
            return SkillDirection.Left;
        }

        return SkillDirection.None;
    }

    public static bool IsDirectionSelectorTile(
        FightGridTile origin,
        FightGridTile target)
    {
        return GetDirection(origin, target) !=
               SkillDirection.None;
    }

    public static bool IsTileInCone(
        FightGridTile origin,
        FightGridTile target,
        SkillDirection direction,
        int maxRange)
    {
        if (origin == null ||
            target == null ||
            direction == SkillDirection.None ||
            maxRange <= 0)
        {
            return false;
        }

        int deltaX =
            target.GridX - origin.GridX;

        int deltaY =
            target.GridY - origin.GridY;

        int forwardDistance;
        int sideDistance;

        switch (direction)
        {
            case SkillDirection.Up:
                forwardDistance = deltaY;
                sideDistance = Mathf.Abs(deltaX);
                break;

            case SkillDirection.Right:
                forwardDistance = deltaX;
                sideDistance = Mathf.Abs(deltaY);
                break;

            case SkillDirection.Down:
                forwardDistance = -deltaY;
                sideDistance = Mathf.Abs(deltaX);
                break;

            case SkillDirection.Left:
                forwardDistance = -deltaX;
                sideDistance = Mathf.Abs(deltaY);
                break;

            default:
                return false;
        }

        if (forwardDistance < 1 ||
            forwardDistance > maxRange)
        {
            return false;
        }

        int allowedSideDistance =
            forwardDistance - 1;

        return sideDistance <=
               allowedSideDistance;
    }
}