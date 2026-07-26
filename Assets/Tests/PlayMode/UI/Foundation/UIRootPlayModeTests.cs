using System.Collections;
using DiceBossArena.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DiceBossArena.Tests.PlayMode
{
    public sealed class UIRootPlayModeTests
    {
        private const string PrefabPath =
            "Assets/_Project/Prefabs/UI/Foundation/" +
            "UIRoot.prefab";

        [UnityTest]
        public IEnumerator Prefab_HasExpectedLayerConfiguration()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Assert.That(
                prefab,
                Is.Not.Null,
                $"Prefab was not found at: {PrefabPath}");

            GameObject rootObject =
                Object.Instantiate(prefab);

            UIRootView view =
                rootObject.GetComponent<UIRootView>();

            Assert.That(
                view,
                Is.Not.Null,
                "UIRootView is missing.");

            AssertLayer(
                view.StaticNavigationLayer,
                "StaticNavigationLayer",
                0,
                true);

            AssertLayer(
                view.DynamicHudLayer,
                "DynamicHudLayer",
                100,
                false);

            AssertLayer(
                view.ScreenLayer,
                "ScreenLayer",
                200,
                true);

            AssertLayer(
                view.ModalLayer,
                "ModalLayer",
                300,
                true);

            AssertLayer(
                view.TooltipLayer,
                "TooltipLayer",
                400,
                false);

            AssertLayer(
                view.TransitionLayer,
                "TransitionLayer",
                500,
                false);

            GraphicRaycaster[] raycasters =
                rootObject.GetComponentsInChildren<
                    GraphicRaycaster>(true);

            Assert.That(
                raycasters.Length,
                Is.EqualTo(3),
                "UIRoot should contain exactly three " +
                "GraphicRaycasters.");

            CanvasScaler scaler =
                rootObject.GetComponent<CanvasScaler>();

            Assert.That(scaler, Is.Not.Null);

            Assert.That(
                scaler.uiScaleMode,
                Is.EqualTo(
                    CanvasScaler.ScaleMode.ScaleWithScreenSize));

            Assert.That(
                scaler.referenceResolution,
                Is.EqualTo(new Vector2(1080f, 1920f)));

            Assert.That(
                scaler.screenMatchMode,
                Is.EqualTo(
                    CanvasScaler.ScreenMatchMode
                        .MatchWidthOrHeight));

            Assert.That(
                scaler.matchWidthOrHeight,
                Is.EqualTo(0.5f).Within(0.001f));

            Object.Destroy(rootObject);

            yield return null;

            LogAssert.NoUnexpectedReceived();
        }

        private static void AssertLayer(
            Canvas layer,
            string expectedName,
            int expectedSortOrder,
            bool expectsRaycaster)
        {
            Assert.That(
                layer,
                Is.Not.Null,
                $"{expectedName} reference is missing.");

            Assert.That(
                layer.name,
                Is.EqualTo(expectedName));

            Assert.That(
                layer.overrideSorting,
                Is.True,
                $"{expectedName} must override sorting.");

            Assert.That(
                layer.sortingOrder,
                Is.EqualTo(expectedSortOrder));

            GraphicRaycaster raycaster =
                layer.GetComponent<GraphicRaycaster>();

            Assert.That(
                raycaster != null,
                Is.EqualTo(expectsRaycaster),
                $"{expectedName} has incorrect " +
                "GraphicRaycaster configuration.");
        }
    }
}