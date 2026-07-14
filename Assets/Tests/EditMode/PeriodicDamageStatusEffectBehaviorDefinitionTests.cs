using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        PeriodicDamageStatusEffectBehaviorDefinitionTests
    {
        private GameObject unitObject;
        private FightUnit unit;

        private StatusEffectDefinition statusDefinition;

        private PeriodicDamageStatusEffectBehaviorDefinition
            behavior;

        [SetUp]
        public void SetUp()
        {
            unitObject =
                new GameObject(
                    "Unit");

            unit =
                unitObject.AddComponent<
                    FightUnit>();

            unit.Initialize(
                newUnitName:
                    "Unit",
                newTeam:
                    FightTeam.Player,
                newMaxHealth:
                    20,
                newAttackPower:
                    5,
                newInitiative:
                    5);

            behavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();
        }

        [TearDown]
        public void TearDown()
        {
            if (behavior != null)
            {
                Object.DestroyImmediate(
                    behavior);
            }

            if (statusDefinition != null)
            {
                Object.DestroyImmediate(
                    statusDefinition);
            }

            if (unitObject != null)
            {
                Object.DestroyImmediate(
                    unitObject);
            }
        }

        [Test]
        public void InitializeForTests_ClampsDamageToZero()
        {
            behavior.InitializeForTests(
                -5);

            Assert.That(
                behavior.DamagePerStack,
                Is.EqualTo(0));
        }

        [Test]
        public void CalculateDamage_UsesSingleStack()
        {
            behavior.InitializeForTests(
                3);

            int damage =
                behavior.CalculateDamage(
                    1);

            Assert.That(
                damage,
                Is.EqualTo(3));
        }

        [Test]
        public void CalculateDamage_MultipliesByStacks()
        {
            behavior.InitializeForTests(
                3);

            int damage =
                behavior.CalculateDamage(
                    4);

            Assert.That(
                damage,
                Is.EqualTo(12));
        }

        [Test]
        public void CalculateDamage_ZeroStacksUsesOneStack()
        {
            behavior.InitializeForTests(
                3);

            int damage =
                behavior.CalculateDamage(
                    0);

            Assert.That(
                damage,
                Is.EqualTo(3));
        }

        [Test]
        public void Execute_DealsDamageToOwner()
        {
            behavior.InitializeForTests(
                3);

            StatusEffectExecutionContext context =
                CreateContext(
                    maxStacks:
                        1,
                    applications:
                        1);

            behavior.Execute(
                context);

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(17));
        }

        [Test]
        public void Execute_DealsDamageForEveryStack()
        {
            behavior.InitializeForTests(
                2);

            StatusEffectExecutionContext context =
                CreateContext(
                    maxStacks:
                        5,
                    applications:
                        3);

            behavior.Execute(
                context);

            Assert.That(
                context.Stacks,
                Is.EqualTo(3));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(14));
        }

        [Test]
        public void Execute_ZeroDamageDoesNotChangeHealth()
        {
            behavior.InitializeForTests(
                0);

            StatusEffectExecutionContext context =
                CreateContext(
                    maxStacks:
                        1,
                    applications:
                        1);

            behavior.Execute(
                context);

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(20));
        }

        [Test]
        public void Execute_CanKillOwner()
        {
            behavior.InitializeForTests(
                25);

            StatusEffectExecutionContext context =
                CreateContext(
                    maxStacks:
                        1,
                    applications:
                        1);

            bool diedRaised = false;

            unit.Died +=
                _ => diedRaised = true;

            behavior.Execute(
                context);

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(0));

            Assert.That(
                unit.IsAlive,
                Is.False);

            Assert.That(
                diedRaised,
                Is.True);
        }

        private StatusEffectExecutionContext CreateContext(
            int maxStacks,
            int applications)
        {
            statusDefinition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            statusDefinition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newBaseDurationTurns:
                    3,
                newMaxStacks:
                    maxStacks,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .AddStacksAndRefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                newBehaviors:
                    new[]
                    {
                        behavior
                    });

            StatusEffectCollection collection =
                new StatusEffectCollection();

            for (int i = 0;
                 i < applications;
                 i++)
            {
                collection.Apply(
                    statusDefinition);
            }

            collection.TryGet(
                new StatusEffectId(
                    "bleed"),
                out StatusEffectRuntimeState state);

            return new StatusEffectExecutionContext(
                unit,
                state);
        }
    }
}