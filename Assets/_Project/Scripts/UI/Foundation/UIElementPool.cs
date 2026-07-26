using System;
using System.Collections.Generic;

namespace DiceBossArena.UI
{
    public sealed class UIElementPool<TElement>
        where TElement : class, IReusableUIElement
    {
        private readonly Func<TElement> createElement;
        private readonly Stack<TElement> availableElements =
            new();
        private readonly HashSet<TElement> activeElements =
            new();
        private readonly List<TElement> releaseBuffer =
            new();

        public UIElementPool(
            Func<TElement> createElement)
        {
            this.createElement =
                createElement ??
                throw new ArgumentNullException(
                    nameof(createElement));
        }

        public int CreatedCount { get; private set; }

        public int ActiveCount =>
            activeElements.Count;

        public int AvailableCount =>
            availableElements.Count;

        public TElement Get()
        {
            TElement element =
                availableElements.Count > 0
                    ? availableElements.Pop()
                    : CreateElement();

            if (!activeElements.Add(element))
            {
                throw new InvalidOperationException(
                    "UI element is already active.");
            }

            element.PrepareForUse();

            return element;
        }

        public void Release(
            TElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(
                    nameof(element));
            }

            if (!activeElements.Remove(element))
            {
                throw new InvalidOperationException(
                    "UI element does not belong to the active pool.");
            }

            element.ResetForPool();
            availableElements.Push(element);
        }

        public void ReleaseAll()
        {
            releaseBuffer.Clear();
            releaseBuffer.AddRange(activeElements);

            foreach (TElement element in releaseBuffer)
            {
                Release(element);
            }

            releaseBuffer.Clear();
        }

        private TElement CreateElement()
        {
            TElement element =
                createElement.Invoke();

            if (element == null)
            {
                throw new InvalidOperationException(
                    "UI element factory returned null.");
            }

            CreatedCount++;

            return element;
        }
    }
}