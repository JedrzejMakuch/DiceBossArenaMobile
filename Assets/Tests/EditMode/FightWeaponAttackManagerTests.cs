using DiceBossArena.Game;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public sealed class FightWeaponAttackManagerTests
{
    private GameObject managerObject;
    private FightWeaponAttackManager manager;

    private readonly List<Object> createdObjects =
        new();

    [SetUp]
    public void SetUp()
    {
        managerObject =
            new GameObject(
                nameof(FightWeaponAttackManagerTests));

        createdObjects.Add(
            managerObject);

        manager =
            managerObject.AddComponent<
                FightWeaponAttackManager>();
    }

    [TearDown]
    public void TearDown()
    {
        for (int index = createdObjects.Count - 1;
             index >= 0;
             index--)
        {
            if (createdObjects[index] != null)
            {
                Object.DestroyImmediate(
                    createdObjects[index]);
            }
        }

        createdObjects.Clear();
    }

    [Test]
    public void TryExecute_ReturnsFalse_WhenAttackerIsNull()
    {
        bool result =
            manager.TryExecute(
                null,
                null);

        Assert.That(
            result,
            Is.False);
    }

    [Test]
    public void Initialize_NullDamageRollerReturnsFalse()
    {
        bool result =
            manager.Initialize(
                null,
                CreateEffectsProfileResolver(
                    new SequenceRandomSource(0)),
                new WeaponAttackDamageApplier(),
                CreateLifeStealApplier());

        Assert.That(
            result,
            Is.False);
    }

    [Test]
    public void Initialize_NullEffectsResolverReturnsFalse()
    {
        WeaponAttackProfileDamageRoller
            profileDamageRoller =
                CreateProfileDamageRoller(
                    new SequenceRandomSource(1));

        bool result =
            manager.Initialize(
                profileDamageRoller,
                null,
                new WeaponAttackDamageApplier(),
                CreateLifeStealApplier());

        Assert.That(
            result,
            Is.False);
    }

    [Test]
    public void Initialize_NullDamageApplierReturnsFalse()
    {
        WeaponAttackProfileDamageRoller
            profileDamageRoller =
                CreateProfileDamageRoller(
                    new SequenceRandomSource(1));

        WeaponAttackEffectsProfileResolver
            effectsProfileResolver =
                CreateEffectsProfileResolver(
                    new SequenceRandomSource(0));

        bool result =
            manager.Initialize(
                profileDamageRoller,
                effectsProfileResolver,
                null,
                CreateLifeStealApplier());

        Assert.That(
            result,
            Is.False);
    }

    [Test]
    public void Initialize_NullLifeStealApplierReturnsFalse()
    {
        WeaponAttackProfileDamageRoller
            profileDamageRoller =
                CreateProfileDamageRoller(
                    new SequenceRandomSource(1));

        WeaponAttackEffectsProfileResolver
            effectsProfileResolver =
                CreateEffectsProfileResolver(
                    new SequenceRandomSource(0));

        bool result =
            manager.Initialize(
                profileDamageRoller,
                effectsProfileResolver,
                new WeaponAttackDamageApplier(),
                null);

        Assert.That(
            result,
            Is.False);
    }

    [Test]
    public void TryExecute_ValidAttackRollsProfileAppliesDamageAndRaisesEvent()
    {
        SequenceRandomSource damageRandomSource =
            new SequenceRandomSource(
                6,
                3);

        SequenceRandomSource effectRandomSource =
            new SequenceRandomSource();

        WeaponAttackProfileDamageRoller
            profileDamageRoller =
                CreateProfileDamageRoller(
                    damageRandomSource);

        WeaponAttackEffectsProfileResolver
            effectsProfileResolver =
                CreateEffectsProfileResolver(
                    effectRandomSource);

        Assert.That(
            manager.Initialize(
                profileDamageRoller,
                effectsProfileResolver,
                new WeaponAttackDamageApplier(),
                CreateLifeStealApplier()),
            Is.True);

        FightUnit attacker =
            CreateUnit(
                "Attacker",
                FightTeam.Player);

        FightUnit target =
            CreateUnit(
                "Target",
                FightTeam.Enemy);

        RolledWeaponProfile weaponProfile =
            new RolledWeaponProfile(
                new[]
                {
                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "primary_damage"),
                        WeaponAttackElement.Neutral,
                        4,
                        8),

                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "fire_damage"),
                        WeaponAttackElement.Fire,
                        2,
                        5)
                });

        Assert.That(
            attacker.ApplyActionSet(
                CreateActionSet(
                    weaponProfile)),
            Is.True);

        int healthBefore =
            target.CurrentHealth;

        int eventCount = 0;

        WeaponAttackRollResult reportedResult =
            null;

        manager.WeaponAttackRolled +=
            attackResult =>
            {
                eventCount++;

                reportedResult =
                    attackResult;
            };

        bool result =
            manager.TryExecute(
                attacker,
                target);

        Assert.That(
            result,
            Is.True);

        Assert.That(
            eventCount,
            Is.EqualTo(1));

        Assert.That(
            reportedResult,
            Is.Not.Null);

        Assert.That(
            reportedResult.Attacker,
            Is.SameAs(attacker));

        Assert.That(
            reportedResult.Target,
            Is.SameAs(target));

        Assert.That(
            reportedResult.DamageLines.Count,
            Is.EqualTo(2));

        Assert.That(
            reportedResult.DamageLines[0].Damage,
            Is.EqualTo(6));

        Assert.That(
            reportedResult.DamageLines[1].Damage,
            Is.EqualTo(3));

        Assert.That(
            reportedResult.TotalDamage,
            Is.EqualTo(9));

        Assert.That(
            reportedResult.EffectLines.Count,
            Is.EqualTo(2));

        Assert.That(
            reportedResult.EffectLines[0].DamageLine,
            Is.SameAs(
                reportedResult.DamageLines[0]));

        Assert.That(
            reportedResult.EffectLines[1].DamageLine,
            Is.SameAs(
                reportedResult.DamageLines[1]));

        Assert.That(
            reportedResult.EffectLines[0]
                .EffectResults.Count,
            Is.EqualTo(0));

        Assert.That(
            reportedResult.EffectLines[1]
                .EffectResults.Count,
            Is.EqualTo(0));

        Assert.That(
            target.CurrentHealth,
            Is.EqualTo(
                healthBefore - 9));
    }

    [Test]
    public void Execute_TriggeredEffectIsIncludedInResult()
    {
        WeaponAttackProfileDamageRoller
            profileDamageRoller =
                CreateProfileDamageRoller(
                    new SequenceRandomSource(8));

        WeaponAttackEffectsProfileResolver
            effectsProfileResolver =
                CreateEffectsProfileResolver(
                    new SequenceRandomSource(0));

        Assert.That(
            manager.Initialize(
                profileDamageRoller,
                effectsProfileResolver,
                new WeaponAttackDamageApplier(),
                CreateLifeStealApplier()),
            Is.True);

        FightUnit attacker =
            CreateUnit(
                "Attacker",
                FightTeam.Player);

        FightUnit target =
            CreateUnit(
                "Target",
                FightTeam.Enemy);

        WeaponAttackEffectDefinition lifeSteal =
            CreateLifeStealDefinition(
                25);

        RolledWeaponProfile weaponProfile =
            new RolledWeaponProfile(
                new[]
                {
                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "primary_damage"),
                        WeaponAttackElement.Neutral,
                        8,
                        9,
                        new[]
                        {
                            lifeSteal
                        })
                });

        Assert.That(
            attacker.ApplyActionSet(
                CreateActionSet(
                    weaponProfile)),
            Is.True);

        WeaponAttackRollResult reportedResult =
            null;

        manager.WeaponAttackRolled +=
            attackResult =>
                reportedResult =
                    attackResult;

        WeaponAttackExecutionResult result =
            manager.Execute(
                attacker,
                target);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackExecutionResult.Success));

        Assert.That(
            reportedResult,
            Is.Not.Null);

        Assert.That(
            reportedResult.EffectLines.Count,
            Is.EqualTo(1));

        Assert.That(
            reportedResult.EffectLines[0]
                .EffectResults.Count,
            Is.EqualTo(1));

        Assert.That(
            reportedResult.EffectLines[0]
                .EffectResults[0]
                .IsTriggered,
            Is.True);

        Assert.That(
            reportedResult.EffectLines[0]
                .EffectResults[0]
                .Definition,
            Is.SameAs(lifeSteal));
    }

    [Test]
    public void Execute_TriggeredLifeStealHealsAttacker()
    {
        WeaponAttackProfileDamageRoller
            profileDamageRoller =
                CreateProfileDamageRoller(
                    new SequenceRandomSource(8));

        WeaponAttackEffectsProfileResolver
            effectsProfileResolver =
                CreateEffectsProfileResolver(
                    new SequenceRandomSource(0));

        Assert.That(
            manager.Initialize(
                profileDamageRoller,
                effectsProfileResolver,
                new WeaponAttackDamageApplier(),
                CreateLifeStealApplier()),
            Is.True);

        FightUnit attacker =
            CreateUnit(
                "Attacker",
                FightTeam.Player);

        FightUnit target =
            CreateUnit(
                "Target",
                FightTeam.Enemy);

        attacker.TakeDamage(
            10);

        WeaponAttackEffectDefinition lifeSteal =
            CreateLifeStealDefinition(
                25);

        RolledWeaponProfile weaponProfile =
            new RolledWeaponProfile(
                new[]
                {
                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "primary_damage"),
                        WeaponAttackElement.Neutral,
                        8,
                        9,
                        new[]
                        {
                            lifeSteal
                        })
                });

        Assert.That(
            attacker.ApplyActionSet(
                CreateActionSet(
                    weaponProfile)),
            Is.True);

        int attackerHealthBefore =
            attacker.CurrentHealth;

        int targetHealthBefore =
            target.CurrentHealth;

        WeaponAttackExecutionResult result =
            manager.Execute(
                attacker,
                target);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackExecutionResult.Success));

        Assert.That(
            target.CurrentHealth,
            Is.EqualTo(
                targetHealthBefore - 8));

        Assert.That(
            attacker.CurrentHealth,
            Is.EqualTo(
                attackerHealthBefore + 2));
    }

    [Test]
    public void Execute_NullAttackerReturnsInvalidAttacker()
    {
        WeaponAttackExecutionResult result =
            manager.Execute(
                null,
                null);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackExecutionResult
                    .InvalidAttacker));
    }

    [Test]
    public void Execute_NullTargetReturnsInvalidTarget()
    {
        FightUnit attacker =
            CreateUnit(
                "Attacker",
                FightTeam.Player);

        WeaponAttackExecutionResult result =
            manager.Execute(
                attacker,
                null);

        Assert.That(
            result,
            Is.EqualTo(
                WeaponAttackExecutionResult
                    .InvalidTarget));
    }

    [Test]
    public void TryExecute_ReturnsFalse_WhenTargetIsNull()
    {
        FightUnit attacker =
            CreateUnit(
                "Attacker",
                FightTeam.Player);

        bool result =
            manager.TryExecute(
                attacker,
                null);

        Assert.That(
            result,
            Is.False);
    }

    private static WeaponAttackProfileDamageRoller
        CreateProfileDamageRoller(
            IWeaponAttackRandomSource randomSource)
    {
        WeaponAttackDamageRoller damageRoller =
            new WeaponAttackDamageRoller(
                randomSource);

        return new WeaponAttackProfileDamageRoller(
            damageRoller);
    }

    private static WeaponAttackEffectsProfileResolver
    CreateEffectsProfileResolver(
        IWeaponAttackRandomSource randomSource)
    {
        WeaponAttackEffectTriggerResolver
            triggerResolver =
                new WeaponAttackEffectTriggerResolver(
                    randomSource);

        WeaponAttackEffectsTriggerResolver
            effectsTriggerResolver =
                new WeaponAttackEffectsTriggerResolver(
                    triggerResolver);

        WeaponAttackEffectLineResolver
            effectLineResolver =
                new WeaponAttackEffectLineResolver(
                    effectsTriggerResolver);

        return new WeaponAttackEffectsProfileResolver(
            effectLineResolver);
    }

    private static WeaponAttackLifeStealApplier
        CreateLifeStealApplier()
    {
        return new WeaponAttackLifeStealApplier(
            new WeaponAttackLifeStealCalculator());
    }

    private static WeaponAttackEffectDefinition
        CreateLifeStealDefinition(
            int lifeStealPercent)
    {
        return new WeaponAttackEffectDefinition(
            WeaponAttackEffectType.LifeSteal,
            100,
            null,
            lifeStealPercent);
    }

    private static CharacterActionSet CreateActionSet(
        RolledWeaponProfile weaponProfile)
    {
        return new CharacterActionSet(
            new[]
            {
                CharacterActionContent
                    .CreateWeaponAttack(
                        weaponProfile),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.BasicAttack,
                    CreateBuildSkill(
                        CharacterSkillIds.BasicAttack)),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillOne,
                    CreateBuildSkill(
                        "skill_one")),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillTwo,
                    CreateBuildSkill(
                        "skill_two")),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillThree,
                    CreateBuildSkill(
                        "skill_three")),

                CharacterActionContent.CreateSkill(
                    CharacterActionSlot.SkillFour,
                    CreateBuildSkill(
                        "skill_four"))
            });
    }

    private static CharacterBuildSkill CreateBuildSkill(
        string skillId)
    {
        return new CharacterBuildSkill(
            skillId,
            1);
    }

    private FightUnit CreateUnit(
        string unitName,
        FightTeam team)
    {
        GameObject unitObject =
            new GameObject(
                unitName);

        createdObjects.Add(
            unitObject);

        unitObject.AddComponent<
            FightUnitTurnResources>();

        unitObject.AddComponent<
            FightUnitSkills>();

        FightUnit unit =
            unitObject.AddComponent<
                FightUnit>();

        unit.Initialize(
            newUnitName: unitName,
            newTeam: team,
            newMaxHealth: 20,
            newAttackPower: 5,
            newInitiative: 10);

        return unit;
    }

    private sealed class SequenceRandomSource :
        IWeaponAttackRandomSource
    {
        private readonly int[] results;
        private int currentIndex;

        public SequenceRandomSource(
            params int[] results)
        {
            this.results =
                results;
        }

        public int Next(
            int minimumInclusive,
            int maximumExclusive)
        {
            int result =
                results[currentIndex];

            currentIndex++;

            return result;
        }
    }
}