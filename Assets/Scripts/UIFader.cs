using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkoltechLab
{
    public class UIFader : MonoBehaviour, IUIShow
    {
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private bool includeInactive = true;
    
        /// <summary>
        /// Проявляет все UI-элементы (Image, RawImage, TMP_Text) в дочерних.
        /// </summary>
        public void ShowUIElement()
        {
            // Image
            var images = GetComponentsInChildren<Image>(includeInactive);
            foreach (var img in images)
            {
                var c = img.color;
                c.a = 0f;
                img.color = c;
                img.DOFade(1f, duration);
            }

            // RawImage
            var rawImages = GetComponentsInChildren<RawImage>(includeInactive);
            foreach (var rimg in rawImages)
            {
                var c = rimg.color;
                c.a = 0f;
                rimg.color = c;
                rimg.DOFade(1f, duration);
            }

            // TextMeshPro
            var texts = GetComponentsInChildren<TMP_Text>(includeInactive);
            foreach (var txt in texts)
            {
                var c = txt.color;
                c.a = 0f;
                txt.color = c;
                txt.DOFade(1f, duration);
            }
        }
        
        public void HideUIElement()
        {
            int tweens = 0;

            foreach (var img in GetComponentsInChildren<Image>(includeInactive))
            {
                tweens++;
                img.DOFade(0f, duration);
            }

            foreach (var r in GetComponentsInChildren<RawImage>(includeInactive))
            {
                tweens++;
                r.DOFade(0f, duration);
            }

            foreach (var t in GetComponentsInChildren<TMP_Text>(includeInactive))
            {
                tweens++;
                t.DOFade(0f, duration);
            }

            
            // выключим объект после завершения последнего твина
            // (если твинов нет — выключим сразу)
            if (tweens == 0) gameObject.SetActive(false);
            else DOVirtual.DelayedCall(duration, () => gameObject.SetActive(false));
            
        }
    }
}