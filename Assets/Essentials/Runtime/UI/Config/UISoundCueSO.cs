using System;
using System.Collections.Generic;
using UnityEngine;

namespace SUG.Essentials
{
    // GUI 交互音效配置文件
    [CreateAssetMenu(fileName = "UISoundCueConfig", menuName = "Essentials/UI/AudioConfig")]
    public class UISoundCueSO : ScriptableObject
    {
        [Serializable] public class InteractionAudioRule
        {
            public InteractionTrigger trigger;
            public ControlType types;
            public AudioClip clip;
        }
        
        public List<InteractionAudioRule> rules;
    }
}
