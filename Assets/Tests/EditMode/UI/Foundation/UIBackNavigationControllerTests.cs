using System;
using DiceBossArena.UI;
using NUnit.Framework;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class UIBackNavigationControllerTests
    {
        [Test]
        public void HandleBack_WithModal_ClosesModalFirst()
        {
            UIModalStack modalStack = new();
            UIScreenRouter<TestScreenId> screenRouter =
                CreateRouterWithHistory();

            TestModalView modal = new();
            modalStack.Push(modal);

            int exitRequestCount = 0;

            UIBackNavigationController<TestScreenId> controller =
                new(
                    modalStack,
                    screenRouter,
                    () => exitRequestCount++);

            controller.HandleBack();

            Assert.That(modalStack.HasModal, Is.False);
            Assert.That(modal.IsVisible, Is.False);

            Assert.That(
                screenRouter.CurrentScreen,
                Is.EqualTo(TestScreenId.Inventory));

            Assert.That(screenRouter.CanGoBack, Is.True);
            Assert.That(exitRequestCount, Is.EqualTo(0));
        }

        [Test]
        public void HandleBack_WithoutModal_GoesBackToPreviousScreen()
        {
            UIModalStack modalStack = new();
            UIScreenRouter<TestScreenId> screenRouter =
                CreateRouterWithHistory();

            int exitRequestCount = 0;

            UIBackNavigationController<TestScreenId> controller =
                new(
                    modalStack,
                    screenRouter,
                    () => exitRequestCount++);

            controller.HandleBack();

            Assert.That(
                screenRouter.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));

            Assert.That(screenRouter.CanGoBack, Is.False);
            Assert.That(exitRequestCount, Is.EqualTo(0));
        }

        [Test]
        public void HandleBack_WithoutModalOrHistory_RequestsExitConfirmation()
        {
            UIModalStack modalStack = new();
            UIScreenRouter<TestScreenId> screenRouter = new();

            TestUIView mainMenu = new();

            screenRouter.Register(
                TestScreenId.MainMenu,
                mainMenu);

            screenRouter.Open(TestScreenId.MainMenu);

            int exitRequestCount = 0;

            UIBackNavigationController<TestScreenId> controller =
                new(
                    modalStack,
                    screenRouter,
                    () => exitRequestCount++);

            controller.HandleBack();

            Assert.That(exitRequestCount, Is.EqualTo(1));
            Assert.That(mainMenu.IsVisible, Is.True);

            Assert.That(
                screenRouter.CurrentScreen,
                Is.EqualTo(TestScreenId.MainMenu));
        }

        [Test]
        public void Constructor_NullModalStack_ThrowsException()
        {
            UIScreenRouter<TestScreenId> screenRouter = new();

            Assert.Throws<ArgumentNullException>(
                () => new UIBackNavigationController<TestScreenId>(
                    null,
                    screenRouter,
                    () => { }));
        }

        [Test]
        public void Constructor_NullScreenRouter_ThrowsException()
        {
            UIModalStack modalStack = new();

            Assert.Throws<ArgumentNullException>(
                () => new UIBackNavigationController<TestScreenId>(
                    modalStack,
                    null,
                    () => { }));
        }

        [Test]
        public void Constructor_NullExitCallback_ThrowsException()
        {
            UIModalStack modalStack = new();
            UIScreenRouter<TestScreenId> screenRouter = new();

            Assert.Throws<ArgumentNullException>(
                () => new UIBackNavigationController<TestScreenId>(
                    modalStack,
                    screenRouter,
                    null));
        }

        private static UIScreenRouter<TestScreenId>
            CreateRouterWithHistory()
        {
            UIScreenRouter<TestScreenId> screenRouter = new();

            screenRouter.Register(
                TestScreenId.MainMenu,
                new TestUIView());

            screenRouter.Register(
                TestScreenId.Inventory,
                new TestUIView());

            screenRouter.Open(TestScreenId.MainMenu);
            screenRouter.Open(TestScreenId.Inventory);

            return screenRouter;
        }

        private enum TestScreenId
        {
            MainMenu,
            Inventory
        }

        private sealed class TestUIView : IUIView
        {
            public bool IsVisible { get; private set; }

            public void Show()
            {
                IsVisible = true;
            }

            public void Hide()
            {
                IsVisible = false;
            }
        }

        private sealed class TestModalView : IUIModalView
        {
            public bool IsVisible { get; private set; }
            public bool IsInputEnabled { get; private set; }

            public void Show()
            {
                IsVisible = true;
            }

            public void Hide()
            {
                IsVisible = false;
            }

            public void SetInputEnabled(bool isEnabled)
            {
                IsInputEnabled = isEnabled;
            }
        }
    }
}