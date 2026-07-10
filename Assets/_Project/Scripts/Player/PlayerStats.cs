using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;
    [SerializeField] private int currentHp = 10;
    [SerializeField] private int gold = 0;

    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    public int Gold => gold;
    public bool IsDead => currentHp <= 0;

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp < 0)
            currentHp = 0;
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if (currentHp > maxHp)
            currentHp = maxHp;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }
}