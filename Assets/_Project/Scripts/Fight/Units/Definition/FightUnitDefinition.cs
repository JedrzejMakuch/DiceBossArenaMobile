using UnityEngine;

[CreateAssetMenu(
    fileName = "Unit_",
    menuName = "Dice Boss Arena/Units/Unit Definition")]
public sealed class FightUnitDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string unitName = "Unit";
    [SerializeField] private FightTeam team;

    [Header("Combat Stats")]
    [SerializeField, Min(1)] private int maxHealth = 10;
    [SerializeField, Min(0)] private int attackPower = 2;
    [SerializeField, Min(0)] private int initiative = 10;

    public string UnitName => unitName;
    public FightTeam Team => team;

    public int MaxHealth => maxHealth;
    public int AttackPower => attackPower;
    public int Initiative => initiative;

    public void InitializeForTests(
        string newUnitName,
        FightTeam newTeam,
        int newMaxHealth,
        int newAttackPower,
        int newInitiative)
    {
        unitName = newUnitName;
        team = newTeam;
        maxHealth = Mathf.Max(1, newMaxHealth);
        attackPower = Mathf.Max(0, newAttackPower);
        initiative = Mathf.Max(0, newInitiative);
    }

    private void OnValidate()
    {
        maxHealth = Mathf.Max(1, maxHealth);
        attackPower = Mathf.Max(0, attackPower);
        initiative = Mathf.Max(0, initiative);
    }
}