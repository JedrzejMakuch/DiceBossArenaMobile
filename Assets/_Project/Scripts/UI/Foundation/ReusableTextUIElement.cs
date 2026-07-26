using TMPro;
using UnityEngine;

namespace DiceBossArena.UI
{
    public sealed class ReusableTextUIElement :
        MonoBehaviour,
        IReusableUIElement
    {
        [SerializeField]
        private TMP_Text text;

        public void SetText(
            string value)
        {
            if (text == null)
            {
                Debug.LogError(
                    $"{nameof(ReusableTextUIElement)} requires " +
                    $"a {nameof(TMP_Text)} reference.",
                    this);

                return;
            }

            text.text =
                value ?? string.Empty;
        }

        public void PrepareForUse()
        {
            gameObject.SetActive(true);
        }

        public void ResetForPool()
        {
            if (text != null)
            {
                text.text = string.Empty;
            }

            gameObject.SetActive(false);
        }
    }
}