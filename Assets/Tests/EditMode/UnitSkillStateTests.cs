using DiceBossArena.Tests.Fixtures;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class UnitSkillStateTests
    {
        [Test]
        public void TryStartCooldown_UsesCooldownFromCurrentLevel()
        {
            SkillDefinition skill =
                TestSkillFactory.Create(
                    maxLevel: 1,
                    cooldown: 3);

            try
            {
                UnitSkillState state =
                    new UnitSkillState(
                        skill,
                        1);

                Assert.That(
                    state.IsReady,
                    Is.True);

                Assert.That(
                    state.TryStartCooldown(),
                    Is.True);

                Assert.That(
                    state.CurrentCooldown,
                    Is.EqualTo(3));

                Assert.That(
                    state.IsReady,
                    Is.False);
            }
            finally
            {
                Object.DestroyImmediate(skill);
            }
        }

        [Test]
        public void ReduceCooldown_ReachesZeroAndMakesSkillReady()
        {
            SkillDefinition skill =
                TestSkillFactory.Create(
                    cooldown: 2);

            try
            {
                UnitSkillState state =
                    new UnitSkillState(
                        skill,
                        1);

                state.TryStartCooldown();

                state.ReduceCooldown();

                Assert.That(
                    state.CurrentCooldown,
                    Is.EqualTo(1));

                Assert.That(
                    state.IsReady,
                    Is.False);

                state.ReduceCooldown();

                Assert.That(
                    state.CurrentCooldown,
                    Is.Zero);

                Assert.That(
                    state.IsReady,
                    Is.True);

                state.ReduceCooldown();

                Assert.That(
                    state.CurrentCooldown,
                    Is.Zero);
            }
            finally
            {
                Object.DestroyImmediate(skill);
            }
        }
    }
}