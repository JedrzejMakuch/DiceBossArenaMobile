using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightTurnUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightActionExecutor actionExecutor;

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
        if (turnManager == null ||
    actionExecutor == null)
        {
            return;
        }

        turnManager.TurnStarted += HandleTurnStarted;
        turnManager.CombatStopped += HandleCombatStopped;
    }

    private void OnDisable()
    {
        if (turnManager == null ||
    actionExecutor == null)
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

    public static bool CanLocalPlayerEndTurn(
    FightUnit unit)
    {
        return unit != null &&
               unit.IsAlive &&
               unit.IsControlledBy(
                   FightControllerType.LocalPlayer);
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
                CanLocalPlayerEndTurn(unit);
        }
    }

    private void HandleEndTurnClicked()
    {
        if (turnManager == null ||
            actionExecutor == null)
        {
            return;
        }

        FightUnit activeUnit =
            turnManager.ActiveUnit;

        if (!CanLocalPlayerEndTurn(activeUnit))
        {
            return;
        }

        FightEndTurnActionRequest request =
            new FightEndTurnActionRequest(
                activeUnit);

        actionExecutor.TryExecute(request);
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