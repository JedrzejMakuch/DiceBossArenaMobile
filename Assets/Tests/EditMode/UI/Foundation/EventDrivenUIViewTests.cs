using System;
using System.Collections.Generic;
using DiceBossArena.UI;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EventDrivenUIViewTests
    {
        private GameObject viewObject;
        private TestEventDrivenUIView view;
        private TestViewModelSource source;

        [SetUp]
        public void SetUp()
        {
            viewObject =
                new GameObject("Test Event Driven UI View");

            view =
                viewObject.AddComponent<TestEventDrivenUIView>();

            source =
                new TestViewModelSource(10);
        }

        [TearDown]
        public void TearDown()
        {
            if (viewObject != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    viewObject);
            }
        }

        [Test]
        public void Bind_RendersCurrentViewModelExactlyOnce()
        {
            view.Bind(source);

            Assert.That(
                view.RenderedValues,
                Is.EqualTo(new[] { 10 }));

            Assert.That(view.IsBound, Is.True);
            Assert.That(view.IsVisible, Is.False);
            Assert.That(source.SubscriberCount, Is.Zero);
        }

        [Test]
        public void BoundHiddenView_DoesNotReactToChanges()
        {
            view.Bind(source);

            source.Publish(20);

            Assert.That(
                view.RenderedValues,
                Is.EqualTo(new[] { 10 }));

            Assert.That(source.SubscriberCount, Is.Zero);
        }

        [Test]
        public void Show_ActivatesEventDrivenUpdates()
        {
            view.Bind(source);
            view.Show();

            source.Publish(20);

            Assert.That(view.IsVisible, Is.True);
            Assert.That(source.SubscriberCount, Is.EqualTo(1));

            Assert.That(
                view.RenderedValues,
                Is.EqualTo(new[] { 10, 20 }));
        }

        [Test]
        public void Hide_StopsEventDrivenUpdates()
        {
            view.Bind(source);
            view.Show();
            view.Hide();

            source.Publish(20);

            Assert.That(view.IsVisible, Is.False);
            Assert.That(source.SubscriberCount, Is.Zero);

            Assert.That(
                view.RenderedValues,
                Is.EqualTo(new[] { 10 }));
        }

        [Test]
        public void ShowAfterHide_RestoresSingleSubscription()
        {
            view.Bind(source);
            view.Show();
            view.Hide();
            view.Show();

            source.Publish(20);

            Assert.That(source.SubscriberCount, Is.EqualTo(1));

            Assert.That(
                view.RenderedValues,
                Is.EqualTo(new[] { 10, 20 }));
        }

        [Test]
        public void Unbind_AfterHide_RemovesBinding()
        {
            view.Bind(source);
            view.Show();
            view.Hide();
            view.Unbind();

            source.Publish(20);

            Assert.That(view.IsBound, Is.False);
            Assert.That(view.IsVisible, Is.False);
            Assert.That(source.SubscriberCount, Is.Zero);

            Assert.That(
                view.RenderedValues,
                Is.EqualTo(new[] { 10 }));
        }

        [Test]
        public void View_CanBindToAnotherSourceAfterUnbind()
        {
            TestViewModelSource secondSource =
                new TestViewModelSource(30);

            view.Bind(source);
            view.Unbind();

            view.Bind(secondSource);
            view.Show();

            secondSource.Publish(40);

            Assert.That(source.SubscriberCount, Is.Zero);
            Assert.That(secondSource.SubscriberCount, Is.EqualTo(1));

            Assert.That(
                view.RenderedValues,
                Is.EqualTo(new[] { 10, 30, 40 }));
        }
    }

    public sealed class TestEventDrivenUIView :
        EventDrivenUIView<int>
    {
        public List<int> RenderedValues { get; } =
            new();

        protected override void Render(
            int viewModel)
        {
            RenderedValues.Add(viewModel);
        }
    }

    public sealed class TestViewModelSource :
        IUIViewModelSource<int>
    {
        private Action<int> changed;

        public TestViewModelSource(
            int current)
        {
            Current = current;
        }

        public int Current { get; private set; }

        public int SubscriberCount =>
            changed?.GetInvocationList().Length ?? 0;

        public event Action<int> Changed
        {
            add => changed += value;
            remove => changed -= value;
        }

        public void Publish(
            int value)
        {
            Current = value;
            changed?.Invoke(value);
        }
    }
}