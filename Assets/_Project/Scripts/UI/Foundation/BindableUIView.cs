using System;
using UnityEngine;

namespace DiceBossArena.UI
{
    public abstract class BindableUIView<TModel> :
    MonoBehaviour,
    IBindableUIView<TModel>
    {
        private TModel model;
        private bool isBound;
        private bool isVisible;

        public bool IsBound => isBound;
        public bool IsVisible => isVisible;

        protected TModel Model
        {
            get
            {
                if (!isBound)
                {
                    throw new InvalidOperationException(
                        $"{GetType().Name} has no bound model.");
                }

                return model;
            }
        }

        public void Bind(TModel newModel)
        {
            if (isBound)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} is already bound.");
            }

            model = newModel;
            isBound = true;

            try
            {
                OnBind(newModel);
            }
            catch
            {
                model = default;
                isBound = false;
                throw;
            }
        }

        public void Show()
        {
            if (!isBound)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} cannot be shown before Bind.");
            }

            if (isVisible)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} is already visible.");
            }

            OnShow();
            isVisible = true;
        }

        public void Hide()
        {
            if (!isVisible)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} is already hidden.");
            }

            OnHide();
            isVisible = false;
        }

        public void Unbind()
        {
            if (!isBound)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} is not bound.");
            }

            if (isVisible)
            {
                throw new InvalidOperationException(
                    $"{GetType().Name} must be hidden before Unbind.");
            }

            OnUnbind();

            model = default;
            isBound = false;
        }

        protected abstract void OnBind(TModel boundModel);

        protected virtual void OnShow()
        {
        }

        protected virtual void OnHide()
        {
        }

        protected abstract void OnUnbind();
    }
}