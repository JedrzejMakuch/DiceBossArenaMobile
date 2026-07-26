using System;
using System.Collections.Generic;

namespace DiceBossArena.UI
{
    public sealed class UIModalStack
    {
        private readonly Stack<IUIModalView> modals =
            new();

        public int Count =>
            modals.Count;

        public bool HasModal =>
            modals.Count > 0;

        public IUIModalView Top
        {
            get
            {
                if (!HasModal)
                {
                    throw new InvalidOperationException(
                        "Modal stack is empty.");
                }

                return modals.Peek();
            }
        }

        public void Push(IUIModalView modal)
        {
            if (modal == null)
            {
                throw new ArgumentNullException(nameof(modal));
            }

            if (modals.Contains(modal))
            {
                throw new InvalidOperationException(
                    "Modal is already present in the stack.");
            }

            if (HasModal)
            {
                Top.SetInputEnabled(false);
            }

            modals.Push(modal);

            modal.Show();
            modal.SetInputEnabled(true);
        }

        public bool TryPop()
        {
            if (!HasModal)
            {
                return false;
            }

            IUIModalView modal = modals.Pop();

            modal.SetInputEnabled(false);

            if (modal.IsVisible)
            {
                modal.Hide();
            }

            if (HasModal)
            {
                Top.SetInputEnabled(true);
            }

            return true;
        }

        public void Clear()
        {
            while (modals.Count > 0)
            {
                IUIModalView modal = modals.Pop();

                modal.SetInputEnabled(false);

                if (modal.IsVisible)
                {
                    modal.Hide();
                }
            }
        }
    }
}