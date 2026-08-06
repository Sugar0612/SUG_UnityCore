using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    // GUI 交互音效配置文件
    [CreateAssetMenu(fileName = "UISoundCueConfig", menuName = "Essentials/UI/UI_AudioConfig")]
    public class UISoundCueSO : ScriptableObject
    {
        [Serializable] public class InteractionAudioRule
        {
            public InteractionTrigger trigger;
            public ObjectTagSO[] tags;
            public AudioClip clip;
        }
        
        public List<InteractionAudioRule> rules;
    }
}
