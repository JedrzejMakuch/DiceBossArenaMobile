using DiceBossArena.UI;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace DiceBossArena.Tests.EditMode
{
    public sealed class ResourceBarUIViewTests
    {
        private GameObject root;
        private ResourceBarUIView view;
        private Image fillImage;
        private TMP_Text labelText;
        private TMP_Text valueText;

        [SetUp]
        public void SetUp()
        {
            root =
                new GameObject(
                    "Resource Bar",
                    typeof(RectTransform));

            view =
                root.AddComponent<ResourceBarUIView>();

            fillImage =
                CreateChild<Image>("Fill");

            labelText =
                CreateChild<TextMeshProUGUI>("Label");

            valueText =
                CreateChild<TextMeshProUGUI>("Value");

            SerializedObject serializedView =
                new(view);

            serializedView
                .FindProperty("fillImage")
                .objectReferenceValue = fillImage;

            serializedView
                .FindProperty("labelText")
                .objectReferenceValue = labelText;

            serializedView
                .FindProperty("valueText")
                .objectReferenceValue = valueText;

            serializedView.ApplyModifiedPropertiesWithoutUndo();
        }

        [TearDown]
        public void TearDown()
        {
            if (root != null)
            {
                Object.DestroyImmediate(root);
            }
        }

        [Test]
        public void Bind_RendersCurrentViewModel()
        {
            ResourceBarViewModelSource source =
                new(
                    new ResourceBarViewModel(
                        75,
                        100,
                        "HP"));

            view.Bind(source);

            Assert.That(
                fillImage.fillAmount,
                Is.EqualTo(0.75f));

            Assert.That(labelText.text, Is.EqualTo("HP"));
            Assert.That(valueText.text, Is.EqualTo("75 / 100"));
        }

        [Test]
        public void VisibleView_RendersPublishedChange()
        {
            ResourceBarViewModelSource source =
                new(
                    new ResourceBarViewModel(
                        100,
                        100,
                        "HP"));

            view.Bind(source);
            view.Show();

            source.Set(
                new ResourceBarViewModel(
                    25,
                    100,
                    "HP",
                    ResourceBarVisualState.Critical));

            Assert.That(
                fillImage.fillAmount,
                Is.EqualTo(0.25f));

            Assert.That(valueText.text, Is.EqualTo("25 / 100"));
            Assert.That(fillImage.color, Is.EqualTo(Color.red));
        }

        [Test]
        public void HiddenView_DoesNotRenderPublishedChange()
        {
            ResourceBarViewModelSource source =
                new(
                    new ResourceBarViewModel(
                        100,
                        100,
                        "HP"));

            view.Bind(source);

            source.Set(
                new ResourceBarViewModel(
                    50,
                    100,
                    "HP"));

            Assert.That(fillImage.fillAmount, Is.EqualTo(1f));
            Assert.That(valueText.text, Is.EqualTo("100 / 100"));
        }

        [Test]
        public void Unbind_StopsRenderingOldSource()
        {
            ResourceBarViewModelSource source =
                new(
                    new ResourceBarViewModel(
                        100,
                        100,
                        "HP"));

            view.Bind(source);
            view.Show();
            view.Hide();
            view.Unbind();

            source.Set(
                new ResourceBarViewModel(
                    0,
                    100,
                    "HP",
                    ResourceBarVisualState.Depleted));

            Assert.That(fillImage.fillAmount, Is.EqualTo(1f));
            Assert.That(valueText.text, Is.EqualTo("100 / 100"));
        }

        [Test]
        public void View_CanBeReusedWithAnotherSource()
        {
            ResourceBarViewModelSource firstSource =
                new(
                    new ResourceBarViewModel(
                        100,
                        100,
                        "HP"));

            ResourceBarViewModelSource secondSource =
                new(
                    new ResourceBarViewModel(
                        3,
                        5,
                        "AP",
                        ResourceBarVisualState.Enhanced));

            view.Bind(firstSource);
            view.Show();
            view.Hide();
            view.Unbind();

            view.Bind(secondSource);
            view.Show();

            firstSource.Set(
                new ResourceBarViewModel(
                    0,
                    100,
                    "HP"));

            Assert.That(
                fillImage.fillAmount,
                Is.EqualTo(0.6f));

            Assert.That(labelText.text, Is.EqualTo("AP"));
            Assert.That(valueText.text, Is.EqualTo("3 / 5"));
            Assert.That(fillImage.color, Is.EqualTo(Color.cyan));
        }

        [Test]
        public void DepletedState_RendersEmptyBar()
        {
            ResourceBarViewModelSource source =
                new(
                    new ResourceBarViewModel(
                        0,
                        100,
                        "HP",
                        ResourceBarVisualState.Depleted));

            view.Bind(source);

            Assert.That(fillImage.fillAmount, Is.Zero);
            Assert.That(fillImage.color, Is.EqualTo(Color.gray));
            Assert.That(valueText.text, Is.EqualTo("0 / 100"));
        }

        private T CreateChild<T>(
            string objectName)
            where T : Component
        {
            GameObject child =
                new GameObject(
                    objectName,
                    typeof(RectTransform));

            child.transform.SetParent(
                root.transform,
                false);

            return child.AddComponent<T>();
        }
    }
}