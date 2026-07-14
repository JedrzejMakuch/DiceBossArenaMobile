using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class StatusEffectBehaviorExecutorTests
    {
        private GameObject unitObject;
        private StatusEffectDefinition definition;
        private TestStatusEffectBehavior behavior;

        [TearDown]
        public void TearDown()
        {
            if (behavior != null)
            {
                Object.DestroyImmediate(
                    behavior);
            }

            if (definition != null)
            {
                Object.DestroyImmediate(
                    definition);
            }

            if (unitObject != null)
            {
                Object.DestroyImmediate(
                    unitObject);
            }
        }

        [Test]
        public void Execute_RunsConfiguredBehavior()
        {
            FightUnit unit =
                CreateUnit();

            behavior =
                ScriptableObject.CreateInstance<
                    TestStatusEffectBehavior>();

            definition =
                CreateDefinition(
                    behavior);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            StatusEffectBehaviorExecutor executor =
                new StatusEffectBehaviorExecutor();

            int executed =
                executor.Execute(
                    unit,
                    state);

            Assert.That(
                executed,
                Is.EqualTo(1));

            Assert.That(
                behavior.ExecutionCount,
                Is.EqualTo(1));

            Assert.That(
                behavior.LastOwner,
                Is.SameAs(unit));

            Assert.That(
                behavior.LastState,
                Is.SameAs(state));
        }

        [Test]
        public void Execute_NoBehaviorsReturnsZero()
        {
            FightUnit unit =
                CreateUnit();

            definition =
                CreateDefinition();

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            StatusEffectBehaviorExecutor executor =
                new StatusEffectBehaviorExecutor();

            int executed =
                executor.Execute(
                    unit,
                    state);

            Assert.That(
                executed,
                Is.EqualTo(0));
        }

        [Test]
        public void Execute_NullBehaviorIsSkipped()
        {
            FightUnit unit =
                CreateUnit();

            definition =
                CreateDefinition(
                    null);

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            StatusEffectBehaviorExecutor executor =
                new StatusEffectBehaviorExecutor();

            int executed =
                executor.Execute(
                    unit,
                    state);

            Assert.That(
                executed,
                Is.EqualTo(0));
        }

        private FightUnit CreateUnit()
        {
            unitObject =
                new GameObject(
                    "Unit");

            FightUnit unit =
                unitObject.AddComponent<
                    FightUnit>();

            unit.Initialize(
                "Unit",
                FightTeam.Player,
                20,
                5,
                5);

            return unit;
        }

        private StatusEffectDefinition CreateDefinition(
            params StatusEffectBehaviorDefinition[] behaviors)
        {
            StatusEffectDefinition result =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            result.InitializeForTests(
                newStatusEffectId:
                    "test_effect",
                newBehaviors:
                    behaviors);

            return result;
        }

        private sealed class TestStatusEffectBehavior :
            StatusEffectBehaviorDefinition
        {
            public int ExecutionCount { get; private set; }

            public FightUnit LastOwner { get; private set; }

            public StatusEffectRuntimeState LastState
            {
                get;
                private set;
            }

            public override void Execute(
                StatusEffectExecutionContext context)
            {
                ExecutionCount++;

                LastOwner =
                    context.Owner;

                LastState =
                    context.State;
            }
        }
    }
}