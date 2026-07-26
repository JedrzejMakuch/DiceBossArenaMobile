using DiceBossArena.UI;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using System.Reflection;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ReusableTextListTests
    {
        private GameObject rootObject;
        private GameObject containerObject;
        private GameObject prefabObject;

        private ReusableTextUIElementFactory factory;
        private ReusableTextList list;

        [SetUp]
        public void SetUp()
        {
            rootObject =
                new GameObject(
                    "Reusable Text List");

            containerObject =
                new GameObject(
                    "Container",
                    typeof(RectTransform));

            containerObject.transform.SetParent(
                rootObject.transform);

            prefabObject =
                new GameObject(
                    "Reusable Text UI Element",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(TextMeshProUGUI),
                    typeof(ReusableTextUIElement));

            ReusableTextUIElement prefab =
                prefabObject.GetComponent<
                    ReusableTextUIElement>();

            AssignReference(
                prefab,
                "text",
                prefabObject.GetComponent<TextMeshProUGUI>());

            factory =
                rootObject.AddComponent<
                    ReusableTextUIElementFactory>();

            factory =
                rootObject.AddComponent<
                    ReusableTextUIElementFactory>();

            AssignReference(
                factory,
                "prefab",
                prefab);

            AssignReference(
                factory,
                "container",
                containerObject.transform);

            list =
                rootObject.AddComponent<
                    ReusableTextList>();

            AssignReference(
                list,
                "factory",
                factory);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(rootObject);
            Object.DestroyImmediate(prefabObject);
        }

        [Test]
        public void SetItems_CreatesElementsAndAssignsTexts()
        {
            InvokeAwake();

            list.SetItems(
                new[]
                {
                    "First",
                    "Second",
                    "Third"
                });

            Assert.That(
                containerObject.transform.childCount,
                Is.EqualTo(3));

            AssertText(0, "First");
            AssertText(1, "Second");
            AssertText(2, "Third");
        }

        [Test]
        public void SetItems_SecondRefresh_ReusesExistingElements()
        {
            InvokeAwake();

            list.SetItems(
                new[]
                {
                    "First",
                    "Second"
                });

            Transform first =
                containerObject.transform.GetChild(0);

            Transform second =
                containerObject.transform.GetChild(1);

            list.SetItems(
                new[]
                {
                    "Updated First",
                    "Updated Second"
                });

            Assert.That(
                containerObject.transform.childCount,
                Is.EqualTo(2));

            Assert.That(
                containerObject.transform.GetChild(0),
                Is.SameAs(first));

            Assert.That(
                containerObject.transform.GetChild(1),
                Is.SameAs(second));

            AssertText(0, "Updated First");
            AssertText(1, "Updated Second");
        }

        [Test]
        public void Clear_DeactivatesAllCreatedElements()
        {
            InvokeAwake();

            list.SetItems(
                new[]
                {
                    "First",
                    "Second",
                    "Third"
                });

            list.Clear();

            Assert.That(
                containerObject.transform.childCount,
                Is.EqualTo(3));

            for (int index = 0;
                 index < containerObject.transform.childCount;
                 index++)
            {
                Assert.That(
                    containerObject.transform
                        .GetChild(index)
                        .gameObject
                        .activeSelf,
                    Is.False);
            }
        }

        [Test]
        public void Awake_MissingFactory_LogsError()
        {
            AssignReference(
                list,
                "factory",
                null);

            LogAssert.Expect(
                LogType.Error,
                "ReusableTextList requires a " +
                "ReusableTextUIElementFactory reference.");

            InvokeAwake();
        }

        [Test]
        public void SetItems_BeforeInitialization_LogsError()
        {
            LogAssert.Expect(
                LogType.Error,
                "ReusableTextList is not initialized.");

            list.SetItems(
                new[] { "First" });
        }

        private void InvokeAwake()
        {
            MethodInfo awakeMethod =
                typeof(ReusableTextList).GetMethod(
                    "Awake",
                    BindingFlags.Instance |
                    BindingFlags.NonPublic);

            Assert.That(
                awakeMethod,
                Is.Not.Null);

            awakeMethod.Invoke(
                list,
                null);
        }

        private void AssertText(
            int index,
            string expected)
        {
            TMP_Text text =
                containerObject.transform
                    .GetChild(index)
                    .GetComponent<TMP_Text>();

            Assert.That(
                text.text,
                Is.EqualTo(expected));
        }

        private static void AssignReference(
            Object target,
            string propertyName,
            Object value)
        {
            SerializedObject serializedTarget =
                new(target);

            SerializedProperty property =
                serializedTarget.FindProperty(
                    propertyName);

            property.objectReferenceValue = value;
            serializedTarget.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}