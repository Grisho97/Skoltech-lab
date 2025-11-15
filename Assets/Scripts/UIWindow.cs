using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SkoltechLab
{
    public class UIWindow : MonoBehaviour
    {
        [SerializeField] private float animationDuration = 1f;
        [SerializeField] private Button closeButton;

        private RectTransform rect;
        private Vector2 hiddenPos;
        private Vector2 shownPos;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();

            // shownPos — это конечная позиция (куда окно должно приехать)
            shownPos = rect.anchoredPosition;

            // hiddenPos — ниже экрана, можно просто -screenHeight, но возьмём по месту
            hiddenPos = new Vector2(shownPos.x, shownPos.y - Screen.height);

            rect.anchoredPosition = hiddenPos;

            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
        }

        public void Open()
        {
            gameObject.SetActive(true);
            rect.DOKill();
            rect.anchoredPosition = hiddenPos;
            rect.DOAnchorPos(shownPos, animationDuration).SetEase(Ease.OutCubic);
        }

        public void Close()
        {
            rect.DOKill();
            rect.DOAnchorPos(hiddenPos, animationDuration)
                .SetEase(Ease.InCubic)
                .OnComplete(() => { gameObject.SetActive(false); });
        }
    }
}
