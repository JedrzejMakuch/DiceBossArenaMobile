using System.Collections;
using DiceBossArena.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DiceBossArena.Tests.PlayMode
{
    public sealed class UIScreenViewPlayModeTests
    {
        [UnityTest]
        public IEnumerator Screen_PerformsCompleteLifecycle()
        {
            GameObject screenObject =
                new GameObject("Test UI Screen");

            TestUIScreenView screen =
                screenObject.AddComponent<TestUIScreenView>();

            TestUIScreenModel model =
                new TestUIScreenModel();

            screenObject.SetActive(false);

            screen.Bind(model);

            Assert.That(screen.IsBound, Is.True);
            Assert.That(screen.IsVisible, Is.False);
            Assert.That(screenObject.activeSelf, Is.False);
            Assert.That(model.BindCount, Is.EqualTo(1));

            screen.Show();

            Assert.That(screen.IsVisible, Is.True);
            Assert.That(screenObject.activeSelf, Is.True);
            Assert.That(model.ShowCount, Is.EqualTo(1));

            yield return null;

            Assert.That(
                screen.UpdateCount,
                Is.GreaterThan(0),
                "Visible screen should perform Update.");

            screen.Hide();

            Assert.That(screen.IsVisible, Is.False);
            Assert.That(screenObject.activeSelf, Is.False);
            Assert.That(model.HideCount, Is.EqualTo(1));

            int updateCountAfterHide =
                screen.UpdateCount;

            yield return null;
            yield return null;

            Assert.That(
                screen.UpdateCount,
                Is.EqualTo(updateCountAfterHide),
                "Hidden screen must not perform Update.");

            screen.Unbind();

            Assert.That(screen.IsBound, Is.False);
            Assert.That(model.UnbindCount, Is.EqualTo(1));

            Object.Destroy(screenObject);

            yield return null;

            LogAssert.NoUnexpectedReceived();
        }
    }

    public sealed class TestUIScreenView :
        UIScreenView<TestUIScreenModel>
    {
        public int UpdateCount { get; private set; }

        private void Update()
        {
            UpdateCount++;
        }

        protected override void OnBind(
            TestUIScreenModel boundModel)
        {
            boundModel.BindCount++;
        }

        protected override void OnScreenShown()
        {
            Model.ShowCount++;
        }

        protected override void OnScreenHidden()
        {
            Model.HideCount++;
        }

        protected override void OnUnbind()
        {
            Model.UnbindCount++;
        }
    }

    public sealed class TestUIScreenModel
    {
        public int BindCount { get; set; }
        public int ShowCount { get; set; }
        public int HideCount { get; set; }
        public int UnbindCount { get; set; }
    }
}