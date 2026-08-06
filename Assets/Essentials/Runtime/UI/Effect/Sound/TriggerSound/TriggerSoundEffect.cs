using SUG.Essentials;
using System.Linq;
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
        private AudioClip GetClip(InteractionTrigger trigger, string t)
        {
            if (_soundCfg == null) _soundCfg = Essentials.Settings.uiSetting.sound;
            foreach (var rule in _soundCfg.rules)
            {
                if (rule == null || rule.trigger != trigger) continue;
                if (rule.tags.FirstOrDefault(x => x.tagName == t) != null) return rule.clip;
            }

            return null;
        }

        private void PlayClip(InteractionTrigger trigger, string t)
        {
            if (_soundCfg == null) _soundCfg = _cfgMgr.GetConfig<UISoundCueSO>();
            AudioClip c = GetClip(trigger, t);
            _audioMgr.Play(c);
        }

        // =====================
        // Override function
        // =====================
        public override void Play() => PlayClip(_currInterTrigger, _currControlTag.tagName);
    }
}