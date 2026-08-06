// ==============================
// 全局父节点，挂在父节点下的子物体不会随着场景切换而销毁
//===============================
using UnityEngine;

namespace SUG.Essentials
{
    internal sealed class GlobalManagersRoot : MonoBehaviour
    {
        private static GlobalManagersRoot _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}