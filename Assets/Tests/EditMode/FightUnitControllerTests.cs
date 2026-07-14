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

        [Test]
        public void MaxHealth_WithoutModifiers_ReturnsBaseValue()
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
                newInitiative: 5);

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(10));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(10));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void MaxHealth_IncreaseDoesNotHealCurrentHealth()
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
                newInitiative: 5);

            unit.TakeDamage(4);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10));

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(20));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(6));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void MaxHealth_DecreaseClampsCurrentHealth()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 20,
                newAttackPower: 2,
                newInitiative: 5);

            unit.TakeDamage(2);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    -10));

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(10));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(10));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void MaxHealth_CannotDropBelowOne()
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
                newInitiative: 5);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    -100));

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(1));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(1));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void TurnResources_ResetForTurn_UsesFinalActionAndMovementPoints()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            FightUnitTurnResources resources =
                unitObject.AddComponent<FightUnitTurnResources>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxActionPoints,
                    FightStatModifierType.Flat,
                    2));

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxMovementPoints,
                    FightStatModifierType.Flat,
                    3));

            resources.ResetForTurn();

            Assert.That(
                resources.MaxActionPoints,
                Is.EqualTo(
                    resources.ConfiguredMaxActionPoints + 2));

            Assert.That(
                resources.CurrentActionPoints,
                Is.EqualTo(
                    resources.MaxActionPoints));

            Assert.That(
                resources.MaxMovementPoints,
                Is.EqualTo(
                    resources.ConfiguredMaxMovementPoints + 3));

            Assert.That(
                resources.CurrentMovementPoints,
                Is.EqualTo(
                    resources.MaxMovementPoints));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void TurnResources_IncreaseDuringTurn_DoesNotRestoreSpentPoints()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            FightUnitTurnResources resources =
                unitObject.AddComponent<FightUnitTurnResources>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            resources.ResetForTurn();

            Assert.That(
                resources.TrySpendMovementPoints(2),
                Is.True);

            int currentBeforeBonus =
                resources.CurrentMovementPoints;

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxMovementPoints,
                    FightStatModifierType.Flat,
                    3));

            Assert.That(
                resources.CurrentMovementPoints,
                Is.EqualTo(currentBeforeBonus));

            Assert.That(
                resources.MaxMovementPoints,
                Is.EqualTo(
                    resources.ConfiguredMaxMovementPoints + 3));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void TurnResources_DecreaseDuringTurn_ClampsCurrentPoints()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            FightUnitTurnResources resources =
                unitObject.AddComponent<FightUnitTurnResources>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            resources.ResetForTurn();

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxMovementPoints,
                    FightStatModifierType.Flat,
                    -3));

            Assert.That(
                resources.MaxMovementPoints,
                Is.EqualTo(1));

            Assert.That(
                resources.CurrentMovementPoints,
                Is.EqualTo(1));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void TurnResources_RemovingBuff_ClampsCurrentPointsToNewMaximum()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            FightUnitTurnResources resources =
                unitObject.AddComponent<FightUnitTurnResources>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            FightStatModifier modifier =
                new FightStatModifier(
                    FightStatType.MaxMovementPoints,
                    FightStatModifierType.Flat,
                    3);

            unit.Stats.AddModifier(modifier);

            resources.ResetForTurn();

            Assert.That(
                resources.CurrentMovementPoints,
                Is.EqualTo(7));

            Assert.That(
                unit.Stats.RemoveModifier(modifier),
                Is.True);

            Assert.That(
                resources.MaxMovementPoints,
                Is.EqualTo(
                    resources.ConfiguredMaxMovementPoints));

            Assert.That(
                resources.CurrentMovementPoints,
                Is.EqualTo(
                    resources.ConfiguredMaxMovementPoints));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void TurnResources_MaximumCannotDropBelowZero()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            FightUnitTurnResources resources =
                unitObject.AddComponent<FightUnitTurnResources>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            resources.ResetForTurn();

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxActionPoints,
                    FightStatModifierType.Flat,
                    -100));

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxMovementPoints,
                    FightStatModifierType.Flat,
                    -100));

            Assert.That(
                resources.MaxActionPoints,
                Is.Zero);

            Assert.That(
                resources.CurrentActionPoints,
                Is.Zero);

            Assert.That(
                resources.MaxMovementPoints,
                Is.Zero);

            Assert.That(
                resources.CurrentMovementPoints,
                Is.Zero);

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void UnitsUsingSameDefinition_HaveIndependentRuntimeStats()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: "Shared Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 5,
                newInitiative: 7);

            GameObject firstObject =
                new GameObject("First Unit");

            GameObject secondObject =
                new GameObject("Second Unit");

            FightUnit firstUnit =
                firstObject.AddComponent<FightUnit>();

            FightUnit secondUnit =
                secondObject.AddComponent<FightUnit>();

            Assert.That(
                firstUnit.Initialize(definition),
                Is.True);

            Assert.That(
                secondUnit.Initialize(definition),
                Is.True);

            firstUnit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5));

            firstUnit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10));

            Assert.That(
                firstUnit.AttackPower,
                Is.EqualTo(10));

            Assert.That(
                secondUnit.AttackPower,
                Is.EqualTo(5));

            Assert.That(
                firstUnit.MaxHealth,
                Is.EqualTo(20));

            Assert.That(
                secondUnit.MaxHealth,
                Is.EqualTo(10));

            Object.DestroyImmediate(firstObject);
            Object.DestroyImmediate(secondObject);
            Object.DestroyImmediate(definition);
        }

        [Test]
        public void RuntimeModifiers_DoNotModifyUnitDefinition()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<
                    FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: "Definition Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 5,
                newInitiative: 7);

            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            Assert.That(
                unit.Initialize(definition),
                Is.True);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10));

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5));

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.Initiative,
                    FightStatModifierType.Flat,
                    3));

            Assert.That(
                definition.MaxHealth,
                Is.EqualTo(10));

            Assert.That(
                definition.AttackPower,
                Is.EqualTo(5));

            Assert.That(
                definition.Initiative,
                Is.EqualTo(7));

            Object.DestroyImmediate(unitObject);
            Object.DestroyImmediate(definition);
        }

        [Test]
        public void Reinitialize_RebuildsStatsAndRemovesPreviousModifiers()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            unit.Initialize(
                newUnitName: "First",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 5,
                newInitiative: 7);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Flat,
                    5));

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(10));

            unit.Initialize(
                newUnitName: "Second",
                newTeam: FightTeam.Player,
                newMaxHealth: 20,
                newAttackPower: 3,
                newInitiative: 4);

            Assert.That(
                unit.AttackPower,
                Is.EqualTo(3));

            Assert.That(
                unit.MaxHealth,
                Is.EqualTo(20));

            Assert.That(
                unit.CurrentHealth,
                Is.EqualTo(20));

            Assert.That(
                unit.Initiative,
                Is.EqualTo(4));

            Object.DestroyImmediate(unitObject);
        }

        [Test]
        public void Reinitialize_RefreshesTurnResourceBaseValues()
        {
            GameObject unitObject =
                new GameObject("Unit");

            FightUnit unit =
                unitObject.AddComponent<FightUnit>();

            FightUnitTurnResources resources =
                unitObject.AddComponent<FightUnitTurnResources>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            unit.Stats.AddModifier(
                new FightStatModifier(
                    FightStatType.MaxMovementPoints,
                    FightStatModifierType.Flat,
                    3));

            resources.ResetForTurn();

            Assert.That(
                resources.CurrentMovementPoints,
                Is.EqualTo(
                    resources.ConfiguredMaxMovementPoints + 3));

            unit.Initialize(
                newUnitName: "Reinitialized Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 5);

            resources.ResetForTurn();

            Assert.That(
                resources.MaxMovementPoints,
                Is.EqualTo(
                    resources.ConfiguredMaxMovementPoints));

            Assert.That(
                resources.CurrentMovementPoints,
                Is.EqualTo(
                    resources.ConfiguredMaxMovementPoints));

            Object.DestroyImmediate(unitObject);
        }
    }
}