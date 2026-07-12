using UnityEngine;

public class PlayerCombatInputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightTargetingManager targetingManager;
    [SerializeField] private FightCombatManager combatManager;

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
            combatManager == null)
        {
            return;
        }

        FightUnit attacker = turnManager.ActiveUnit;

        if (attacker == null ||
            attacker.Team != FightTeam.Player)
        {
            return;
        }

        bool attackExecuted =
            combatManager.TryExecuteBasicAttack(
            attacker,
            target);

        if (!attackExecuted)
        {
            return;
        }

        targetingManager.ClearTarget();
    }
}