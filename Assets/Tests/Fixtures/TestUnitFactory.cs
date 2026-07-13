using UnityEngine;
using UnityEngine.Rendering;

namespace DiceBossArena.Tests.Fixtures
{
    public static class TestUnitFactory
    {
        public static FightUnit Create(
            string unitName = "Test Unit",
            FightTeam team = FightTeam.Player,
            int maxHealth = 10,
            int attackPower = 2,
            int initiative = 10)
        {
            GameObject gameObject = new GameObject(unitName);
            FightUnit unit = gameObject.AddComponent<FightUnit>();

            unit.Initialize(
                unitName,
                team,
                maxHealth,
                attackPower,
                initiative);

            return unit;
        }
    }
}