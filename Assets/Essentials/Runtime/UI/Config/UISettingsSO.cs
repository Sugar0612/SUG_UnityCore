using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    // UI 总配置文件
    [CreateAssetMenu(fileName = "UISettings", menuName = "Essentials/UI/ASettings")]
    public class UISettingsSO : ScriptableObject
    {
        [Header("Inter Sound Config")]
        public UISoundCueSO sound;

        [Header("Panel Config")]
        public UIPanelConfigSO panelCfg;
    }
}