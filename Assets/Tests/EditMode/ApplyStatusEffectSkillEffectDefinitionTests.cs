using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class
        ApplyStatusEffectSkillEffectDefinitionTests
    {
        private GameObject casterObject;
        private GameObject targetObject;

        private FightUnit caster;
        private FightUnit target;

        private StatusEffectDefinition statusDefinition;

        private ApplyStatusEffectSkillEffectDefinition
            skillEffect;

        private PeriodicDamageStatusEffectBehaviorDefinition
    damageBehavior;

        [SetUp]
        public void SetUp()
        {
            caster =
                CreateUnit(
                    ref casterObject,
                    "Caster",
                    FightTeam.Player);

            target =
                CreateUnit(
                    ref targetObject,
                    "Target",
                    FightTeam.Enemy);

            statusDefinition =
                ScriptableObject.CreateInstance<
                    StatusEffectDefinition>();

            statusDefinition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newCategory:
                    StatusEffectCategory.Debuff,
                newBaseDurationTurns:
                    3,
                newMaxStacks:
                    5,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .AddStacksAndRefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn);

            skillEffect =
                ScriptableObject.CreateInstance<
                    ApplyStatusEffectSkillEffectDefinition>();

            skillEffect.InitializeForTests(
                statusDefinition,
                StatusEffectTargetRelation.Enemy);
        }

        [TearDown]
        public void TearDown()
        {
            if (damageBehavior != null)
            {
                Object.DestroyImmediate(
                    damageBehavior);
            }

            if (skillEffect != null)
            {
                Object.DestroyImmediate(
                    skillEffect);
            }

            if (statusDefinition != null)
            {
                Object.DestroyImmediate(
                    statusDefinition);
            }

            if (casterObject != null)
            {
                Object.DestroyImmediate(
                    casterObject);
            }

            if (targetObject != null)
            {
                Object.DestroyImmediate(
                    targetObject);
            }
        }

        [Test]
        public void CanApply_EnemyTargetReturnsTrue()
        {
            SkillExecutionContext context =
                CreateContext(
                    caster,
                    target);

            Assert.That(
                skillEffect.CanApply(
                    context),
                Is.True);
        }

        [Test]
        public void Apply_AddsStatusToEnemyTarget()
        {
            SkillExecutionContext context =
                CreateContext(
                    caster,
                    target);

            skillEffect.Apply(
                context);

            Assert.That(
                target.HasStatusEffect(
                    new StatusEffectId(
                        "bleed")),
                Is.True);

            Assert.That(
                caster.HasStatusEffect(
                    new StatusEffectId(
                        "bleed")),
                Is.False);
        }

        [Test]
        public void Apply_RepeatedApplicationAddsStack()
        {
            SkillExecutionContext context =
                CreateContext(
                    caster,
                    target);

            skillEffect.Apply(
                context);

            skillEffect.Apply(
                context);

            Assert.That(
                target.StatusEffects.TryGet(
                    new StatusEffectId(
                        "bleed"),
                    out StatusEffectRuntimeState state),
                Is.True);

            Assert.That(
                state.Stacks,
                Is.EqualTo(2));
        }

        [Test]
        public void CanApply_EnemyEffectRejectsAlly()
        {
            target.Initialize(
                newUnitName:
                    "Target",
                newTeam:
                    FightTeam.Player,
                newMaxHealth:
                    20,
                newAttackPower:
                    5,
                newInitiative:
                    5);

            SkillExecutionContext context =
                CreateContext(
                    caster,
                    target);

            Assert.That(
                skillEffect.CanApply(
                    context),
                Is.False);
        }

        [Test]
        public void CanApply_SelfRelationAcceptsCaster()
        {
            skillEffect.InitializeForTests(
                statusDefinition,
                StatusEffectTargetRelation.Self);

            SkillExecutionContext context =
                CreateContext(
                    caster,
                    caster);

            Assert.That(
                skillEffect.CanApply(
                    context),
                Is.True);
        }

        [Test]
        public void CanApply_SelfRelationRejectsOtherUnit()
        {
            skillEffect.InitializeForTests(
                statusDefinition,
                StatusEffectTargetRelation.Self);

            SkillExecutionContext context =
                CreateContext(
                    caster,
                    target);

            Assert.That(
                skillEffect.CanApply(
                    context),
                Is.False);
        }

        [Test]
        public void CanApply_AllyRelationAcceptsOtherAlly()
        {
            target.Initialize(
                newUnitName:
                    "Target",
                newTeam:
                    FightTeam.Player,
                newMaxHealth:
                    20,
                newAttackPower:
                    5,
                newInitiative:
                    5);

            skillEffect.InitializeForTests(
                statusDefinition,
                StatusEffectTargetRelation.Ally);

            SkillExecutionContext context =
                CreateContext(
                    caster,
                    target);

            Assert.That(
                skillEffect.CanApply(
                    context),
                Is.True);
        }

        [Test]
        public void CanApply_AllyRelationRejectsCasterItself()
        {
            skillEffect.InitializeForTests(
                statusDefinition,
                StatusEffectTargetRelation.Ally);

            SkillExecutionContext context =
                CreateContext(
                    caster,
                    caster);

            Assert.That(
                skillEffect.CanApply(
                    context),
                Is.False);
        }

        [Test]
        public void CanApply_NullContextReturnsFalse()
        {
            Assert.That(
                skillEffect.CanApply(
                    null),
                Is.False);
        }

        [Test]
        public void CanApply_MissingStatusDefinitionReturnsFalse()
        {
            skillEffect.InitializeForTests(
                null);

            SkillExecutionContext context =
                CreateContext(
                    caster,
                    target);

            Assert.That(
                skillEffect.CanApply(
                    context),
                Is.False);
        }

        [Test]
        public void CanApply_NullTargetReturnsFalse()
        {
            SkillExecutionContext context =
                CreateContext(
                    caster,
                    null);

            Assert.That(
                skillEffect.CanApply(
                    context),
                Is.False);
        }

        private FightUnit CreateUnit(
            ref GameObject unitGameObject,
            string unitName,
            FightTeam team)
        {
            unitGameObject =
                new GameObject(
                    unitName);

            FightUnit unit =
                unitGameObject.AddComponent<
                    FightUnit>();

            unit.Initialize(
                newUnitName:
                    unitName,
                newTeam:
                    team,
                newMaxHealth:
                    20,
                newAttackPower:
                    5,
                newInitiative:
                    5);

            return unit;
        }

        private SkillExecutionContext CreateContext(
            FightUnit contextCaster,
            FightUnit contextTarget)
        {
            return new SkillExecutionContext(
                skill:
                    null,
                skillLevel:
                    1,
                caster:
                    contextCaster,
                primaryTarget:
                    contextTarget,
                targetTile:
                    null,
                affectedUnits:
                    contextTarget != null
                        ? new[]
                        {
                            contextTarget
                        }
                        : null);
        }

        [Test]
        public void ApplyStatusSkillEffect_BleedDealsDamageAtEndOfTargetTurn()
        {
            damageBehavior =
                ScriptableObject.CreateInstance<
                    PeriodicDamageStatusEffectBehaviorDefinition>();

            damageBehavior.InitializeForTests(
                2);

            statusDefinition.InitializeForTests(
                newStatusEffectId:
                    "bleed",
                newDisplayName:
                    "Bleed",
                newCategory:
                    StatusEffectCategory.Debuff,
                newBaseDurationTurns:
                    3,
                newMaxStacks:
                    5,
                newStackingPolicy:
                    StatusEffectStackingPolicy
                        .AddStacksAndRefreshDuration,
                newTickTiming:
                    StatusEffectTickTiming
                        .EndOfOwnerTurn,
                newBehaviors:
                    new[]
                    {
                damageBehavior
                    });

            SkillExecutionContext context =
                CreateContext(
                    caster,
                    target);

            skillEffect.Apply(
                context);

            Assert.That(
                target.CurrentHealth,
                Is.EqualTo(20));

            target.ProcessStatusEffectsAtEndOfTurn();

            Assert.That(
                target.CurrentHealth,
                Is.EqualTo(18));
        }
    }
}