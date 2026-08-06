using SUG.Essentials;
using UnityEngine;

namespace SUG.Essentials
{
    public class TriggerSoundEffect : EffectBase
    {
        // —— Config variable ——
        private UISoundCueSO _soundCfg;

        // Inject
        [Inject] private ICfgService _cfgMgr;
        [Inject] private IAudioService _audioMgr;

        // =====================
        // Core function
        // =====================
        private AudioClip GetClip(InteractionTrigger trigger, ControlType t)
        {
            if (_soundCfg == null) _soundCfg = Essentials.Settings.uiSetting.sound;
            foreach (var rule in _soundCfg.rules)
            {
                if (rule == null || rule.trigger != trigger) continue;
                if ((rule.types & t) != 0) return rule.clip;
            }

            return null;
        }

        private void PlayClip(InteractionTrigger trigger, ControlType types)
        {
            if (_soundCfg == null) _soundCfg = _cfgMgr.GetConfig<UISoundCueSO>();
            AudioClip c = GetClip(trigger, types);
            _audioMgr.Play(c);
        }

        // =====================
        // Override function
        // =====================
        public override void Play() => PlayClip(_currInterTrigger, _currControlType);
    }
}