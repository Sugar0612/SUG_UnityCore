using UnityEngine;
using SUG.Essentials;
using DG.Tweening;
using System;

namespace SUG.Essentials
{
    public class ScaleEffect : EffectBase
    {
        // —— Config variable ——
        [Header("交互变量")]
        public Vector3 _normalScale = new Vector3(1.0f, 1.0f, 1.0f);
        public Vector3 _zoominScale = new Vector3(1.1f, 1.1f, 1.1f);
        public Vector3 _zoomoutScale = new Vector3(0.9f, 0.9f, 0.9f);

        [Header("动画时长")]
        public float zoominDur = 1.0f;
        public float zoomoutDur = 0.3f;

        // ===================
        // Core
        // ===================
        public void OnZoomIn(Action callback = null)
        {
            SetScale(_zoominScale, zoominDur, callback);
        }

        public void OnNormal(Action callback = null)
        {
            SetScale(_normalScale, zoominDur, callback);
        }

        public void OnZoomOut(Action callback = null)
        {
            SetScale(_zoomoutScale, zoomoutDur, callback);
        }

        // ===================
        // Toolkit Function
        // ===================
        private void SetScale(Vector3 target, float duration, Action callback = null)
        {
            transform.DOScale(target, duration).OnComplete(() => { callback?.Invoke(); });
        }

        // ===================
        // Override function
        // ===================
        public override void Play() => OnZoomIn();

        public override void Stop() => OnNormal();

        public override void Other() => SetScale(_zoomoutScale, zoomoutDur, () => OnNormal());
    }
}