using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitDefinitionTests
    {
        [Test]
        public void TwoUnits_WithSameDefinition_HaveIndependentHealth()
        {
            FightUnitDefinition definition =
                ScriptableObject.CreateInstance<FightUnitDefinition>();

            definition.InitializeForTests(
                newUnitName: "Goblin",
                newTeam: FightTeam.Enemy,
                newMaxHealth: 20,
                newAttackPower: 4,
                newInitiative: 8);

            GameObject firstObject =
                new GameObject("First Goblin");

            GameObject secondObject =
                new GameObject("Second Goblin");

            FightUnit first =
                firstObject.AddComponent<FightUnit>();

            FightUnit second =
                secondObject.AddComponent<FightUnit>();

            Assert.That(
                first.Initialize(definition),
                Is.True);

            Assert.That(
                second.Initialize(definition),
                Is.True);

            first.TakeDamage(7);

            Assert.That(first.Definition, Is.SameAs(definition));
            Assert.That(second.Definition, Is.SameAs(definition));

            Assert.That(first.UnitName, Is.EqualTo("Goblin"));
            Assert.That(second.UnitName, Is.EqualTo("Goblin"));

            Assert.That(first.CurrentHealth, Is.EqualTo(13));
            Assert.That(second.CurrentHealth, Is.EqualTo(20));

            Assert.That(definition.MaxHealth, Is.EqualTo(20));

            Object.DestroyImmediate(firstObject);
            Object.DestroyImmediate(secondObject);
            Object.DestroyImmediate(definition);
        }
    }
}