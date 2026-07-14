using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitRuntimeStateTests
    {
        [Test]
        public void TwoStates_FromSameDefinition_HaveIndependentHealth()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: "Goblin",
                newTeam: FightTeam.Enemy,
                newMaxHealth: 20,
                newAttackPower: 4,
                newInitiative: 8);

            FightUnitRuntimeState first =
                new FightUnitRuntimeState(definition.MaxHealth);

            FightUnitRuntimeState second =
                new FightUnitRuntimeState(definition.MaxHealth);

            first.ApplyDamage(7);

            Assert.That(first.CurrentHealth, Is.EqualTo(13));
            Assert.That(second.CurrentHealth, Is.EqualTo(20));
            Assert.That(definition.MaxHealth, Is.EqualTo(20));

            Object.DestroyImmediate(definition);
        }

        [Test]
        public void RestoreCurrentHealth_PreservesValueWithinMaximum()
        {
            FightUnitRuntimeState state =
                new FightUnitRuntimeState(20);

            state.RestoreCurrentHealth(
                restoredHealth: 7,
                maxHealth: 20);

            Assert.That(
                state.CurrentHealth,
                Is.EqualTo(7));
        }

        [Test]
        public void RestoreCurrentHealth_ClampsValueToMaximum()
        {
            FightUnitRuntimeState state =
                new FightUnitRuntimeState(20);

            state.RestoreCurrentHealth(
                restoredHealth: 50,
                maxHealth: 20);

            Assert.That(
                state.CurrentHealth,
                Is.EqualTo(20));
        }

        [Test]
        public void RestoreCurrentHealth_AllowsZero()
        {
            FightUnitRuntimeState state =
                new FightUnitRuntimeState(20);

            state.RestoreCurrentHealth(
                restoredHealth: 0,
                maxHealth: 20);

            Assert.That(
                state.CurrentHealth,
                Is.EqualTo(0));

            Assert.That(
                state.IsAlive,
                Is.False);
        }

        [Test]
        public void RestoreCurrentHealth_ClampsNegativeValueToZero()
        {
            FightUnitRuntimeState state =
                new FightUnitRuntimeState(20);

            state.RestoreCurrentHealth(
                restoredHealth: -5,
                maxHealth: 20);

            Assert.That(
                state.CurrentHealth,
                Is.EqualTo(0));
        }
    }
}