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

        private string skillId = "basic_attack";
        private string skillName = "Basic Attack";
        private int skillMinRange = 1;
        private int skillMaxRange = 1;
        private int skillActionPointCost = 1;

        public FightTestBuilder WithPlayer(
            string unitName = "Player",
            int maxHealth = 20,
            int attackPower = 5,
            int initiative = 10)
        {
            playerName = unitName;
            playerHealth = maxHealth;
            playerAttack = attackPower;
            playerInitiative = initiative;

            return this;
        }

        public FightTestBuilder WithEnemy(
            string unitName = "Enemy",
            int maxHealth = 12,
            int attackPower = 3,
            int initiative = 5)
        {
            enemyName = unitName;
            enemyHealth = maxHealth;
            enemyAttack = attackPower;
            enemyInitiative = initiative;

            return this;
        }

        public FightTestBuilder WithPlayerSkill(
            string newSkillId = "basic_attack",
            string displayName = "Basic Attack",
            int minRange = 1,
            int maxRange = 1,
            int actionPointCost = 1)
        {
            skillId = newSkillId;
            skillName = displayName;
            skillMinRange = minRange;
            skillMaxRange = maxRange;
            skillActionPointCost = actionPointCost;

            return this;
        }

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
                skillId: skillId,
                displayName: skillName,
                targetType: SkillTargetType.SingleEnemy,
                rangeShape: SkillRangeShape.Manhattan,
                minRange: skillMinRange,
                maxRange: skillMaxRange,
                actionPointCost: skillActionPointCost);

            return new FightTestContext(
                player,
                enemy,
                playerSkill);
        }
    }
}