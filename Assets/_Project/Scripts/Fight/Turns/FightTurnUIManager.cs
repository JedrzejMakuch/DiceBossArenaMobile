using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightTurnUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;

    [Header("UI")]
    [SerializeField] private TMP_Text turnInfoText;
    [SerializeField] private Button endTurnButton;

    private void Awake()
    {
        if (turnInfoText != null)
        {
            turnInfoText.text = "Waiting for combat";
        }

        if (endTurnButton != null)
        {
            endTurnButton.interactable = false;
            endTurnButton.onClick.AddListener(HandleEndTurnClicked);
        }
    }

    private void OnEnable()
    {
        if (turnManager == null)
        {
            return;
        }

        turnManager.TurnStarted += HandleTurnStarted;
        turnManager.CombatStopped += HandleCombatStopped;
    }

    private void OnDisable()
    {
        if (turnManager == null)
        {
            return;
        }

        turnManager.TurnStarted -= HandleTurnStarted;
        turnManager.CombatStopped -= HandleCombatStopped;
    }

    private void OnDestroy()
    {
        if (endTurnButton != null)
        {
            endTurnButton.onClick.RemoveListener(HandleEndTurnClicked);
        }
    }

    private void HandleTurnStarted(FightUnit unit, int roundNumber)
    {
        if (unit == null)
        {
            return;
        }

        if (turnInfoText != null)
        {
            turnInfoText.text =
                $"Round {roundNumber}\n" +
                $"Turn: {unit.UnitName}";
        }

        if (endTurnButton != null)
        {
            endTurnButton.interactable =
                unit.Team == FightTeam.Player;
        }
    }

    private void HandleEndTurnClicked()
    {
        if (turnManager == null)
        {
            return;
        }

        FightUnit activeUnit = turnManager.ActiveUnit;

        if (activeUnit == null ||
            activeUnit.Team != FightTeam.Player)
        {
            return;
        }

        turnManager.EndCurrentTurn();
    }

    private void HandleCombatStopped(string reason)
    {
        if (turnInfoText != null)
        {
            turnInfoText.text = "Combat stopped";
        }

        if (endTurnButton != null)
        {
            endTurnButton.interactable = false;
        }
    }
}