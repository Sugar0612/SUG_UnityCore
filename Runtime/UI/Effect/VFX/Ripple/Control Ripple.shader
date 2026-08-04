Shader "Custom/UI/ControlPipple"
{
    Properties
    {
        [PerRendererData]_MainTex ("Sprite", 2D) = "white" {}

        _Color ("Tint", Color) = (1,1,1,1)

        _GlowColor ("Glow Color", Color) = (0.3,0.7,1,1)
        _GlowIntensity ("Glow Intensity", Range(0,20)) = 0
        _GlowWidth ("Glow Width", Range(1,20)) = 4

        _PulseSpeed ("Pulse Speed", Range(0,10)) = 2

        _FlowColor ("Flow Color", Color) = (1,1,1,1)
        _FlowWidth ("Flow Width", Range(0.01,0.5)) = 0.15
        _FlowSpeed ("Flow Speed", Range(0,10)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            float4 _Color;

            float4 _GlowColor;
            float _GlowIntensity;
            float _GlowWidth;

            float _PulseSpeed;

            float4 _FlowColor;
            float _FlowWidth;
            float _FlowSpeed;

            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;

                return o;
            }

            float SampleOutline(float2 uv)
            {
                float2 texel =
                    _MainTex_TexelSize.xy *
                    _GlowWidth;

                float maxAlpha = 0;

                maxAlpha = max(maxAlpha, tex2D(_MainTex, uv + float2( texel.x,0)).a);
                maxAlpha = max(maxAlpha, tex2D(_MainTex, uv + float2(-texel.x,0)).a);

                maxAlpha = max(maxAlpha, tex2D(_MainTex, uv + float2(0, texel.y)).a);
                maxAlpha = max(maxAlpha, tex2D(_MainTex, uv + float2(0,-texel.y)).a);

                maxAlpha = max(maxAlpha, tex2D(_MainTex, uv + texel).a);
                maxAlpha = max(maxAlpha, tex2D(_MainTex, uv - texel).a);

                maxAlpha = max(maxAlpha, tex2D(_MainTex, uv + float2(texel.x,-texel.y)).a);
                maxAlpha = max(maxAlpha, tex2D(_MainTex, uv + float2(-texel.x,texel.y)).a);

                return maxAlpha;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col =
                    tex2D(_MainTex, i.uv) *
                    i.color;

                float alpha = col.a;
                
                float outline =
                    SampleOutline(i.uv)
                    *
                    (1 - alpha);

                float pulse =
                    0.75 +
                    0.25 *
                    sin(_Time.y * _PulseSpeed);

                float glow =
                    outline *
                    pulse *
                    _GlowIntensity;

                col.rgb +=
                    _GlowColor.rgb *
                    glow;

                float flowPos =
                    frac(
                        i.uv.x +
                        _Time.y * _FlowSpeed
                    );

                float flow =
                    smoothstep(
                        0,
                        _FlowWidth,
                        flowPos
                    )
                    *
                    (1 -
                     smoothstep(
                        _FlowWidth,
                        _FlowWidth * 2,
                        flowPos
                     ));

                col.rgb +=
                    _FlowColor.rgb *
                    flow *
                    outline *
                    _GlowIntensity;

                return col;
            }

            ENDCG
        }
    }
}