using System;
using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class UIScreenHistoryTests
    {
        [Test]
        public void Open_FirstScreen_SetsCurrentScreen()
        {
            UIScreenHistory<TestScreenId> history = new();

            history.Open(TestScreenId.MainMenu);

            Assert.That(history.HasCurrentScreen, Is.True);
            Assert.That(
                history.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));
            Assert.That(history.CanGoBack, Is.False);
        }

        [Test]
        public void Open_SecondScreen_AddsPreviousScreenToHistory()
        {
            UIScreenHistory<TestScreenId> history = new();

            history.Open(TestScreenId.MainMenu);
            history.Open(TestScreenId.Inventory);

            Assert.That(
                history.CurrentScreen,
                Is.EqualTo(TestScreenId.Inventory));
            Assert.That(history.CanGoBack, Is.True);

            bool didGoBack =
                history.TryGoBack(out TestScreenId screenId);

            Assert.That(didGoBack, Is.True);
            Assert.That(
                screenId,
                Is.EqualTo(TestScreenId.MainMenu));
            Assert.That(
                history.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));
            Assert.That(history.CanGoBack, Is.False);
        }

        [Test]
        public void Open_CurrentScreen_DoesNotDuplicateHistory()
        {
            UIScreenHistory<TestScreenId> history = new();

            history.Open(TestScreenId.MainMenu);
            history.Open(TestScreenId.Inventory);
            history.Open(TestScreenId.Inventory);

            Assert.That(history.TryGoBack(out _), Is.True);
            Assert.That(
                history.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));
            Assert.That(history.CanGoBack, Is.False);
        }

        [Test]
        public void Replace_ChangesCurrentScreenWithoutAddingHistory()
        {
            UIScreenHistory<TestScreenId> history = new();

            history.Open(TestScreenId.MainMenu);
            history.Open(TestScreenId.Inventory);
            history.Replace(TestScreenId.Settings);

            Assert.That(
                history.CurrentScreen,
                Is.EqualTo(TestScreenId.Settings));

            Assert.That(
                history.TryGoBack(out TestScreenId screenId),
                Is.True);

            Assert.That(
                screenId,
                Is.EqualTo(TestScreenId.MainMenu));

            Assert.That(history.CanGoBack, Is.False);
        }

        [Test]
        public void TryGoBack_WithoutPreviousScreen_ReturnsFalse()
        {
            UIScreenHistory<TestScreenId> history = new();

            history.Open(TestScreenId.MainMenu);

            bool didGoBack =
                history.TryGoBack(out TestScreenId screenId);

            Assert.That(didGoBack, Is.False);
            Assert.That(screenId, Is.EqualTo(default(TestScreenId)));
            Assert.That(
                history.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));
        }

        [Test]
        public void Clear_RemovesCurrentScreenAndHistory()
        {
            UIScreenHistory<TestScreenId> history = new();

            history.Open(TestScreenId.MainMenu);
            history.Open(TestScreenId.Inventory);

            history.Clear();

            Assert.That(history.HasCurrentScreen, Is.False);
            Assert.That(history.CanGoBack, Is.False);

            Assert.Throws<InvalidOperationException>(
                () =>
                {
                    TestScreenId unused =
                        history.CurrentScreen;
                });
        }

        private enum TestScreenId
        {
            MainMenu,
            Inventory,
            Settings
        }
    }
}