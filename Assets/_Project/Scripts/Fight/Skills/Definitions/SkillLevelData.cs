using System;
using UnityEngine;

[Serializable]
public class SkillLevelData
{
    [SerializeField] private float powerMultiplier = 1f;
    [SerializeField] private int flatValueBonus;
    [SerializeField, Min(0)] private int cooldown;

    public float PowerMultiplier => powerMultiplier;
    public int FlatValueBonus => flatValueBonus;
    public int Cooldown => cooldown;

    public static SkillLevelData Default =>
        new SkillLevelData
        {
            powerMultiplier = 1f,
            flatValueBonus = 0,
            cooldown = 0
        };
}