using UnityEngine;

public class PlayerCombatInputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightTargetingManager targetingManager;
    [SerializeField] private FightSkillManager skillManager;

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

    private void HandleTargetSelected(FightUnit target)
    {
        if (turnManager == null ||
            targetingManager == null ||
            skillManager == null)
        {
            return;
        }

        FightUnit attacker = turnManager.ActiveUnit;

        if (attacker == null ||
            attacker.Team != FightTeam.Player)
        {
            return;
        }

        FightUnitSkills unitSkills =
            attacker.GetComponent<FightUnitSkills>();

        if (unitSkills == null)
        {
            Debug.LogError(
                $"{attacker.UnitName} has no FightUnitSkills.",
                attacker);

            return;
        }

        UnitSkillState basicAttack =
            unitSkills.GetSkillById("basic_attack");

        if (basicAttack == null)
        {
            Debug.LogError(
                $"{attacker.UnitName} has no Basic Attack.",
                attacker);

            return;
        }

        bool skillExecuted =
            skillManager.TryExecuteSkill(
                attacker,
                basicAttack,
                target);

        if (!skillExecuted)
        {
            return;
        }

        targetingManager.ClearTarget();
    }
}