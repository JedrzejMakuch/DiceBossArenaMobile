using System;
using System.Collections.Generic;

namespace DiceBossArena.UI
{
    public sealed class UIScreenRouter<TScreenId>
        where TScreenId : Enum
    {
        private readonly Dictionary<TScreenId, IUIView> screens =
            new();

        private readonly UIScreenHistory<TScreenId> history =
            new();

        public bool HasCurrentScreen =>
            history.HasCurrentScreen;

        public TScreenId CurrentScreen =>
            history.CurrentScreen;

        public bool CanGoBack =>
            history.CanGoBack;

        public void Register(
            TScreenId screenId,
            IUIView screen)
        {
            if (screen == null)
            {
                throw new ArgumentNullException(nameof(screen));
            }

            if (!screens.TryAdd(screenId, screen))
            {
                throw new InvalidOperationException(
                    $"Screen {screenId} is already registered.");
            }
        }

        public void Open(TScreenId screenId)
        {
            if (history.HasCurrentScreen &&
                EqualityComparer<TScreenId>.Default.Equals(
                    history.CurrentScreen,
                    screenId))
            {
                return;
            }

            IUIView nextScreen =
                GetRegisteredScreen(screenId);

            HideCurrentScreen();

            nextScreen.Show();
            history.Open(screenId);
        }

        public void Replace(TScreenId screenId)
        {
            if (history.HasCurrentScreen &&
                EqualityComparer<TScreenId>.Default.Equals(
                    history.CurrentScreen,
                    screenId))
            {
                return;
            }

            IUIView nextScreen =
                GetRegisteredScreen(screenId);

            HideCurrentScreen();

            nextScreen.Show();
            history.Replace(screenId);
        }

        public bool TryGoBack()
        {
            if (!history.TryGoBack(
                    out TScreenId previousScreenId))
            {
                return false;
            }

            HideVisibleScreens();

            IUIView previousScreen =
                GetRegisteredScreen(previousScreenId);

            previousScreen.Show();
            return true;
        }

        public void Clear()
        {
            HideVisibleScreens();
            history.Clear();
        }

        private void HideCurrentScreen()
        {
            if (!history.HasCurrentScreen)
            {
                return;
            }

            IUIView currentScreen =
                GetRegisteredScreen(history.CurrentScreen);

            if (currentScreen.IsVisible)
            {
                currentScreen.Hide();
            }
        }

        private void HideVisibleScreens()
        {
            foreach (IUIView screen in screens.Values)
            {
                if (screen.IsVisible)
                {
                    screen.Hide();
                }
            }
        }

        private IUIView GetRegisteredScreen(
            TScreenId screenId)
        {
            if (screens.TryGetValue(
                    screenId,
                    out IUIView screen))
            {
                return screen;
            }

            throw new InvalidOperationException(
                $"Screen {screenId} is not registered.");
        }
    }
}