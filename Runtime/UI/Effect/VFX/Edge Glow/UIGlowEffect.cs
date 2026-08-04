using UnityEngine;
using UnityEngine.UI;

namespace SUG.Essentials
{
    public class UIEdgeGlowEffect : EffectBase
    {
        [Header("Reference")]
        [SerializeField]
        private Image _image;

        [Header("Glow")]
        [SerializeField]
        private Color glowColor =
            new Color(0.3f, 0.7f, 1f);

        [SerializeField]
        private float glowWidth = 4f;

        [SerializeField]
        private float glowIntensity = 10f;

        private Material _runtimeMat;

        private static readonly int GlowColorID =
            Shader.PropertyToID("_GlowColor");

        private static readonly int GlowWidthID =
            Shader.PropertyToID("_GlowWidth");

        private static readonly int GlowIntensityID =
            Shader.PropertyToID("_GlowIntensity");

        protected virtual void Start()
        {
            if (_image == null)
                _image = GetComponent<Image>();

            _runtimeMat =
                Instantiate(_image.material);

            _image.material =
                _runtimeMat;

            Apply();

            _image.SetMaterialDirty();

            SetGlow(false);
        }

        private void Apply()
        {
            if (_runtimeMat == null)
                return;

            _runtimeMat.SetColor(
                GlowColorID,
                glowColor);

            _runtimeMat.SetFloat(
                GlowWidthID,
                glowWidth);
        }

        public void SetGlow(bool active)
        {
            if (_runtimeMat == null)
                return;

            _runtimeMat.SetFloat(
                GlowIntensityID,
                active ? glowIntensity : 0f);
        }

        // ==================
        // Override function
        // ==================

        public override void Play()
        {
            SetGlow(true);
        }

        public override void Stop()
        {
            SetGlow(false);
        }
    }
}