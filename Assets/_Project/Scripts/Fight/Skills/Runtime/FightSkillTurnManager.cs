using System;
using UnityEngine;

public class FightSkillTurnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnResourceManager turnResourceManager;

    public event Action<FightUnit> SkillTurnReady;

    private void OnEnable()
    {
        if (turnResourceManager != null)
        {
            turnResourceManager.TurnResourcesReady +=
                HandleTurnResourcesReady;
        }
    }

    private void OnDisable()
    {
        if (turnResourceManager != null)
        {
            turnResourceManager.TurnResourcesReady -=
                HandleTurnResourcesReady;
        }
    }

    private void HandleTurnResourcesReady(FightUnit unit)
    {
        if (unit == null || !unit.IsAlive)
        {
            return;
        }

        FightUnitSkills unitSkills =
            unit.GetComponent<FightUnitSkills>();

        if (unitSkills == null)
        {
            Debug.LogWarning(
                $"FightSkillTurnManager: " +
                $"{unit.UnitName} has no FightUnitSkills.",
                unit);

            SkillTurnReady?.Invoke(unit);
            return;
        }

        unitSkills.ReduceCooldowns();

        SkillTurnReady?.Invoke(unit);

        Debug.Log(
            $"FightSkillTurnManager: skill turn prepared " +
            $"for {unit.UnitName}.",
            unit);
    }
}