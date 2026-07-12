using UnityEngine;

[CreateAssetMenu(
    fileName = "Effect_Dash_",
    menuName = "Dice Boss Arena/Skills/Effects/Dash")]
public class DashSkillEffectDefinition :
    SkillEffectDefinition
{
    public override bool CanApply(
        SkillExecutionContext context)
    {
        if (context == null ||
            context.Caster == null ||
            context.TargetTile == null)
        {
            return false;
        }

        if (!context.Caster.IsAlive ||
            context.Caster.CurrentTile == null)
        {
            return false;
        }

        if (context.TargetTile ==
            context.Caster.CurrentTile)
        {
            return false;
        }

        return context.TargetTile.CanBeOccupiedBy(
            context.Caster);
    }

    public override void Apply(
        SkillExecutionContext context)
    {
        if (!CanApply(context))
        {
            return;
        }

        FightGridTile previousTile =
            context.Caster.CurrentTile;

        bool moved =
            context.Caster.TryAssignToTile(
                context.TargetTile);

        if (!moved)
        {
            Debug.LogWarning(
                $"{context.Caster.UnitName} could not dash " +
                $"to ({context.TargetTile.GridX}, " +
                $"{context.TargetTile.GridY}).",
                context.Caster);

            return;
        }

        Debug.Log(
            $"{context.Caster.UnitName} dashed from " +
            $"({previousTile.GridX}, {previousTile.GridY}) " +
            $"to ({context.TargetTile.GridX}, " +
            $"{context.TargetTile.GridY}).",
            context.Caster);
    }
}