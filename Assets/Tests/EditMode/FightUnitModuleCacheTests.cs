using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class FightUnitModuleCacheTests
    {
        [Test]
        public void Initialize_CachesUnitModules()
        {
            GameObject gameObject = new GameObject("Unit");

            FightUnitTurnResources resources =
                gameObject.AddComponent<FightUnitTurnResources>();

            FightUnitSkills skills =
                gameObject.AddComponent<FightUnitSkills>();

            FightUnit unit =
                gameObject.AddComponent<FightUnit>();

            unit.Initialize(
                newUnitName: "Unit",
                newTeam: FightTeam.Player,
                newMaxHealth: 10,
                newAttackPower: 2,
                newInitiative: 10);

            try
            {
                Assert.That(
                    unit.TurnResources,
                    Is.SameAs(resources));

                Assert.That(
                    unit.Skills,
                    Is.SameAs(skills));
            }
            finally
            {
                Object.DestroyImmediate(gameObject);
            }
        }
    }
}