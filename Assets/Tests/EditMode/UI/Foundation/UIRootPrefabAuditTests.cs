using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DiceBossArena.UI.Tests
{
    public sealed class UIRootPrefabAuditTests
    {
        private const string PrefabPath =
            "Assets/_Project/Prefabs/UI/Foundation/UIRoot.prefab";

        [Test]
        public void UIRoot_ReportsFoundationComponentCounts()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Assert.That(
                prefab,
                Is.Not.Null,
                $"Could not load prefab at {PrefabPath}.");

            Canvas[] canvases =
                prefab.GetComponentsInChildren<Canvas>(true);

            GraphicRaycaster[] raycasters =
                prefab.GetComponentsInChildren<GraphicRaycaster>(true);

            CanvasScaler[] canvasScalers =
                prefab.GetComponentsInChildren<CanvasScaler>(true);

            SafeAreaFitter[] safeAreaFitters =
                prefab.GetComponentsInChildren<SafeAreaFitter>(true);

            TestContext.WriteLine(
                $"Canvas count: {canvases.Length}");

            TestContext.WriteLine(
                $"GraphicRaycaster count: {raycasters.Length}");

            TestContext.WriteLine(
                $"CanvasScaler count: {canvasScalers.Length}");

            TestContext.WriteLine(
                $"SafeAreaFitter count: {safeAreaFitters.Length}");

            Assert.That(
                canvases.Length,
                Is.EqualTo(7),
                "UIRoot Canvas budget changed.");

            Assert.That(
                raycasters.Length,
                Is.EqualTo(3),
                "UIRoot GraphicRaycaster budget changed.");

            Assert.That(
                canvasScalers.Length,
                Is.EqualTo(1),
                "UIRoot must have exactly one CanvasScaler.");

            Assert.That(
                safeAreaFitters.Length,
                Is.EqualTo(6),
                "Each UI layer must have one SafeAreaFitter.");
        }

        [Test]
        public void UIRoot_HasRaycastersOnlyOnInteractiveLayers()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Assert.That(
                prefab,
                Is.Not.Null,
                $"Could not load prefab at {PrefabPath}.");

            GraphicRaycaster[] raycasters =
                prefab.GetComponentsInChildren<GraphicRaycaster>(true);

            string[] expectedLayerNames =
            {
                "StaticNavigationLayer",
                "ScreenLayer",
                "ModalLayer"
            };

            Assert.That(
                raycasters.Length,
                Is.EqualTo(expectedLayerNames.Length));

            foreach (GraphicRaycaster raycaster in raycasters)
            {
                Assert.That(
                    expectedLayerNames,
                    Does.Contain(raycaster.gameObject.name),
                    $"Unexpected GraphicRaycaster on " +
                    $"{raycaster.gameObject.name}.");
            }

            foreach (string layerName in expectedLayerNames)
            {
                Transform layer =
                    prefab.transform.Find(layerName);

                Assert.That(
                    layer,
                    Is.Not.Null,
                    $"Missing UI layer: {layerName}.");

                Assert.That(
                    layer.GetComponent<GraphicRaycaster>(),
                    Is.Not.Null,
                    $"{layerName} must have a GraphicRaycaster.");
            }
        }

        [Test]
        public void UIRoot_HasExpectedLayerSortingOrder()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Assert.That(
                prefab,
                Is.Not.Null,
                $"Could not load prefab at {PrefabPath}.");

            (string Name, int SortingOrder)[] expectedLayers =
            {
            ("StaticNavigationLayer", 0),
            ("DynamicHudLayer", 100),
            ("ScreenLayer", 200),
            ("ModalLayer", 300),
            ("TooltipLayer", 400),
            ("TransitionLayer", 500)
        };

            foreach ((string name, int sortingOrder)
                     in expectedLayers)
            {
                Transform layer =
                    prefab.transform.Find(name);

                Assert.That(
                    layer,
                    Is.Not.Null,
                    $"Missing UI layer: {name}.");

                Canvas canvas =
                    layer.GetComponent<Canvas>();

                Assert.That(
                    canvas,
                    Is.Not.Null,
                    $"{name} must have a Canvas.");

                Assert.That(
                    canvas.overrideSorting,
                    Is.True,
                    $"{name} must override sorting.");

                Assert.That(
                    canvas.sortingOrder,
                    Is.EqualTo(sortingOrder),
                    $"{name} has an unexpected sorting order.");
            }
        }
    }
}