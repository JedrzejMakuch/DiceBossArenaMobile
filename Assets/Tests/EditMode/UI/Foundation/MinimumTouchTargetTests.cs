using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace DiceBossArena.UI.Tests
{
    public sealed class MinimumTouchTargetTests
    {
        private GameObject testObject;

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(testObject);
        }

        [Test]
        public void AddingComponent_AppliesDefaultMinimumSize()
        {
            testObject =
                new GameObject(
                    "TouchTarget",
                    typeof(RectTransform),
                    typeof(LayoutElement));

            testObject.AddComponent<MinimumTouchTarget>();

            LayoutElement layoutElement =
                testObject.GetComponent<LayoutElement>();

            Assert.That(
                layoutElement.minWidth,
                Is.EqualTo(96f));

            Assert.That(
                layoutElement.minHeight,
                Is.EqualTo(96f));
        }

        [Test]
        public void AddingComponent_DoesNotChangePreferredSize()
        {
            testObject =
                new GameObject(
                    "TouchTarget",
                    typeof(RectTransform),
                    typeof(LayoutElement));

            LayoutElement layoutElement =
                testObject.GetComponent<LayoutElement>();

            layoutElement.preferredWidth = 160f;
            layoutElement.preferredHeight = 120f;

            testObject.AddComponent<MinimumTouchTarget>();

            Assert.That(
                layoutElement.preferredWidth,
                Is.EqualTo(160f));

            Assert.That(
                layoutElement.preferredHeight,
                Is.EqualTo(120f));
        }
    }
}