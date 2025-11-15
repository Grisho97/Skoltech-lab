using UnityEngine;

namespace SkoltechLab
{
    public class Clicker: MonoBehaviour
    {
        [SerializeField] private Camera cam;

        private void Awake()
        {
            if (cam == null)
                cam = Camera.main;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    var uiOpener = hit.collider.GetComponent<SceneUIOpener>();
                    if (uiOpener != null)
                    {
                        uiOpener.OpenUI();
                    }
                }
            }
        }
    }
}