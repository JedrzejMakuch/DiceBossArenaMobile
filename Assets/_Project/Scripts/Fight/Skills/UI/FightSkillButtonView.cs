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

    [Header("Background Visual")]
    [SerializeField] private Color normalBackgroundColor = Color.white;
    [SerializeField] private Color selectedBackgroundColor = new Color(1f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color unavailableBackgroundColor = new Color(0.35f, 0.35f, 0.35f, 1f);

    [Header("Content Visual")]
    [SerializeField] private Color availableIconColor = Color.white;
    [SerializeField] private Color availableTextColor = Color.black;
    [SerializeField] private Color unavailableContentColor = new Color(0.65f, 0.65f, 0.65f, 1f);

    private UnitSkillState skillState;
    private Action<UnitSkillState> clickedCallback;

    private bool isAvailable;
    private bool isSelected;

    public UnitSkillState SkillState => skillState;

    private void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(
                HandleButtonClicked);
        }

        if (iconImage != null)
        {
            iconImage.preserveAspect = true;
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

        isAvailable = false;
        isSelected = false;

        Refresh(false);
    }

    public void Refresh(bool available)
    {
        SkillDefinition definition =
            skillState?.Definition;

        isAvailable = available;

        RefreshName(definition);
        RefreshIcon(definition);
        RefreshVisualState();

        // Przycisk pozostaje klikalny, żeby można było
        // obejrzeć details skilla na cooldownie.
        if (button != null)
        {
            button.interactable =
                skillState != null &&
                definition != null;
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        RefreshVisualState();
    }

    private void RefreshVisualState()
    {
        if (backgroundImage != null)
        {
            if (!isAvailable)
            {
                backgroundImage.color =
                    unavailableBackgroundColor;
            }
            else if (isSelected)
            {
                backgroundImage.color =
                    selectedBackgroundColor;
            }
            else
            {
                backgroundImage.color =
                    normalBackgroundColor;
            }
        }

        if (iconImage != null)
        {
            iconImage.color =
                isAvailable
                    ? availableIconColor
                    : unavailableContentColor;
        }

        if (skillNameText != null)
        {
            skillNameText.color =
                isAvailable
                    ? availableTextColor
                    : unavailableContentColor;
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
                : string.Empty;
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
        iconImage.preserveAspect = true;
        iconImage.enabled = icon != null;
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
}