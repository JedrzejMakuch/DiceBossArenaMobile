using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceBossArena.UI
{
    public sealed class ReusableTextList :
        MonoBehaviour
    {
        private static readonly
            Action<ReusableTextUIElement, string> BindText =
                BindElement;

        [SerializeField]
        private ReusableTextUIElementFactory factory;

        private ReusableUIElementCollection<
            ReusableTextUIElement> elements;

        private void Awake()
        {
            if (factory == null)
            {
                Debug.LogError(
                    $"{nameof(ReusableTextList)} requires " +
                    $"a {nameof(ReusableTextUIElementFactory)} reference.",
                    this);

                return;
            }

            elements =
                new ReusableUIElementCollection<
                    ReusableTextUIElement>(
                    factory.CreatePool());
        }

        public void SetItems(
            IReadOnlyList<string> items)
        {
            if (elements == null)
            {
                Debug.LogError(
                    $"{nameof(ReusableTextList)} is not initialized.",
                    this);

                return;
            }

            elements.SetItems(
                items,
                BindText);
        }

        public void Clear()
        {
            elements?.Clear();
        }

        private static void BindElement(
            ReusableTextUIElement element,
            string value)
        {
            element.SetText(value);
        }
    }
}