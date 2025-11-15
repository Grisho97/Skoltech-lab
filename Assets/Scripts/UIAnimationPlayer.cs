using UnityEngine;
using UnityEngine.EventSystems;

namespace SkoltechLab
{
    public class UIAnimationPlayer : MonoBehaviour, IUIShow
    {
        [Header("Animation")]
        [SerializeField] private Animator animator;      // куда стучимся
        [SerializeField] private string boolName;        // имя bool-параметра в Animator

        [Header("Options")]
        [SerializeField] private bool ignoreWhenEmpty = true;

        private void Awake()
        {
            if (animator == null)
                animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Установить значение bool извне.
        /// </summary>
        public void SetBool(bool value)
        {
            if (!CanUseAnimator()) return;

            animator.SetBool(boolName, value);
        }

        private bool CanUseAnimator()
        {
            if (animator == null)
            {
                Debug.LogWarning($"[{name}] UIAnimationPlayer: Animator not set");
                return false;
            }

            if (string.IsNullOrEmpty(boolName))
            {
                if (!ignoreWhenEmpty)
                    Debug.LogWarning($"[{name}] UIAnimationPlayer: boolName is empty");
                return false;
            }

            return true;
        }

        public void ShowUIElement()
        {
            SetBool(true);
        }

        public void HideUIElement()
        {
            SetBool(false);
        }
    }
}