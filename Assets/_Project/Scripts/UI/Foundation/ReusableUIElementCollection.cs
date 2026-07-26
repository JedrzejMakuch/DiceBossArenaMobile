using System;
using System.Collections.Generic;

namespace DiceBossArena.UI
{
    public sealed class ReusableUIElementCollection<TElement>
        where TElement : class, IReusableUIElement
    {
        private readonly UIElementPool<TElement> pool;
        private readonly List<TElement> elements =
            new();

        public ReusableUIElementCollection(
            UIElementPool<TElement> pool)
        {
            this.pool =
                pool ??
                throw new ArgumentNullException(
                    nameof(pool));
        }

        public int Count =>
            elements.Count;

        public TElement this[int index] =>
            elements[index];

        public void SetCount(
            int requiredCount)
        {
            if (requiredCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(requiredCount));
            }

            while (elements.Count < requiredCount)
            {
                elements.Add(
                    pool.Get());
            }

            while (elements.Count > requiredCount)
            {
                int lastIndex =
                    elements.Count - 1;

                TElement element =
                    elements[lastIndex];

                elements.RemoveAt(lastIndex);
                pool.Release(element);
            }
        }

        public void SetItems<TItem>(
    IReadOnlyList<TItem> items,
    Action<TElement, TItem> bindElement)
        {
            if (items == null)
            {
                throw new ArgumentNullException(
                    nameof(items));
            }

            if (bindElement == null)
            {
                throw new ArgumentNullException(
                    nameof(bindElement));
            }

            SetCount(items.Count);

            for (int index = 0;
                 index < items.Count;
                 index++)
            {
                bindElement.Invoke(
                    elements[index],
                    items[index]);
            }
        }

        public void Clear()
        {
            SetCount(0);
        }
    }
}