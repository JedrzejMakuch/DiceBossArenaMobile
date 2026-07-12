using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightSkillUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightSkillTurnManager skillTurnManager;
    [SerializeField]
    private PlayerSkillSelectionManager skillSelectionManager;

    [Header("Panel")]
    [SerializeField] private GameObject skillPanel;

    [Header("Dynamic Skill Buttons")]
    [SerializeField] private Transform skillButtonsContainer;
    [SerializeField]
    private FightSkillButtonView skillButtonPrefab;

    [Header("Selection")]
    [SerializeField] private Button cancelSkillButton;
    [SerializeField] private TMP_Text selectedSkillText;

    private readonly List<FightSkillButtonView>
        skillButtonViews = new();

    private FightUnit currentPlayer;
    private FightUnitSkills currentPlayerSkills;

    private void OnEnable()
    {
        if (skillTurnManager != null)
        {
            skillTurnManager.SkillTurnReady +=
                HandleSkillTurnReady;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded += HandleTurnEnded;
            turnManager.CombatStopped +=
                HandleCombatStopped;
        }

        if (skillSelectionManager != null)
        {
            skillSelectionManager.SkillSelected +=
                HandleSkillSelected;

            skillSelectionManager.SkillSelectionCleared +=
                HandleSkillSelectionCleared;
        }

        if (cancelSkillButton != null)
        {
            cancelSkillButton.onClick.AddListener(
                HandleCancelSkillClicked);
        }

        ClearSkillButtons();
        HidePanel();
    }

    private void OnDisable()
    {
        if (skillTurnManager != null)
        {
            skillTurnManager.SkillTurnReady -=
                HandleSkillTurnReady;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded -= HandleTurnEnded;
            turnManager.CombatStopped -=
                HandleCombatStopped;
        }

        if (skillSelectionManager != null)
        {
            skillSelectionManager.SkillSelected -=
                HandleSkillSelected;

            skillSelectionManager.SkillSelectionCleared -=
                HandleSkillSelectionCleared;
        }

        if (cancelSkillButton != null)
        {
            cancelSkillButton.onClick.RemoveListener(
                HandleCancelSkillClicked);
        }

        ClearCurrentPlayer();
        ClearSkillButtons();
    }

    private void HandleSkillTurnReady(FightUnit unit)
    {
        ClearCurrentPlayer();
        ClearSkillButtons();

        if (unit == null ||
            !unit.IsAlive ||
            unit.Team != FightTeam.Player)
        {
            HidePanel();
            return;
        }

        currentPlayer = unit;
        currentPlayerSkills =
            unit.GetComponent<FightUnitSkills>();

        if (currentPlayerSkills == null)
        {
            HidePanel();

            Debug.LogWarning(
                $"{unit.name} has no FightUnitSkills component.",
                unit);

            return;
        }

        BuildSkillButtons();

        ShowPanel();
        RefreshUI();
    }

    private void HandleTurnEnded(FightUnit unit)
    {
        ClearCurrentPlayer();
        ClearSkillButtons();
        HidePanel();
    }

    private void HandleCombatStopped(string reason)
    {
        ClearCurrentPlayer();
        ClearSkillButtons();
        HidePanel();
    }

    private void BuildSkillButtons()
    {
        if (currentPlayerSkills == null ||
            skillButtonPrefab == null ||
            skillButtonsContainer == null)
        {
            return;
        }

        foreach (UnitSkillState skillState
                 in currentPlayerSkills.Skills)
        {
            if (skillState == null ||
                skillState.Definition == null)
            {
                continue;
            }

            FightSkillButtonView buttonView =
                Instantiate(
                    skillButtonPrefab,
                    skillButtonsContainer);

            buttonView.Bind(
                skillState,
                HandleSkillButtonClicked);

            skillButtonViews.Add(buttonView);
        }
    }

    private void ClearSkillButtons()
    {
        foreach (FightSkillButtonView buttonView
                 in skillButtonViews)
        {
            if (buttonView == null)
            {
                continue;
            }

            buttonView.Bind(null, null);
            Destroy(buttonView.gameObject);
        }

        skillButtonViews.Clear();
    }

    private void HandleSkillButtonClicked(
        UnitSkillState clickedSkillState)
    {
        if (currentPlayer == null ||
            clickedSkillState == null ||
            skillSelectionManager == null)
        {
            return;
        }

        skillSelectionManager.TrySelectSkill(
            currentPlayer,
            clickedSkillState);

        RefreshUI();
    }

    private void HandleCancelSkillClicked()
    {
        if (skillSelectionManager == null)
        {
            return;
        }

        skillSelectionManager.ClearSelection();
        RefreshUI();
    }

    private void HandleSkillSelected(
        FightUnit caster,
        UnitSkillState skill)
    {
        RefreshUI();
    }

    private void HandleSkillSelectionCleared()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (FightSkillButtonView buttonView
                 in skillButtonViews)
        {
            if (buttonView == null)
            {
                continue;
            }

            bool canSelect =
                CanSelectSkill(buttonView.SkillState);

            buttonView.Refresh(canSelect);
        }

        bool hasSelection =
            skillSelectionManager != null &&
            skillSelectionManager.HasSelectedSkill;

        if (cancelSkillButton != null)
        {
            cancelSkillButton.gameObject.SetActive(
                hasSelection);
        }

        if (selectedSkillText != null)
        {
            if (!hasSelection)
            {
                selectedSkillText.text =
                    "Movement mode";
            }
            else
            {
                SkillDefinition selectedDefinition =
                    skillSelectionManager
                        .SelectedSkill
                        .Definition;

                selectedSkillText.text =
                    $"Selected: " +
                    $"{selectedDefinition.DisplayName}";
            }
        }
    }

    private bool CanSelectSkill(
        UnitSkillState skillState)
    {
        if (currentPlayer == null ||
            skillState == null ||
            skillState.Definition == null ||
            !skillState.IsReady)
        {
            return false;
        }

        if (turnManager == null ||
            !turnManager.CombatRunning ||
            turnManager.ActiveUnit != currentPlayer)
        {
            return false;
        }

        FightUnitTurnResources resources =
            currentPlayer.GetComponent<
                FightUnitTurnResources>();

        if (resources == null)
        {
            return false;
        }

        SkillDefinition definition =
            skillState.Definition;

        if (definition.ActionPointCost > 0 &&
            !resources.CanSpendActionPoints(
                definition.ActionPointCost))
        {
            return false;
        }

        if (definition.MovementPointCost > 0 &&
            !resources.CanSpendMovementPoints(
                definition.MovementPointCost))
        {
            return false;
        }

        return true;
    }

    private void ClearCurrentPlayer()
    {
        currentPlayer = null;
        currentPlayerSkills = null;
    }

    private void ShowPanel()
    {
        if (skillPanel != null)
        {
            skillPanel.SetActive(true);
        }
    }

    private void HidePanel()
    {
        if (skillPanel != null)
        {
            skillPanel.SetActive(false);
        }
    }
}