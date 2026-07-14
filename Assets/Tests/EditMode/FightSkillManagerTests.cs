using System.Collections.Generic;
using System.Reflection;
using DiceBossArena.Tests.Fixtures;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightSkillManagerTests
    {
        private readonly List<Object> createdObjects = new();

        private FightUnitRegistry registry;
        private FightTurnManager turnManager;
        private FightSkillManager skillManager;

        private FightUnit caster;
        private FightUnit target;

        private FightGridTile casterTile;
        private FightGridTile targetTile;

        private SkillDefinition skill;
        private UnitSkillState skillState;

        [SetUp]
        public void SetUp()
        {
            registry =
                CreateComponent<FightUnitRegistry>(
                    "FightUnitRegistry");

            turnManager =
                CreateComponent<FightTurnManager>(
                    "FightTurnManager");

            skillManager =
                CreateComponent<FightSkillManager>(
                    "FightSkillManager");

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            SetPrivateField(
                skillManager,
                "turnManager",
                turnManager);

            caster =
                CreateUnit(
                    "Caster",
                    FightTeam.Player,
                    initiative: 20);

            target =
                CreateUnit(
                    "Target",
                    FightTeam.Enemy,
                    initiative: 5);

            casterTile =
                CreateTile(
                    "CasterTile",
                    0,
                    0);

            targetTile =
                CreateTile(
                    "TargetTile",
                    1,
                    0);

            Assert.That(
                caster.TryAssignToTile(casterTile),
                Is.True);

            Assert.That(
                target.TryAssignToTile(targetTile),
                Is.True);

            skill =
                TestSkillFactory.Create(
                    targetType: SkillTargetType.SingleEnemy,
                    minRange: 1,
                    maxRange: 1,
                    actionPointCost: 1,
                    movementPointCost: 0,
                    cooldown: 2);

            createdObjects.Add(skill);

            Assert.That(
                caster.Skills.AddSkill(skill),
                Is.True);

            skillState =
                caster.Skills.GetSkillState(skill);

            registry.Register(caster);
            registry.Register(target);

            caster.TurnResources.ResetForTurn();

            turnManager.StartCombat();

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(caster));
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = createdObjects.Count - 1; i >= 0; i--)
            {
                Object createdObject =
                    createdObjects[i];

                if (createdObject != null)
                {
                    Object.DestroyImmediate(createdObject);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void TryExecuteSkill_WithValidEnemyTarget_SpendsApAndExecutesExactlyOnce()
        {
            int executedCount = 0;
            SkillExecutionContext executedContext = null;

            skillManager.SkillExecuted +=
                context =>
                {
                    executedCount++;
                    executedContext = context;
                };

            bool firstResult =
                skillManager.TryExecuteSkill(
                    caster,
                    skillState,
                    target,
                    targetTile);

            Assert.That(
                firstResult,
                Is.True);

            Assert.That(
                caster.TurnResources.CurrentActionPoints,
                Is.Zero);

            Assert.That(
                skillState.CurrentCooldown,
                Is.EqualTo(2));

            Assert.That(
                executedCount,
                Is.EqualTo(1));

            Assert.That(
                executedContext,
                Is.Not.Null);

            Assert.That(
                executedContext.Caster,
                Is.SameAs(caster));

            Assert.That(
                executedContext.PrimaryTarget,
                Is.SameAs(target));

            bool secondResult =
                skillManager.TryExecuteSkill(
                    caster,
                    skillState,
                    target,
                    targetTile);

            Assert.That(
                secondResult,
                Is.False);

            Assert.That(
                executedCount,
                Is.EqualTo(1));

            Assert.That(
                caster.TurnResources.CurrentActionPoints,
                Is.Zero);

            Assert.That(
                skillState.CurrentCooldown,
                Is.EqualTo(2));
        }

        [Test]
        public void TryExecuteSkill_WithAlliedTarget_DoesNotSpendResourcesOrExecute()
        {
            FightUnit ally =
                CreateUnit(
                    "Ally",
                    FightTeam.Player,
                    initiative: 2);

            FightGridTile allyTile =
                CreateTile(
                    "AllyTile",
                    0,
                    1);

            Assert.That(
                ally.TryAssignToTile(allyTile),
                Is.True);

            registry.Register(ally);

            int executedCount = 0;

            skillManager.SkillExecuted +=
                context =>
                {
                    executedCount++;
                };

            int actionPointsBefore =
                caster.TurnResources.CurrentActionPoints;

            bool result =
                skillManager.TryExecuteSkill(
                    caster,
                    skillState,
                    ally,
                    allyTile);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                executedCount,
                Is.Zero);

            Assert.That(
                caster.TurnResources.CurrentActionPoints,
                Is.EqualTo(actionPointsBefore));

            Assert.That(
                skillState.CurrentCooldown,
                Is.Zero);

            Assert.That(
                ally.CurrentHealth,
                Is.EqualTo(ally.MaxHealth));
        }

        [Test]
        public void TryExecuteSkill_WithTargetOutsideRange_DoesNotSpendResourcesOrExecute()
        {
            FightGridTile distantTile =
                CreateTile(
                    "DistantTargetTile",
                    3,
                    0);

            Assert.That(
                target.TryAssignToTile(distantTile),
                Is.True);

            int executedCount = 0;

            skillManager.SkillExecuted +=
                context =>
                {
                    executedCount++;
                };

            int actionPointsBefore =
                caster.TurnResources.CurrentActionPoints;

            bool result =
                skillManager.TryExecuteSkill(
                    caster,
                    skillState,
                    target,
                    distantTile);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                executedCount,
                Is.Zero);

            Assert.That(
                caster.TurnResources.CurrentActionPoints,
                Is.EqualTo(actionPointsBefore));

            Assert.That(
                skillState.CurrentCooldown,
                Is.Zero);

            Assert.That(
                target.CurrentHealth,
                Is.EqualTo(target.MaxHealth));
        }

        [Test]
        public void TryExecuteSkill_WithDamageEffect_DealsExpectedDamageExactlyOnce()
        {
            DamageSkillEffectDefinition damageEffect =
                ScriptableObject.CreateInstance<DamageSkillEffectDefinition>();

            createdObjects.Add(damageEffect);

            SetPrivateField(
                damageEffect,
                "scalingSource",
                DamageScalingSource.CasterAttackPower);

            SetPrivateField(
                damageEffect,
                "baseValue",
                2);

            SetPrivateField(
                damageEffect,
                "scalingMultiplier",
                1f);

            SetPrivateField(
                skill,
                "effects",
                new List<SkillEffectDefinition>
                {
            damageEffect
                });

            int healthBefore =
                target.CurrentHealth;

            int expectedDamage =
                2 + caster.AttackPower;

            int healthChangedCount = 0;

            target.HealthChanged +=
                unit =>
                {
                    healthChangedCount++;
                };

            bool firstResult =
                skillManager.TryExecuteSkill(
                    caster,
                    skillState,
                    target,
                    targetTile);

            Assert.That(
                firstResult,
                Is.True);

            Assert.That(
                target.CurrentHealth,
                Is.EqualTo(
                    healthBefore - expectedDamage));

            Assert.That(
                healthChangedCount,
                Is.EqualTo(1));

            int healthAfterFirstExecution =
                target.CurrentHealth;

            bool secondResult =
                skillManager.TryExecuteSkill(
                    caster,
                    skillState,
                    target,
                    targetTile);

            Assert.That(
                secondResult,
                Is.False);

            Assert.That(
                target.CurrentHealth,
                Is.EqualTo(
                    healthAfterFirstExecution));

            Assert.That(
                healthChangedCount,
                Is.EqualTo(1));
        }

        [Test]
        public void TryExecuteSkill_WithHealingEffect_HealsExpectedAmountExactlyOnce()
        {
            SkillDefinition healingSkill =
                TestSkillFactory.Create(
                    skillId: "test_heal",
                    displayName: "Test Heal",
                    targetType: SkillTargetType.SingleAlly,
                    minRange: 0,
                    maxRange: 0,
                    actionPointCost: 1,
                    cooldown: 2);

            createdObjects.Add(healingSkill);

            HealingSkillEffectDefinition healingEffect =
                ScriptableObject.CreateInstance<HealingSkillEffectDefinition>();

            createdObjects.Add(healingEffect);

            SetPrivateField(
                healingEffect,
                "baseHeal",
                6);

            SetPrivateField(
                healingSkill,
                "effects",
                new List<SkillEffectDefinition>
                {
            healingEffect
                });

            Assert.That(
                caster.Skills.AddSkill(healingSkill),
                Is.True);

            UnitSkillState healingSkillState =
                caster.Skills.GetSkillState(healingSkill);

            caster.TakeDamage(4);

            Assert.That(
                caster.CurrentHealth,
                Is.EqualTo(caster.MaxHealth - 4));

            int healthChangedCount = 0;

            caster.HealthChanged +=
                unit =>
                {
                    healthChangedCount++;
                };

            bool firstResult =
                skillManager.TryExecuteSkill(
                    caster,
                    healingSkillState,
                    caster,
                    casterTile);

            Assert.That(
                firstResult,
                Is.True);

            Assert.That(
                caster.CurrentHealth,
                Is.EqualTo(caster.MaxHealth));

            Assert.That(
                healthChangedCount,
                Is.EqualTo(1));

            Assert.That(
                caster.TurnResources.CurrentActionPoints,
                Is.Zero);

            Assert.That(
                healingSkillState.CurrentCooldown,
                Is.EqualTo(2));

            int healthAfterFirstExecution =
                caster.CurrentHealth;

            bool secondResult =
                skillManager.TryExecuteSkill(
                    caster,
                    healingSkillState,
                    caster,
                    casterTile);

            Assert.That(
                secondResult,
                Is.False);

            Assert.That(
                caster.CurrentHealth,
                Is.EqualTo(healthAfterFirstExecution));

            Assert.That(
                healthChangedCount,
                Is.EqualTo(1));

            Assert.That(
                healingSkillState.CurrentCooldown,
                Is.EqualTo(2));
        }

        [Test]
        public void TryExecuteSkill_WithApAndMpCost_SpendsBothExactlyOnce()
        {
            SkillDefinition movementCostSkill =
                TestSkillFactory.Create(
                    skillId: "test_ap_mp_skill",
                    displayName: "Test AP MP Skill",
                    targetType: SkillTargetType.SingleEnemy,
                    minRange: 1,
                    maxRange: 1,
                    actionPointCost: 1,
                    movementPointCost: 2,
                    cooldown: 1);

            createdObjects.Add(movementCostSkill);

            Assert.That(
                caster.Skills.AddSkill(movementCostSkill),
                Is.True);

            UnitSkillState movementCostState =
                caster.Skills.GetSkillState(
                    movementCostSkill);

            int actionPointsBefore =
                caster.TurnResources.CurrentActionPoints;

            int movementPointsBefore =
                caster.TurnResources.CurrentMovementPoints;

            bool firstResult =
                skillManager.TryExecuteSkill(
                    caster,
                    movementCostState,
                    target,
                    targetTile);

            Assert.That(
                firstResult,
                Is.True);

            Assert.That(
                caster.TurnResources.CurrentActionPoints,
                Is.EqualTo(actionPointsBefore - 1));

            Assert.That(
                caster.TurnResources.CurrentMovementPoints,
                Is.EqualTo(movementPointsBefore - 2));

            Assert.That(
                movementCostState.CurrentCooldown,
                Is.EqualTo(1));

            int actionPointsAfterFirstExecution =
                caster.TurnResources.CurrentActionPoints;

            int movementPointsAfterFirstExecution =
                caster.TurnResources.CurrentMovementPoints;

            bool secondResult =
                skillManager.TryExecuteSkill(
                    caster,
                    movementCostState,
                    target,
                    targetTile);

            Assert.That(
                secondResult,
                Is.False);

            Assert.That(
                caster.TurnResources.CurrentActionPoints,
                Is.EqualTo(actionPointsAfterFirstExecution));

            Assert.That(
                caster.TurnResources.CurrentMovementPoints,
                Is.EqualTo(movementPointsAfterFirstExecution));
        }

        [Test]
        public void TryExecuteSkill_WithoutEnoughMovementPoints_DoesNotSpendActionPoints()
        {
            SkillDefinition expensiveMovementSkill =
                TestSkillFactory.Create(
                    skillId: "test_expensive_movement_skill",
                    displayName: "Expensive Movement Skill",
                    targetType: SkillTargetType.SingleEnemy,
                    minRange: 1,
                    maxRange: 1,
                    actionPointCost: 1,
                    movementPointCost: 5,
                    cooldown: 1);

            createdObjects.Add(expensiveMovementSkill);

            Assert.That(
                caster.Skills.AddSkill(expensiveMovementSkill),
                Is.True);

            UnitSkillState expensiveMovementState =
                caster.Skills.GetSkillState(
                    expensiveMovementSkill);

            int actionPointsBefore =
                caster.TurnResources.CurrentActionPoints;

            int movementPointsBefore =
                caster.TurnResources.CurrentMovementPoints;

            bool result =
                skillManager.TryExecuteSkill(
                    caster,
                    expensiveMovementState,
                    target,
                    targetTile);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                caster.TurnResources.CurrentActionPoints,
                Is.EqualTo(actionPointsBefore));

            Assert.That(
                caster.TurnResources.CurrentMovementPoints,
                Is.EqualTo(movementPointsBefore));

            Assert.That(
                expensiveMovementState.CurrentCooldown,
                Is.Zero);
        }

        private FightUnit CreateUnit(
            string unitName,
            FightTeam team,
            int initiative)
        {
            GameObject unitObject =
                new GameObject(unitName);

            createdObjects.Add(unitObject);

            unitObject.AddComponent<FightUnitTurnResources>();
            unitObject.AddComponent<FightUnitSkills>();

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                unitName,
                team,
                20,
                5,
                initiative);

            return unit;
        }

        private FightGridTile CreateTile(
            string objectName,
            int gridX,
            int gridY)
        {
            FightGridTile tile =
                CreateComponent<FightGridTile>(
                    objectName);

            tile.Initialize(
                gridX,
                gridY);

            return tile;
        }

        private T CreateComponent<T>(
            string objectName)
            where T : Component
        {
            GameObject gameObject =
                new GameObject(objectName);

            createdObjects.Add(gameObject);

            return gameObject.AddComponent<T>();
        }

        private static void SetPrivateField<T>(
            object targetObject,
            string fieldName,
            T value)
        {
            FieldInfo field =
                targetObject.GetType().GetField(
                    fieldName,
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.That(
                field,
                Is.Not.Null,
                $"Field '{fieldName}' was not found.");

            field.SetValue(
                targetObject,
                value);
        }
    }
}