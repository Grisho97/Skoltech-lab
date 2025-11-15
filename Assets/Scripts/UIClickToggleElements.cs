using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkoltechLab
{
    public class UIClickToggleElements : MonoBehaviour, IPointerClickHandler
    {
        [Header("Targets")]
        [SerializeField] private List<GameObject> elements = new List<GameObject>();

        [Header("Chaining")]
        [Tooltip("Список других UIClickToggleElements, которым передавать состояние.")]
        [SerializeField] private List<UIClickToggleElements> propagateTo = new List<UIClickToggleElements>();
        [SerializeField] private bool propagateOnShow = false;
        [SerializeField] private bool propagateOnHide = true;

        [Header("Options")]
        [SerializeField] private bool startOn = false;           // начальное состояние
        [SerializeField] private bool setActiveOnShow = true;    // активировать GO перед Show
        [SerializeField] private bool ignoreMissing = true;      // не спамить варнингами

        private bool isOn;

        private void Awake()
        {
            isOn = startOn;
            // Приводим к начальному состоянию без дальнейшей пропагации
            if (isOn) ShowAll(propagate:false);
            else      HideAll(propagate:false);
        }

        public void OnPointerClick(PointerEventData eventData) => Toggle();

        /// <summary> Инвертировать состояние (по клику/внешне). </summary>
        public void Toggle(bool propagate = true)
        {
            SetState(!isOn, propagate);
        }

        /// <summary> Принудительно задать состояние. </summary>
        public void SetState(bool on, bool propagate = true)
        {
            if (on == isOn)
                return;

            // локальное применение
            if (on) ShowAll(propagate);
            else    HideAll(propagate);

            isOn = on;
        }

        /// <summary> Показать все элементы. </summary>
        public void ShowAll(bool propagate = true)
        {
            ShowAllInternal(propagate, visited: null);
            isOn = true;
        }

        /// <summary> Спрятать все элементы. </summary>
        public void HideAll(bool propagate = true)
        {
            HideAllInternal(propagate, visited: null);
            isOn = false;
        }

        // ===== ВНУТРЕННИЕ РЕАЛИЗАЦИИ С ЗАЩИТОЙ ОТ ЦИКЛОВ =====

        private void ShowAllInternal(bool propagate, HashSet<UIClickToggleElements> visited)
        {
            if (visited == null) visited = new HashSet<UIClickToggleElements>();
            if (!visited.Add(this)) return; // уже обрабатывали — выходим (защита от циклов)

            // 1) локально показать
            foreach (var go in elements)
            {
                if (!go) continue;

                if (setActiveOnShow && !go.activeSelf)
                    go.SetActive(true);

                var show = go.GetComponent<IUIShow>();
                if (show != null) show.ShowUIElement();
                else if (!ignoreMissing) Debug.LogWarning($"[{name}] {go.name} не реализует IUIShow", go);
            }

            // 2) пропагация дальше (опционально)
            if (propagate && propagateOnShow)
            {
                foreach (var next in propagateTo)
                {
                    if (!next) continue;
                    // Ставим состояние у следующего без повторной локальной инверсии:
                    next.ShowAllInternal(true, visited);
                    next.isOn = true;
                }
            }
        }

        private void HideAllInternal(bool propagate, HashSet<UIClickToggleElements> visited)
        {
            if (visited == null) visited = new HashSet<UIClickToggleElements>();
            if (!visited.Add(this)) return; // уже обрабатывали — выходим (защита от циклов)

            // 1) локально спрятать
            foreach (var go in elements)
            {
                if (!go) continue;

                var show = go.GetComponent<IUIShow>();
                if (show != null) show.HideUIElement();
                else if (!ignoreMissing) Debug.LogWarning($"[{name}] {go.name} не реализует IUIShow", go);
            }

            // 2) пропагация дальше (опционально)
            if (propagate && propagateOnHide)
            {
                foreach (var next in propagateTo)
                {
                    if (!next) continue;
                    next.HideAllInternal(true, visited);
                    next.isOn = false;
                }
            }
        }
    }
}