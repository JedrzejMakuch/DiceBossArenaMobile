using System;
using System.Collections.Generic;
using UnityEngine;

public class FightSkillManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;

    public event Action<SkillExecutionContext> SkillExecuted;

    public bool TryExecuteSkill(
        FightUnit caster,
        UnitSkillState skillState,
        FightUnit primaryTarget = null,
        FightGridTile targetTile = null,
        IReadOnlyList<FightUnit> affectedUnits = null)
    {
        if (!CanExecuteSkill(
                caster,
                skillState,
                primaryTarget,
                targetTile,
                affectedUnits,
                out SkillExecutionContext context))
        {
            return false;
        }

        SkillDefinition skill = skillState.Definition;

        FightUnitTurnResources resources =
            caster.TurnResources;

        if (!resources.TrySpendActionPoints(
                skill.ActionPointCost))
        {
            return false;
        }

        if (skill.MovementPointCost > 0 &&
            !resources.TrySpendMovementPoints(
                skill.MovementPointCost))
        {
            Debug.LogError(
                $"FightSkillManager: failed to spend MP for " +
                $"{skill.DisplayName} after validation.",
                caster);

            return false;
        }

        foreach (SkillEffectDefinition effect in skill.Effects)
        {
            if (effect == null)
            {
                continue;
            }

            effect.Apply(context);
        }

        skillState.TryStartCooldown();

        SkillExecuted?.Invoke(context);

        Debug.Log(
            $"{caster.UnitName} used {skill.DisplayName}. " +
            $"AP left: {resources.CurrentActionPoints}. " +
            $"MP left: {resources.CurrentMovementPoints}. " +
            $"Cooldown: {skillState.CurrentCooldown}.",
            caster);

        return true;
    }

    public bool CanExecuteSkill(
        FightUnit caster,
        UnitSkillState skillState,
        FightUnit primaryTarget,
        FightGridTile targetTile,
        IReadOnlyList<FightUnit> affectedUnits,
        out SkillExecutionContext context)
    {
        context = null;

        if (caster == null ||
            skillState == null ||
            skillState.Definition == null)
        {
            return false;
        }

        if (!caster.IsAlive)
        {
            return false;
        }

        if (turnManager == null ||
            !turnManager.CombatRunning ||
            turnManager.ActiveUnit != caster)
        {
            Debug.LogWarning(
                $"{caster.UnitName} cannot use a skill " +
                $"outside its active turn.",
                caster);

            return false;
        }

        FightUnitSkills unitSkills =
            caster.Skills;

        if (unitSkills == null ||
            unitSkills.GetSkillState(skillState.Definition) != skillState)
        {
            Debug.LogWarning(
                $"{caster.UnitName} does not own " +
                $"{skillState.Definition.DisplayName}.",
                caster);

            return false;
        }

        if (!skillState.IsReady)
        {
            Debug.LogWarning(
                $"{skillState.Definition.DisplayName} is on cooldown. " +
                $"Remaining: {skillState.CurrentCooldown}.",
                caster);

            return false;
        }

        FightUnitTurnResources resources =
            caster.TurnResources;

        if (resources == null)
        {
            Debug.LogError(
                $"{caster.UnitName} has no FightUnitTurnResources.",
                caster);

            return false;
        }

        SkillDefinition skill = skillState.Definition;

        if (skill.ActionPointCost > 0 &&
            !resources.CanSpendActionPoints(
                skill.ActionPointCost))
        {
            return false;
        }

        if (skill.MovementPointCost > 0 &&
            !resources.CanSpendMovementPoints(
                skill.MovementPointCost))
        {
            return false;
        }

        if (!IsValidTarget(
                caster,
                skill,
                primaryTarget,
                targetTile))
        {
            return false;
        }

        List<FightUnit> resolvedAffectedUnits =
            affectedUnits != null
                ? new List<FightUnit>(affectedUnits)
                : new List<FightUnit>();

        if (primaryTarget != null &&
            !resolvedAffectedUnits.Contains(primaryTarget))
        {
            resolvedAffectedUnits.Add(primaryTarget);
        }

        context = new SkillExecutionContext(
            skill,
            skillState.Level,
            caster,
            primaryTarget,
            targetTile,
            resolvedAffectedUnits);

        foreach (SkillEffectDefinition effect in skill.Effects)
        {
            if (effect == null)
            {
                continue;
            }

            if (!effect.CanApply(context))
            {
                Debug.LogWarning(
                    $"Effect {effect.name} cannot be applied " +
                    $"for skill {skill.DisplayName}.",
                    caster);

                context = null;
                return false;
            }
        }

        return true;
    }

    private bool IsValidTarget(
        FightUnit caster,
        SkillDefinition skill,
        FightUnit primaryTarget,
        FightGridTile targetTile)
    {
        if (caster.CurrentTile == null)
        {
            return false;
        }

        switch (skill.TargetType)
        {
            case SkillTargetType.Self:
                return primaryTarget == caster &&
                       IsWithinRange(
                           caster.CurrentTile,
                           caster.CurrentTile,
                           skill);

            case SkillTargetType.SingleEnemy:
                return primaryTarget != null &&
                       primaryTarget.IsAlive &&
                       primaryTarget.Team != caster.Team &&
                       IsWithinRange(
                           caster.CurrentTile,
                           primaryTarget.CurrentTile,
                           skill);

            case SkillTargetType.SingleAlly:
                return primaryTarget != null &&
                       primaryTarget.IsAlive &&
                       primaryTarget.Team == caster.Team &&
                       IsWithinRange(
                           caster.CurrentTile,
                           primaryTarget.CurrentTile,
                           skill);

            case SkillTargetType.AnyUnit:
                return primaryTarget != null &&
                       primaryTarget.IsAlive &&
                       IsWithinRange(
                           caster.CurrentTile,
                           primaryTarget.CurrentTile,
                           skill);

            case SkillTargetType.Tile:
                return targetTile != null &&
                       IsWithinRange(
                           caster.CurrentTile,
                           targetTile,
                           skill);

            case SkillTargetType.Area:
                if (targetTile == null)
                {
                    return false;
                }

                if (skill.RangeShape ==
                    SkillRangeShape.Cone)
                {
                    return SkillRangeUtility
                        .IsDirectionSelectorTile(
                            caster.CurrentTile,
                            targetTile);
                }

                return IsWithinRange(
                    caster.CurrentTile,
                    targetTile,
                    skill);

            default:
                return false;
        }
    }

    private bool IsWithinRange(
   FightGridTile origin,
   FightGridTile destination,
   SkillDefinition skill)
    {
        return SkillRangeUtility.IsWithinRange(
            origin,
            destination,
            skill);
    }
}