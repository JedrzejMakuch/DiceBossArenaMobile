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

    [Header("Temporary Single Skill Slot")]
    [SerializeField] private FightSkillButtonView basicAttackButtonView;

    [Header("Selection")]
    [SerializeField] private Button cancelSkillButton;
    [SerializeField] private TMP_Text selectedSkillText;

    private FightUnit currentPlayer;
    private UnitSkillState basicAttackState;

    private void OnEnable()
    {
        if (skillTurnManager != null)
        {
            skillTurnManager.SkillTurnReady += HandleSkillTurnReady;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded += HandleTurnEnded;
            turnManager.CombatStopped += HandleCombatStopped;
        }

        if (skillSelectionManager != null)
        {
            skillSelectionManager.SkillSelected += HandleSkillSelected;

            skillSelectionManager.SkillSelectionCleared += HandleSkillSelectionCleared;
        }

        if (cancelSkillButton != null)
        {
            cancelSkillButton.onClick.AddListener(HandleCancelSkillClicked);
        }

        HidePanel();
    }

    private void OnDisable()
    {
        if (skillTurnManager != null)
        {
            skillTurnManager.SkillTurnReady -= HandleSkillTurnReady;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded -= HandleTurnEnded;
            turnManager.CombatStopped -= HandleCombatStopped;
        }

        if (skillSelectionManager != null)
        {
            skillSelectionManager.SkillSelected -= HandleSkillSelected;

            skillSelectionManager.SkillSelectionCleared -= HandleSkillSelectionCleared;
        }

        if (cancelSkillButton != null)
        {
            cancelSkillButton.onClick.RemoveListener(HandleCancelSkillClicked);
        }

        if (basicAttackButtonView != null)
        {
            basicAttackButtonView.Bind(null, null);
        }
    }

    private void HandleSkillTurnReady(FightUnit unit)
    {
        currentPlayer = null;
        basicAttackState = null;

        if (unit == null ||
            !unit.IsAlive ||
            unit.Team != FightTeam.Player)
        {
            HidePanel();
            return;
        }

        currentPlayer = unit;

        FightUnitSkills unitSkills = unit.GetComponent<FightUnitSkills>();

        if (unitSkills != null)
        {
            basicAttackState =
                unitSkills.GetSkillById("basic_attack");
        }

        if (basicAttackButtonView != null)
        {
            basicAttackButtonView.Bind(
                basicAttackState,
                HandleBasicAttackClicked);
        }

        ShowPanel();
        RefreshUI();
    }

    private void HandleTurnEnded(FightUnit unit)
    {
        currentPlayer = null;
        basicAttackState = null;

        HidePanel();
    }

    private void HandleCombatStopped(string reason)
    {
        currentPlayer = null;
        basicAttackState = null;

        HidePanel();
    }

    private void HandleBasicAttackClicked(
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
        bool canSelectBasicAttack = CanSelectBasicAttack();

        if (basicAttackButtonView != null)
        {
            basicAttackButtonView.Refresh(
                canSelectBasicAttack);
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
                    $"Selected: {selectedDefinition.DisplayName}";
            }
        }
    }

    private bool CanSelectBasicAttack()
    {
        if (currentPlayer == null ||
            basicAttackState == null ||
            basicAttackState.Definition == null ||
            !basicAttackState.IsReady)
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
            basicAttackState.Definition;

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