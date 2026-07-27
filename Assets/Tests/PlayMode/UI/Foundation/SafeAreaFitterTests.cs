using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace DiceBossArena.UI.Tests
{
    public sealed class SafeAreaFitterTests
    {
        private GameObject testObject;
        private RectTransform rectTransform;

        [SetUp]
        public void SetUp()
        {
            testObject =
                new GameObject(
                    "SafeAreaContent",
                    typeof(RectTransform));

            rectTransform =
                testObject.GetComponent<RectTransform>();
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(testObject);
        }

        [UnityTest]
        public IEnumerator OnEnable_AppliesCurrentSafeArea()
        {
            testObject.AddComponent<SafeAreaFitter>();

            yield return null;

            Rect safeArea =
                Screen.safeArea;

            Vector2 expectedAnchorMin =
                new(
                    safeArea.xMin / Screen.width,
                    safeArea.yMin / Screen.height);

            Vector2 expectedAnchorMax =
                new(
                    safeArea.xMax / Screen.width,
                    safeArea.yMax / Screen.height);

            Assert.That(
                rectTransform.anchorMin.x,
                Is.EqualTo(expectedAnchorMin.x)
                    .Within(0.0001f));

            Assert.That(
                rectTransform.anchorMin.y,
                Is.EqualTo(expectedAnchorMin.y)
                    .Within(0.0001f));

            Assert.That(
                rectTransform.anchorMax.x,
                Is.EqualTo(expectedAnchorMax.x)
                    .Within(0.0001f));

            Assert.That(
                rectTransform.anchorMax.y,
                Is.EqualTo(expectedAnchorMax.y)
                    .Within(0.0001f));
        }

        [UnityTest]
        public IEnumerator OnEnable_ClearsOffsets()
        {
            rectTransform.offsetMin =
                new Vector2(20f, 30f);

            rectTransform.offsetMax =
                new Vector2(-40f, -50f);

            testObject.AddComponent<SafeAreaFitter>();

            yield return null;

            Assert.That(
                rectTransform.offsetMin,
                Is.EqualTo(Vector2.zero));

            Assert.That(
                rectTransform.offsetMax,
                Is.EqualTo(Vector2.zero));
        }
    }
}