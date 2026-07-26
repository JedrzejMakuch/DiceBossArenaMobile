using UnityEngine;

namespace DiceBossArena.UI
{
    public sealed class ReusableTextListTestController :
        MonoBehaviour
    {
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
    }
}