using System;
using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ReusableUIElementCollectionTests
    {
        private UIElementPool<TestReusableElement> pool;
        private ReusableUIElementCollection<TestReusableElement>
            collection;

        [SetUp]
        public void SetUp()
        {
            pool =
                new UIElementPool<TestReusableElement>(
                    () => new TestReusableElement());

            collection =
                new ReusableUIElementCollection<TestReusableElement>(
                    pool);
        }

        [Test]
        public void Constructor_NullPool_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () =>
                    new ReusableUIElementCollection<
                        TestReusableElement>(null));
        }

        [Test]
        public void SetCount_NegativeCount_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => collection.SetCount(-1));

            Assert.That(collection.Count, Is.Zero);
            Assert.That(pool.CreatedCount, Is.Zero);
        }

        [Test]
        public void SetCount_FromZero_CreatesRequiredElements()
        {
            collection.SetCount(3);

            Assert.That(collection.Count, Is.EqualTo(3));
            Assert.That(pool.CreatedCount, Is.EqualTo(3));
            Assert.That(pool.ActiveCount, Is.EqualTo(3));
            Assert.That(pool.AvailableCount, Is.Zero);

            Assert.That(
                collection[0].PrepareCount,
                Is.EqualTo(1));

            Assert.That(
                collection[1].PrepareCount,
                Is.EqualTo(1));

            Assert.That(
                collection[2].PrepareCount,
                Is.EqualTo(1));
        }

        [Test]
        public void SetCount_SameCount_PreservesExistingElements()
        {
            collection.SetCount(3);

            TestReusableElement first = collection[0];
            TestReusableElement second = collection[1];
            TestReusableElement third = collection[2];

            collection.SetCount(3);

            Assert.That(collection[0], Is.SameAs(first));
            Assert.That(collection[1], Is.SameAs(second));
            Assert.That(collection[2], Is.SameAs(third));

            Assert.That(pool.CreatedCount, Is.EqualTo(3));
            Assert.That(pool.ActiveCount, Is.EqualTo(3));
        }

        [Test]
        public void SetCount_Increase_PreservesExistingAndAddsMissing()
        {
            collection.SetCount(2);

            TestReusableElement first = collection[0];
            TestReusableElement second = collection[1];

            collection.SetCount(4);

            Assert.That(collection[0], Is.SameAs(first));
            Assert.That(collection[1], Is.SameAs(second));
            Assert.That(collection.Count, Is.EqualTo(4));

            Assert.That(pool.CreatedCount, Is.EqualTo(4));
            Assert.That(pool.ActiveCount, Is.EqualTo(4));
            Assert.That(pool.AvailableCount, Is.Zero);
        }

        [Test]
        public void SetCount_Decrease_ReleasesElementsFromEnd()
        {
            collection.SetCount(3);

            TestReusableElement first = collection[0];
            TestReusableElement second = collection[1];
            TestReusableElement third = collection[2];

            collection.SetCount(1);

            Assert.That(collection.Count, Is.EqualTo(1));
            Assert.That(collection[0], Is.SameAs(first));

            Assert.That(first.ResetCount, Is.Zero);
            Assert.That(second.ResetCount, Is.EqualTo(1));
            Assert.That(third.ResetCount, Is.EqualTo(1));

            Assert.That(pool.CreatedCount, Is.EqualTo(3));
            Assert.That(pool.ActiveCount, Is.EqualTo(1));
            Assert.That(pool.AvailableCount, Is.EqualTo(2));
        }

        [Test]
        public void Clear_ReleasesAllCollectionElements()
        {
            collection.SetCount(3);

            TestReusableElement first = collection[0];
            TestReusableElement second = collection[1];
            TestReusableElement third = collection[2];

            collection.Clear();

            Assert.That(collection.Count, Is.Zero);

            Assert.That(first.ResetCount, Is.EqualTo(1));
            Assert.That(second.ResetCount, Is.EqualTo(1));
            Assert.That(third.ResetCount, Is.EqualTo(1));

            Assert.That(pool.ActiveCount, Is.Zero);
            Assert.That(pool.AvailableCount, Is.EqualTo(3));
        }

        [Test]
        public void SetCount_AfterClear_ReusesPooledElements()
        {
            collection.SetCount(3);
            collection.Clear();

            collection.SetCount(3);

            Assert.That(collection.Count, Is.EqualTo(3));
            Assert.That(pool.CreatedCount, Is.EqualTo(3));
            Assert.That(pool.ActiveCount, Is.EqualTo(3));
            Assert.That(pool.AvailableCount, Is.Zero);
        }

        [Test]
        public void FiftyRefreshes_DoNotCreateMoreThanPeakCount()
        {
            for (int index = 0; index < 50; index++)
            {
                collection.SetCount(10);
                collection.SetCount(3);
            }

            Assert.That(collection.Count, Is.EqualTo(3));
            Assert.That(pool.CreatedCount, Is.EqualTo(10));
            Assert.That(pool.ActiveCount, Is.EqualTo(3));
            Assert.That(pool.AvailableCount, Is.EqualTo(7));
        }

        [Test]
        public void SetItems_NullItems_ThrowsExceptionWithoutChangingCollection()
        {
            collection.SetCount(2);

            Assert.Throws<ArgumentNullException>(
                () => collection.SetItems<string>(
                    null,
                    (element, item) => element.Bind(item)));

            Assert.That(collection.Count, Is.EqualTo(2));
            Assert.That(pool.CreatedCount, Is.EqualTo(2));
            Assert.That(pool.ActiveCount, Is.EqualTo(2));
        }

        [Test]
        public void SetItems_NullBinder_ThrowsExceptionWithoutChangingCollection()
        {
            collection.SetCount(2);

            string[] items =
            {
        "First",
        "Second"
    };

            Assert.Throws<ArgumentNullException>(
                () => collection.SetItems(
                    items,
                    null));

            Assert.That(collection.Count, Is.EqualTo(2));
            Assert.That(pool.CreatedCount, Is.EqualTo(2));
            Assert.That(pool.ActiveCount, Is.EqualTo(2));
        }

        [Test]
        public void SetItems_BindsEveryItemInOrder()
        {
            string[] items =
            {
        "First",
        "Second",
        "Third"
    };

            collection.SetItems(
                items,
                (element, item) => element.Bind(item));

            Assert.That(collection.Count, Is.EqualTo(3));
            Assert.That(collection[0].BoundValue, Is.EqualTo("First"));
            Assert.That(collection[1].BoundValue, Is.EqualTo("Second"));
            Assert.That(collection[2].BoundValue, Is.EqualTo("Third"));
        }

        [Test]
        public void SetItems_SameCount_ReusesExistingElements()
        {
            string[] firstItems =
            {
        "First",
        "Second"
    };

            collection.SetItems(
                firstItems,
                (element, item) => element.Bind(item));

            TestReusableElement first = collection[0];
            TestReusableElement second = collection[1];

            string[] updatedItems =
            {
        "Updated First",
        "Updated Second"
    };

            collection.SetItems(
                updatedItems,
                (element, item) => element.Bind(item));

            Assert.That(collection[0], Is.SameAs(first));
            Assert.That(collection[1], Is.SameAs(second));
            Assert.That(first.BoundValue, Is.EqualTo("Updated First"));
            Assert.That(second.BoundValue, Is.EqualTo("Updated Second"));
            Assert.That(pool.CreatedCount, Is.EqualTo(2));
        }

        [Test]
        public void SetItems_EmptyList_ReleasesAllElements()
        {
            collection.SetItems(
                new[] { "First", "Second", "Third" },
                (element, item) => element.Bind(item));

            collection.SetItems(
                Array.Empty<string>(),
                (element, item) => element.Bind(item));

            Assert.That(collection.Count, Is.Zero);
            Assert.That(pool.ActiveCount, Is.Zero);
            Assert.That(pool.AvailableCount, Is.EqualTo(3));
        }

        private sealed class TestReusableElement :
            IReusableUIElement
        {
            public int PrepareCount { get; private set; }

            public int ResetCount { get; private set; }

            public void PrepareForUse()
            {
                PrepareCount++;
            }

            public string BoundValue { get; private set; }

            public void Bind(
                string value)
            {
                BoundValue = value;
            }

            public void ResetForPool()
            {
                ResetCount++;
                BoundValue = null;
            }
        }
    }
}