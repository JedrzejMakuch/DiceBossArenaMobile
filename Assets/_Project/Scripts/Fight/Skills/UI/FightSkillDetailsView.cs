using TMPro;
using UnityEngine;

public class FightSkillDetailsView : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text cooldownText;
    [SerializeField] private TMP_Text powerText;

    public void Show(UnitSkillState skillState)
    {
        if (skillState == null ||
            skillState.Definition == null)
        {
            Hide();
            return;
        }

        SkillDefinition definition =
            skillState.Definition;

        SkillLevelData levelData =
            skillState.LevelData;

        if (detailsPanel != null)
        {
            detailsPanel.SetActive(true);
        }

        if (skillNameText != null)
        {
            skillNameText.text =
                definition.DisplayName;
        }

        if (levelText != null)
        {
            levelText.text =
                $"Level {skillState.Level}";
        }

        if (descriptionText != null)
        {
            descriptionText.text =
                string.IsNullOrWhiteSpace(
                    definition.Description)
                    ? "No description."
                    : definition.Description;
        }

        if (rangeText != null)
        {
            rangeText.text =
                BuildRangeText(definition);
        }

        if (costText != null)
        {
            costText.text =
                BuildCostText(definition);
        }

        if (cooldownText != null)
        {
            cooldownText.text =
                BuildCooldownText(
                    skillState,
                    levelData);
        }

        if (powerText != null)
        {
            powerText.text =
                BuildPowerText(levelData);
        }
    }

    public void Hide()
    {
        if (detailsPanel != null)
        {
            detailsPanel.SetActive(false);
        }
    }

    private string BuildRangeText(
        SkillDefinition definition)
    {
        if (definition.MinRange ==
            definition.MaxRange)
        {
            return
                $"Range: {definition.MaxRange}";
        }

        return
            $"Range: {definition.MinRange}" +
            $"–{definition.MaxRange}";
    }

    private string BuildCostText(
        SkillDefinition definition)
    {
        int actionPointCost =
            definition.ActionPointCost;

        int movementPointCost =
            definition.MovementPointCost;

        if (actionPointCost <= 0 &&
            movementPointCost <= 0)
        {
            return "Cost: Free";
        }

        if (actionPointCost > 0 &&
            movementPointCost > 0)
        {
            return
                $"Cost: {actionPointCost} AP / " +
                $"{movementPointCost} MP";
        }

        if (actionPointCost > 0)
        {
            return
                $"Cost: {actionPointCost} AP";
        }

        return
            $"Cost: {movementPointCost} MP";
    }

    private string BuildCooldownText(
        UnitSkillState skillState,
        SkillLevelData levelData)
    {
        if (skillState.CurrentCooldown > 0)
        {
            return
                $"Cooldown: " +
                $"{skillState.CurrentCooldown} " +
                $"remaining / {levelData.Cooldown}";
        }

        if (levelData.Cooldown <= 0)
        {
            return "Cooldown: None";
        }

        return
            $"Cooldown: {levelData.Cooldown}";
    }

    private string BuildPowerText(
        SkillLevelData levelData)
    {
        int percentage =
            Mathf.RoundToInt(
                levelData.PowerMultiplier * 100f);

        if (levelData.FlatValueBonus == 0)
        {
            return $"Power: {percentage}%";
        }

        string bonusSign =
            levelData.FlatValueBonus > 0
                ? "+"
                : string.Empty;

        return
            $"Power: {percentage}% " +
            $"{bonusSign}" +
            $"{levelData.FlatValueBonus}";
    }
}