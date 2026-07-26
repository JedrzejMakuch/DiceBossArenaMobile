using System;
using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class UIElementPoolTests
    {
        [Test]
        public void Constructor_NullFactory_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(
                () => new UIElementPool<TestReusableElement>(
                    null));
        }

        [Test]
        public void Get_EmptyPool_CreatesAndPreparesElement()
        {
            UIElementPool<TestReusableElement> pool =
                CreatePool();

            TestReusableElement element =
                pool.Get();

            Assert.That(element, Is.Not.Null);
            Assert.That(element.PrepareCount, Is.EqualTo(1));
            Assert.That(element.ResetCount, Is.Zero);

            Assert.That(pool.CreatedCount, Is.EqualTo(1));
            Assert.That(pool.ActiveCount, Is.EqualTo(1));
            Assert.That(pool.AvailableCount, Is.Zero);
        }

        [Test]
        public void Get_TwiceWithoutRelease_CreatesTwoElements()
        {
            UIElementPool<TestReusableElement> pool =
                CreatePool();

            TestReusableElement first =
                pool.Get();

            TestReusableElement second =
                pool.Get();

            Assert.That(second, Is.Not.SameAs(first));
            Assert.That(pool.CreatedCount, Is.EqualTo(2));
            Assert.That(pool.ActiveCount, Is.EqualTo(2));
            Assert.That(pool.AvailableCount, Is.Zero);
        }

        [Test]
        public void Release_ResetsElementAndUpdatesCounts()
        {
            UIElementPool<TestReusableElement> pool =
                CreatePool();

            TestReusableElement element =
                pool.Get();

            pool.Release(element);

            Assert.That(element.ResetCount, Is.EqualTo(1));
            Assert.That(pool.CreatedCount, Is.EqualTo(1));
            Assert.That(pool.ActiveCount, Is.Zero);
            Assert.That(pool.AvailableCount, Is.EqualTo(1));
        }

        [Test]
        public void Get_AfterRelease_ReusesSameElement()
        {
            UIElementPool<TestReusableElement> pool =
                CreatePool();

            TestReusableElement first =
                pool.Get();

            pool.Release(first);

            TestReusableElement reused =
                pool.Get();

            Assert.That(reused, Is.SameAs(first));
            Assert.That(reused.PrepareCount, Is.EqualTo(2));
            Assert.That(reused.ResetCount, Is.EqualTo(1));

            Assert.That(pool.CreatedCount, Is.EqualTo(1));
            Assert.That(pool.ActiveCount, Is.EqualTo(1));
            Assert.That(pool.AvailableCount, Is.Zero);
        }

        [Test]
        public void Release_NullElement_ThrowsException()
        {
            UIElementPool<TestReusableElement> pool =
                CreatePool();

            Assert.Throws<ArgumentNullException>(
                () => pool.Release(null));
        }

        [Test]
        public void Release_ForeignElement_ThrowsException()
        {
            UIElementPool<TestReusableElement> pool =
                CreatePool();

            TestReusableElement foreignElement =
                new();

            Assert.Throws<InvalidOperationException>(
                () => pool.Release(foreignElement));

            Assert.That(foreignElement.ResetCount, Is.Zero);
            Assert.That(pool.ActiveCount, Is.Zero);
            Assert.That(pool.AvailableCount, Is.Zero);
        }

        [Test]
        public void Release_Twice_ThrowsException()
        {
            UIElementPool<TestReusableElement> pool =
                CreatePool();

            TestReusableElement element =
                pool.Get();

            pool.Release(element);

            Assert.Throws<InvalidOperationException>(
                () => pool.Release(element));

            Assert.That(element.ResetCount, Is.EqualTo(1));
            Assert.That(pool.ActiveCount, Is.Zero);
            Assert.That(pool.AvailableCount, Is.EqualTo(1));
        }

        [Test]
        public void Get_FactoryReturnsNull_ThrowsException()
        {
            UIElementPool<TestReusableElement> pool =
                new(() => null);

            Assert.Throws<InvalidOperationException>(
                () => pool.Get());

            Assert.That(pool.CreatedCount, Is.Zero);
            Assert.That(pool.ActiveCount, Is.Zero);
            Assert.That(pool.AvailableCount, Is.Zero);
        }

        [Test]
        public void FiftySequentialUses_CreateOnlyOneElement()
        {
            UIElementPool<TestReusableElement> pool =
                CreatePool();

            for (int index = 0; index < 50; index++)
            {
                TestReusableElement element =
                    pool.Get();

                pool.Release(element);
            }

            Assert.That(pool.CreatedCount, Is.EqualTo(1));
            Assert.That(pool.ActiveCount, Is.Zero);
            Assert.That(pool.AvailableCount, Is.EqualTo(1));
        }

        private static UIElementPool<TestReusableElement>
            CreatePool()
        {
            return new UIElementPool<TestReusableElement>(
                () => new TestReusableElement());
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

            public void ResetForPool()
            {
                ResetCount++;
            }
        }
    }
}