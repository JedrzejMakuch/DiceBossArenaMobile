using DiceBossArena.UI;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ReusableTextUIElementTests
    {
        private GameObject gameObject;
        private TextMeshProUGUI text;
        private ReusableTextUIElement element;

        [SetUp]
        public void SetUp()
        {
            gameObject =
                new GameObject(
                    "Reusable Text UI Element",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(TextMeshProUGUI),
                    typeof(ReusableTextUIElement));

            text =
                gameObject.GetComponent<TextMeshProUGUI>();

            element =
                gameObject.GetComponent<ReusableTextUIElement>();

            AssignTextReference(text);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(gameObject);
        }

        [Test]
        public void SetText_Value_AssignsText()
        {
            element.SetText("Test value");

            Assert.That(
                text.text,
                Is.EqualTo("Test value"));
        }

        [Test]
        public void SetText_Null_AssignsEmptyText()
        {
            text.text = "Previous value";

            element.SetText(null);

            Assert.That(
                text.text,
                Is.Empty);
        }

        [Test]
        public void PrepareForUse_InactiveObject_ActivatesObject()
        {
            gameObject.SetActive(false);

            element.PrepareForUse();

            Assert.That(
                gameObject.activeSelf,
                Is.True);
        }

        [Test]
        public void ResetForPool_ClearsTextAndDeactivatesObject()
        {
            text.text = "Previous value";

            element.ResetForPool();

            Assert.That(
                text.text,
                Is.Empty);

            Assert.That(
                gameObject.activeSelf,
                Is.False);
        }

        [Test]
        public void SetText_MissingReference_LogsError()
        {
            AssignTextReference(null);

            LogAssert.Expect(
                LogType.Error,
                "ReusableTextUIElement requires a TMP_Text reference.");

            element.SetText("Test value");
        }

        private void AssignTextReference(
            TMP_Text value)
        {
            SerializedObject serializedElement =
                new(element);

            SerializedProperty textProperty =
                serializedElement.FindProperty("text");

            textProperty.objectReferenceValue = value;
            serializedElement.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}