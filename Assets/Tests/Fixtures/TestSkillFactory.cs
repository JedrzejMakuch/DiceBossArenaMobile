using UnityEngine;

namespace DiceBossArena.Tests.Fixtures
{
    public static class TestSkillFactory
    {
        public static SkillDefinition Create()
        {
            SkillDefinition skill =
                ScriptableObject.CreateInstance<SkillDefinition>();

            skill.name = "Test Skill";

            return skill;
        }
    }
}