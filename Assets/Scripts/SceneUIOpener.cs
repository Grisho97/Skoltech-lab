using UnityEngine;

namespace SkoltechLab
{
    public class SceneUIOpener: MonoBehaviour
    {
        [SerializeField] private string windowId;  // например: "ChestWindow" или "NPCDialog"

        public void OpenUI()
        {
            UIManager.Instance.Open(windowId);
        }
    }
}