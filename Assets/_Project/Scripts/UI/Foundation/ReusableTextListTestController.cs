using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

namespace DiceBossArena.UI
{
    public sealed class ReusableTextListTestController :
        MonoBehaviour
    {
        private bool refreshFiftyItemsRequested;
        private const int ItemCount = 50;
        private const int RefreshCount = 100;

        private static readonly ProfilerMarker
            RefreshFiftyItemsMarker =
                new("UI Foundation 5.Refresh Fifty Items");

        private readonly string[] firstFiftyItems =
            CreateItems("Item");

        private readonly string[] updatedFiftyItems =
            CreateItems("Updated Item");

        [SerializeField]
        private ReusableTextList list;

        [ContextMenu("Show Five Items")]
        public void ShowFiveItems()
        {
            list.SetItems(
                new[]
                {
                    "First",
                    "Second",
                    "Third",
                    "Fourth",
                    "Fifth"
                });
        }

        [ContextMenu("Update Five Items")]
        public void UpdateFiveItems()
        {
            list.SetItems(
                new[]
                {
                    "Updated First",
                    "Updated Second",
                    "Updated Third",
                    "Updated Fourth",
                    "Updated Fifth"
                });
        }

        [ContextMenu("Show Two Items")]
        public void ShowTwoItems()
        {
            list.SetItems(
                new[]
                {
                    "Only First",
                    "Only Second"
                });
        }

        [ContextMenu("Clear Items")]
        public void ClearItems()
        {
            list.Clear();
        }

        [ContextMenu("Show Fifty Items")]
        public void ShowFiftyItems()
        {
            list.SetItems(firstFiftyItems);
        }

        [ContextMenu("Refresh Fifty Items 100 Times")]
        public void RequestRefreshFiftyItemsRepeatedly()
        {
            refreshFiftyItemsRequested = true;
        }

        private void Update()
        {
            if (!refreshFiftyItemsRequested)
            {
                return;
            }

            refreshFiftyItemsRequested = false;
            RefreshFiftyItemsRepeatedly();
        }

        [ContextMenu("Refresh Fifty Items 100 Times")]
        private void RefreshFiftyItemsRepeatedly()
        {
            using (RefreshFiftyItemsMarker.Auto())
            {
                for (int index = 0;
                     index < RefreshCount;
                     index++)
                {
                    IReadOnlyList<string> items =
                        index % 2 == 0
                            ? updatedFiftyItems
                            : firstFiftyItems;

                    list.SetItems(items);
                }
            }
        }

        private static string[] CreateItems(
            string prefix)
        {
            string[] items =
                new string[ItemCount];

            for (int index = 0;
                 index < items.Length;
                 index++)
            {
                items[index] =
                    $"{prefix} {index + 1}";
            }

            return items;
        }
    }
}