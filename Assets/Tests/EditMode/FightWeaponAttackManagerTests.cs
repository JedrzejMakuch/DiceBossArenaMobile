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
        for (int i = createdObjects.Count - 1;
             i >= 0;
             i--)
        {
            if (createdObjects[i] != null)
            {
                Object.DestroyImmediate(
                    createdObjects[i]);
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
                new WeaponAttackDamageApplier());

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
                    new SequenceRandomSource(
                        1));

        bool result =
            manager.Initialize(
                profileDamageRoller,
                null);

        Assert.That(
            result,
            Is.False);
    }

    [Test]
    public void TryExecute_ValidAttackRollsProfileAppliesDamageAndRaisesEvent()
    {
        SequenceRandomSource randomSource =
            new SequenceRandomSource(
                6,
                3);

        WeaponAttackProfileDamageRoller
            profileDamageRoller =
                CreateProfileDamageRoller(
                    randomSource);

        WeaponAttackDamageApplier damageApplier =
            new WeaponAttackDamageApplier();

        Assert.That(
            manager.Initialize(
                profileDamageRoller,
                damageApplier),
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

        CharacterActionSet actionSet =
            CreateActionSet(
                weaponProfile);

        Assert.That(
            attacker.ApplyActionSet(
                actionSet),
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
            target.CurrentHealth,
            Is.EqualTo(
                healthBefore - 9));
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
            new GameObject(unitName);

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