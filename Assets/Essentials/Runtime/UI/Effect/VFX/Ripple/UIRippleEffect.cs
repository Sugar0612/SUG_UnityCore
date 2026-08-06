using UnityEngine;
using UnityEngine.UI;

namespace SUG.Essentials
{
    /// <summary>
    /// 特效控件波光
    /// </summary>
    public class UIRippleEffect : EffectBase
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
        private float glowIntensity = 15f;

        [SerializeField]
        private float pulseSpeed = 2f;

        [Header("Flow")]
        [SerializeField]
        private Color flowColor = Color.white;

        [SerializeField]
        private float flowWidth = 0.15f;

        [SerializeField]
        private float flowSpeed = 1f;

        private Material _runtimeMat;

        private static readonly int GlowColorID =
            Shader.PropertyToID("_GlowColor");

        private static readonly int GlowWidthID =
            Shader.PropertyToID("_GlowWidth");

        private static readonly int GlowIntensityID =
            Shader.PropertyToID("_GlowIntensity");

        private static readonly int PulseSpeedID =
            Shader.PropertyToID("_PulseSpeed");

        private static readonly int FlowColorID =
            Shader.PropertyToID("_FlowColor");

        private static readonly int FlowWidthID =
            Shader.PropertyToID("_FlowWidth");

        private static readonly int FlowSpeedID =
            Shader.PropertyToID("_FlowSpeed");

        protected void Start()
        {
            if (_image == null)
                _image = GetComponent<Image>();

            _runtimeMat =
                Instantiate(_image.material);

            _image.material =
                _runtimeMat;

            Apply();
            _image.SetMaterialDirty();
            SetRipple(false);
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

            _runtimeMat.SetFloat(
                PulseSpeedID,
                pulseSpeed);

            _runtimeMat.SetColor(
                FlowColorID,
                flowColor);

            _runtimeMat.SetFloat(
                FlowWidthID,
                flowWidth);

            _runtimeMat.SetFloat(
                FlowSpeedID,
                flowSpeed);
        }

        public void SetRipple(bool active)
        {
            if (_runtimeMat == null)
                return;

            float value = active ? glowIntensity : 0;
            _runtimeMat.SetFloat(
                GlowIntensityID,
                value);
        }

        // =================
        // Override
        // =================
        public override void Play() => SetRipple(true);
        public override void Stop() => SetRipple(false);
    }
}