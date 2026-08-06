using UnityEngine;
using SUG.Essentials;
using DG.Tweening;
using System;

namespace SUG.Essentials
{
    public class RotateEffect : EffectBase
    {
        [Header("旋转方向")]
        [SerializeField]
        private bool _clockwise = true;

        [Header("旋转轴")]
        [SerializeField]
        private Vector3 _axis = Vector3.up;

        [Header("是否公转")]
        [SerializeField]
        private bool _isOrbit = false;

        [Header("公转中心")]
        [SerializeField]
        private Transform _center;

        [Header("一圈时间")]
        [SerializeField]
        private float _duration = 2f;

        private Tween _tween;

        // ===================
        // Core
        // ===================

        /// <summary>
        /// 自转
        /// </summary>
        private void SelfRotate()
        {
            float angle = _clockwise ? 360f : -360f;

            _tween = transform
                .DORotate(
                    _axis.normalized * angle,
                    _duration,
                    RotateMode.FastBeyond360
                )
                .SetEase(Ease.Linear)
                .SetLoops(-1);
        }

        /// <summary>
        /// 公转
        /// </summary>
        private void Orbit()
        {
            if (_center == null)
            {
                Debug.LogError("公转模式必须设置中心点");
                return;
            }


            float currentAngle = 0;

            float direction = _clockwise ? 1 : -1;


            _tween = DOTween.To(
                () => currentAngle,
                x =>
                {
                    float delta = x - currentAngle;

                    transform.RotateAround(
                        _center.position,
                        _axis.normalized,
                        delta * direction
                    );

                    currentAngle = x;
                },
                360f,
                _duration
            )
            .SetEase(Ease.Linear)
            .SetLoops(-1);
        }

        // ===================
        // Override function
        // ===================
        public override void Play()
        {
            Stop();

            if (_isOrbit)
            {
                Orbit();
            }
            else
            {
                SelfRotate();
            }
        }

        public override void Stop()
        {
            if (_tween != null)
            {
                _tween.Kill();
                _tween = null;
            }
        }
    }
}