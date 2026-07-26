using System;
using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class UIModalStackTests
    {
        [Test]
        public void Push_FirstModal_ShowsItAndEnablesInput()
        {
            UIModalStack stack = new();
            TestModalView modal = new();

            stack.Push(modal);

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.HasModal, Is.True);
            Assert.That(stack.Top, Is.SameAs(modal));

            Assert.That(modal.IsVisible, Is.True);
            Assert.That(modal.IsInputEnabled, Is.True);
            Assert.That(modal.ShowCount, Is.EqualTo(1));
        }

        [Test]
        public void Push_SecondModal_DisablesInputOnPreviousModal()
        {
            UIModalStack stack = new();
            TestModalView firstModal = new();
            TestModalView secondModal = new();

            stack.Push(firstModal);
            stack.Push(secondModal);

            Assert.That(stack.Count, Is.EqualTo(2));
            Assert.That(stack.Top, Is.SameAs(secondModal));

            Assert.That(firstModal.IsVisible, Is.True);
            Assert.That(firstModal.IsInputEnabled, Is.False);

            Assert.That(secondModal.IsVisible, Is.True);
            Assert.That(secondModal.IsInputEnabled, Is.True);
        }

        [Test]
        public void TryPop_ClosesTopModalAndRestoresPreviousInput()
        {
            UIModalStack stack = new();
            TestModalView firstModal = new();
            TestModalView secondModal = new();

            stack.Push(firstModal);
            stack.Push(secondModal);

            bool didPop = stack.TryPop();

            Assert.That(didPop, Is.True);
            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Top, Is.SameAs(firstModal));

            Assert.That(secondModal.IsVisible, Is.False);
            Assert.That(secondModal.IsInputEnabled, Is.False);
            Assert.That(secondModal.HideCount, Is.EqualTo(1));

            Assert.That(firstModal.IsVisible, Is.True);
            Assert.That(firstModal.IsInputEnabled, Is.True);
        }

        [Test]
        public void TryPop_EmptyStack_ReturnsFalse()
        {
            UIModalStack stack = new();

            bool didPop = stack.TryPop();

            Assert.That(didPop, Is.False);
            Assert.That(stack.Count, Is.EqualTo(0));
            Assert.That(stack.HasModal, Is.False);
        }

        [Test]
        public void Clear_ClosesAllModalsAndDisablesTheirInput()
        {
            UIModalStack stack = new();
            TestModalView firstModal = new();
            TestModalView secondModal = new();
            TestModalView thirdModal = new();

            stack.Push(firstModal);
            stack.Push(secondModal);
            stack.Push(thirdModal);

            stack.Clear();

            Assert.That(stack.Count, Is.EqualTo(0));
            Assert.That(stack.HasModal, Is.False);

            AssertClosed(firstModal);
            AssertClosed(secondModal);
            AssertClosed(thirdModal);
        }

        [Test]
        public void Push_SameModalTwice_ThrowsException()
        {
            UIModalStack stack = new();
            TestModalView modal = new();

            stack.Push(modal);

            Assert.Throws<InvalidOperationException>(
                () => stack.Push(modal));

            Assert.That(stack.Count, Is.EqualTo(1));
            Assert.That(stack.Top, Is.SameAs(modal));
        }

        [Test]
        public void Push_NullModal_ThrowsException()
        {
            UIModalStack stack = new();

            Assert.Throws<ArgumentNullException>(
                () => stack.Push(null));
        }

        [Test]
        public void Top_EmptyStack_ThrowsException()
        {
            UIModalStack stack = new();

            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    IUIModalView unused = stack.Top;
                });
        }

        private static void AssertClosed(
            TestModalView modal)
        {
            Assert.That(modal.IsVisible, Is.False);
            Assert.That(modal.IsInputEnabled, Is.False);
            Assert.That(modal.HideCount, Is.EqualTo(1));
        }

        private sealed class TestModalView : IUIModalView
        {
            public bool IsVisible { get; private set; }
            public bool IsInputEnabled { get; private set; }

            public int ShowCount { get; private set; }
            public int HideCount { get; private set; }

            public void Show()
            {
                IsVisible = true;
                ShowCount++;
            }

            public void Hide()
            {
                IsVisible = false;
                HideCount++;
            }

            public void SetInputEnabled(bool isEnabled)
            {
                IsInputEnabled = isEnabled;
            }
        }
    }
}