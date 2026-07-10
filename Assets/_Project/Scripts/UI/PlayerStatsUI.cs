using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text goldText;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        hpText.text = $"HP: {playerStats.CurrentHp} / {playerStats.MaxHp}";
        goldText.text = $"Gold: {playerStats.Gold}";
    }
}