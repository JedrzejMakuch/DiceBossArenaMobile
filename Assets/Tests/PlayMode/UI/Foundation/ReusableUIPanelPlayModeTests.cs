using System.Collections;
using DiceBossArena.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

namespace DiceBossArena.Tests.PlayMode
{
    public sealed class ReusableUIPanelPlayModeTests
    {
        private const string PrefabPath =
            "Assets/_Project/Prefabs/UI/Foundation/" +
            "ReusableUIPanel.prefab";

        [UnityTest]
        public IEnumerator Prefab_PerformsCompleteLifecycle()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Assert.That(
                prefab,
                Is.Not.Null,
                $"Prefab was not found at: {PrefabPath}");

            GameObject canvasObject =
                new GameObject(
                    "Test Canvas",
                    typeof(Canvas),
                    typeof(GraphicRaycaster));

            GameObject eventSystemObject =
                new GameObject(
                    "Test EventSystem",
                    typeof(EventSystem));

            GameObject panelObject =
                Object.Instantiate(
                    prefab,
                    canvasObject.transform);

            ReusableUIPanelView view =
                panelObject.GetComponent<
                    ReusableUIPanelView>();

            Assert.That(
                view,
                Is.Not.Null,
                "ReusableUIPanelView is missing.");

            Transform panelRoot =
                panelObject.transform.Find("PanelRoot");

            Transform titleTransform =
                panelRoot?.Find("Header/TitleText");

            Transform valueTransform =
                panelRoot?.Find("Content/ValueText");

            Transform closeButtonTransform =
                panelRoot?.Find("CloseButton");

            Assert.That(panelRoot, Is.Not.Null);
            Assert.That(titleTransform, Is.Not.Null);
            Assert.That(valueTransform, Is.Not.Null);
            Assert.That(closeButtonTransform, Is.Not.Null);

            Assert.That(
                panelRoot.gameObject.activeSelf,
                Is.False,
                "PanelRoot should initially be hidden.");

            int closeRequestCount = 0;

            ReusableUIPanelModel model =
                new ReusableUIPanelModel(
                    "Test Title",
                    "Test Value",
                    () => closeRequestCount++);

            view.Bind(model);

            TMP_Text titleText =
                titleTransform.GetComponent<TMP_Text>();

            TMP_Text valueText =
                valueTransform.GetComponent<TMP_Text>();

            Button closeButton =
                closeButtonTransform.GetComponent<Button>();

            Assert.That(titleText.text, Is.EqualTo("Test Title"));
            Assert.That(valueText.text, Is.EqualTo("Test Value"));
            Assert.That(view.IsBound, Is.True);
            Assert.That(view.IsVisible, Is.False);

            view.Show();

            Assert.That(view.IsVisible, Is.True);
            Assert.That(panelRoot.gameObject.activeSelf, Is.True);

            closeButton.onClick.Invoke();

            Assert.That(closeRequestCount, Is.EqualTo(1));

            view.Hide();

            Assert.That(view.IsVisible, Is.False);
            Assert.That(panelRoot.gameObject.activeSelf, Is.False);

            view.Unbind();

            Assert.That(view.IsBound, Is.False);
            Assert.That(titleText.text, Is.Empty);
            Assert.That(valueText.text, Is.Empty);

            Object.Destroy(panelObject);
            Object.Destroy(canvasObject);
            Object.Destroy(eventSystemObject);

            yield return null;

            LogAssert.NoUnexpectedReceived();
        }
    }
}