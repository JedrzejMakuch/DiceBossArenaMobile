using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
	public sealed class FightUnitControllerTests
	{
		[Test]
		public void LegacyEnemy_CanBeControlledByLocalPlayer()
		{
			FightUnitDefinition definition =
				ScriptableObject.CreateInstance<
					FightUnitDefinition>();

			definition.InitializeForTests(
				newUnitName: "Possessed Enemy",
				newTeam: FightTeam.Enemy,
				newMaxHealth: 10,
				newAttackPower: 2,
				newInitiative: 8);

			FightUnitOwnership ownership =
				new FightUnitOwnership(
					FightTeamId.TeamA,
					new FightParticipantId("local-player"),
					FightControllerType.LocalPlayer);

			GameObject unitObject =
				new GameObject("Possessed Enemy");

			FightUnit unit =
				unitObject.AddComponent<FightUnit>();

			Assert.That(
				unit.Initialize(
					definition,
					ownership),
				Is.True);

			Assert.That(
				unit.Team,
				Is.EqualTo(FightTeam.Enemy));

			Assert.That(
				unit.IsControlledBy(
					FightControllerType.LocalPlayer),
				Is.True);

			Assert.That(
				unit.IsControlledBy(
					FightControllerType.AI),
				Is.False);

			Object.DestroyImmediate(unitObject);
			Object.DestroyImmediate(definition);
		}

		[Test]
		public void LegacyPlayer_CanBeControlledByAi()
		{
			FightUnitDefinition definition =
				ScriptableObject.CreateInstance<
					FightUnitDefinition>();

			definition.InitializeForTests(
				newUnitName: "AI Companion",
				newTeam: FightTeam.Player,
				newMaxHealth: 10,
				newAttackPower: 2,
				newInitiative: 8);

			FightUnitOwnership ownership =
				new FightUnitOwnership(
					FightTeamId.TeamA,
					new FightParticipantId("companion"),
					FightControllerType.AI);

			GameObject unitObject =
				new GameObject("AI Companion");

			FightUnit unit =
				unitObject.AddComponent<FightUnit>();

			Assert.That(
				unit.Initialize(
					definition,
					ownership),
				Is.True);

			Assert.That(
				unit.Team,
				Is.EqualTo(FightTeam.Player));

			Assert.That(
				unit.IsControlledBy(
					FightControllerType.AI),
				Is.True);

			Object.DestroyImmediate(unitObject);
			Object.DestroyImmediate(definition);
		}

        [Test]
        public void AttackPower_WithoutModifiers_ReturnsBaseValue()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 7,
                newInitiative: 5);

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(7));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void AttackPower_WithModifier_ReturnsModifiedValue()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 7,
                newInitiative: 5);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    3));

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(10));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void Initiative_WithoutModifiers_ReturnsBaseValue()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 7);

            Assert.That(
                unit.Initiative,
                Is.EqualTo(7));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void Initiative_WithModifier_ReturnsModifiedValue()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 7);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.Initiative,
                    FightStatModifierType.Flat,
                    5));

            Assert.That(
                unit.Initiative,
                Is.EqualTo(12));

            Object.DestroyImmediate(unitObject);
        }
    }
}