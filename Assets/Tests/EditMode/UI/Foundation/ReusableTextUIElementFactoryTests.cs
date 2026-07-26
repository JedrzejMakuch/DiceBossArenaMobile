using DiceBossArena.UI;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ReusableTextUIElementFactoryTests
    {
        private GameObject factoryObject;
        private GameObject containerObject;
        private GameObject prefabObject;

        private ReusableTextUIElementFactory factory;
        private ReusableTextUIElement prefab;

        [SetUp]
        public void SetUp()
        {
            factoryObject =
                new GameObject(
                    "Reusable Text UI Element Factory");

            containerObject =
                new GameObject(
                    "Container",
                    typeof(RectTransform));

            prefabObject =
                new GameObject(
                    "Reusable Text UI Element",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(TextMeshProUGUI),
                    typeof(ReusableTextUIElement));

            factory =
                factoryObject.AddComponent<
                    ReusableTextUIElementFactory>();

            prefab =
                prefabObject.GetComponent<
                    ReusableTextUIElement>();

            AssignReference(
                "prefab",
                prefab);

            AssignReference(
                "container",
                containerObject.transform);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(factoryObject);
            Object.DestroyImmediate(containerObject);
            Object.DestroyImmediate(prefabObject);
        }

        [Test]
        public void Create_WithConfiguredContainer_CreatesElementUnderContainer()
        {
            ReusableTextUIElement created =
                factory.Create();

            Assert.That(created, Is.Not.Null);
            Assert.That(created, Is.Not.SameAs(prefab));

            Assert.That(
                created.transform.parent,
                Is.SameAs(containerObject.transform));
        }

        [Test]
        public void Create_MissingContainer_UsesFactoryTransform()
        {
            AssignReference(
                "container",
                null);

            ReusableTextUIElement created =
                factory.Create();

            Assert.That(created, Is.Not.Null);

            Assert.That(
                created.transform.parent,
                Is.SameAs(factoryObject.transform));
        }

        [Test]
        public void Create_MissingPrefab_LogsErrorAndReturnsNull()
        {
            AssignReference(
                "prefab",
                null);

            LogAssert.Expect(
                LogType.Error,
                "ReusableTextUIElementFactory requires " +
                "a ReusableTextUIElement prefab.");

            ReusableTextUIElement created =
                factory.Create();

            Assert.That(created, Is.Null);
        }

        private void AssignReference(
            string propertyName,
            Object value)
        {
            SerializedObject serializedFactory =
                new(factory);

            SerializedProperty property =
                serializedFactory.FindProperty(
                    propertyName);

            property.objectReferenceValue = value;
            serializedFactory.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}