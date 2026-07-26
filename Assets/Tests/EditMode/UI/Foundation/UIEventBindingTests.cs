using System;
using System.Collections.Generic;
using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class UIEventBindingTests
    {
        [Test]
        public void Bind_RendersCurrentViewModelExactlyOnce()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            List<int> renderedValues = new();

            binding.Bind(
                source,
                renderedValues.Add);

            Assert.That(binding.IsBound, Is.True);
            Assert.That(binding.IsActive, Is.False);

            Assert.That(
                renderedValues,
                Is.EqualTo(new[] { 10 }));

            Assert.That(source.SubscriberCount, Is.EqualTo(0));
        }

        [Test]
        public void Bind_DoesNotReactUntilActivated()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            List<int> renderedValues = new();

            binding.Bind(
                source,
                renderedValues.Add);

            source.Publish(20);

            Assert.That(
                renderedValues,
                Is.EqualTo(new[] { 10 }));
        }

        [Test]
        public void Activate_ReactToSourceChanges()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            List<int> renderedValues = new();

            binding.Bind(
                source,
                renderedValues.Add);

            binding.Activate();
            source.Publish(20);

            Assert.That(binding.IsActive, Is.True);
            Assert.That(source.SubscriberCount, Is.EqualTo(1));

            Assert.That(
                renderedValues,
                Is.EqualTo(new[] { 10, 20 }));
        }

        [Test]
        public void Activate_Twice_DoesNotDuplicateSubscription()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            List<int> renderedValues = new();

            binding.Bind(
                source,
                renderedValues.Add);

            binding.Activate();
            binding.Activate();

            source.Publish(20);

            Assert.That(source.SubscriberCount, Is.EqualTo(1));

            Assert.That(
                renderedValues,
                Is.EqualTo(new[] { 10, 20 }));
        }

        [Test]
        public void Deactivate_StopsReactingToSourceChanges()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            List<int> renderedValues = new();

            binding.Bind(
                source,
                renderedValues.Add);

            binding.Activate();
            binding.Deactivate();

            source.Publish(20);

            Assert.That(binding.IsActive, Is.False);
            Assert.That(source.SubscriberCount, Is.EqualTo(0));

            Assert.That(
                renderedValues,
                Is.EqualTo(new[] { 10 }));
        }

        [Test]
        public void Deactivate_Twice_RemainsSafe()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            binding.Bind(
                source,
                _ => { });

            binding.Activate();
            binding.Deactivate();
            binding.Deactivate();

            Assert.That(binding.IsActive, Is.False);
            Assert.That(source.SubscriberCount, Is.EqualTo(0));
        }

        [Test]
        public void Reactivate_AfterDeactivate_RestoresSingleSubscription()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            List<int> renderedValues = new();

            binding.Bind(
                source,
                renderedValues.Add);

            binding.Activate();
            binding.Deactivate();
            binding.Activate();

            source.Publish(20);

            Assert.That(binding.IsActive, Is.True);
            Assert.That(source.SubscriberCount, Is.EqualTo(1));

            Assert.That(
                renderedValues,
                Is.EqualTo(new[] { 10, 20 }));
        }

        [Test]
        public void Unbind_WhileActive_RemovesSubscription()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            List<int> renderedValues = new();

            binding.Bind(
                source,
                renderedValues.Add);

            binding.Activate();
            binding.Unbind();

            source.Publish(20);

            Assert.That(binding.IsBound, Is.False);
            Assert.That(binding.IsActive, Is.False);
            Assert.That(source.SubscriberCount, Is.EqualTo(0));

            Assert.That(
                renderedValues,
                Is.EqualTo(new[] { 10 }));
        }

        [Test]
        public void Bind_WhenAlreadyBound_ThrowsException()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            binding.Bind(
                source,
                _ => { });

            Assert.Throws<InvalidOperationException>(
                () => binding.Bind(
                    source,
                    _ => { }));
        }

        [Test]
        public void Bind_NullSource_ThrowsException()
        {
            UIEventBinding<int> binding = new();

            Assert.Throws<ArgumentNullException>(
                () => binding.Bind(
                    null,
                    _ => { }));
        }

        [Test]
        public void Bind_NullRenderAction_ThrowsException()
        {
            TestViewModelSource source = new(10);
            UIEventBinding<int> binding = new();

            Assert.Throws<ArgumentNullException>(
                () => binding.Bind(
                    source,
                    null));
        }

        [Test]
        public void Activate_WhenNotBound_ThrowsException()
        {
            UIEventBinding<int> binding = new();

            Assert.Throws<InvalidOperationException>(
                binding.Activate);
        }

        [Test]
        public void Unbind_WhenNotBound_ThrowsException()
        {
            UIEventBinding<int> binding = new();

            Assert.Throws<InvalidOperationException>(
                binding.Unbind);
        }

        private sealed class TestViewModelSource :
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
}