using System;
using DiceBossArena.UI;
using NUnit.Framework;
using UnityEngine;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class BindableUIViewTests
    {
        private GameObject viewObject;
        private TestBindableUIView view;
        private TestUIViewModel model;

        [SetUp]
        public void SetUp()
        {
            viewObject =
                new GameObject("Test Bindable UI View");

            view =
                viewObject.AddComponent<TestBindableUIView>();

            model =
                new TestUIViewModel();
        }

        [TearDown]
        public void TearDown()
        {
            if (viewObject != null)
            {
                UnityEngine.Object.DestroyImmediate(
                    viewObject);
            }
        }

        [Test]
        public void NewView_IsUnboundAndHidden()
        {
            Assert.That(view.IsBound, Is.False);
            Assert.That(view.IsVisible, Is.False);
        }

        [Test]
        public void Bind_PerformsInitialRenderExactlyOnce()
        {
            view.Bind(model);

            Assert.That(view.IsBound, Is.True);
            Assert.That(view.IsVisible, Is.False);
            Assert.That(model.BindCount, Is.EqualTo(1));
            Assert.That(view.BoundModel, Is.SameAs(model));
        }

        [Test]
        public void BindTwice_ThrowsInvalidOperationException()
        {
            view.Bind(model);

            Assert.Throws<InvalidOperationException>(
                () => view.Bind(
                    new TestUIViewModel()));

            Assert.That(model.BindCount, Is.EqualTo(1));
            Assert.That(view.BoundModel, Is.SameAs(model));
        }

        [Test]
        public void ShowBeforeBind_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(
                () => view.Show());

            Assert.That(view.IsVisible, Is.False);
            Assert.That(model.ShowCount, Is.Zero);
        }

        [Test]
        public void Show_AfterBind_CallsOnShowExactlyOnce()
        {
            view.Bind(model);

            view.Show();

            Assert.That(view.IsVisible, Is.True);
            Assert.That(model.ShowCount, Is.EqualTo(1));
        }

        [Test]
        public void ShowTwice_ThrowsInvalidOperationException()
        {
            view.Bind(model);
            view.Show();

            Assert.Throws<InvalidOperationException>(
                () => view.Show());

            Assert.That(model.ShowCount, Is.EqualTo(1));
        }

        [Test]
        public void Hide_AfterShow_CallsOnHideExactlyOnce()
        {
            view.Bind(model);
            view.Show();

            view.Hide();

            Assert.That(view.IsVisible, Is.False);
            Assert.That(model.HideCount, Is.EqualTo(1));
        }

        [Test]
        public void HideBeforeShow_ThrowsInvalidOperationException()
        {
            view.Bind(model);

            Assert.Throws<InvalidOperationException>(
                () => view.Hide());

            Assert.That(model.HideCount, Is.Zero);
        }

        [Test]
        public void UnbindWhileVisible_ThrowsInvalidOperationException()
        {
            view.Bind(model);
            view.Show();

            Assert.Throws<InvalidOperationException>(
                () => view.Unbind());

            Assert.That(view.IsBound, Is.True);
            Assert.That(view.IsVisible, Is.True);
            Assert.That(model.UnbindCount, Is.Zero);
        }

        [Test]
        public void Unbind_AfterHide_ClearsModelExactlyOnce()
        {
            view.Bind(model);
            view.Show();
            view.Hide();

            view.Unbind();

            Assert.That(view.IsBound, Is.False);
            Assert.That(view.IsVisible, Is.False);
            Assert.That(model.UnbindCount, Is.EqualTo(1));
            Assert.That(view.BoundModel, Is.Null);
        }

        [Test]
        public void UnbindBeforeBind_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(
                () => view.Unbind());

            Assert.That(model.UnbindCount, Is.Zero);
        }

        [Test]
        public void View_CanBeBoundAgainAfterUnbind()
        {
            TestUIViewModel secondModel =
                new TestUIViewModel();

            view.Bind(model);
            view.Unbind();

            view.Bind(secondModel);

            Assert.That(view.IsBound, Is.True);
            Assert.That(view.IsVisible, Is.False);
            Assert.That(model.UnbindCount, Is.EqualTo(1));
            Assert.That(secondModel.BindCount, Is.EqualTo(1));
            Assert.That(view.BoundModel, Is.SameAs(secondModel));
        }
    }

    public sealed class TestBindableUIView :
        BindableUIView<TestUIViewModel>
    {
        public TestUIViewModel BoundModel =>
            IsBound
                ? Model
                : null;

        protected override void OnBind(
            TestUIViewModel boundModel)
        {
            boundModel.BindCount++;
        }

        protected override void OnShow()
        {
            Model.ShowCount++;
        }

        protected override void OnHide()
        {
            Model.HideCount++;
        }

        protected override void OnUnbind()
        {
            Model.UnbindCount++;
        }
    }

    public sealed class TestUIViewModel
    {
        public int BindCount { get; set; }
        public int ShowCount { get; set; }
        public int HideCount { get; set; }
        public int UnbindCount { get; set; }
    }
}