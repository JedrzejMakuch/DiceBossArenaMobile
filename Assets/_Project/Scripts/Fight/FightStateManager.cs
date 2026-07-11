using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum FightState
{
    Loading,
    Deployment,
    ReadyToStart,
    CombatStarted
}

public class FightStateManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text fightInfoText;
    [SerializeField] private Button startFightButton;

    [Header("Managers References")]
    [SerializeField] private FightDeploymentManager deploymentManager;
    [SerializeField] private FightTurnManager turnManager;

    public FightState CurrentState { get; private set; }

    private void Start()
    {
        SetState(FightState.Deployment);

        if (startFightButton != null)
        {
            startFightButton.interactable = false;
            startFightButton.onClick.AddListener(StartFight);
        }
        else
        {
            Debug.LogError(
                "FightStateManager: Start Fight Button is not assigned.",
                this);
        }
    }

    private void OnDestroy()
    {
        if (startFightButton != null)
        {
            startFightButton.onClick.RemoveListener(StartFight);
        }
    }

    public void SetReadyToStart()
    {
        SetState(FightState.ReadyToStart);

        if (startFightButton != null)
        {
            startFightButton.interactable = true;
        }
    }

    private void StartFight()
    {
        if (CurrentState != FightState.ReadyToStart)
        {
            Debug.Log(
                "Cannot start fight. Player has not selected a starting tile.");

            return;
        }

        if (deploymentManager == null)
        {
            Debug.LogError(
                "FightStateManager: Deployment Manager is not assigned.",
                this);

            return;
        }

        if (turnManager == null)
        {
            Debug.LogError(
                "FightStateManager: Fight Turn Manager is not assigned.",
                this);

            return;
        }

        deploymentManager.LockDeployment();

        SetState(FightState.CombatStarted);

        turnManager.StartCombat();

        if (startFightButton != null)
        {
            startFightButton.interactable = false;
        }

        Debug.Log("Fight started.");
    }

    private void SetState(FightState newState)
    {
        CurrentState = newState;
        UpdateInfoText();
    }

    private void UpdateInfoText()
    {
        if (fightInfoText == null)
        {
            return;
        }

        fightInfoText.text = CurrentState switch
        {
            FightState.Loading => "Loading fight...",
            FightState.Deployment => "Choose starting tile",
            FightState.ReadyToStart => "Ready to start",
            FightState.CombatStarted => "Fight started",
            _ => CurrentState.ToString()
        };
    }
}