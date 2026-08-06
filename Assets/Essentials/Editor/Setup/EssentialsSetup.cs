using UnityEditor;
using UnityEngine;

namespace SUG.Essentials.Editor
{
    public static class EssentialsSetup
    {
        private const string Root = "Assets/Resources/Essentials/";

        // SO
        private static UISettingsSO _ui;

        [MenuItem("Tools/Essentials/Setup Project")]
        public static void Setup()
        {
            UISetup();
            BootstrapSetup();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        // Bootstrap
        public static void BootstrapSetup()
        {
            EssentialsSettingsSO settings = AssetUtility.CreateAssetIfNotExist<EssentialsSettingsSO>(Root + "Bootstrap.asset");
            if (settings.uiSetting != _ui) settings.uiSetting = _ui;

            EditorUtility.SetDirty(settings);
        }

        // UI 模块初始化
        public static void UISetup()
        {
            UISettingsSO ui =
               AssetUtility.CreateAssetIfNotExist<UISettingsSO>(
                   Root + "UI/UISettings.asset");

            UIPanelConfigSO panel =
                AssetUtility.CreateAssetIfNotExist<UIPanelConfigSO>(
                    Root + "UI/UIPanelConfig.asset");

            UISoundCueSO sound =
                AssetUtility.CreateAssetIfNotExist<UISoundCueSO>(
                    Root + "UI/UISoundCue.asset");

            //------------------------------------------------
            // 自动建立引用
            //------------------------------------------------

            bool dirty = false;

            if (ui.panelCfg != panel)
            {
                ui.panelCfg = panel;
                dirty = true;
            }

            if (ui.sound != sound)
            {
                ui.sound = sound;
                dirty = true;
            }

            if (dirty)
            {
                EditorUtility.SetDirty(ui);
                _ui = ui;
            }

            //Selection.activeObject = ui;
            //EditorGUIUtility.PingObject(ui);
        }
    }
}