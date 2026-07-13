namespace DiceBossArena.Tests.Fixtures
{
    public sealed class FightTestBuilder
    {
        private string playerName = "Player";
        private int playerHealth = 20;
        private int playerAttack = 5;
        private int playerInitiative = 10;

        private string enemyName = "Enemy";
        private int enemyHealth = 12;
        private int enemyAttack = 3;
        private int enemyInitiative = 5;

        public FightTestContext Build()
        {
            FightUnit player = TestUnitFactory.Create(
                unitName: playerName,
                team: FightTeam.Player,
                maxHealth: playerHealth,
                attackPower: playerAttack,
                initiative: playerInitiative);

            FightUnit enemy = TestUnitFactory.Create(
                unitName: enemyName,
                team: FightTeam.Enemy,
                maxHealth: enemyHealth,
                attackPower: enemyAttack,
                initiative: enemyInitiative);

            SkillDefinition playerSkill = TestSkillFactory.Create(
                skillId: "basic_attack",
                displayName: "Basic Attack",
                targetType: SkillTargetType.SingleEnemy,
                rangeShape: SkillRangeShape.Manhattan,
                minRange: 1,
                maxRange: 1,
                actionPointCost: 1);

            return new FightTestContext(
                player,
                enemy,
                playerSkill);
        }
    }
}