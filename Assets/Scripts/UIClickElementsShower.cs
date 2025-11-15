using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SkoltechLab
{
    public class UIClickElementsShower : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private List<GameObject> elementsToShow = new List<GameObject>();
    
        
        // этот метод можно повесить на кнопку в OnClick
        public void ShowAll()
        {
            foreach (var go in elementsToShow)
            {
                if (go == null) continue;

                go.gameObject.SetActive(true);
                var showable = go.GetComponent<IUIShow>();
                if (showable != null)
                {
                    showable.ShowUIElement();
                }
                else
                {
                    Debug.LogWarning($"Object {go.name} не имеет компонента, реализующего IUIShow.", go);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ShowAll();
        }
    }
}