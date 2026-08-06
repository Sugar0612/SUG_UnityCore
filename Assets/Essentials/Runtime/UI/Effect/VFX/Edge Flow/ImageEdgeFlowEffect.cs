using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SUG.Essentials;

namespace SUG.Essentials
{
    public class ImageEdgeFlowEffect : EffectBase
    {
        // —— Public variable ——
        [Header("材质设置")]
        public Material mat;            // EdgeFlow Shader材质
        public float speed = 0.2f;      // 光流速度

        // —— Private variable ——
        private bool _running = false;

        // ===================
        // Life cycle
        // ===================
        protected override void Awake()
        {
            base.Awake();
            // if(mat == null)
            // {
            //     Image img = GetComponent<Image>();
            //     if(img != null)
            //         mat = img.material;
            // }

            // 默认关闭流光
            if(mat != null)
            {
                Image img = GetComponent<Image>();
                if (img != null) img.material = new Material(mat);
                mat = img.material;
                mat.SetFloat("_SweepPos", -1f); // -1表示不显示
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);
            }

            StopFlow();
        }

        void Update()
        {
            if(mat != null && _running)
            {
                float pos = mat.GetFloat("_SweepPos");
                pos += Time.deltaTime * speed;
                if(pos > 1f) pos -= 1f;
                mat.SetFloat("_SweepPos", pos);
            }
        }

        // ===================
        // Core function
        // ===================
        
        /// <summary>
        /// 点击开始流光
        /// </summary>
        public void PlayFlow()
        {
            _running = true;
            if(mat != null && mat.GetFloat("_SweepPos") < 0)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 1.0f);
                mat.SetFloat("_SweepPos", 0f);
            }
        }

        /// <summary>
        /// 停止流光
        /// </summary>
        public void StopFlow()
        {
            _running = false;
            if(mat != null)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, 0.0f);
                mat.SetFloat("_SweepPos", -1f); // 隐藏流光
            }
        }

        private IEnumerator SweepOnceCoroutine(float duration)
        {
            _running = false; // 暂停Update里持续流动
            float t = 0f;

            if(mat != null)
                mat.SetFloat("_SweepPos", 0f);

            while(t < duration)
            {
                float pos = t / duration;
                mat.SetFloat("_SweepPos", pos);
                t += Time.deltaTime;
                yield return null;
            }

            if(mat != null)
                mat.SetFloat("_SweepPos", -1f); // 播放完隐藏
        }

        // =====================
        // Override function
        // ====================
        public override void Play() => PlayFlow();
        public override void Stop() => StopFlow();
    }
}