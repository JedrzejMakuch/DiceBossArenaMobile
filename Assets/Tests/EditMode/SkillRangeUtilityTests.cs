using DiceBossArena.Tests.Fixtures;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class SkillRangeUtilityTests
    {
        [Test]
        public void ManhattanRange_AcceptsTileWithinDistance()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile destination =
                CreateTile(
                    "Destination",
                    1,
                    1);

            SkillDefinition skill =
                TestSkillFactory.Create(
                    rangeShape: SkillRangeShape.Manhattan,
                    minRange: 1,
                    maxRange: 2);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsWithinRange(
                        origin,
                        destination,
                        skill),
                    Is.True);
            }
            finally
            {
                Destroy(
                    origin,
                    destination,
                    skill);
            }
        }

        [Test]
        public void ManhattanRange_RejectsTileAboveMaximumRange()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile destination =
                CreateTile(
                    "Destination",
                    2,
                    2);

            SkillDefinition skill =
                TestSkillFactory.Create(
                    rangeShape: SkillRangeShape.Manhattan,
                    minRange: 1,
                    maxRange: 3);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsWithinRange(
                        origin,
                        destination,
                        skill),
                    Is.False);
            }
            finally
            {
                Destroy(
                    origin,
                    destination,
                    skill);
            }
        }

        [Test]
        public void StraightLineRange_AcceptsOrthogonalTile()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile destination =
                CreateTile(
                    "Destination",
                    3,
                    0);

            SkillDefinition skill =
                TestSkillFactory.Create(
                    rangeShape: SkillRangeShape.StraightLine,
                    minRange: 1,
                    maxRange: 3);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsWithinRange(
                        origin,
                        destination,
                        skill),
                    Is.True);
            }
            finally
            {
                Destroy(
                    origin,
                    destination,
                    skill);
            }
        }

        [Test]
        public void StraightLineRange_RejectsDiagonalTile()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile destination =
                CreateTile(
                    "Destination",
                    1,
                    1);

            SkillDefinition skill =
                TestSkillFactory.Create(
                    rangeShape: SkillRangeShape.StraightLine,
                    minRange: 1,
                    maxRange: 3);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsWithinRange(
                        origin,
                        destination,
                        skill),
                    Is.False);
            }
            finally
            {
                Destroy(
                    origin,
                    destination,
                    skill);
            }
        }

        [Test]
        public void Range_RejectsTileBelowMinimumRange()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile destination =
                CreateTile(
                    "Destination",
                    1,
                    0);

            SkillDefinition skill =
                TestSkillFactory.Create(
                    rangeShape: SkillRangeShape.Manhattan,
                    minRange: 2,
                    maxRange: 4);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsWithinRange(
                        origin,
                        destination,
                        skill),
                    Is.False);
            }
            finally
            {
                Destroy(
                    origin,
                    destination,
                    skill);
            }
        }

        [Test]
        public void Range_AcceptsTilesExactlyAtMinimumAndMaximum()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile minimumTile =
                CreateTile(
                    "MinimumTile",
                    2,
                    0);

            FightGridTile maximumTile =
                CreateTile(
                    "MaximumTile",
                    4,
                    0);

            SkillDefinition skill =
                TestSkillFactory.Create(
                    rangeShape: SkillRangeShape.Manhattan,
                    minRange: 2,
                    maxRange: 4);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsWithinRange(
                        origin,
                        minimumTile,
                        skill),
                    Is.True);

                Assert.That(
                    SkillRangeUtility.IsWithinRange(
                        origin,
                        maximumTile,
                        skill),
                    Is.True);
            }
            finally
            {
                Destroy(
                    origin,
                    minimumTile,
                    maximumTile,
                    skill);
            }
        }

        [Test]
        public void DirectionSelector_AcceptsAdjacentOrthogonalTile()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile directionTile =
                CreateTile(
                    "DirectionTile",
                    0,
                    1);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsDirectionSelectorTile(
                        origin,
                        directionTile),
                    Is.True);

                Assert.That(
                    SkillRangeUtility.GetDirection(
                        origin,
                        directionTile),
                    Is.EqualTo(SkillDirection.Up));
            }
            finally
            {
                Destroy(
                    origin,
                    directionTile);
            }
        }

        [Test]
        public void DirectionSelector_RejectsDiagonalTile()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile directionTile =
                CreateTile(
                    "DirectionTile",
                    1,
                    1);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsDirectionSelectorTile(
                        origin,
                        directionTile),
                    Is.False);

                Assert.That(
                    SkillRangeUtility.GetDirection(
                        origin,
                        directionTile),
                    Is.EqualTo(SkillDirection.None));
            }
            finally
            {
                Destroy(
                    origin,
                    directionTile);
            }
        }

        [Test]
        public void Cone_AcceptsForwardTilesWithinWidth()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile centerTile =
                CreateTile(
                    "CenterTile",
                    0,
                    3);

            FightGridTile sideTile =
                CreateTile(
                    "SideTile",
                    2,
                    3);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsTileInCone(
                        origin,
                        centerTile,
                        SkillDirection.Up,
                        3),
                    Is.True);

                Assert.That(
                    SkillRangeUtility.IsTileInCone(
                        origin,
                        sideTile,
                        SkillDirection.Up,
                        3),
                    Is.True);
            }
            finally
            {
                Destroy(
                    origin,
                    centerTile,
                    sideTile);
            }
        }

        [Test]
        public void Cone_RejectsTileOutsideConeWidth()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile outsideTile =
                CreateTile(
                    "OutsideTile",
                    3,
                    3);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsTileInCone(
                        origin,
                        outsideTile,
                        SkillDirection.Up,
                        3),
                    Is.False);
            }
            finally
            {
                Destroy(
                    origin,
                    outsideTile);
            }
        }

        [Test]
        public void Cone_RejectsTileBehindCaster()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile behindTile =
                CreateTile(
                    "BehindTile",
                    0,
                    -1);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsTileInCone(
                        origin,
                        behindTile,
                        SkillDirection.Up,
                        3),
                    Is.False);
            }
            finally
            {
                Destroy(
                    origin,
                    behindTile);
            }
        }

        [Test]
        public void Cone_RejectsTileAboveMaximumRange()
        {
            FightGridTile origin =
                CreateTile(
                    "Origin",
                    0,
                    0);

            FightGridTile distantTile =
                CreateTile(
                    "DistantTile",
                    0,
                    4);

            try
            {
                Assert.That(
                    SkillRangeUtility.IsTileInCone(
                        origin,
                        distantTile,
                        SkillDirection.Up,
                        3),
                    Is.False);
            }
            finally
            {
                Destroy(
                    origin,
                    distantTile);
            }
        }

        private static FightGridTile CreateTile(
            string objectName,
            int gridX,
            int gridY)
        {
            GameObject tileObject =
                new GameObject(objectName);

            FightGridTile tile =
                tileObject.AddComponent<FightGridTile>();

            tile.Initialize(
                gridX,
                gridY);

            return tile;
        }

        private static void Destroy(
            params Object[] objects)
        {
            foreach (Object createdObject in objects)
            {
                if (createdObject == null)
                {
                    continue;
                }

                if (createdObject is Component component)
                {
                    Object.DestroyImmediate(
                        component.gameObject);
                }
                else
                {
                    Object.DestroyImmediate(
                        createdObject);
                }
            }
        }
    }
}