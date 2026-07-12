using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightSkillButtonView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text cooldownText;

    private UnitSkillState skillState;
    private Action<UnitSkillState> clickedCallback;

    public UnitSkillState SkillState => skillState;

    private void Awake()
    {
        if (button != null)
        {
            button.onClick.AddListener(HandleButtonClicked);
        }
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(HandleButtonClicked);
        }
    }

    public void Bind(
        UnitSkillState newSkillState,
        Action<UnitSkillState> newClickedCallback)
    {
        skillState = newSkillState;
        clickedCallback = newClickedCallback;

        Refresh(false);
    }

    public void Refresh(bool interactable)
    {
        SkillDefinition definition =
            skillState?.Definition;

        if (skillNameText != null)
        {
            skillNameText.text =
                definition != null
                    ? definition.DisplayName
                    : "EMPTY SKILL";
        }

        if (iconImage != null)
        {
            Sprite icon =
                definition != null
                    ? definition.Icon
                    : null;

            iconImage.sprite = icon;
            iconImage.enabled = icon != null;
        }

        if (cooldownText != null)
        {
            int currentCooldown =
                skillState != null
                    ? skillState.CurrentCooldown
                    : 0;

            cooldownText.text =
                currentCooldown > 0
                    ? $"CD: {currentCooldown}"
                    : string.Empty;
        }

        if (button != null)
        {
            button.interactable =
                skillState != null &&
                definition != null &&
                interactable;
        }
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