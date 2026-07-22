using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using DiceBossArena.Tests.Fixtures;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightActionExecutorTests
    {
        private readonly List<Object> createdObjects =
            new();

        private FightUnitRegistry registry;
        private FightTurnManager turnManager;
        private FightActionExecutor executor;
        private FightMovementManager movementManager;
        private FightSkillManager skillManager;

        private FightUnit activeUnit;
        private FightUnit waitingUnit;

        [SetUp]
        public void SetUp()
        {
            registry =
                CreateComponent<FightUnitRegistry>(
                    "Registry");

            turnManager =
                CreateComponent<FightTurnManager>(
                    "TurnManager");

            movementManager =
                CreateComponent<FightMovementManager>(
                    "MovementManager");

            executor =
                CreateComponent<FightActionExecutor>(
                    "ActionExecutor");

            skillManager =
                CreateComponent<FightSkillManager>(
                    "SkillManager");

            SetPrivateField(
                turnManager,
                "unitRegistry",
                registry);

            SetPrivateField(
                movementManager,
                "turnManager",
                turnManager);

            SetPrivateField(
                executor,
                "turnManager",
                turnManager);

            SetPrivateField(
                executor,
                "movementManager",
                movementManager);

            SetPrivateField(
                skillManager,
                "turnManager",
                turnManager);

            SetPrivateField(
                executor,
                "skillManager",
                skillManager);

            activeUnit =
                CreateUnit(
                    "ActiveUnit",
                    FightTeam.Player,
                    initiative: 20);

            waitingUnit =
                CreateUnit(
                    "WaitingUnit",
                    FightTeam.Enemy,
                    initiative: 5);

            registry.Register(activeUnit);
            registry.Register(waitingUnit);

            turnManager.StartCombat();

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(activeUnit));
        }

        [TearDown]
        public void TearDown()
        {
            for (int i =
                     createdObjects.Count - 1;
                 i >= 0;
                 i--)
            {
                Object createdObject =
                    createdObjects[i];

                if (createdObject != null)
                {
                    Object.DestroyImmediate(
                        createdObject);
                }
            }

            createdObjects.Clear();
        }

        [Test]
        public void TryExecute_WeaponAttackWithoutManager_ReturnsFalse()
        {
            FightWeaponAttackActionRequest request =
                new FightWeaponAttackActionRequest(
                    activeUnit,
                    waitingUnit);

            bool result =
                executor.TryExecute(request);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(activeUnit));
        }

        [Test]
        public void TryExecute_WithNullRequest_ReturnsFalse()
        {
            bool result =
                executor.TryExecute(null);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(activeUnit));
        }

        [Test]
        public void TryExecute_WithWaitingActor_ReturnsFalse()
        {
            FightEndTurnActionRequest request =
                new FightEndTurnActionRequest(
                    waitingUnit);

            bool result =
                executor.TryExecute(request);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(activeUnit));
        }

        [Test]
        public void TryExecute_EndTurnRequest_EndsActiveTurnExactlyOnce()
        {
            int endedCount = 0;
            FightUnit endedUnit = null;

            turnManager.TurnEnded +=
                unit =>
                {
                    endedCount++;
                    endedUnit = unit;
                };

            FightEndTurnActionRequest request =
                new FightEndTurnActionRequest(
                    activeUnit);

            bool result =
                executor.TryExecute(request);

            Assert.That(
                result,
                Is.True);

            Assert.That(
                endedCount,
                Is.EqualTo(1));

            Assert.That(
                endedUnit,
                Is.SameAs(activeUnit));

            Assert.That(
                turnManager.ActiveUnit,
                Is.SameAs(waitingUnit));
        }

        [Test]
        public void TryExecute_AfterCombatStopped_ReturnsFalse()
        {
            turnManager.EndCombat(
                "Test stop.");

            FightEndTurnActionRequest request =
                new FightEndTurnActionRequest(
                    activeUnit);

            bool result =
                executor.TryExecute(request);

            Assert.That(
                result,
                Is.False);
        }

        [Test]
        public void TryExecute_MoveRequest_MovesUnitAndSpendsMovementPointsExactlyOnce()
        {
            FightGridTile startTile =
                CreateTile(
                    "StartTile",
                    0,
                    0);

            FightGridTile targetTile =
                CreateTile(
                    "TargetTile",
                    2,
                    0);

            Assert.That(
                activeUnit.TryAssignToTile(startTile),
                Is.True);

            activeUnit.TurnResources.ResetForTurn();

            int movementPointsBefore =
                activeUnit
                    .TurnResources
                    .CurrentMovementPoints;

            int movedCount = 0;
            FightUnit movedUnit = null;
            FightGridTile movedTarget = null;
            int reportedCost = 0;

            movementManager.UnitMoved +=
                (unit, tile, cost) =>
                {
                    movedCount++;
                    movedUnit = unit;
                    movedTarget = tile;
                    reportedCost = cost;
                };

            FightMoveActionRequest request =
                new FightMoveActionRequest(
                    activeUnit,
                    targetTile);

            bool result =
                executor.TryExecute(request);

            Assert.That(
                result,
                Is.True);

            Assert.That(
                activeUnit.CurrentTile,
                Is.SameAs(targetTile));

            Assert.That(
                startTile.OccupyingUnit,
                Is.Null);

            Assert.That(
                targetTile.OccupyingUnit,
                Is.SameAs(activeUnit));

            Assert.That(
                activeUnit
                    .TurnResources
                    .CurrentMovementPoints,
                Is.EqualTo(movementPointsBefore - 2));

            Assert.That(
                movedCount,
                Is.EqualTo(1));

            Assert.That(
                movedUnit,
                Is.SameAs(activeUnit));

            Assert.That(
                movedTarget,
                Is.SameAs(targetTile));

            Assert.That(
                reportedCost,
                Is.EqualTo(2));
        }

        [Test]
        public void TryExecute_MoveOutsideAvailableRange_DoesNotMoveOrSpendResources()
        {
            FightGridTile startTile =
                CreateTile(
                    "StartTile",
                    0,
                    0);

            FightGridTile distantTile =
                CreateTile(
                    "DistantTile",
                    5,
                    0);

            Assert.That(
                activeUnit.TryAssignToTile(startTile),
                Is.True);

            activeUnit.TurnResources.ResetForTurn();

            int movementPointsBefore =
                activeUnit
                    .TurnResources
                    .CurrentMovementPoints;

            int movedCount = 0;

            movementManager.UnitMoved +=
                (unit, tile, cost) =>
                {
                    movedCount++;
                };

            FightMoveActionRequest request =
                new FightMoveActionRequest(
                    activeUnit,
                    distantTile);

            bool result =
                executor.TryExecute(request);

            Assert.That(
                result,
                Is.False);

            Assert.That(
                activeUnit.CurrentTile,
                Is.SameAs(startTile));

            Assert.That(
                startTile.OccupyingUnit,
                Is.SameAs(activeUnit));

            Assert.That(
                distantTile.OccupyingUnit,
                Is.Null);

            Assert.That(
                activeUnit
                    .TurnResources
                    .CurrentMovementPoints,
                Is.EqualTo(movementPointsBefore));

            Assert.That(
                movedCount,
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

            unitObject.AddComponent<
                FightUnitTurnResources>();

            unitObject.AddComponent<
                FightUnitSkills>();

            FightUnit unit =
                unitObject.AddComponent<
                    FightUnit>();

            unit.Initialize(
                unitName,
                team,
                20,
                5,
                initiative);

            return unit;
        }

        [Test]
        public void TryExecute_SkillRequest_ExecutesSkillAndSpendsResourcesExactlyOnce()
        {
            FightGridTile casterTile =
                CreateTile(
                    "CasterTile",
                    0,
                    0);

            FightGridTile targetTile =
                CreateTile(
                    "TargetTile",
                    1,
                    0);

            Assert.That(
                activeUnit.TryAssignToTile(casterTile),
                Is.True);

            Assert.That(
                waitingUnit.TryAssignToTile(targetTile),
                Is.True);

            activeUnit.TurnResources.ResetForTurn();

            SkillDefinition skill =
                TestSkillFactory.Create(
                    skillId: "executor_test_skill",
                    displayName: "Executor Test Skill",
                    targetType: SkillTargetType.SingleEnemy,
                    minRange: 1,
                    maxRange: 1,
                    actionPointCost: 1,
                    movementPointCost: 0,
                    cooldown: 2);

            createdObjects.Add(skill);

            Assert.That(
                activeUnit.Skills.AddSkill(skill),
                Is.True);

            UnitSkillState skillState =
                activeUnit.Skills.GetSkillState(skill);

            int actionPointsBefore =
                activeUnit
                    .TurnResources
                    .CurrentActionPoints;

            int executedCount = 0;
            SkillExecutionContext executedContext = null;

            skillManager.SkillExecuted +=
                context =>
                {
                    executedCount++;
                    executedContext = context;
                };

            FightSkillActionRequest request =
                new FightSkillActionRequest(
                    activeUnit,
                    skillState,
                    waitingUnit,
                    targetTile);

            bool firstResult =
                executor.TryExecute(request);

            Assert.That(
                firstResult,
                Is.True);

            Assert.That(
                activeUnit
                    .TurnResources
                    .CurrentActionPoints,
                Is.EqualTo(actionPointsBefore - 1));

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
                Is.SameAs(activeUnit));

            Assert.That(
                executedContext.PrimaryTarget,
                Is.SameAs(waitingUnit));

            bool secondResult =
                executor.TryExecute(request);

            Assert.That(
                secondResult,
                Is.False);

            Assert.That(
                executedCount,
                Is.EqualTo(1));

            Assert.That(
                activeUnit
                    .TurnResources
                    .CurrentActionPoints,
                Is.EqualTo(actionPointsBefore - 1));

            Assert.That(
                skillState.CurrentCooldown,
                Is.EqualTo(2));
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

        private FightGridTile CreateTile(
    string objectName,
    int gridX,
    int gridY)
        {
            GameObject tileObject =
                new GameObject(objectName);

            createdObjects.Add(tileObject);

            FightGridTile tile =
                tileObject.AddComponent<FightGridTile>();

            tile.Initialize(
                gridX,
                gridY);

            return tile;
        }

        private static void SetPrivateField<T>(
            object targetObject,
            string fieldName,
            T value)
        {
            FieldInfo field =
                targetObject
                    .GetType()
                    .GetField(
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