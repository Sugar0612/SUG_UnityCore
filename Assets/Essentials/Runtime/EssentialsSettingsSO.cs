using UnityEngine;

namespace SUG.Essentials
{
    [CreateAssetMenu(
        fileName = "EssentialsSettings",
        menuName = "Essentials/Settings")]
    public class EssentialsSettingsSO : ScriptableObject
    {
        [Header("UI")]
        public UISettingsSO uiSetting;

        //[Header("Audio")]
        //public AudioSettingsSO Audio;

        //[Header("Pool")]
        //public PoolSettingsSO Pool;
    }
}