using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    // UI 预制体管理配置文件
    [CreateAssetMenu(fileName = "UIPanelConfig", menuName = "Essentials/UI/PanelConfig")]
    public class UIPanelConfigSO : ScriptableObject
    {
        [System.Serializable]
        public class UIConfigItem
        {
            public GameObject prefab;
        }

        public List<UIConfigItem> uiConfigs = new();
    }
}
