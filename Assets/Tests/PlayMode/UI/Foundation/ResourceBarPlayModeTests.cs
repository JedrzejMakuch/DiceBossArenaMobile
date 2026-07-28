using System.Collections;
using DiceBossArena.UI;
using NUnit.Framework;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace DiceBossArena.Tests.PlayMode
{
    public sealed class ResourceBarPlayModeTests
    {
        private const string PrefabPath =
            "Assets/_Project/Prefabs/UI/Foundation/" +
            "ResourceBar.prefab";

        [UnityTest]
        public IEnumerator InstantiatedPrefab_RendersEventDrivenChanges()
        {
            GameObject prefab =
                AssetDatabase.LoadAssetAtPath<GameObject>(
                    PrefabPath);

            Assert.That(prefab, Is.Not.Null);

            GameObject instance =
                Object.Instantiate(prefab);

            ResourceBarUIView view =
                instance.GetComponent<ResourceBarUIView>();

            Image fillImage =
                instance.transform
                    .Find("Fill")
                    .GetComponent<Image>();

            TMP_Text labelText =
                instance.transform
                    .Find("LabelText")
                    .GetComponent<TMP_Text>();

            TMP_Text valueText =
                instance.transform
                    .Find("ValueText")
                    .GetComponent<TMP_Text>();

            ResourceBarViewModelSource source =
                new(
                    new ResourceBarViewModel(
                        100,
                        100,
                        "HP"));

            view.Bind(source);
            view.Show();

            yield return null;

            Assert.That(fillImage.fillAmount, Is.EqualTo(1f));
            Assert.That(labelText.text, Is.EqualTo("HP"));
            Assert.That(valueText.text, Is.EqualTo("100 / 100"));

            source.Set(
                new ResourceBarViewModel(
                    20,
                    100,
                    "HP",
                    ResourceBarVisualState.Critical));

            yield return null;

            Assert.That(
                fillImage.fillAmount,
                Is.EqualTo(0.2f));

            Assert.That(valueText.text, Is.EqualTo("20 / 100"));
            Assert.That(fillImage.color, Is.EqualTo(Color.red));

            view.Hide();
            view.Unbind();

            Object.Destroy(instance);

            yield return null;
        }
    }
}