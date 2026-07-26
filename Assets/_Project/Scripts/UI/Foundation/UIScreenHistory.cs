using System;
using System.Collections.Generic;

namespace DiceBossArena.UI
{
    public sealed class UIScreenHistory<TScreenId>
        where TScreenId : Enum
    {
        private readonly Stack<TScreenId> previousScreens =
            new();

        private TScreenId currentScreen;

        public bool HasCurrentScreen { get; private set; }

        public TScreenId CurrentScreen
        {
            get
            {
                if (!HasCurrentScreen)
                {
                    throw new InvalidOperationException(
                        "Screen history has no current screen.");
                }

                return currentScreen;
            }
        }

        public bool CanGoBack =>
            previousScreens.Count > 0;

        public void Open(TScreenId screenId)
        {
            if (HasCurrentScreen)
            {
                if (EqualityComparer<TScreenId>.Default.Equals(
                        currentScreen,
                        screenId))
                {
                    return;
                }

                previousScreens.Push(currentScreen);
            }

            currentScreen = screenId;
            HasCurrentScreen = true;
        }

        public void Replace(TScreenId screenId)
        {
            currentScreen = screenId;
            HasCurrentScreen = true;
        }

        public bool TryGoBack(out TScreenId screenId)
        {
            if (!CanGoBack)
            {
                screenId = default;
                return false;
            }

            currentScreen = previousScreens.Pop();
            screenId = currentScreen;
            return true;
        }

        public void Clear()
        {
            previousScreens.Clear();
            currentScreen = default;
            HasCurrentScreen = false;
        }
    }
}