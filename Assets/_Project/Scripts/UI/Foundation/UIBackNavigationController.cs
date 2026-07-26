using System;

namespace DiceBossArena.UI
{
    public sealed class UIBackNavigationController<TScreenId>
        where TScreenId : Enum
    {
        private readonly UIModalStack modalStack;
        private readonly UIScreenRouter<TScreenId> screenRouter;
        private readonly Action requestExitConfirmation;

        public UIBackNavigationController(
            UIModalStack modalStack,
            UIScreenRouter<TScreenId> screenRouter,
            Action requestExitConfirmation)
        {
            this.modalStack = modalStack ??
                throw new ArgumentNullException(
                    nameof(modalStack));

            this.screenRouter = screenRouter ??
                throw new ArgumentNullException(
                    nameof(screenRouter));

            this.requestExitConfirmation =
                requestExitConfirmation ??
                throw new ArgumentNullException(
                    nameof(requestExitConfirmation));
        }

        public void HandleBack()
        {
            if (modalStack.TryPop())
            {
                return;
            }

            if (screenRouter.TryGoBack())
            {
                return;
            }

            requestExitConfirmation.Invoke();
        }
    }
}