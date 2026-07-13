using System;
using UnityEngine;

namespace DiceBossArena.Tests.Fixtures
{
    public sealed class FightTestContext : IDisposable
    {
        public FightUnit Player { get; }
        public FightUnit Enemy { get; }
        public SkillDefinition PlayerSkill { get; }

        public FightTestContext(
            FightUnit player,
            FightUnit enemy,
            SkillDefinition playerSkill)
        {
            Player = player;
            Enemy = enemy;
            PlayerSkill = playerSkill;
        }

        public void Dispose()
        {
            DestroyUnit(Player);
            DestroyUnit(Enemy);

            if (PlayerSkill != null)
            {
                UnityEngine.Object.DestroyImmediate(PlayerSkill);
            }
        }

        private static void DestroyUnit(FightUnit unit)
        {
            if (unit != null)
            {
                UnityEngine.Object.DestroyImmediate(unit.gameObject);
            }
        }
    }
}