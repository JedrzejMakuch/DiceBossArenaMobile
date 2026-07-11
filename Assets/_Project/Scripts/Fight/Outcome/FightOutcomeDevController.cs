using UnityEngine;
using UnityEngine.UI;

public class FightOutcomeDevController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightOutcomeManager outcomeManager;

    [Header("Dev Buttons")]
    [SerializeField] private Button victoryButton;
    [SerializeField] private Button defeatButton;

    private void Awake()
    {
        if (victoryButton != null)
        {
            victoryButton.onClick.AddListener(HandleVictoryClicked);
        }

        if (defeatButton != null)
        {
            defeatButton.onClick.AddListener(HandleDefeatClicked);
        }
    }

    private void OnDestroy()
    {
        if (victoryButton != null)
        {
            victoryButton.onClick.RemoveListener(HandleVictoryClicked);
        }

        if (defeatButton != null)
        {
            defeatButton.onClick.RemoveListener(HandleDefeatClicked);
        }
    }

    private void HandleVictoryClicked()
    {
        if (outcomeManager != null)
        {
            outcomeManager.ForceVictory();
        }
    }

    private void HandleDefeatClicked()
    {
        if (outcomeManager != null)
        {
            outcomeManager.ForceDefeat();
        }
    }
}