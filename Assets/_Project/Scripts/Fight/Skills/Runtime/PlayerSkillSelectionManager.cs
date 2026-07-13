using System;
using UnityEngine;

public class PlayerSkillSelectionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightSkillTurnManager skillTurnManager;

    [Header("Default Selection")]
    [SerializeField] private bool autoSelectDefaultSkill = true;
    [SerializeField] private string defaultSkillId = "basic_attack";

    private FightUnit selectedCaster;
    private UnitSkillState selectedSkill;

    public FightUnit SelectedCaster => selectedCaster;
    public UnitSkillState SelectedSkill => selectedSkill;

    public bool HasSelectedSkill =>
        selectedCaster != null &&
        selectedSkill != null &&
        selectedSkill.Definition != null;

    public event Action<
        FightUnit,
        UnitSkillState> SkillSelected;

    public event Action SkillSelectionCleared;

    private void OnEnable()
    {
        if (skillTurnManager != null)
        {
            skillTurnManager.SkillTurnReady += HandleSkillTurnReady;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded += HandleTurnEnded;
            turnManager.CombatStopped += HandleCombatStopped;
        }
    }

    private void OnDisable()
    {
        if (skillTurnManager != null)
        {
            skillTurnManager.SkillTurnReady -= HandleSkillTurnReady;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded -= HandleTurnEnded;
            turnManager.CombatStopped -= HandleCombatStopped;
        }

        ClearSelection();
    }

    public bool TrySelectSkill(
        FightUnit caster,
        UnitSkillState skill)
    {
        if (caster == null ||
            skill == null ||
            skill.Definition == null)
        {
            return false;
        }

        if (!caster.IsAlive ||
            caster.Team != FightTeam.Player)
        {
            return false;
        }

        if (turnManager == null ||
            !turnManager.CombatRunning ||
            turnManager.ActiveUnit != caster)
        {
            return false;
        }

        FightUnitSkills unitSkills =
            caster.Skills;

        if (unitSkills == null ||
            unitSkills.GetSkillState(
                skill.Definition) != skill)
        {
            Debug.LogWarning(
                $"{caster.UnitName} does not own " +
                $"{skill.Definition.DisplayName}.",
                caster);

            return false;
        }

        if (!skill.IsReady)
        {
            Debug.LogWarning(
                $"{skill.Definition.DisplayName} is on cooldown. " +
                $"Remaining: {skill.CurrentCooldown}.",
                caster);

            return false;
        }

        FightUnitTurnResources resources =
            caster.TurnResources;

        if (resources == null)
        {
            return false;
        }

        SkillDefinition definition =
            skill.Definition;

        if (definition.ActionPointCost > 0 && !resources.CanSpendActionPoints(definition.ActionPointCost))
        {
            Debug.LogWarning(
                $"Not enough AP to select " +
                $"{definition.DisplayName}.",
                caster);

            return false;
        }

        if (definition.MovementPointCost > 0 && !resources.CanSpendMovementPoints(definition.MovementPointCost))
        {
            Debug.LogWarning(
                $"Not enough MP to select " +
                $"{definition.DisplayName}.",
                caster);

            return false;
        }

        selectedCaster = caster;
        selectedSkill = skill;

        SkillSelected?.Invoke(
            selectedCaster,
            selectedSkill);

        Debug.Log(
            $"{caster.UnitName} selected skill " +
            $"{definition.DisplayName}.",
            caster);

        return true;
    }

    public bool TrySelectSkillById(
        FightUnit caster,
        string skillId)
    {
        if (caster == null ||
            string.IsNullOrWhiteSpace(skillId))
        {
            return false;
        }

        FightUnitSkills unitSkills =
            caster.Skills;

        if (unitSkills == null)
        {
            return false;
        }

        UnitSkillState skill =
            unitSkills.GetSkillById(skillId);

        return TrySelectSkill(caster, skill);
    }

    public void ClearSelection()
    {
        bool hadSelection =
            selectedCaster != null ||
            selectedSkill != null;

        selectedCaster = null;
        selectedSkill = null;

        if (hadSelection)
        {
            SkillSelectionCleared?.Invoke();
        }
    }

    private void HandleSkillTurnReady(FightUnit unit)
    {
        ClearSelection();

        if (unit == null ||
            !unit.IsAlive ||
            unit.Team != FightTeam.Player)
        {
            return;
        }

        if (!autoSelectDefaultSkill)
        {
            return;
        }

        if (!TrySelectSkillById(
                unit,
                defaultSkillId))
        {
            Debug.LogWarning(
                $"PlayerSkillSelectionManager: " +
                $"could not select default skill " +
                $"'{defaultSkillId}' for {unit.UnitName}.",
                unit);
        }
    }

    private void HandleTurnEnded(
        FightUnit unit)
    {
        ClearSelection();
    }

    private void HandleCombatStopped(
        string reason)
    {
        ClearSelection();
    }
}