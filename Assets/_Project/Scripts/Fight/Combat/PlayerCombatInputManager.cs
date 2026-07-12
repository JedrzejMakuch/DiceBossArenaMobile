using UnityEngine;

public class PlayerCombatInputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightTargetingManager targetingManager;
    [SerializeField] private FightSkillManager skillManager;
    [SerializeField] private PlayerSkillSelectionManager skillSelectionManager;
    [SerializeField] private FightSkillRangeManager skillRangeManager;

    private void OnEnable()
    {
        if (targetingManager != null)
        {
            targetingManager.TargetSelected +=
                HandleTargetSelected;
        }
    }

    private void OnDisable()
    {
        if (targetingManager != null)
        {
            targetingManager.TargetSelected -=
                HandleTargetSelected;
        }
    }

    private void HandleTargetSelected(
        FightUnit target)
    {
        if (turnManager == null ||
            targetingManager == null ||
            skillManager == null ||
            skillSelectionManager == null)
        {
            return;
        }

        FightUnit attacker = turnManager.ActiveUnit;

        if (attacker == null ||
            attacker.Team != FightTeam.Player)
        {
            return;
        }

        if (!skillSelectionManager.HasSelectedSkill)
        {
            Debug.Log(
                "PlayerCombatInputManager: " +
                "no skill is currently selected.");

            return;
        }

        if (skillSelectionManager.SelectedCaster !=
            attacker)
        {
            Debug.LogWarning(
                "PlayerCombatInputManager: " +
                "selected skill belongs to another unit.",
                attacker);

            skillSelectionManager.ClearSelection();
            return;
        }

        UnitSkillState selectedSkill = skillSelectionManager.SelectedSkill;

        if (skillRangeManager == null || !skillRangeManager.IsUnitInCurrentRange(target))
        {
            Debug.Log(
                $"{target.UnitName} is outside the selected skill range. " +
                $"Skill selection cancelled.",
                target);

            targetingManager.ClearTarget();
            skillSelectionManager.ClearSelection();

            return;
        }

        bool skillExecuted =
            skillManager.TryExecuteSkill(
                attacker,
                selectedSkill,
                target);

        if (!skillExecuted)
        {
            return;
        }

        targetingManager.ClearTarget();

        // Po wykonaniu skill nie pozostaje zaznaczony.
        // UI później będzie mogło ponownie wybrać kolejny skill,
        // jeżeli jednostka nadal ma AP.
        skillSelectionManager.ClearSelection();
    }
}