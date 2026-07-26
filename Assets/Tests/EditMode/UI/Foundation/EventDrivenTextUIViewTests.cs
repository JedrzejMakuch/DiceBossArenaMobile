using System;
using System.Reflection;
using DiceBossArena.UI;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class EventDrivenTextUIViewTests
    {
        private GameObject viewObject;
        private TextMeshProUGUI text;
        private EventDrivenTextUIView view;
        private StringViewModelSource source;

        [SetUp]
        public void SetUp()
        {
            viewObject =
                new GameObject(
                    "Test Event Driven Text UI View",
                    typeof(RectTransform));

            text =
                viewObject.AddComponent<TextMeshProUGUI>();

            view =
                viewObject.AddComponent<EventDrivenTextUIView>();

            SetTextReference(
                view,
                text);

            source =
                new StringViewModelSource("Initial");
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
        public void Bind_RendersCurrentText()
        {
            view.Bind(source);

            Assert.That(
                text.text,
                Is.EqualTo("Initial"));

            Assert.That(
                source.SubscriberCount,
                Is.Zero);
        }

        [Test]
        public void Show_UpdatesTextWhenSourceChanges()
        {
            view.Bind(source);
            view.Show();

            source.Publish("Updated");

            Assert.That(
                text.text,
                Is.EqualTo("Updated"));

            Assert.That(
                source.SubscriberCount,
                Is.EqualTo(1));
        }

        [Test]
        public void Hide_StopsUpdatingText()
        {
            view.Bind(source);
            view.Show();

            source.Publish("Visible");

            view.Hide();

            source.Publish("Hidden");

            Assert.That(
                text.text,
                Is.EqualTo("Visible"));

            Assert.That(
                source.SubscriberCount,
                Is.Zero);
        }

        [Test]
        public void Bind_NullText_RendersEmptyString()
        {
            StringViewModelSource nullSource =
                new(null);

            view.Bind(nullSource);

            Assert.That(
                text.text,
                Is.Empty);
        }

        [Test]
        public void Bind_WithoutTextReference_LogsError()
        {
            SetTextReference(
                view,
                null);

            LogAssert.Expect(
                LogType.Error,
                "EventDrivenTextUIView requires " +
                "a TMP_Text reference.");

            view.Bind(source);
        }

        private static void SetTextReference(
            EventDrivenTextUIView targetView,
            TMP_Text targetText)
        {
            FieldInfo textField =
                typeof(EventDrivenTextUIView).GetField(
                    "text",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.That(
                textField,
                Is.Not.Null);

            textField.SetValue(
                targetView,
                targetText);
        }

        private sealed class StringViewModelSource :
            IUIViewModelSource<string>
        {
            private Action<string> changed;

            public StringViewModelSource(
                string current)
            {
                Current = current;
            }

            public string Current { get; private set; }

            public int SubscriberCount =>
                changed?.GetInvocationList().Length ?? 0;

            public event Action<string> Changed
            {
                add => changed += value;
                remove => changed -= value;
            }

            public void Publish(
                string value)
            {
                Current = value;
                changed?.Invoke(value);
            }
        }
    }
}