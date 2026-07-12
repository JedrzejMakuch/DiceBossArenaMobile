using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightSkillButtonView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text statusText;

    [Header("Selection Visual")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color selectedColor = new Color(1f, 0.8f, 0.2f, 1f);

    private UnitSkillState skillState;
    private Action<UnitSkillState> clickedCallback;

    public UnitSkillState SkillState => skillState;

    private void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(
                HandleButtonClicked);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(
                HandleButtonClicked);
        }
    }

    public void Bind(
        UnitSkillState newSkillState,
        Action<UnitSkillState> newClickedCallback)
    {
        skillState = newSkillState;
        clickedCallback = newClickedCallback;

        Refresh(false);
        SetSelected(false);
    }

    public void Refresh(bool interactable)
    {
        SkillDefinition definition =
            skillState?.Definition;

        RefreshName(definition);
        RefreshIcon(definition);
        RefreshCost(definition);
        RefreshStatus(definition, interactable);

        if (button != null)
        {
            button.interactable =
                skillState != null &&
                definition != null &&
                interactable;
        }
    }

    private void RefreshName(
        SkillDefinition definition)
    {
        if (skillNameText == null)
        {
            return;
        }

        skillNameText.text =
            definition != null
                ? definition.DisplayName
                : "EMPTY SKILL";
    }

    private void RefreshIcon(
        SkillDefinition definition)
    {
        if (iconImage == null)
        {
            return;
        }

        Sprite icon =
            definition != null
                ? definition.Icon
                : null;

        iconImage.sprite = icon;
        iconImage.enabled = icon != null;
    }

    private void RefreshCost(
        SkillDefinition definition)
    {
        if (costText == null)
        {
            return;
        }

        if (definition == null)
        {
            costText.text = string.Empty;
            return;
        }

        int actionPointCost =
            definition.ActionPointCost;

        int movementPointCost =
            definition.MovementPointCost;

        if (actionPointCost <= 0 &&
            movementPointCost <= 0)
        {
            costText.text = "FREE";
            return;
        }

        if (actionPointCost > 0 &&
            movementPointCost > 0)
        {
            costText.text =
                $"{actionPointCost} AP / " +
                $"{movementPointCost} MP";

            return;
        }

        if (actionPointCost > 0)
        {
            costText.text =
                $"{actionPointCost} AP";

            return;
        }

        costText.text =
            $"{movementPointCost} MP";
    }

    private void RefreshStatus(
        SkillDefinition definition,
        bool interactable)
    {
        if (statusText == null)
        {
            return;
        }

        if (skillState == null ||
            definition == null)
        {
            statusText.text = string.Empty;
            return;
        }

        if (skillState.CurrentCooldown > 0)
        {
            statusText.text =
                $"CD: {skillState.CurrentCooldown}";

            return;
        }

        statusText.text =
            interactable
                ? "READY"
                : "UNAVAILABLE";
    }

    private void HandleButtonClicked()
    {
        if (skillState == null ||
            skillState.Definition == null)
        {
            return;
        }

        clickedCallback?.Invoke(skillState);
    }

    public void SetSelected(bool selected)
    {
        if (backgroundImage == null)
        {
            return;
        }

        backgroundImage.color =
            selected
                ? selectedColor
                : normalColor;
    }
}