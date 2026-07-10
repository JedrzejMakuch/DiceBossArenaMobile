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

    public FightState CurrentState { get; private set; }

    private void Start()
    {
        SetState(FightState.Deployment);

        if (startFightButton != null)
        {
            startFightButton.onClick.AddListener(StartFight);
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
    }

    private void StartFight()
    {
        if (CurrentState != FightState.ReadyToStart)
        {
            Debug.Log("Cannot start fight. Player has not selected a starting tile.");
            return;
        }

        if (deploymentManager != null)
        {
            deploymentManager.LockDeployment();
        }

        SetState(FightState.CombatStarted);

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