using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DiceBossArena.UI
{
    public sealed class ReusableUIPanelView :
        BindableUIView<ReusableUIPanelModel>
    {
        [Header("Visibility")]
        [SerializeField]
        private GameObject panelRoot;

        [Header("Content")]
        [SerializeField]
        private TMP_Text titleText;

        [SerializeField]
        private TMP_Text valueText;

        [Header("Interaction")]
        [SerializeField]
        private Button closeButton;

        private void Awake()
        {
            panelRoot.SetActive(false);
        }

        protected override void OnBind(
            ReusableUIPanelModel boundModel)
        {
            titleText.text = boundModel.Title;
            valueText.text = boundModel.Value;

            closeButton.onClick.AddListener(
                HandleCloseClicked);
        }

        protected override void OnShow()
        {
            panelRoot.SetActive(true);
        }

        protected override void OnHide()
        {
            panelRoot.SetActive(false);
        }

        protected override void OnUnbind()
        {
            closeButton.onClick.RemoveListener(
                HandleCloseClicked);

            titleText.text = string.Empty;
            valueText.text = string.Empty;
        }

        private void HandleCloseClicked()
        {
            Model.RequestClose();
        }
    }
}