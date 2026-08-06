using DG.Tweening;
using SUG.Essentials;
using UnityEngine;

namespace SUG.Essentials
{
    public sealed class ShakeEffect : EffectBase
    {
        [Header("Shake Config")]
        [SerializeField] private float angle = 10f;
        [SerializeField] private float duration = 0.4f;
        [SerializeField] private int vibrato = 10;

        private Tween _shakeTween;

        // =================
        // Core
        // =================
        private void PlayShake()
        {
            _shakeTween?.Kill();

            transform.localRotation = Quaternion.identity;

            _shakeTween = transform.DOShakeRotation(
                duration,
                new Vector3(0, 0, angle),
                vibrato
            );
        }

        // ============================
        // Override function from base
        // ============================

        public override void Play() => PlayShake();
    }
}