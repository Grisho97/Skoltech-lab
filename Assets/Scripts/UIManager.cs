using System.Collections.Generic;
using UnityEngine;

namespace SkoltechLab
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [System.Serializable]
        public class WindowEntry
        {
            public string id;
            public UIWindow window;
        }

        [SerializeField] private List<WindowEntry> windows = new List<WindowEntry>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void Open(string id)
        {
            var w = windows.Find(x => x.id == id);
            if (w != null && w.window != null)
            {
                w.window.Open();
            }
            else
            {
                Debug.LogWarning($"Window with id {id} not found in UIWindowManager");
            }
        }
    }
}
