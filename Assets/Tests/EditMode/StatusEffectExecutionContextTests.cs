using System;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class StatusEffectExecutionContextTests
    {
        private GameObject unitObject;
        private StatusEffectDefinition definition;

        [TearDown]
        public void TearDown()
        {
            if (definition != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    definition);
            }

            if (unitObject != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    unitObject);
            }
        }

        [Test]
        public void Constructor_AssignsOwnerAndState()
        {
            FightUnit unit =
                CreateUnit();

            definition =
                CreateDefinition();

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            StatusEffectExecutionContext context =
                new StatusEffectExecutionContext(
                    unit,
                    state);

            Assert.That(
                context.Owner,
                Is.SameAs(unit));

            Assert.That(
                context.State,
                Is.SameAs(state));

            Assert.That(
                context.Definition,
                Is.SameAs(definition));

            Assert.That(
                context.Stacks,
                Is.EqualTo(1));
        }

        [Test]
        public void Constructor_NullOwnerThrows()
        {
            definition =
                CreateDefinition();

            StatusEffectRuntimeState state =
                new StatusEffectRuntimeState(
                    definition);

            Assert.Throws<ArgumentNullException>(
                () =>
                    new StatusEffectExecutionContext(
                        null,
                        state));
        }

        [Test]
        public void Constructor_NullStateThrows()
        {
            FightUnit unit =
                CreateUnit();

            Assert.Throws<ArgumentNullException>(
                () =>
                    new StatusEffectExecutionContext(
                        unit,
                        null));
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

        private StatusEffectDefinition CreateDefinition()
        {
            StatusEffectDefinition result =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            result.InitializeForTests(
                newStatusEffectId:
                    "test_effect");

            return result;
        }
    }
}