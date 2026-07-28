using DiceBossArena.UI;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ResourceBarPrefabAuditTests
    {
        private const string PrefabPath =
            "Assets/_Project/Prefabs/UI/Foundation/" +
            "ResourceBar.prefab";

        [Test]
        public void Prefab_HasExpectedStructure()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Assert.That(
                prefab,
                Is.Not.Null,
                $"Could not load prefab at {PrefabPath}.");

            Assert.That(
                prefab.GetComponent<ResourceBarUIView>(),
                Is.Not.Null);

            Assert.That(
                prefab.transform.Find("Background"),
                Is.Not.Null);

            Assert.That(
                prefab.transform.Find("Fill"),
                Is.Not.Null);

            Assert.That(
                prefab.transform.Find("LabelText"),
                Is.Not.Null);

            Assert.That(
                prefab.transform.Find("ValueText"),
                Is.Not.Null);
        }

        [Test]
        public void Prefab_HasWiredViewReferences()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            ResourceBarUIView view =
                prefab.GetComponent<ResourceBarUIView>();

            Assert.That(view, Is.Not.Null);

            SerializedObject serializedView =
                new(view);

            Assert.That(
                serializedView
                    .FindProperty("fillImage")
                    .objectReferenceValue,
                Is.Not.Null);

            Assert.That(
                serializedView
                    .FindProperty("labelText")
                    .objectReferenceValue,
                Is.Not.Null);

            Assert.That(
                serializedView
                    .FindProperty("valueText")
                    .objectReferenceValue,
                Is.Not.Null);
        }

        [Test]
        public void Prefab_FillUsesHorizontalFilledImage()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Image fillImage =
                prefab.transform
                    .Find("Fill")
                    .GetComponent<Image>();

            Assert.That(fillImage, Is.Not.Null);
            Assert.That(fillImage.type, Is.EqualTo(Image.Type.Filled));

            Assert.That(
                fillImage.fillMethod,
                Is.EqualTo(Image.FillMethod.Horizontal));

            Assert.That(
                fillImage.fillOrigin,
                Is.EqualTo((int)Image.OriginHorizontal.Left));
        }

        [Test]
        public void Prefab_DoesNotCreateCanvasOrRaycaster()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Assert.That(
                prefab.GetComponentsInChildren<Canvas>(true),
                Is.Empty);

            Assert.That(
                prefab.GetComponentsInChildren<GraphicRaycaster>(true),
                Is.Empty);
        }

        [Test]
        public void Prefab_PresentationGraphicsDoNotBlockRaycasts()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Graphic[] graphics =
                prefab.GetComponentsInChildren<Graphic>(true);

            Assert.That(graphics, Is.Not.Empty);

            foreach (Graphic graphic in graphics)
            {
                Assert.That(
                    graphic.raycastTarget,
                    Is.False,
                    $"{graphic.gameObject.name} must not " +
                    "block raycasts.");
            }
        }

        [Test]
        public void Prefab_UsesExpectedTextComponents()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            TMP_Text labelText =
                prefab.transform
                    .Find("LabelText")
                    .GetComponent<TMP_Text>();

            TMP_Text valueText =
                prefab.transform
                    .Find("ValueText")
                    .GetComponent<TMP_Text>();

            Assert.That(labelText, Is.Not.Null);
            Assert.That(valueText, Is.Not.Null);
        }
    }
}