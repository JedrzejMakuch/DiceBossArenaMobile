using System;
using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class UIScreenRouterTests
    {
        [Test]
        public void Open_FirstScreen_ShowsRegisteredView()
        {
            UIScreenRouter<TestScreenId> router = new();
            TestUIView mainMenu = new();

            router.Register(
                TestScreenId.MainMenu,
                mainMenu);

            router.Open(TestScreenId.MainMenu);

            Assert.That(router.HasCurrentScreen, Is.True);
            Assert.That(
                router.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));

            Assert.That(mainMenu.IsVisible, Is.True);
            Assert.That(mainMenu.ShowCount, Is.EqualTo(1));
            Assert.That(mainMenu.HideCount, Is.EqualTo(0));
        }

        [Test]
        public void Open_SecondScreen_HidesPreviousView()
        {
            UIScreenRouter<TestScreenId> router = new();
            TestUIView mainMenu = new();
            TestUIView inventory = new();

            router.Register(
                TestScreenId.MainMenu,
                mainMenu);

            router.Register(
                TestScreenId.Inventory,
                inventory);

            router.Open(TestScreenId.MainMenu);
            router.Open(TestScreenId.Inventory);

            Assert.That(mainMenu.IsVisible, Is.False);
            Assert.That(mainMenu.HideCount, Is.EqualTo(1));

            Assert.That(inventory.IsVisible, Is.True);
            Assert.That(inventory.ShowCount, Is.EqualTo(1));

            Assert.That(
                router.CurrentScreen,
                Is.EqualTo(TestScreenId.Inventory));

            Assert.That(router.CanGoBack, Is.True);
        }

        [Test]
        public void Open_CurrentScreen_DoesNotShowItAgain()
        {
            UIScreenRouter<TestScreenId> router = new();
            TestUIView mainMenu = new();

            router.Register(
                TestScreenId.MainMenu,
                mainMenu);

            router.Open(TestScreenId.MainMenu);
            router.Open(TestScreenId.MainMenu);

            Assert.That(mainMenu.ShowCount, Is.EqualTo(1));
            Assert.That(mainMenu.HideCount, Is.EqualTo(0));
            Assert.That(router.CanGoBack, Is.False);
        }

        [Test]
        public void Replace_ChangesViewWithoutAddingHistoryEntry()
        {
            UIScreenRouter<TestScreenId> router = new();
            TestUIView mainMenu = new();
            TestUIView inventory = new();
            TestUIView settings = new();

            router.Register(
                TestScreenId.MainMenu,
                mainMenu);

            router.Register(
                TestScreenId.Inventory,
                inventory);

            router.Register(
                TestScreenId.Settings,
                settings);

            router.Open(TestScreenId.MainMenu);
            router.Open(TestScreenId.Inventory);
            router.Replace(TestScreenId.Settings);

            Assert.That(inventory.IsVisible, Is.False);
            Assert.That(settings.IsVisible, Is.True);

            Assert.That(
                router.CurrentScreen,
                Is.EqualTo(TestScreenId.Settings));

            Assert.That(router.TryGoBack(), Is.True);

            Assert.That(mainMenu.IsVisible, Is.True);
            Assert.That(settings.IsVisible, Is.False);

            Assert.That(
                router.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));

            Assert.That(router.CanGoBack, Is.False);
        }

        [Test]
        public void TryGoBack_ShowsPreviousScreen()
        {
            UIScreenRouter<TestScreenId> router = new();
            TestUIView mainMenu = new();
            TestUIView inventory = new();

            router.Register(
                TestScreenId.MainMenu,
                mainMenu);

            router.Register(
                TestScreenId.Inventory,
                inventory);

            router.Open(TestScreenId.MainMenu);
            router.Open(TestScreenId.Inventory);

            bool didGoBack = router.TryGoBack();

            Assert.That(didGoBack, Is.True);
            Assert.That(mainMenu.IsVisible, Is.True);
            Assert.That(inventory.IsVisible, Is.False);

            Assert.That(
                router.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));
        }

        [Test]
        public void TryGoBack_WithoutHistory_LeavesScreenVisible()
        {
            UIScreenRouter<TestScreenId> router = new();
            TestUIView mainMenu = new();

            router.Register(
                TestScreenId.MainMenu,
                mainMenu);

            router.Open(TestScreenId.MainMenu);

            bool didGoBack = router.TryGoBack();

            Assert.That(didGoBack, Is.False);
            Assert.That(mainMenu.IsVisible, Is.True);
            Assert.That(mainMenu.HideCount, Is.EqualTo(0));
        }

        [Test]
        public void Clear_HidesVisibleScreenAndClearsHistory()
        {
            UIScreenRouter<TestScreenId> router = new();
            TestUIView mainMenu = new();
            TestUIView inventory = new();

            router.Register(
                TestScreenId.MainMenu,
                mainMenu);

            router.Register(
                TestScreenId.Inventory,
                inventory);

            router.Open(TestScreenId.MainMenu);
            router.Open(TestScreenId.Inventory);

            router.Clear();

            Assert.That(mainMenu.IsVisible, Is.False);
            Assert.That(inventory.IsVisible, Is.False);
            Assert.That(router.HasCurrentScreen, Is.False);
            Assert.That(router.CanGoBack, Is.False);
        }

        [Test]
        public void Register_DuplicateId_ThrowsException()
        {
            UIScreenRouter<TestScreenId> router = new();

            router.Register(
                TestScreenId.MainMenu,
                new TestUIView());

            Assert.Throws<InvalidOperationException>(
                () => router.Register(
                    TestScreenId.MainMenu,
                    new TestUIView()));
        }

        [Test]
        public void Open_UnregisteredScreen_ThrowsException()
        {
            UIScreenRouter<TestScreenId> router = new();

            Assert.Throws<InvalidOperationException>(
                () => router.Open(TestScreenId.MainMenu));
        }

        private enum TestScreenId
        {
            MainMenu,
            Inventory,
            Settings
        }

        private sealed class TestUIView : IUIView
        {
            public bool IsVisible { get; private set; }

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
        }
    }
}