Shader "UI/EdgeFlow/AlphaEdgeSmooth"
{
    Properties
    {
        _MainTex("Sprite", 2D) = "white" {}

        _Color("Glow Color", Color) = (0,1,1,1)

        _SweepWidth("Sweep Width", Range(0.01,1)) = 0.2

        _SweepPos("Sweep Position", Range(0,1)) = 0

        _Intensity("Intensity", Range(0,10)) = 2

        _EdgeSoft("Edge Softness", Range(0.001,0.2)) = 0.03

        _SampleOffset("AA Strength", Range(0.0001,0.01)) = 0.002
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
        }

        Cull Off
        ZWrite Off

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;

            float _SweepWidth;
            float _SweepPos;
            float _Intensity;
            float _EdgeSoft;
            float _SampleOffset;

            v2f vert(appdata_t v)
            {
                v2f o;

                o.vertex = TransformObjectToHClip(v.vertex);

                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                return o;
            }

            float SampleAlpha(float2 uv)
            {
                return tex2D(_MainTex, uv).a;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                //----------------------------------
                // Alpha AA
                //----------------------------------

                float alphaCenter = SampleAlpha(uv);

                float alpha =
                    alphaCenter +
                    SampleAlpha(uv + float2(_SampleOffset,0)) +
                    SampleAlpha(uv + float2(-_SampleOffset,0)) +
                    SampleAlpha(uv + float2(0,_SampleOffset)) +
                    SampleAlpha(uv + float2(0,-_SampleOffset));

                alpha *= 0.2;

                //----------------------------------
                // Edge Detection
                //----------------------------------

                float inner =
                    smoothstep(
                        0.0,
                        _EdgeSoft,
                        alpha
                    );

                float outer =
                    smoothstep(
                        1.0 - _EdgeSoft,
                        1.0,
                        alpha
                    );

                float edge = saturate(inner - outer);

                //----------------------------------
                // Sweep
                //----------------------------------

                float sweepCoord =
                    frac(
                        _SweepPos +
                        uv.x * 0.5 +
                        uv.y * 0.5
                    );

                float wave =
                    0.5 +
                    0.5 *
                    sin(
                        sweepCoord *
                        6.2831853
                    );

                float sweep =
                    smoothstep(
                        1.0 - _SweepWidth,
                        1.0,
                        wave
                    );

                //----------------------------------
                // Final
                //----------------------------------

                half4 col =
                    _Color *
                    edge *
                    sweep *
                    _Intensity;

                col.a *= edge;

                return col;
            }

            ENDHLSL
        }
    }
}