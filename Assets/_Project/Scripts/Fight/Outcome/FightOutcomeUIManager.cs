using TMPro;
using UnityEngine;

public class FightOutcomeUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightOutcomeManager outcomeManager;

    [Header("UI")]
    [SerializeField] private GameObject outcomePanel;
    [SerializeField] private TMP_Text outcomeText;

    private void Awake()
    {
        if (outcomePanel != null)
        {
            outcomePanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (outcomeManager != null)
        {
            outcomeManager.OutcomeResolved += HandleOutcomeResolved;
        }
    }

    private void OnDisable()
    {
        if (outcomeManager != null)
        {
            outcomeManager.OutcomeResolved -= HandleOutcomeResolved;
        }
    }

    private void HandleOutcomeResolved(FightOutcome outcome)
    {
        if (outcomePanel != null)
        {
            outcomePanel.SetActive(true);
        }

        if (outcomeText == null)
        {
            return;
        }

        outcomeText.text = outcome switch
        {
            FightOutcome.Victory => "VICTORY",
            FightOutcome.Defeat => "DEFEAT",
            _ => string.Empty
        };
    }
}